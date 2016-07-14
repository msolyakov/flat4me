using Flat4me.Core.Data.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flat4me.Core.Data
{
    public interface IHotelierProfileRepository
    {
        Task Add(HotelierProfileDTO item);
        Task Update(HotelierProfileDTO item);
        Task Delete(int userId);
        Task<HotelierProfileDTO> Get(int userId);
        Task<IEnumerable<HotelierProfileUnapprovedDTO>> GetUnapprovedList();
        Task Approve(int userId, bool approve);
    }
}
