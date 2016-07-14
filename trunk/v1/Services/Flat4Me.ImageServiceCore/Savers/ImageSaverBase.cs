using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat4Me.Core.Consts;
using Flat4Me.Data.Repository.MsSql;
using Flat4Me.Data.DTO;
using Flat4Me.ImageServiceCore.Exceptions;

namespace Flat4Me.ImageServiceCore.Savers
{
    /// <summary>
    /// Базовый модуль сохранения, реализующий сохранение путей к изображениям в БД
    /// </summary>
    internal abstract class ImageSaverBase : IImageSaver
    {
        private ImageServiceSettings _settings;
        private List<uint> _sizeList;

        public ImageSaverBase()
        {
            _settings = ImageServiceSettings.Default;
            _sizeList = CreateSizeList();
        }

        /// <summary>
        /// Настройки ImageServiceSettings, запрошенные из конфига
        /// </summary>
        protected ImageServiceSettings Settings
        {
            get{ return _settings; }
        }

        /// <summary>
        /// Список размеров в пикселах, до который нужно преобразовывать изображения
        /// </summary>
        protected List<uint> SizeList 
        {
            get { return _sizeList; }
        }
        
        // Инициализирует модуль перед сохранением изображения 
        protected virtual void Init()
        {
        }

        /// <summary>
        /// Сохраняет 4 размера изображения и пути к ним в БД
        /// </summary>
        public bool SaveImage(int imageId, byte[] imageData, out string errorMessage)
        {
            errorMessage = String.Empty;
            bool result = true;

            try
            {
                Init();
                
                string subfolder;
                string savePath = GetSavePath(out subfolder);
                string uriPath = GetImageUriPath(subfolder);

                Dictionary<uint, string> sizeUriList = new Dictionary<uint, string>();

                // Сохраняем файлы:
                foreach (uint size in SizeList)
                {
                    string sizeFileName = SaveImageAs(savePath, imageId, size, imageData);
                    sizeUriList.Add(size, String.Format("{0}/{1}", uriPath, sizeFileName));
                }

                // Запись информации о путях хранения в БД. Ждем результата
                SaveImageUri(imageId, sizeUriList);
            }
            catch(ImageProcessingException e)
            {
                errorMessage = String.Format("{0}: {1}.\r\nStack trace:\r\n{2}",
                    e.GetType().FullName, e.Message, e.StackTrace);
                
                if (e.InnerException != null)
                {
                    errorMessage += String.Format("\r\nInner exception:\r\n{0}: {1}.\r\nStack trace:\r\n{2}",
                        e.InnerException.GetType().FullName, e.InnerException.Message, e.InnerException.StackTrace);
                }
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Создает список размеров, в которые будет конвертировать поступающее фото данный экземпляр сервиса.
        /// </summary>
        /// <returns></returns>
        protected abstract List<uint> CreateSizeList();

        /// <summary>
        /// Сохраняет изображение в заданном размере
        /// </summary>
        /// <returns>Имя файла в заданном размере</returns>
        protected abstract string SaveImageAs(string savePath, int imageId, uint resolution, byte[] imageData, string wmark = null);

        /// <summary>
        /// Возвращает путь (абсолютный или относительный) сохранения файла.
        /// Результат передается в метод SaveImageAs в качестве параметра.
        /// </summary>
        /// <param name="datePath">Out-параметр, автоматически созданная внутри SavePath подпапка для сохранения файла</param>
        /// <returns>Путь сохранения изображения</returns>
        protected abstract string GetSavePath(out string subfolder);

        protected abstract string GetImageUriPath(string subfolder);

        /// <summary>
        /// Реализация должна содержать код сохранения информации о путях
        /// </summary>
        /// <param name="imageId"></param>
        /// <param name="uriPath"></param>
        /// <param name="sizeFileNames"></param>
        protected abstract void SaveImageUri(int imageId, Dictionary<uint, string> uris);

        /*
        /// <summary>
        /// Сохраняет пути к изображению в БД.
        /// </summary>
        protected void SaveImageUri(int imageId, string uriPath, string largeFileName, string mediumFileName, string smallFileName, string tinyFileName)
        {
            PhotoDTO photo = new PhotoDTO();

            photo.PhotoId = imageId;
            photo.LargePath = String.Format(String.Format("{0}/{1}", uriPath, largeFileName));
            photo.MediumPath = String.Format(String.Format("{0}/{1}", uriPath, mediumFileName));
            photo.SmallPath = String.Format(String.Format("{0}/{1}", uriPath, smallFileName));
            photo.TinyPath = String.Format(String.Format("{0}/{1}", uriPath, tinyFileName));

            List<PhotoDTO> photoList = new List<PhotoDTO>();
            photoList.Add(photo);

            try
            {
                PhotoRepository rep = new PhotoRepository();
                rep.Update(photoList).Wait();
            }
            catch (AggregateException ae)
            {
                foreach (Exception ie in ae.Flatten().InnerExceptions)
                {
                    if (!(ie is OperationCanceledException))
                        throw ie;
                }
            }
        }
         */

        protected string GetFileName(int imageId, uint resolution)
        {
            return String.Format("img{0}-{1}.jpg", imageId, resolution);
        }
    }
}
