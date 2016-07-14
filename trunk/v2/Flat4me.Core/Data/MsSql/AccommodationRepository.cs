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
    public class AccommodationRepository : BaseRepository, IAccommodationRepository
    {
        #region Stored procedure methods

        private async Task sp_Accommodation_Publish(int accommodationId, bool publish, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Accommodation_Publish";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", accommodationId);
            p.Add("@IsPublished", publish);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task sp_Accommodation_Approve(int accommodationId, bool approve, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Accommodation_Approve";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", accommodationId);
            p.Add("@IsApproved", approve);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }

        private async Task sp_AccommodationBase_Add(AccommodationShortDTO item, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_AccommodationBase_Add";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", item.AccommodationId);
            p.Add("@AccommodationTypeCode", item.AccommodationTypeCode);
            p.Add("@CityId", item.CityId);
            p.Add("@Name", item.Name);
            p.Add("@Area", item.Area);
            p.Add("@StreetName", item.StreetName);
            p.Add("@HouseNumber", item.HouseNumber);
            p.Add("@RoomCount", item.RoomCount);
            p.Add("@BedroomCount", item.BedroomCount);
            p.Add("@BathroomCount", item.BathroomCount);
            p.Add("@LoungeRoomCount", item.LoungeRoomCount);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task sp_AccommodationBase_Update(AccommodationShortDTO item, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_AccommodationBase_Update";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", item.AccommodationId);
            p.Add("@Area", item.Area);
            p.Add("@RoomCount", item.RoomCount);
            p.Add("@BedroomCount", item.BedroomCount);
            p.Add("@BathroomCount", item.BathroomCount);
            p.Add("@LoungeRoomCount", item.LoungeRoomCount);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }

        private async Task sp_Short_Accommodation_Add(AccommodationShortDTO item, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Accommodation_Add";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", item.AccommodationId);
            p.Add("@CurrencyCode", item.CurrencyCode);
            p.Add("@MinDurationDays", item.MinDurationDays);
            p.Add("@SleepsCount", item.SleepsCount);
            p.Add("@MaxGuestsCount", item.MaxGuestsCount);
            p.Add("@HasAirConditioning", item.HasAirConditioning);
            p.Add("@HasWashingMachine", item.HasWashingMachine);
            p.Add("@HasRefrigerator", item.HasRefrigerator);
            p.Add("@HasKitchen", item.HasKitchen);
            p.Add("@HasElevator", item.HasElevator);
            p.Add("@HasParking", item.HasParking);
            p.Add("@HasJacuzzi", item.HasJacuzzi);
            p.Add("@HasInternet", item.HasInternet);
            p.Add("@HasWiFi", item.HasWiFi);
            p.Add("@HasGasWaterHeater", item.HasGasWaterHeater);
            p.Add("@HasElectricWaterHeater", item.HasElectricWaterHeater);
            p.Add("@HasDish", item.HasDish);
            p.Add("@HasDishwasher", item.HasDishwasher);
            p.Add("@HasMicrowave", item.HasMicrowave);
            p.Add("@HasElectricKettle", item.HasElectricKettle);
            p.Add("@HasHairDryer", item.HasHairDryer);
            p.Add("@HasIron", item.HasIron);
            p.Add("@HasIntercom", item.HasIntercom);
            p.Add("@HasConcierge", item.HasConcierge);
            p.Add("@HasSecurity", item.HasSecurity);
            p.Add("@HasToiletries", item.HasToiletries);
            p.Add("@HasTV", item.HasTV);
            p.Add("@HasTVCable", item.HasTVCable);
            p.Add("@HasDeposit", item.HasDeposit);
            p.Add("@IsSmokingAllowed", item.IsSmokingAllowed);
            p.Add("@IsAnimalsAllowed", item.IsAnimalsAllowed);
            p.Add("@IsPrivateAllowed", item.IsPrivateAllowed);
            p.Add("@IsPhotoSessionAllowed", item.IsPhotoSessionAllowed);
            p.Add("@IsHypoallergenic", item.IsHypoallergenic);
            p.Add("@IsDepositWhenReservation", item.IsDepositWhenReservation);
            p.Add("@Deposit", item.Deposit);
            p.Add("@Furniture", item.Furniture);
            p.Add("@Infrastructure", item.Infrastructure);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task sp_Short_Accommodation_Update(AccommodationShortDTO item, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Accommodation_Update";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", item.AccommodationId);
            p.Add("@CurrencyCode", item.CurrencyCode);
            p.Add("@MinDurationDays", item.MinDurationDays);
            p.Add("@SleepsCount", item.SleepsCount);
            p.Add("@MaxGuestsCount", item.MaxGuestsCount);
            p.Add("@HasAirConditioning", item.HasAirConditioning);
            p.Add("@HasWashingMachine", item.HasWashingMachine);
            p.Add("@HasRefrigerator", item.HasRefrigerator);
            p.Add("@HasKitchen", item.HasKitchen);
            p.Add("@HasElevator", item.HasElevator);
            p.Add("@HasParking", item.HasParking);
            p.Add("@HasJacuzzi", item.HasJacuzzi);
            p.Add("@HasInternet", item.HasInternet);
            p.Add("@HasWiFi", item.HasWiFi);
            p.Add("@HasGasWaterHeater", item.HasGasWaterHeater);
            p.Add("@HasElectricWaterHeater", item.HasElectricWaterHeater);
            p.Add("@HasDish", item.HasDish);
            p.Add("@HasDishwasher", item.HasDishwasher);
            p.Add("@HasMicrowave", item.HasMicrowave);
            p.Add("@HasElectricKettle", item.HasElectricKettle);
            p.Add("@HasHairDryer", item.HasHairDryer);
            p.Add("@HasIron", item.HasIron);
            p.Add("@HasIntercom", item.HasIntercom);
            p.Add("@HasConcierge", item.HasConcierge);
            p.Add("@HasSecurity", item.HasSecurity);
            p.Add("@HasToiletries", item.HasToiletries);
            p.Add("@HasTV", item.HasTV);
            p.Add("@HasTVCable", item.HasTVCable);
            p.Add("@HasDeposit", item.HasDeposit);
            p.Add("@IsSmokingAllowed", item.IsSmokingAllowed);
            p.Add("@IsAnimalsAllowed", item.IsAnimalsAllowed);
            p.Add("@IsPrivateAllowed", item.IsPrivateAllowed);
            p.Add("@IsPhotoSessionAllowed", item.IsPhotoSessionAllowed);
            p.Add("@IsHypoallergenic", item.IsHypoallergenic);
            p.Add("@IsDepositWhenReservation", item.IsDepositWhenReservation);
            p.Add("@Deposit", item.Deposit);
            p.Add("@Furniture", item.Furniture);
            p.Add("@Infrastructure", item.Infrastructure);

            await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
        }
        private async Task<AccommodationShortDTO> sp_Short_Accommodation_Get(int accommodationId, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Accommodation_Get";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", accommodationId);

            var item = (await conn.QueryAsync<AccommodationShortDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure)).SingleOrDefault();

            return item;
        }

        private async Task sp_Short_Price_Add(int accommodationId, IEnumerable<PriceDTO> priceList, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Price_Add";
            foreach (var item in priceList)
            {
                var p = new DynamicParameters();
                p.Add("@AccommodationId", accommodationId);
                p.Add("@DurationDays", item.DurationDays);
                p.Add("@Amount", item.Amount);

                await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
            }
        }
        private async Task sp_Short_Price_Update(IEnumerable<PriceDTO> priceList, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Price_Update";
            foreach (var item in priceList)
            {
                var p = new DynamicParameters();
                p.Add("@PriceId", item.PriceId);
                p.Add("@DurationDays", item.DurationDays);
                p.Add("@Amount", item.Amount);

                await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
            }
        }
        private async Task sp_Short_Price_Destroy(IEnumerable<PriceDTO> priceList, SqlConnection conn, SqlTransaction tran = null)
        {
            var spName = "cmn.sp_Short_Price_Destroy";

            foreach (var item in priceList)
            {
                var p = new DynamicParameters();
                p.Add("@PriceId", item.PriceId);

                await conn.ExecuteAsync(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);
            }
        }
        private async Task<List<PriceDTO>> sp_Short_Price_GetList(int accommodationId, SqlConnection conn, SqlTransaction tran = null)
        {
            IEnumerable<PriceDTO> items = null;

            var spName = "cmn.sp_Short_Price_GetList";
            var p = new DynamicParameters();
            p.Add("@AccommodationId", accommodationId);

            items = await conn.QueryAsync<PriceDTO>(
                sql: spName, param: p, transaction: tran,
                commandType: CommandType.StoredProcedure);

            return items.ToList();
        }
        
        #endregion

        public async Task<List<AccommodationShortMainDTO>> MainList(int cityId)
        {
            IEnumerable<AccommodationShortMainDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Short_Accommodation_GetMainList";
                var p = new DynamicParameters();
                p.Add("@CityId", cityId);

                await con.OpenAsync();
                items = await con.QueryAsync<AccommodationShortMainDTO>(
                    sql: spName, param: p,
                    commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }

        public async Task<List<AccommodationShortMainDTO>> MainList(int cityId, double lowerLeftY, double lowerLeftX, double upperRightY, double upperRightX)
        {
            IEnumerable<AccommodationShortMainDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Short_Accommodation_GetMainListByRegion";
                var p = new DynamicParameters();
                p.Add("@CityId", cityId);
                p.Add("@LowerLeftY", lowerLeftY);
                p.Add("@LowerLeftX", lowerLeftX);
                p.Add("@UpperRightY", upperRightY);
                p.Add("@UpperRightX", upperRightX);

                await con.OpenAsync();
                items = await con.QueryAsync<AccommodationShortMainDTO>(
                    sql: spName, param: p,
                    commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }

        public async Task<List<AccommodationShortMainDTO>> MyList(int userId)
        {
            IEnumerable<AccommodationShortMainDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Short_Accommodation_GetMyList";
                var p = new DynamicParameters();
                p.Add("@UserId", userId);

                await con.OpenAsync();
                items = await con.QueryAsync<AccommodationShortMainDTO>(
                    sql: spName, param: p,
                    commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }

        public async Task<AccommodationShortDTO> AddDraft(int userId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Accommodation_Add";
                var p = new DynamicParameters();
                p.Add("@UserId", userId);

                await conn.OpenAsync();
                var accommodationId = (await conn.QueryAsync<int>(
                    sql: spName, param: p,
                    commandType: CommandType.StoredProcedure)).SingleOrDefault();

                return new AccommodationShortDTO
                {
                    AccommodationId = accommodationId
                };
            }
        }

        public async Task CompleteDraft(AccommodationShortDTO item, IEnumerable<PriceDTO> priceList)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        await sp_AccommodationBase_Add(item, conn, tran);
                        await sp_Short_Accommodation_Add(item, conn, tran);
                        await sp_Short_Price_Add(item.AccommodationId, priceList, conn, tran);

                        if (item.IsPublished)
                            await sp_Accommodation_Publish(item.AccommodationId, true, conn, tran);

                        if (item.IsApproved)
                            await sp_Accommodation_Approve(item.AccommodationId, true, conn, tran);

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

        public async Task<AccommodationShortDTO> Get(int accommodationId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                return await sp_Short_Accommodation_Get(accommodationId, conn);
            }
        }

        public async Task<List<PriceDTO>> GetPriceList(int accommodationId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                return await sp_Short_Price_GetList(accommodationId, conn);
            }
        }

        public async Task Update(AccommodationShortDTO item)
        {
            await Update(item, null, null);
        }
        public async Task Update(AccommodationShortDTO item, IEnumerable<PriceDTO> priceListAdd, IEnumerable<PriceDTO> priceListUpdate)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                var dbValue = await sp_Short_Accommodation_Get(item.AccommodationId, conn);

                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        if (dbValue.BaseProperiesHasChanged(item))
                            await sp_AccommodationBase_Update(item, conn, tran);

                        if (dbValue.ShortProperiesHasChanged(item))
                            await sp_Short_Accommodation_Update(item, conn, tran);

                        if (priceListUpdate != null)
                        {
                            // Find prices for deleting
                            // Current price list in db
                            var priceIdListInDb = await sp_Short_Price_GetList(item.AccommodationId, conn, tran);
                            // Take PriceId from Update List.
                            var priceIdListFromClient = priceListUpdate.Select(p => p.PriceId);
                            // Records finded in db, but user deleted it
                            var priceIdListToDelete = priceIdListInDb.Where(p => !priceIdListFromClient.Contains(p.PriceId));

                            // Destroy old records in db                    
                            await sp_Short_Price_Destroy(priceIdListToDelete, conn, tran);
                            // Update prices
                            await sp_Short_Price_Update(priceListUpdate, conn, tran);
                        }

                        if (priceListAdd != null)
                            await sp_Short_Price_Add(item.AccommodationId, priceListAdd, conn, tran);

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

        public async Task Publish(int accommodationId, bool publish)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Accommodation_Publish(accommodationId, publish, conn);
            }
        }

        public async Task Approve(int accommodationId, bool approve)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();
                await sp_Accommodation_Approve(accommodationId, approve, conn);
            }
        }

        public async Task Delete(int accommodationId, bool delete)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_Accommodation_Delete";
                var p = new DynamicParameters();
                p.Add("@AccommodationId", accommodationId);
                p.Add("@IsDeleted", delete);

                await conn.OpenAsync();
                await conn.ExecuteAsync(
                    sql: spName, param: p,
                    commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Completly delete (remove from db) all accommodation data. Include Prices, Photos, Reservations.
        /// </summary>
        /// <param name="accommodationId"></param>
        public async Task DeleteCompletly(int accommodationId)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                await conn.OpenAsync();

                using (var tran = conn.BeginTransaction())
                {
                    // Shared param for all stored procedures
                    var p = new DynamicParameters();
                    p.Add("@AccommodationId", accommodationId);

                    try
                    {
                        await conn.ExecuteAsync("cmn.sp_Map_Location_DestroyByAccommodation", param: p, transaction: tran, commandType: CommandType.StoredProcedure);                        
                        await conn.ExecuteAsync("cmn.sp_Short_Price_DestroyByAccommodation", param: p, transaction: tran, commandType: CommandType.StoredProcedure);
                        await conn.ExecuteAsync("cmn.sp_Photo_DestroyByAccommodation", param: p, transaction: tran, commandType: CommandType.StoredProcedure);
                        await conn.ExecuteAsync("cmn.sp_Short_Accommodation_Destroy", param: p, transaction: tran, commandType: CommandType.StoredProcedure);
                        await conn.ExecuteAsync("cmn.sp_AccommodationBase_Destroy", param: p, transaction: tran, commandType: CommandType.StoredProcedure);
                        await conn.ExecuteAsync("cmn.sp_Accommodation_Destroy", param: p, transaction: tran, commandType: CommandType.StoredProcedure);
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
    }
}
