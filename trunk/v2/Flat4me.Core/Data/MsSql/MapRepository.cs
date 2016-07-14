using Dapper;
using Flat4me.Core.Data.Objects;
using Flat4me.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.MsSql
{
    public class MapRepository : BaseRepository, IMapRepository
    {
        /// <summary>
        /// Возвращает подтвержденное месторасположение квартиры для отображение на карте на странице квартиры.
        /// </summary>
        public async Task<AccommodationLocationDTO> GetConfirmedLocation(int accommodationId)
        {
            AccommodationLocationDTO item = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Map_GetConfirmedLocation";
                var p = new DynamicParameters();
                p.Add("@AccommodationId", accommodationId);

                await con.OpenAsync();
                item = (await con.QueryAsync<AccommodationLocationDTO>(
                    sql: spName, param: p, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }

            return item;
        }

        /// <summary>
        /// Возвращает список координат квартиры для подтверждения при редактировании.
        /// </summary>
        public async Task<List<AccommodationLocationDTO>> GetLocationList(int accommodationId)
        {
            IEnumerable<AccommodationLocationDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Map_GetLocationList";
                var p = new DynamicParameters();
                p.Add("@AccommodationId", accommodationId);

                await con.OpenAsync();
                items = await con.QueryAsync<AccommodationLocationDTO>(
                    sql: spName, param: p, commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }

        public async Task<List<AccommodationLocationDTO>> GetLocationListByLandmarkId(int landmarkId)
        {
            IEnumerable<AccommodationLocationDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Map_GetLocationListByLandmarkId";
                var p = new DynamicParameters();
                p.Add("@LandmarkId", landmarkId);

                await con.OpenAsync();
                items = await con.QueryAsync<AccommodationLocationDTO>(
                    sql: spName, param: p, commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }

        public async Task<List<AccommodationLocationDTO>> GetLocationListByRegion(double lowerLeftY, double lowerLeftX, double upperRightY, double upperRightX)
        {
            IEnumerable<AccommodationLocationDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Map_GetLocationListByRegion";
                var p = new DynamicParameters();
                p.Add("@LowerLeftY", lowerLeftY);
                p.Add("@LowerLeftX", lowerLeftX);
                p.Add("@UpperRightY", upperRightY);
                p.Add("@UpperRightX", upperRightX);

                await con.OpenAsync();
                items = await con.QueryAsync<AccommodationLocationDTO>(
                    sql: spName, param: p, commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }

        public async Task<List<AccommodationLocationDTO>> GetLocationListByCityId(int cityId)
        {
            IEnumerable<AccommodationLocationDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Map_GetLocationListByCityId";
                var p = new DynamicParameters();
                p.Add("@CityId", cityId);

                await con.OpenAsync();
                items = await con.QueryAsync<AccommodationLocationDTO>(
                    sql: spName, param: p, commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }

        public async Task<List<CityDistanceCodeDTO>> GetCityDistanceCodeList()
        {
            IEnumerable<CityDistanceCodeDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Map_GetCityDistanceCodeList";

                await con.OpenAsync();
                items = await con.QueryAsync<CityDistanceCodeDTO>(
                    sql: spName, commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }

        public async Task AddLocation(AccommodationLocationDTO location)
        {
            List<AccommodationLocationDTO> list = new List<AccommodationLocationDTO>();
            list.Add(location);
            await AddLocationList(list);
        }

        /// <summary>
        /// Добавляет список AccommodationLocation в БД
        /// </summary>
        public async Task AddLocationList(List<AccommodationLocationDTO> locations)
        {
            using (var con = new SqlConnection(base.ConnectionString))
            {
                await con.OpenAsync();
                SqlTransaction tran = null;
                try
                {
                    var spName = "cmn.sp_Map_AddLocation";
                    tran = con.BeginTransaction();

                    foreach (AccommodationLocationDTO al in locations)
                    {
                        var p = new DynamicParameters();
                        p.Add("@AccommodationId", al.AccommodationId);
                        p.Add("@FullAddress", al.FullAddress);
                        p.Add("@PointY", al.PointY);
                        p.Add("@PointX", al.PointX);

                        await con.ExecuteAsync(
                            sql: spName, param: p,
                            commandType: CommandType.StoredProcedure,
                            transaction: tran);
                    }

                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.RollbackSafe();
                    con.Close();
                    throw e;
                }
                con.Close();
            }
        }

        /// <summary>
        /// Устанавливает признак IsConfirmed для указанного AccommodationLocation.
        /// У остальных Locations для этой квартиры признак IsConfirmed сбрасыватся.
        /// </summary>
        public async Task ConfirmLocation(long locationId)
        {
            using (var con = new SqlConnection(base.ConnectionString))
            {
                await con.OpenAsync();
                SqlTransaction tran = null;
                try
                {
                    var spName = "cmn.sp_Map_ConfirmLocation";
                    var p = new DynamicParameters();
                    p.Add("@LocationId", locationId);

                    tran = con.BeginTransaction();

                    await con.ExecuteAsync(
                        sql: spName, param: p,
                        commandType: CommandType.StoredProcedure,
                        transaction: tran);

                    tran.Commit();
                }
                catch (SqlException e)
                {
                    tran.RollbackSafe();
                    con.Close();
                    throw e;
                }
                con.Close();
            }
        }

        public async Task<List<CityLandmarkDTO>> GetLandmarkList(int cityId)
        {
            IEnumerable<CityLandmarkDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Map_GetLandmarkList";
                var p = new DynamicParameters();
                p.Add("@CityId", cityId);

                await con.OpenAsync();
                items = await con.QueryAsync<CityLandmarkDTO>(
                    sql: spName, param: p, commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }
    }
}
