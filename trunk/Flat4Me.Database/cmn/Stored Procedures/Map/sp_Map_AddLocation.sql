create procedure [cmn].[sp_Map_AddLocation]
	@AccommodationId int,
	@FullAddress nvarchar(512), 
	@PointY float, 
	@PointX float
AS
BEGIN
	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
					
        INSERT INTO cmn.Map_AccommodationLocation
		(
			[AccommodationId], [FullAddress],
			[PointY], [PointX], -- Широта/Долгота
			[IsConfirmed], [IsDeleted]
		)
        VALUES 
		(
			@AccommodationId, @FullAddress, 
			@PointY, @PointX, 
			0, 0
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
GO