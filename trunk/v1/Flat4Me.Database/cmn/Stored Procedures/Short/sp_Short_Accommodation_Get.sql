create procedure [cmn].[sp_Short_Accommodation_Get]
	@AccommodationId int
AS
begin

	select
		acc.AccommodationId,
		acc.UserId,
		acc.IsDeleted,
		acc.IsPublished,
		acc.IsApproved,
		accb.[Name],

		accb.CityId,
		ct.Name as CityName,	
		
		accb.Area,
		accb.StreetName,
		accb.HouseNumber,
		   
		accb.RoomCount,
		accb.BedroomCount,
		accb.BathroomCount,
		accb.LoungeRoomCount,	
		
		acct.Code as AccommodationTypeCode,
		crr.Code as CurrencyCode,

		accs.MinDurationDays,
		
		accs.SleepsCount,
		accs.MaxGuestsCount,
		
		accs.HasAirConditioning,
		accs.HasWashingMachine,
		accs.HasRefrigerator,
		accs.HasKitchen,
		accs.HasElevator,
		accs.HasParking,
		accs.HasJacuzzi,
		accs.HasInternet,
		accs.HasWiFi,
		accs.HasGasWaterHeater,
		accs.HasElectricWaterHeater,
		accs.HasDish,
		accs.HasDishwasher,
		accs.HasMicrowave,
		accs.HasElectricKettle,
		accs.HasHairDryer,
		accs.HasIron,
		accs.HasIntercom,
		accs.HasConcierge,
		accs.HasSecurity,
		accs.HasToiletries,
		accs.HasTV,				
		accs.HasTVCable,
		
		accs.IsSmokingAllowed,
		accs.IsAnimalsAllowed,
		accs.IsPrivateAllowed,
		accs.IsPhotoSessionAllowed,
		accs.IsHypoallergenic,

		accs.HasDeposit,
		accs.IsDepositWhenReservation,
		accs.Deposit,
		
		accs.Furniture,			
		accs.Infrastructure

	from cmn.Accommodation acc with(nolock)

		inner join cmn.AccommodationBase accb with(nolock)
			on acc.AccommodationId = accb.AccommodationId		
		
		inner join cmn.Short_Accommodation accs with(nolock)
			on acc.AccommodationId = accs.AccommodationId
		
		inner join cmn.City ct with(nolock)
			on accb.CityId = ct.CityId

		inner join cmn.AccommodationType acct with(nolock)
			on accb.AccommodationTypeId = acct.AccommodationTypeId

		inner join cmn.Currency crr with(nolock)
			on accs.CurrencyId = crr.CurrencyId

	where
		acc.AccommodationId = @AccommodationId

end