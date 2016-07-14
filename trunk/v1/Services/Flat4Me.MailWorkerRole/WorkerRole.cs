using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Flat4Me.Data.Repository.Azure;
using System.IO;
using Flat4Me.Core.Rabbit;
using Flat4Me.MailServiceCore;

namespace Flat4Me.MailWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        MailQueueProcessor mqp;
        private bool _processMails = false;
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
                _logger.LogInfo("[Mail] Starting...");
            }

            return result;
        }

        public override void Run()
        {
            _processMails = true;

            // Запускаем асинхронно обработку почты.
            Task _processorTask = Task.Run(() =>
            {
                mqp = new MailQueueProcessor();

                // Подписываемся на события ошибок и предупреждений
                mqp.ProcessingError += onError;
                mqp.ProcessingWarning += onWarning;

                _logger.LogInfo(String.Format("[Mail] Listening {0}:{1}/{2}", mqp.Host, mqp.Port, mqp.Vhost));
                
                // Запускаем обработку
                mqp.DoProcessMessages(ref _processMails);
            });

            _logger.LogInfo("[Mail] Running");
            _processorTask.Wait();            
        }

        public override void OnStop()
        {
            _processMails = false;
            Thread.Sleep(2000);
            base.OnStop();

            _logger.LogInfo("[Mail] Stopped");
        }

        #region Error/Warning handlers

        private void onError(object o, ErrorEventArgs ea)
        {
            Exception e = ea.GetException();
            Exception reason = (e.InnerException != null) ? e.InnerException : e;

            _logger.LogException(String.Format("[Mail] Listening {0}:{1}/{2}. {3}", mqp.Host, mqp.Port, mqp.Vhost, e.Message), reason);

            // В случае ошибки останавливаем сервис
            _processMails = false;
        }

        private void onWarning(object o, ErrorEventArgs ea)
        {
            Exception e = ea.GetException();
            Exception reason = (e.InnerException != null) ? e.InnerException : e;

            _logger.LogWarning(String.Format("[Mail] Listening {0}:{1}/{2}. {3}", mqp.Host, mqp.Port, mqp.Vhost, e.Message), reason);
        }

        #endregion
    }
}
