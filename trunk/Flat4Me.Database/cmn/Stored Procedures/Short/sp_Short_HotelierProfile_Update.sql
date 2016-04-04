create procedure [cmn].[sp_Short_HotelierProfile_Update]
	@UserId							int,

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
	
		-- CityId never changes
        update cmn.Short_HotelierProfile set 
			CheckinFrom						= @CheckinFrom, 
			CheckinTo						= @CheckinTo,
			CheckoutFrom					= @CheckoutFrom,
			CheckoutTo						= @CheckoutTo,
			HasAirportTransfer				= @HasAirportTransfer, 
			EstimatedAirportTransferCost	= @EstimatedAirportTransferCost, 
			HasTrainTransfer				= @HasTrainTransfer, 
			EstimatedTrainTransferCost		= @EstimatedTrainTransferCost
		where
			UserId = @UserId

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