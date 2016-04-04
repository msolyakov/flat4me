using Flat4Me.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Data.Repository.Interfaces
{
    public interface ICityRepository
    {
        Task<List<CityDTO>> Find(string name);
        Task<CityDTO> GetByUrl(string url);
        Task<IEnumerable<CityDTO>> GetAll();
    }
}
