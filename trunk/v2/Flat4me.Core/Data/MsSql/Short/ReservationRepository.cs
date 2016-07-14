using Dapper;
using Flat4me.Core.Data.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.MsSql
{
    public class ReservationRepository : BaseRepository, IReservationRepository
    {
        #region Stored procedures methods

        private async Task<int> sp_Short_Reservation_Add(ReservationDTO reservation, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Reservation_Add";

            var p = new DynamicParameters();
            p.Add("@AccommodationId", reservation.AccommodationId);
            p.Add("@UserId", reservation.UserId);
            p.Add("@CreatedOnUtc", reservation.CreatedOnUtc);
            p.Add("@Checkin", reservation.Checkin);
            p.Add("@Checkout", reservation.Checkout);
            p.Add("@Guests", reservation.Guests);
            p.Add("@EstimatedAmount", reservation.EstimatedAmount);
            p.Add("@Children", reservation.Children);

            return (await conn.QueryAsync<int>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }
        private async Task sp_Short_Reservation_Cancel(int reservationId, bool isCanceled, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Reservation_Cancel";

            var p = new DynamicParameters();
            p.Add("@ReservationId", reservationId);
            p.Add("@IsCanceled", isCanceled);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task sp_Short_Reservation_Destroy(int reservationId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Reservation_Destroy";
            var p = new DynamicParameters();
            p.Add("@ReservationId", reservationId);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task<List<ReservationDTO>> sp_Short_Reservation_GetList(int accommodationId, SqlConnection conn, DateTime? checkinStart = null, SqlTransaction tran = null)
        {
            IEnumerable<ReservationDTO> items = null;

            var spName = "cmn.sp_Short_Reservation_GetList";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", accommodationId);
            p.Add("@CheckinStart", checkinStart);

            items = await conn.QueryAsync<ReservationDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }
        private async Task<ReservationDTO> sp_Short_Reservation_GetByUser(int accommodationId, int userId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Reservation_GetByUser";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", accommodationId);
            p.Add("@UserId", userId);
            p.Add("@Now", DateTime.Now);// return only future and current reservation. (Checkout >= @Now)

            return (await conn.QueryAsync<ReservationDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();
        }

        #endregion

        public async Task Add(ReservationDTO item)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                item.ReservationId = await sp_Short_Reservation_Add(item, conn);
            }
        }
        public async Task Cancel(int reservationId, bool isCanceled)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Short_Reservation_Cancel(reservationId, isCanceled, conn);
            }
        }
        public async Task Destroy(int reservationId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Short_Reservation_Destroy(reservationId, conn);
            }
        }
        public async Task<List<ReservationDTO>> GetList(int accommodationId, DateTime? checkinStart = null)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                return await sp_Short_Reservation_GetList(accommodationId, conn, checkinStart);
            }
        }

        public async Task<ReservationDTO> Get(int accommodationId, int userId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                return await sp_Short_Reservation_GetByUser(accommodationId, userId, conn);
            }
        }
    }
}
