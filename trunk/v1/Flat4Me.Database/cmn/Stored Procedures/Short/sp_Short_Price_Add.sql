create procedure [cmn].[sp_Short_Price_Add]
	@AccommodationId	int,
	@DurationDays		tinyint,
	@Amount				int
AS
BEGIN

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
			
			
        insert into cmn.Short_Price 
				(AccommodationId, DurationDays, Amount)
        values	(@AccommodationId, @DurationDays, @Amount)

			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
			
        throw;
    END CATCH

END