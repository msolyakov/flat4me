using Flat4Me.Core.Rabbit;
using Flat4Me.ImageServiceCore.Savers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.ImageServiceCore
{
    /// <summary>
    /// Класс, получающий из заданной очереди изображения и сохраняющий их в заданном хранилище, в зависимости от настроек.
    /// </summary>
    public class ImageQueueProcessor : QueueProcessorBase
    {
        IImageSaver _saver;
        
        public ImageQueueProcessor(string queueName, ImageStorageType storageType)
            : base(queueName)
        {
            StorageType = storageType;
        }

        /// <summary>
        /// Тип хранилища для записи изображений
        /// </summary>
        public ImageStorageType StorageType
        {
            get;
            set;
        }

        protected override bool ProcessMessage(ContentType ctype, IDictionary<string, object> headers, byte[] msgbody, bool redelivered, out string errorMessage)
        {
            errorMessage = null;

            // Проверям ContentType
            if (ctype == null || ctype.MediaType != MediaTypeNames.Image.Jpeg)
            {
                // Не задан или неверный ContentType. Игнорируем сообщение.
                errorMessage = "Invalid Content Type for image message.";
                return false;
            }

            // Сохраняем изображение
            return saveImage(headers, msgbody, out errorMessage);
        }

        /// <summary>
        /// Сохраняет изображение.
        /// </summary>
        /// <param name="imageHeaders">Заголовок сообщения. Обязательный параметр - ImageID</param>
        /// <param name="imageData">Данные рисунка</param>
        /// <returns>True, если рисунок успешно сохранен</returns>
        private bool saveImage(IDictionary<string, object> imageHeaders, byte[] imageData, out string errorMessage)
        {
            errorMessage = null;
           
            // Загружаем модуль сохранения здесь, чтобы иметь возможность 
            // задать значение StorageType до вызова ProcessMessage
            this.ensureImageSaver();
            
            if (imageData == null || imageData.LongLength == 0)
            {
                errorMessage = "Image Data is null or empty.";
                return false;
            }

            if (imageHeaders == null ||
                !imageHeaders.Keys.Contains(RabbitFacadeConsts.HEADER_IMAGE_ID) ||
                !(imageHeaders[RabbitFacadeConsts.HEADER_IMAGE_ID] is Int32))
            {
                errorMessage = "ImageID is invalid, or Headers object is null.";
                return false;
            }

            Int32 imageID = (Int32)imageHeaders[RabbitFacadeConsts.HEADER_IMAGE_ID];
            return _saver.SaveImage(imageID, imageData, out errorMessage);
        }

        /// <summary>
        /// Загружаем модуль сохранения. 
        /// </summary>
        private void ensureImageSaver()
        {
            if (_saver == null)
            {
                // Выбираем способ сохранения через фабрику.
                IImageSaver s = ImageSaverFactory.CreateSaver(StorageType);
                if (s == null)
                {
                    throw new NotImplementedException(
                        String.Format("ImageQueueProcessor: Storage Type \"{0}\" not supported.", StorageType));
                }

                _saver = s;
            }
        }
    }
}
