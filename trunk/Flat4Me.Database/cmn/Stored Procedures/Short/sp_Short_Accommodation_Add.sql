create procedure [cmn].[sp_Short_Accommodation_Add]
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
					
			
        INSERT INTO cmn.Short_Accommodation
		(
			AccommodationId, CurrencyId, MinDurationDays,
			SleepsCount, MaxGuestsCount, HasAirConditioning, 
			HasWashingMachine, HasRefrigerator, HasKitchen, 
			HasElevator, HasParking, HasJacuzzi, 
			HasInternet, HasWiFi, HasGasWaterHeater, 
			HasElectricWaterHeater, HasDish, HasDishwasher, 
			HasMicrowave, HasElectricKettle, HasHairDryer, 
			HasIron, HasIntercom, HasConcierge, 
			HasSecurity, HasToiletries, HasTV, 
			HasTVCable, IsSmokingAllowed, IsAnimalsAllowed, 
			IsPrivateAllowed, IsPhotoSessionAllowed, IsHypoallergenic, 
			HasDeposit,IsDepositWhenReservation, Deposit,
			Furniture, Infrastructure
		)
        VALUES 
		(
			@AccommodationId, @CurrencyId, @MinDurationDays, 
			@SleepsCount, @MaxGuestsCount, @HasAirConditioning,
			@HasWashingMachine, @HasRefrigerator, @HasKitchen, 
			@HasElevator, @HasParking, @HasJacuzzi, 
			@HasInternet, @HasWiFi, @HasGasWaterHeater, 
			@HasElectricWaterHeater, @HasDish, @HasDishwasher, 
			@HasMicrowave, @HasElectricKettle, @HasHairDryer,
			@HasIron, @HasIntercom, @HasConcierge, 
			@HasSecurity, @HasToiletries, @HasTV, 
			@HasTVCable, @IsSmokingAllowed, @IsAnimalsAllowed, 
			@IsPrivateAllowed, @IsPhotoSessionAllowed, @IsHypoallergenic, 
			@HasDeposit, @IsDepositWhenReservation, @Deposit,
			@Furniture, @Infrastructure
		)

			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (XACT_STATE() <> 0 AND @startTranCount = 0)
            ROLLBACK TRANSACTION;				
			
        throw;
    END CATCH
END