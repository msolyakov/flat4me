using Flat4me.Core.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.Extensions
{
    public static class SearchExtensions
    {
        /// <summary>
        /// Extention. Фильтрует список квартир по заданным параметрам.
        /// </summary>
        /// <param name="list">Список квартир для фильтрации</param>
        /// <param name="minPrice">Минимальная цена</param>
        /// <param name="maxPrice">Максимальная цена</param>
        /// <param name="guestsCount">Количество гостей</param>
        /// <param name="duration">Длительность проживания в днях</param>
        /// <returns></returns>
        public static async Task<List<AccommodationShortMainDTO>> FilterByParams(this List<AccommodationShortMainDTO> list, int? minPrice, int? maxPrice, ushort? guestsCount = null, ushort? duration = null)
        {
            if (list == null)
                return null;

            // Если параметры поиска не заданы, возвращаем исходый список
            if (minPrice == null && maxPrice == null && guestsCount == null && duration == null)
                return list;

            return await Task.Run(() =>
            {
                // Готовим значения
                int minPriceValue = (minPrice != null) ? minPrice.Value : 0;
                int maxPriceValue = (maxPrice != null) ? maxPrice.Value : Int32.MaxValue;
                ushort guestsCountValue = (guestsCount != null) ? guestsCount.Value : (ushort)1;
                ushort durationValue = (duration != null) ? duration.Value : ushort.MaxValue;
                
                // Фильтруем список
                return (from acc in list
                        where (minPriceValue <= acc.MinPriceAmount && acc.MinPriceAmount <= maxPriceValue)
                        //RETIRED && (guestsCountValue <= acc.MaxGuestsCount) && (acc.MinDuration <= durationValue) retired
                        select acc).ToList();
            });
        }
    }
}
