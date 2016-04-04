using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Flat4Me.ImageServiceCore.Exceptions;

namespace Flat4Me.ImageServiceCore.Savers
{
    /// <summary>
    /// Преобразует и сохраняет изображение на Azure Blob.
    /// </summary>
    /// <remarks>
    /// http://azure.microsoft.com/ru-ru/documentation/articles/storage-dotnet-how-to-use-blobs/
    /// </remarks>
    internal abstract class AzureBlobImageSaver : ImageSaverBase
    {
        CloudBlobClient _blobClient;
        CloudBlobContainer _blobContainer;
        
        #region Init
        
        // Инициализирует клиента и контейнер
        protected override void Init()
        {
            ensureBlobContainer();
        }

        /// <summary>
        /// Проверяет наличие и создает в случае необходимости экземляр BlobClient
        /// </summary>
        private void ensureBlobClient()
        {
            if (_blobClient != null)
                return;

            CloudStorageAccount storageAccount;
            if(!CloudStorageAccount.TryParse(Settings.AzureBlobConnectionString, out storageAccount))
            {
                throw new Exception("Azure Blob connection string is invalid");
            }

            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        private void ensureBlobContainer()
        {
            if (_blobContainer != null)
                return;
            
            ensureBlobClient();

            _blobContainer = _blobClient.GetContainerReference(Settings.AzureBlobContainer);
            if (_blobContainer.CreateIfNotExists())
            {
                // Устанавливаем публичный доступ
                _blobContainer.SetPermissions(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
            }
        }
        
        #endregion

        /// <summary>
        /// Сохраняет изображение в заданном размере
        /// </summary>
        /// <returns>Имя файла в заданном размере</returns>
        protected override string SaveImageAs(string savePath, int imageId, uint resolution, byte[] imageData, string wmark = null)
        {
            string fileName = GetFileName(imageId, resolution);
            string blobPath = String.Format("{0}/{1}", savePath, fileName);

            // TODO: Block blob vs Page blob - investigate
            CloudBlockBlob imageBlob = _blobContainer.GetBlockBlobReference(blobPath);
            using(MemoryStream resizedImage = new MemoryStream())
            {
                // JPEG format only
                Exception jobException;
                ImageResizerWrapper irw = new ImageResizerWrapper(resolution, wmark);

                if (!irw.DoJob(new MemoryStream(imageData), resizedImage, out jobException))
                {
                    // Во время преобразования возникли ошибки
                    throw new ImageProcessingException(
                        String.Format("Image processing error. Image Id - {0}", imageId), jobException);
                }

                // Загружаем изображение в Blob
                resizedImage.Position = 0;
                imageBlob.UploadFromStream(resizedImage);
            }
            
            return fileName;
        }

        /// <summary>
        /// Возвращает относительный путь сохранения файла в формате YYYY/MM.
        /// Для Azure Blob путь хранения совпадает с subfolder.
        /// </summary>
        /// <param name="datePath">Относительный путь сохранения файла в формате YYYY/MM</param>
        /// <returns>Путь сохранения изображения</returns>
        protected override string GetSavePath(out string subfolder)
        {
            DateTime utcNow = DateTime.UtcNow;
            subfolder = String.Format("{0}/{1}", utcNow.Year, utcNow.Month);
            // Для Azure Blob путь хранения совпадает с subfolder
            return subfolder;
        }

        protected override string GetImageUriPath(string subfolder)
        {
            return String.Format(Settings.AzureBlobPublicUriPattern, Settings.AzureBlobContainer, subfolder);
        }
    }
}
