using Flat4Me.Core.Consts;
using Flat4Me.Data.DTO;
using Flat4Me.Data.Repository.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.ImageServiceCore.Savers
{
    internal class FlatPhotoSaver : AzureBlobImageSaver
    {
        /// <summary>
        /// Создает список сохраняемых размеров фотографий квартиры.
        /// </summary>
        /// <returns></returns>
        protected override List<uint> CreateSizeList()
        {
            return new List<uint>() 
            { 
                ImageResolutionList.FLAT_PHOTO_LARGE,  
                ImageResolutionList.FLAT_PHOTO_MEDIUM,  
                ImageResolutionList.FLAT_PHOTO_SMALL,  
                ImageResolutionList.FLAT_PHOTO_TINY  
            };
        }
        
        /// <summary>
        /// Сохраняет пути к изображению в БД.
        /// </summary>
        protected override void SaveImageUri(int imageId, Dictionary<uint, string> uris)
        {
            PhotoDTO photo = new PhotoDTO();

            photo.PhotoId = imageId;
            photo.LargePath = uris[ImageResolutionList.FLAT_PHOTO_LARGE];
            photo.MediumPath = uris[ImageResolutionList.FLAT_PHOTO_MEDIUM];
            photo.SmallPath = uris[ImageResolutionList.FLAT_PHOTO_SMALL];
            photo.TinyPath = uris[ImageResolutionList.FLAT_PHOTO_TINY];

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
    }
}
