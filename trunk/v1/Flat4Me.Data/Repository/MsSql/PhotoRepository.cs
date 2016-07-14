using Dapper;
using Flat4Me.Data.DTO;
using Flat4Me.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Flat4Me.Data.Repository.MsSql
{
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        #region Stored procedure methods

        private async Task<List<PhotoDTO>> sp_Photo_GetList(int accommodationId, bool onlyPrimary, SqlConnection conn, SqlTransaction tran = null)
        {
            IEnumerable<PhotoDTO> items = null;

            var spName = "cmn.sp_Photo_GetList";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", accommodationId);
            p.Add("@OnlyPrimary", onlyPrimary);

            items = await conn.QueryAsync<PhotoDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }
        private async Task sp_Photo_Add(int accommodationId, PhotoDTO photo, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Photo_Add";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", accommodationId);
            p.Add("@LargePath", photo.LargePath);
            p.Add("@MediumPath", photo.MediumPath);
            p.Add("@SmallPath", photo.SmallPath);
            p.Add("@TinyPath", photo.TinyPath);

            photo.PhotoId = (await conn.QueryAsync<int>(
                                sql: spName, param: p, transaction: tran,
                                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private async Task sp_Photo_Update(PhotoDTO photo, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Photo_Update";
            var p = new DynamicParameters();
            p.Add("@PhotoId", photo.PhotoId);
            p.Add("@LargePath", photo.LargePath);
            p.Add("@MediumPath", photo.MediumPath);
            p.Add("@SmallPath", photo.SmallPath);
            p.Add("@TinyPath", photo.TinyPath);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task sp_Photo_Approve(int photoId, bool approve, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Photo_Approve";
            var p = new DynamicParameters();
            p.Add("@PhotoId", photoId);
            p.Add("@IsApproved", approve);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task sp_Photo_SetPrimary(int photoId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Photo_SetPrimary";
            var p = new DynamicParameters();
            p.Add("@PhotoId", photoId);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }

        #endregion

        public async Task<List<PhotoDTO>> GetList(int accommodationId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                // False mean include all photos
                return await sp_Photo_GetList(accommodationId, false, conn);
            }
        }

        public async Task<PhotoDTO> GetPrimary(int accommodationId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                // True mean include only primary photo (There is only one)
                return (await sp_Photo_GetList(accommodationId, false, conn)).SingleOrDefault();
            }
        }


        public async Task Add(int accommodationId, IEnumerable<PhotoDTO> photoList)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var photo in photoList)
                        {
                            await sp_Photo_Add(accommodationId, photo, conn, tran);                            

                            if (photo.IsPrimary)
                                await sp_Photo_SetPrimary(photo.PhotoId.Value, conn, tran);

                            if (photo.IsApproved)
                                await sp_Photo_Approve(photo.PhotoId.Value, true, conn, tran);
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.RollbackSafe();
                        throw;
                    }
                }
            }
        }

        public async Task Update(IEnumerable<PhotoDTO> photoList)
        {
            var hasNullPhotoId = photoList.Any(p => p.PhotoId == null);
            if (hasNullPhotoId)
                throw new ArgumentNullException("itemList", "PhotoId shoul be setted");

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var photo in photoList)                        
                            await sp_Photo_Update(photo, conn, tran);                        

                        tran.Commit();
                    }
                    catch
                    {
                        tran.RollbackSafe();
                        throw;
                    }
                }
            }
        }

        public async Task SetPrimary(int photoId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
                await sp_Photo_SetPrimary(photoId, conn);
        }

        public async Task Approve(int photoId, bool approve)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
                await sp_Photo_Approve(photoId, approve, conn);
        }


        public async Task Delete(IEnumerable<int> photoIdList)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                using (var tran = conn.BeginTransaction())
                {
                    var spName = "cmn.sp_Photo_Delete";

                    try
                    {
                        foreach (var photoId in photoIdList)
                        {
                            var p = new DynamicParameters();
                            p.Add("@PhotoId", photoId);

                            await conn.ExecuteAsync(
                                sql: spName, param: p, transaction: tran,
                                commandType: CommandType.StoredProcedure);
                        }
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.RollbackSafe();
                        throw;
                    }
                }
            }
        }

        public async Task Destroy(IEnumerable<int> photoIdList)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                using (var tran = conn.BeginTransaction())
                {
                    var spName = "cmn.sp_Photo_Destroy";

                    try
                    {
                        foreach (var photoId in photoIdList)
                        {
                            var p = new DynamicParameters();
                            p.Add("@PhotoId", photoId);

                            await conn.OpenAsync();
                            await conn.ExecuteAsync(
                                sql: spName, param: p, transaction: tran,
                                commandType: CommandType.StoredProcedure);
                        }
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.RollbackSafe();
                        throw;
                    }
                }
            }
        }
    }
}
