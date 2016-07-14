using Flat4Me.Data.DTO;
using Flat4Me.Data.DTO.Short;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flat4Me.Data.Repository.Interfaces
{
    public interface IAccommodationRepository
    {
        /// <summary>
        /// Only published accommodation list by city.
        /// </summary>
        /// <returns></returns>
        Task<List<AccommodationShortMainDTO>> MainList(int cityId);

        /// <summary>
        /// Published accommodation list by city, by region.
        /// </summary>
        Task<List<AccommodationShortMainDTO>> MainList(int cityId, double lowerLeftY, double lowerLeftX, double upperRightY, double upperRightX);
        
        /// <summary>
        /// Accommodation list for user.
        /// </summary>
        Task<List<AccommodationShortMainDTO>> MyList(int userId);
        
        /// <summary>
        /// Accommodation with system draft flag. Main idea is return AccommodationId.
        /// If creating will not complete, system should delete it.
        /// </summary>        
        /// <returns>Ready item.</returns>
        Task<AccommodationShortDTO> AddDraft(int userId);
        /// <summary>
        /// Complete creating
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task CompleteDraft(AccommodationShortDTO item, IEnumerable<PriceDTO> priceList);
        /// <summary>
        /// Make visible for booking
        /// </summary>
        /// <param name="accommodationId"></param>
        /// <param name="publish"></param>
        /// <returns></returns>
        Task Publish(int accommodationId, bool publish);
        /// <summary>
        /// Moderate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="publish"></param>
        /// <returns></returns>
        Task Approve(int id, bool approve);
        Task<AccommodationShortDTO> Get(int accommodationId);
        Task<List<PriceDTO>> GetPriceList(int accommodationId);
        /// <summary>
        /// Updates accommodation.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task Update(AccommodationShortDTO item);
        /// <summary>
        /// Updates accommodation. 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="priceListAdd">New prices. If null then take no effect</param>
        /// <param name="priceListUpdate">Should have PriceId. If null then take no effect</param>
        /// <returns></returns>
        Task Update(AccommodationShortDTO item, IEnumerable<PriceDTO> priceListAdd, IEnumerable<PriceDTO> priceListUpdate);
        /// <summary>
        /// Mark as deleted
        /// </summary>        
        /// <returns></returns>
        Task Delete(int accommodationId, bool delete);
        /// <summary>
        /// Completly delete (remove from db) all accommodation data. Include Prices, Photos, Reservations.
        /// </summary>
        /// <param name="accommodationId"></param>
        Task DeleteCompletly(int accommodationId);
    }
}
