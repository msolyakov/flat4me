using System;

namespace Flat4Me.ImageServiceCore
{ 
    /// <summary>
    /// Поддерживаемые типы хранилищ для изображений
    /// </summary>
    [Serializable]
    public enum ImageStorageType    
    {
        FlatPhotoDisk = 0, 
        FlatPhotoAzure = 1,
        UserPhotoAzure = 2
    }
}