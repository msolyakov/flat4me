using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flat4Me.ImageServiceCore;
using Flat4Me.Core.Rabbit;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Flat4Me.Tests
{
    [TestClass]
    public class ImageServiceTests
    {
        bool _processFlatPhoto = true;

        [TestMethod]
        public void Image_SaveFlatPhoto()
        {
            // Запускаем асинхронно обработку изображений.
            Task _processorTask = Task.Run(() =>
            {
                ImageQueueProcessor iqp = new ImageQueueProcessor(
                    RabbitFacadeConsts.QUEUE_IMAGE_MAIN, ImageStorageType.FlatPhotoAzure);

                // Подписываемся на события ошибок и предупреждений
                iqp.ProcessingError += onError;
                iqp.ProcessingWarning += onWarning;

                // Запускаем обработку
                iqp.DoProcessMessages(ref _processFlatPhoto);
            });

            // гоняем тест 5 минут 
            Thread.Sleep(300000);
            _processFlatPhoto = false;
            _processorTask.Wait(); 
        }

        private void onError(object o, ErrorEventArgs ea)
        {
            Exception e = ea.GetException();
            Exception reason = (e.InnerException != null) ? e.InnerException : e;

            string message = String.Format("{0}: {1} at \r\n{2}", reason.GetType().FullName, reason.Message, reason.StackTrace);
            Assert.Fail(message);

            _processFlatPhoto = false;
        }

        private void onWarning(object o, ErrorEventArgs ea)
        {
            Exception e = ea.GetException();
            Exception reason = (e.InnerException != null) ? e.InnerException : e;

            string message = String.Format("{0}: {1} at \r\n{2}", reason.GetType().FullName, reason.Message, reason.StackTrace);
            Assert.Fail(message);

            _processFlatPhoto = false;
        }
    }
}
