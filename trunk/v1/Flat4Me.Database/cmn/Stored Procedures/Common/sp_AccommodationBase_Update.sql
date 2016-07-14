create procedure [cmn].[sp_AccommodationBase_Update]
	@AccommodationId		int,	

	@Area					smallint,

	@RoomCount				tinyint,
	@BedroomCount			tinyint,
	@BathroomCount			tinyint,
	@LoungeRoomCount		tinyint
AS
BEGIN

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    declare @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
			
				
		update cmn.AccommodationBase set 
			Area				= @Area, 
			RoomCount			= @RoomCount, 
			BedroomCount		= @BedroomCount, 
			BathroomCount		= @BathroomCount, 
			LoungeRoomCount		= @LoungeRoomCount where 
		AccommodationId = @AccommodationId


        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
			
        throw;
    END CATCH

END