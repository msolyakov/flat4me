using Dapper;
using Flat4me.Core.Data.Objects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.MsSql
{
    public class HotelierProfileRepository : BaseRepository, IHotelierProfileRepository
    {
        #region Stored procedures methods

        private Task sp_Short_HotelierProfile_Add(HotelierProfileDTO item, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_HotelierProfile_Add";

            var p = new DynamicParameters();
            p.Add("@UserId", item.UserId);
            p.Add("@CityId", item.CityId);
            p.Add("@IsApproved", item.IsApproved);

            p.Add("@CheckinFrom", item.CheckinFrom);
            p.Add("@CheckinTo", item.CheckinTo);
            p.Add("@CheckoutFrom", item.CheckoutFrom);
            p.Add("@CheckoutTo", item.CheckoutTo);

            p.Add("@HasAirportTransfer", item.HasAirportTransfer);
            p.Add("@EstimatedAirportTransferCost", item.EstimatedAirportTransferCost);
            p.Add("@HasTrainTransfer", item.HasTrainTransfer);
            p.Add("@EstimatedTrainTransferCost", item.EstimatedTrainTransferCost);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Short_HotelierProfile_Update(HotelierProfileDTO item, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_HotelierProfile_Update";

            var p = new DynamicParameters();
            p.Add("@UserId", item.UserId);

            p.Add("@CheckinFrom", item.CheckinFrom);
            p.Add("@CheckinTo", item.CheckinTo);
            p.Add("@CheckoutFrom", item.CheckoutFrom);
            p.Add("@CheckoutTo", item.CheckoutTo);

            p.Add("@HasAirportTransfer", item.HasAirportTransfer);
            p.Add("@EstimatedAirportTransferCost", item.EstimatedAirportTransferCost);
            p.Add("@HasTrainTransfer", item.HasTrainTransfer);
            p.Add("@EstimatedTrainTransferCost", item.EstimatedTrainTransferCost);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Short_HotelierProfile_Delete(int userId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_HotelierProfile_Delete";
            var p = new DynamicParameters();
            p.Add("@UserId", userId);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task<HotelierProfileDTO> sp_Short_HotelierProfile_Get(int userId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_HotelierProfile_Get";
            var p = new DynamicParameters();
            p.Add("@UserId", userId);

            return (await conn.QueryAsync<HotelierProfileDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private Task<IEnumerable<HotelierProfileUnapprovedDTO>> sp_Short_HotelierProfile_GetUnapprovedList(SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_HotelierProfile_GetUnapprovedList";

            return conn.QueryAsync<HotelierProfileUnapprovedDTO>(
                sql: spName, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private Task sp_Short_HotelierProfile_Approve(int userId, bool approve, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_HotelierProfile_Approve";

            var p = new DynamicParameters();
            p.Add("@UserId", userId);
            p.Add("@IsApproved", approve);

            return conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }

        #endregion

        public async Task Add(HotelierProfileDTO item)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Short_HotelierProfile_Add(item, conn);
            }
        }
        public async Task Update(HotelierProfileDTO item)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Short_HotelierProfile_Update(item, conn);
            }
        }
        public async Task Delete(int userId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Short_HotelierProfile_Delete(userId, conn);
            }
        }
        public async Task<HotelierProfileDTO> Get(int userId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                return await sp_Short_HotelierProfile_Get(userId, conn);
            }
        }

        public async Task<IEnumerable<HotelierProfileUnapprovedDTO>> GetUnapprovedList()
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                return await sp_Short_HotelierProfile_GetUnapprovedList(conn);
            }
        }
        public async Task Approve(int userId, bool approve)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Short_HotelierProfile_Approve(userId, approve, conn);
            }
        }
    }
}
