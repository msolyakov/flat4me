create procedure [cmn].[sp_Short_HotelierProfile_Add]
	@UserId							int,
	@CityId							int,
	@IsApproved						bit,

	@CheckinFrom					time(0),					
	@CheckinTo						time(0),						
	@CheckoutFrom					time(0),					
	@CheckoutTo						time(0),					
	
	@HasAirportTransfer				bit,			
	@EstimatedAirportTransferCost	int,	
	
	@HasTrainTransfer				bit,				
	@EstimatedTrainTransferCost		int
AS
BEGIN

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;

		-- BEGIN SCRIPT
	
        INSERT INTO cmn.Short_HotelierProfile	
				(UserId, CityId, IsApproved, CheckinFrom, CheckinTo, CheckoutFrom, CheckoutTo, HasAirportTransfer, EstimatedAirportTransferCost, HasTrainTransfer, EstimatedTrainTransferCost)
		VALUES	(@UserId, @CityId, @IsApproved, @CheckinFrom, @CheckinTo, @CheckoutFrom, @CheckoutTo, @HasAirportTransfer, @EstimatedAirportTransferCost, @HasTrainTransfer, @EstimatedTrainTransferCost)
		
		-- END SCRIPT
			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
			
        throw;
    END CATCH
	

END