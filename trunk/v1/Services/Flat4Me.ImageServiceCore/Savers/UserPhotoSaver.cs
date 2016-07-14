using Flat4Me.Core.Consts;
using Flat4Me.Data.DTO.Auth;
using Flat4Me.Data.Repository.MsSql.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.ImageServiceCore.Savers
{
    internal class UserPhotoSaver : AzureBlobImageSaver
    {
        /// <summary>
        /// Создает список сохраняемых размеров фотографий пользователей.
        /// </summary>
        /// <returns></returns>
        protected override List<uint> CreateSizeList()
        {
            return new List<uint>() 
            { 
                ImageResolutionList.USER_PHOTO_SMALL,  
                ImageResolutionList.FLAT_PHOTO_TINY
            };
        }

        /// <summary>
        /// Сохраняет пути к изображению в БД.
        /// </summary>
        /// <param name="imageId">Id объекта, для которого сохраняются пути изображений</param>
        /// <param name="uris">Список путей к сохраненным изображениям</param>
        protected override void SaveImageUri(int imageId, Dictionary<uint, string> uris)
        {
            UserDTO user = new UserDTO();
            user.UserId = imageId;

            user.PhotoSmallPath = uris[ImageResolutionList.USER_PHOTO_SMALL];
            user.PhotoTinyPath = uris[ImageResolutionList.USER_PHOTO_TINY];

            try
            {
                UserRepository<UserDTO> rep = new UserRepository<UserDTO>();
                rep.SetPhoto(user).Wait();
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
