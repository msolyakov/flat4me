create procedure [cmn].[sp_Short_Reservation_Add]
	@AccommodationId	int,
	@UserId				int,
	@CreatedOnUtc		datetime,
	@Checkin			date,
	@Checkout			date,
	@Guests				tinyint,
	@EstimatedAmount	int,
	@Children			tinyint	
AS
begin

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;

		-- BEGIN SCRIPT

        INSERT INTO cmn.Short_Reservation	
				(AccommodationId, UserId, CreatedOnUtc, Checkin, Checkout, Guests, EstimatedAmount, Children)
		VALUES	(@AccommodationId, @UserId, @CreatedOnUtc, @Checkin, @Checkout, @Guests, @EstimatedAmount, @Children)

		SELECT
            cast(scope_identity() as int) AS ReservationId

		-- END SCRIPT
			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
			
        throw;
    END CATCH

end