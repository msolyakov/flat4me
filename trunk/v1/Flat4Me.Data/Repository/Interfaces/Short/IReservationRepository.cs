using Flat4Me.Data.DTO.Short;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.Repository.Interfaces.Short
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
