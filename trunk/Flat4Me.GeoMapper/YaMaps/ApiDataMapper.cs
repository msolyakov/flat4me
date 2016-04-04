using Flat4Me.Data.DTO;
using Flat4Me.Geo.YaMaps.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Geo.YaMaps
{
    public static class ApiDataMapper
    {
        public static ApiData.Feature Compose( AccommodationShortMainDTO info, AccommodationLocationDTO location )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Конвертирует список AccommodationLocationDTO в ApiData.
        /// </summary>
        public static ApiData AsYaMapObject(this List<AccommodationLocationDTO> locations)
        {
            ApiData.Feature[] features = new ApiData.Feature[locations.Count];

            for (int i = 0; i < locations.Count; i++)
            {
                features[i] = locations[i].AsYaMapObject();
            }

            return new ApiData()
            {
                Type = "FeatureCollection",
                Features = features
            };
        }

        /// <summary>
        /// Конвертирует список AccommodationShortMainDTO в ApiData.
        /// </summary>
        public static ApiData AsYaMapObject(this List<AccommodationShortMainDTO> accList)
        {
            ApiData.Feature[] features = new ApiData.Feature[accList.Count];

            for (int i = 0; i < accList.Count; i++)
            {
                features[i] = accList[i].AsYaMapObject();
            }

            return new ApiData()
            {
                Type = "FeatureCollection",
                Features = features
            };
        }
        
        /// <summary>
        /// Конвертирует AccommodationLocationDTO в ApiData.Feature.
        /// Для корректнрого отображения требуется обернуть ApiData.Feature в объект ApiData
        /// </summary>
        public static ApiData.Feature AsYaMapObject( this AccommodationLocationDTO location )
        {
            // Координаты
            ApiData.Geometry2 g1 = new ApiData.Geometry2()
            {
                Type = "Point",
                Coordinates = new double[] { location.PointY, location.PointX }
            };
            // Описание
            ApiData.Properties2 p1 = new ApiData.Properties2()
            {
                BalloonContent = location.FullAddress,
                HintContent = location.FullAddress,              
                LocationId = location.LocationId,
                Distance = ( location.Distance != null) ? location.Distance.Distance : 0
            };            

            return new ApiData.Feature() 
            { 
                LocationId = location.LocationId,
                Type = "Feature", 
                Geometry = g1, 
                Properties = p1 
            };
        }

        /// <summary>
        /// Конвертирует AccommodationShortMainDTO в ApiData.Feature.
        /// Для корректнрого отображения требуется обернуть ApiData.Feature в объект ApiData
        /// </summary>
        public static ApiData.Feature AsYaMapObject(this AccommodationShortMainDTO acc)
        {
            // Координаты
            ApiData.Geometry2 g1 = new ApiData.Geometry2()
            {
                Type = "Point",
                Coordinates = new double[] { acc.PointY, acc.PointX }
            };
            // Описание
            ApiData.Properties2 p1 = new ApiData.Properties2()
            {
                BalloonContent = acc.FullAddress,
                HintContent = acc.FullAddress,
                LocationId = acc.LocationId,
                Distance = (acc.Distance != null) ? acc.Distance.Distance : 0
            };

            return new ApiData.Feature()
            {
                LocationId = acc.LocationId,
                Type = "Feature",
                Geometry = g1,
                Properties = p1
            };
        }
    }
}
