using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using Flat4Me.ImageServiceCore;
using Flat4Me.Core.Rabbit;
using System.IO;
using System.Threading;

namespace Flat4Me.ImageService
{
    public partial class MainImageService : ServiceBase
    {
        private System.Diagnostics.EventLog _eventLog;
        private bool _processImages = false;
        private Task _processorTask = null;

        #region Init, Start/Stop

        public MainImageService()
        {
            InitializeComponent();
            _eventLog = new EventLog(ProjectInstaller.EVENT_LOG_NAME);
            _eventLog.Source = "MainImageService";
        }

        protected override void OnStart(string[] args)
        {
            _processImages = true;
            // Запускаем асинхронно обработку изображений.
            _processorTask = Task.Run(() =>
            {
                ImageQueueProcessor iqp = new ImageQueueProcessor(RabbitFacadeConsts.QUEUE_IMAGE_MAIN, ImageStorageType.FlatPhotoDisk);
                
                // Подписываемся на события ошибок и предупреждений
                iqp.ProcessingError += onError;
                iqp.ProcessingWarning += onWarning;
                
                // Запускаем обработку
                iqp.DoProcessMessages(ref _processImages);
            });
            _eventLog.WriteEntry("Image Processing Service was started.", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            _processImages = false;
            // Ожидаем завершения процесса обработки изображений
            if (_processorTask != null)
            {
                _processorTask.Wait();
            }
            Thread.Sleep(1000);
            _eventLog.WriteEntry("Image Processing Service was stopped.", EventLogEntryType.Information);
        }

        #endregion

        #region Error/Warning handlers

        private void onError(object o, ErrorEventArgs ea)
        {
            if (_eventLog != null)
            {
                Exception e = ea.GetException();
                Exception reason = (e.InnerException != null) ? e.InnerException : e;

                _eventLog.WriteEntry(String.Format("{0}\r\nReason:\r\n{1} : {2} \r\n{3}", e.Message, 
                    reason.GetType().FullName, reason.Message, reason.StackTrace), EventLogEntryType.Error);
            }
            
            // В случае ошибки останавливаем сервис
            this.Stop();
        }

        private void onWarning(object o, ErrorEventArgs ea)
        {
            if (_eventLog != null)
            {
                Exception e = ea.GetException();
                Exception reason = (e.InnerException != null) ? e.InnerException : e;

                _eventLog.WriteEntry(String.Format("{0}\r\nReason:\r\n{1} : {2} \r\n{3}", e.Message,
                    reason.GetType().FullName, reason.Message, reason.StackTrace), EventLogEntryType.Warning);
            }
        }

        #endregion
    }
}
