create procedure [cmn].[sp_Short_Accommodation_Destroy]
	@AccommodationId int
AS
BEGIN
	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
					
			
        delete cmn.Short_Accommodation where AccommodationId = @AccommodationId

			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (XACT_STATE() <> 0 AND @startTranCount = 0)
            ROLLBACK TRANSACTION;				
			
        throw;
    END CATCH
END