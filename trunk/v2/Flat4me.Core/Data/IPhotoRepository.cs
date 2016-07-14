using Flat4me.Core.Data.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flat4me.Core.Data
{
    public interface IPhotoRepository
    {
        Task<List<PhotoDTO>> GetList(int accommodationId);
        Task<PhotoDTO> GetPrimary(int accommodationId);

        Task Add(int accommodationId, IEnumerable<PhotoDTO> photoList);
        Task Update(IEnumerable<PhotoDTO> photoList);
        Task SetPrimary(int photoId);
        Task Approve(int photoId, bool approve);

        Task Delete(IEnumerable<int> photoIdList);
        Task Destroy(IEnumerable<int> photoIdList);
    }
}
