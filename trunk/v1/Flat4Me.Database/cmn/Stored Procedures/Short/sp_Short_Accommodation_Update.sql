create procedure [cmn].[sp_Short_Accommodation_Update]
	@AccommodationId			int,

	@CurrencyCode				char(3),
	
	@MinDurationDays			tinyint,	
	
	@SleepsCount				nvarchar(50),
	@MaxGuestsCount				tinyint,
	
	@HasAirConditioning			bit,
	@HasWashingMachine			bit,
	@HasRefrigerator			bit,
	@HasKitchen					bit,
	@HasElevator				bit,
	@HasParking					bit,
	@HasJacuzzi					bit,
	@HasInternet				bit,
	@HasWiFi					bit,
	@HasGasWaterHeater			bit,
	@HasElectricWaterHeater		bit,
	@HasDish					bit,
	@HasDishwasher				bit,
	@HasMicrowave				bit,
	@HasElectricKettle			bit,
	@HasHairDryer				bit,
	@HasIron					bit,
	@HasIntercom				bit,
	@HasConcierge				bit,
	@HasSecurity				bit,
	@HasToiletries				bit,
	@HasTV						bit,
	@HasTVCable					bit,
								   
	@IsSmokingAllowed			bit,
	@IsAnimalsAllowed			bit,
	@IsPrivateAllowed			bit,
	@IsPhotoSessionAllowed		bit,
	@IsHypoallergenic			bit,
	
	@HasDeposit					bit,
	@IsDepositWhenReservation	bit,
	@Deposit					int,
	
	@Furniture					nvarchar(max),
	@Infrastructure				nvarchar(max)
AS
BEGIN
	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;
	declare @CurrencyId int;

	-- Get currency id
	select @CurrencyId = CurrencyId
	from cmn.Currency with(nolock)
	where Code = @CurrencyCode

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
					
			
        update cmn.Short_Accommodation set
			CurrencyId					= @CurrencyId, 
			MinDurationDays				= @MinDurationDays,
			SleepsCount					= @SleepsCount, 
			MaxGuestsCount				= @MaxGuestsCount, 
			HasAirConditioning			= @HasAirConditioning, 
			HasWashingMachine			= @HasWashingMachine, 
			HasRefrigerator				= @HasRefrigerator, 
			HasKitchen					= @HasKitchen, 
			HasElevator					= @HasElevator, 
			HasParking					= @HasParking, 
			HasJacuzzi					= @HasJacuzzi, 
			HasInternet					= @HasInternet, 
			HasWiFi						= @HasWiFi, 
			HasGasWaterHeater			= @HasGasWaterHeater, 
			HasElectricWaterHeater		= @HasElectricWaterHeater, 
			HasDish						= @HasDish, 
			HasDishwasher				= @HasDishwasher, 
			HasMicrowave				= @HasMicrowave, 
			HasElectricKettle			= @HasElectricKettle, 
			HasHairDryer				= @HasHairDryer, 
			HasIron						= @HasIron, 
			HasIntercom					= @HasIntercom, 
			HasConcierge				= @HasConcierge, 
			HasSecurity					= @HasSecurity, 
			HasToiletries				= @HasToiletries, 
			HasTV						= @HasTV, 
			HasTVCable					= @HasTVCable, 
			IsSmokingAllowed			= @IsSmokingAllowed, 
			IsAnimalsAllowed			= @IsAnimalsAllowed, 
			IsPrivateAllowed			= @IsPrivateAllowed, 
			IsPhotoSessionAllowed		= @IsPhotoSessionAllowed, 
			IsHypoallergenic			= @IsHypoallergenic, 
			HasDeposit					= @HasDeposit,
			IsDepositWhenReservation	= @IsDepositWhenReservation, 
			Deposit						= @Deposit,
			Furniture					= @Furniture, 
			Infrastructure				= @Infrastructure
		where
			AccommodationId = @AccommodationId

			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (XACT_STATE() <> 0 AND @startTranCount = 0)
            ROLLBACK TRANSACTION;				
			
        throw;
    END CATCH
END