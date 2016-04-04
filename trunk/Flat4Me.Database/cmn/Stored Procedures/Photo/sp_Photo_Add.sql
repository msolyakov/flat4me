create procedure [cmn].[sp_Photo_Add]
	@AccommodationId int,

	@LargePath nvarchar(1024),
	@MediumPath nvarchar(1024),
	@SmallPath nvarchar(1024),
	@TinyPath nvarchar(1024)
AS
begin

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;


        INSERT INTO cmn.Photo	
				(AccommodationId, LargePath, MediumPath, SmallPath, TinyPath)
		VALUES	(@AccommodationId, @LargePath, @MediumPath, @SmallPath, @TinyPath)

		SELECT
            cast(scope_identity() as int) AS PhotoId
			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
			
        throw;
    END CATCH

end