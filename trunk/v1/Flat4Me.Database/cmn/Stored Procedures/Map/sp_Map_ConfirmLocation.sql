create procedure [cmn].[sp_Map_ConfirmLocation]
	@LocationId bigint
AS
begin
	SET XACT_ABORT ON;
	SET NOCOUNT ON;	
	
    DECLARE @startTranCount int;
	DECLARE @AccommodationId int;

	-- Получаем значение AccommodationId для заданного LocationId
	SELECT @AccommodationId = AccommodationId
	FROM cmn.Map_AccommodationLocation with(nolock)
	WHERE LocationId = @LocationId

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
					
		-- 1. Сбрасываем значения
		UPDATE cmn.Map_AccommodationLocation
		SET IsConfirmed = 0, IsDeleted = 1
		WHERE AccommodationId = @AccommodationId

		-- 2. Подтверждаем единственный варинат
		UPDATE cmn.Map_AccommodationLocation
		SET IsConfirmed = 1, IsDeleted = 0
		WHERE LocationId = @LocationId
			
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