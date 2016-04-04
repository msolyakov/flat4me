using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Flat4Me.ImageServiceCore;
using Flat4Me.Core.Rabbit;
using System.Diagnostics.Tracing;
using Flat4Me.Data.Repository.Azure;

namespace Flat4Me.ImageWorkerRole.FlatPhoto
{
    ///// <summary>
    ///// Custom event source for ImageWorkerRole. 
    ///// Requires diagnostic configuration after WorkerRole deployment.
    ///// </summary>
    ///// <remarks>
    ///// For more information about confuguring Azure Diagnostics 1.3 see 
    ///// http://azure.microsoft.com/en-us/documentation/articles/cloud-services-dotnet-diagnostics/.
    ///// Azure PowerShell guide: 
    ///// http://azure.microsoft.com/ru-ru/documentation/articles/install-configure-powershell/
    ///// </remarks>
    //sealed class ImageEventWriter : EventSource
    //{
    //    public static ImageEventWriter Log = new ImageEventWriter();

    //    public void WriteInfo(string msg) { if (IsEnabled())  WriteEvent(1, msg); }
    //    public void WriteWarning(string warning) { if (IsEnabled())  WriteEvent(2, warning); }
    //    public void WriteError(string error) { if (IsEnabled())  WriteEvent(3, error); }
    //}
    
    public class WorkerRole : RoleEntryPoint
    {
        ImageQueueProcessor iqp;
        private bool _processImages = false;
        private StorageLogRepository _logger;

        /// <summary>
        /// Set the maximum number of concurrent connections to 5.
        /// Default number is 2.
        /// </summary>
        /// <remarks>
        /// For information on handling configuration changes on Azure SDK 2.4 or ealier
        /// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
        /// </remarks>
        public override bool OnStart()
        {
            try
            {
                _logger = new StorageLogRepository();
                _logger.Init();
            }
            catch
            {
                // Если нет логирования, не старуем
                return false;
            }
            
            ServicePointManager.DefaultConnectionLimit = 5;

            bool result = base.OnStart();
            if (result)
            {
                _logger.LogInfo("[FlatPhoto] Starting...");
            }

            return result;
        }
        
        public override void Run()
        {
            _processImages = true;

            // Запускаем асинхронно обработку изображений.
            Task _processorTask = Task.Run(() =>
            {
                iqp = new ImageQueueProcessor(
                    RabbitFacadeConsts.QUEUE_IMAGE_MAIN, ImageStorageType.FlatPhotoAzure);

                // Подписываемся на события ошибок и предупреждений
                iqp.ProcessingError += onError;
                iqp.ProcessingWarning += onWarning;

                _logger.LogInfo(String.Format("[FlatPhoto] Listening {0}:{1}/{2}", iqp.Host, iqp.Port, iqp.Vhost));

                // Запускаем обработку
                iqp.DoProcessMessages(ref _processImages);
            });

            _logger.LogInfo("[FlatPhoto] Running");
            _processorTask.Wait();            
        }

        public override void OnStop()
        {
            _processImages = false;
            Thread.Sleep(2000);
            base.OnStop();

            _logger.LogInfo("[FlatPhoto] Stopped");
        }

        #region Error/Warning handlers

        private void onError(object o, ErrorEventArgs ea)
        {
            Exception e = ea.GetException();
            Exception reason = (e.InnerException != null) ? e.InnerException : e;

            _logger.LogException(String.Format("[FlatPhoto] Listening {0}:{1}/{2}. {3}", iqp.Host, iqp.Port, iqp.Vhost, e.Message), reason);
           
            // В случае ошибки останавливаем сервис
            _processImages = false;
        }

        private void onWarning(object o, ErrorEventArgs ea)
        {
            Exception e = ea.GetException();
            Exception reason = (e.InnerException != null) ? e.InnerException : e;

            _logger.LogWarning(String.Format("[FlatPhoto] Listening {0}:{1}/{2}. {3}", iqp.Host, iqp.Port, iqp.Vhost, e.Message), reason);
        }

        #endregion
    }
}
