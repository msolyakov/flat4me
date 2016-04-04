using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Flat4Me.ImageServiceCore;
using Flat4Me.Core.Rabbit;
using Flat4Me.Data.Repository.Azure;
using System.IO;

namespace Flat4Me.ImageWorkerRole.UserPhoto
{
    public class WorkerRole : RoleEntryPoint
    {
        ImageQueueProcessor iqp;
        private bool _processImages = false;
        private StorageLogRepository _logger;

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
                _logger.LogInfo("[UserPhoto] Starting...");
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
                    RabbitFacadeConsts.QUEUE_IMAGE_USER, ImageStorageType.UserPhotoAzure);

                // Подписываемся на события ошибок и предупреждений
                iqp.ProcessingError += onError;
                iqp.ProcessingWarning += onWarning;

                _logger.LogInfo(String.Format("[UserPhoto] Listening {0}:{1}/{2}", iqp.Host, iqp.Port, iqp.Vhost));

                // Запускаем обработку
                iqp.DoProcessMessages(ref _processImages);
            });

            _logger.LogInfo("[UserPhoto] Running");
            _processorTask.Wait(); 
        }

        public override void OnStop()
        {
            _processImages = false;
            Thread.Sleep(2000);
            base.OnStop();

            _logger.LogInfo("[UserPhoto] Stopped");
        }

        #region Error/Warning handlers

        private void onError(object o, ErrorEventArgs ea)
        {
            Exception e = ea.GetException();
            Exception reason = (e.InnerException != null) ? e.InnerException : e;

            _logger.LogException(String.Format("[UserPhoto] Listening {0}:{1}/{2}. {3}", iqp.Host, iqp.Port, iqp.Vhost, e.Message), reason);

            // В случае ошибки останавливаем сервис
            _processImages = false;
        }

        private void onWarning(object o, ErrorEventArgs ea)
        {
            Exception e = ea.GetException();
            Exception reason = (e.InnerException != null) ? e.InnerException : e;

            _logger.LogWarning(String.Format("[UserPhoto] Listening {0}:{1}/{2}. {3}", iqp.Host, iqp.Port, iqp.Vhost, e.Message), reason);
        }

        #endregion

    }
}
