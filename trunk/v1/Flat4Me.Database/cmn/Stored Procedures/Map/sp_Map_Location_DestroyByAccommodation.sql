create procedure [cmn].[sp_Map_Location_DestroyByAccommodation]
	@AccommodationId bigint
AS
begin
	SET XACT_ABORT ON;
	SET NOCOUNT ON;	
	
    DECLARE @startTranCount int;
	
    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
		
		delete from cmn.Map_AccommodationLocation where AccommodationId = @AccommodationId
			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (XACT_STATE() <> 0 AND @startTranCount = 0)
            ROLLBACK TRANSACTION;				
			
        throw;
    END CATCH
end
go