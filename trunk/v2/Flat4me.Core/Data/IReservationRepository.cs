using Flat4me.Core.Data.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flat4me.Core.Data
{
    public interface IReservationRepository
    {
        Task Add(ReservationDTO item);
        Task Cancel(int reservationId, bool isCanceled);        
        Task Destroy(int reservationId);
        Task<List<ReservationDTO>> GetList(int accommodationId, DateTime? dateArrivalStart = null);
        Task<ReservationDTO> Get(int accommodationId, int userId);
    }
}
