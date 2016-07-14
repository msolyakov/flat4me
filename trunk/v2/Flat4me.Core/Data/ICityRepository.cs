using Flat4me.Core.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data
{
    public interface ICityRepository
    {
        Task<List<CityDTO>> Find(string name);
        Task<CityDTO> GetByUrl(string url);
        Task<IEnumerable<CityDTO>> GetAll();
    }
}
