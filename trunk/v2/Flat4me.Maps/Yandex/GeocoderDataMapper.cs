using Flat4me.Core.Data.Objects;
using Flat4me.Maps.Yandex.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Maps.Yandex
{
    public static class GeocoderDataMapper
    {
        public static List<AccommodationLocationDTO> AsLocationList(this GeocoderResponse response, int accommodationId)
        {
            List<AccommodationLocationDTO> list = new List<AccommodationLocationDTO>();

            try
            {
                var locations = response.Response.GeoObjectCollection.FeatureMember;
                foreach (GeocoderResponse.FeatureMember2 fm in locations)
                {
                    var precision = fm.GeoObject.MetaDataProperty.GeocoderMetaData.Precision;
                    // Интересует только точное соотвествие, если такое имеется в коллекции
                    if (precision != YandexGeocoder.YAMAPS_PRECISION_EXACT
                        && locations.Any(p => p.GeoObject.MetaDataProperty.GeocoderMetaData.Precision == YandexGeocoder.YAMAPS_PRECISION_EXACT))
                        continue;

                    // Парсим полученные координаты
                    double pointY, pointX;
                    if (!GeoHelper.TryParsePos(fm.GeoObject.Point.Pos, out pointY, out pointX))
                        continue;

                    list.Add(new AccommodationLocationDTO()
                    {
                        AccommodationId = accommodationId,
                        FullAddress = fm.GeoObject.MetaDataProperty.GeocoderMetaData.AddressDetails.Country.AddressLine,
                        PointY = pointY,
                        PointX = pointX,
                        IsConfirmed = false,
                        IsDeleted = false
                    });
                }
            }
            catch
            {
                return null;
            }

            return list;
        }
    }
}
