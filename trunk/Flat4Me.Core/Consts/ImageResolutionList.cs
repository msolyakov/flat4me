using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Core.Consts
{
    public class ImageResolutionList
    {
        //[Obsolete("Use FLAT_PHOTO_LARGE instead.")]
        //public const int LargeResolution = 1280;
        //[Obsolete("Use FLAT_PHOTO_MEDIUM instead.")]
        //public const int MediumResolution = 800;
        //[Obsolete("Use FLAT_PHOTO_SMALL instead.")]
        //public const int SmallResolution = 640;
        //[Obsolete("Use FLAT_PHOTO_TINY instead.")]
        //public const int TinyResolution = 320;

        // 1. Размер фотографий квартир

        /// <summary>
        /// 1280 px
        /// </summary>
        public const int FLAT_PHOTO_LARGE = 1280;
        /// <summary>
        /// 800 px
        /// </summary>
        public const int FLAT_PHOTO_MEDIUM = 800;
        /// <summary>
        /// 640 px
        /// </summary>
        public const int FLAT_PHOTO_SMALL = 640;
        /// <summary>
        /// 320 px
        /// </summary>
        public const int FLAT_PHOTO_TINY = 320;

        // 2. Размер фотографий пользователей

        /// <summary>
        /// 225 px
        /// </summary>
        public const int USER_PHOTO_SMALL = 225;
        /// <summary>
        /// 50 px
        /// </summary>
        public const int USER_PHOTO_TINY = 50;
    }
}
