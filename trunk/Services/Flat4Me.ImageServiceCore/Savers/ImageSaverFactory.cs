using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.ImageServiceCore.Savers
{
    /// <summary>
    /// Фабрика классов, реализующих различные механизмы сохранения изображений.
    /// </summary>
    internal static class ImageSaverFactory
    {
        /// <summary>
        /// Создает экземпляр класса сохранения изображения, в зависимости от переданного параметра.
        /// </summary>
        /// <returns></returns>
        public static IImageSaver CreateSaver(ImageStorageType type)
        {
            IImageSaver result;

            switch (type)
            {
                case ImageStorageType.FlatPhotoDisk:
                    result = new DefaultDiskSaver();
                    break;
                case ImageStorageType.FlatPhotoAzure:
                    result = new FlatPhotoSaver();
                    break;
                case ImageStorageType.UserPhotoAzure:
                    result = new UserPhotoSaver();
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}
