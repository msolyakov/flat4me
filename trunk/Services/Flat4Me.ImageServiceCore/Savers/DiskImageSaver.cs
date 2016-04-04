using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat4Me.Core.Consts;
using Flat4Me.Data.Repository.MsSql;
using Flat4Me.Data.DTO;
using System.IO;
using Flat4Me.ImageServiceCore.Exceptions;

namespace Flat4Me.ImageServiceCore.Savers
{
    /// <summary>
    /// Реализует механизм преобразования и сохранения полученного изображения на диск сервера.
    /// </summary>
    internal abstract class DiskImageSaver : ImageSaverBase
    {
        /// <summary>
        /// Сохраняет изображение в заданном размере
        /// </summary>
        /// <returns>Имя файла в заданном размере</returns>
        protected override string SaveImageAs(string savePath, int imageId, uint resolution, byte[] imageData, string wmark = null)
        {
            string fileName = GetFileName(imageId, resolution);
            string fullFileName = String.Format("{0}\\{1}", savePath, fileName);

            if (File.Exists(fullFileName) && Settings.ReplaceExistingImage)
            {
                File.Delete(fullFileName);
            }

            if (!File.Exists(fullFileName))
            {
                using (FileStream imageFile = new FileStream(fullFileName, FileMode.CreateNew))
                {
                    // JPEG format only
                    Exception jobException;
                    ImageResizerWrapper irw = new ImageResizerWrapper(resolution, wmark);

                    bool ok = irw.DoJob(new MemoryStream(imageData), imageFile, out jobException);
                    imageFile.Close();

                    if (!ok)
                    {
                        // Во время преобразования возникли ошибки
                        throw new ImageProcessingException(
                            String.Format("Image processing error. Image Id - {0}", imageId), jobException);
                    }
                }
            }

            return fileName;
        }

        /// <summary>
        /// Возвращает путь для сохранения файлов на основе шаблона, заданного в конфигурации.
        /// Создает и возвращает в параметре подпапку для хранения в формате YYYY-MM-DD.
        /// </summary>
        /// <returns>Путь для сохранения файла</returns>
        protected override string GetSavePath(out string subfolder)
        {
            DateTime utcNow = DateTime.UtcNow;
            subfolder = String.Format("{0}-{1}-{2}", utcNow.Year, utcNow.Month, utcNow.Day);

            string filePath = ImageServiceSettings.Default.ImageStorageDiskPath;
            filePath = String.Format(!filePath.EndsWith("\\") ? "{0}\\{1}" : "{0}{1}", filePath, subfolder);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            return filePath;
        }

        protected override string GetImageUriPath(string subfolder)
        {
            return String.Format(Settings.ImageUriPattern, subfolder);
        }
    }
}
