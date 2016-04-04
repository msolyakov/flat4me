create procedure [cmn].[sp_Photo_SetPrimary]
	@PhotoId int
AS
begin

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;
	declare @AccommodationId int;
	-- Get Accommodation of photo.
	select top 1 @AccommodationId = AccommodationId
	from cmn.Photo with(nolock)
	where PhotoId = @PhotoId

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
		
		-- Set all photos of accommodation to not primary
		update cmn.Photo set
			IsPrimary = 0
		where
			AccommodationId = @AccommodationId
			and IsDeleted = 0

		-- Set one photo is primary
        update cmn.Photo set  
			IsPrimary = 1
        where 
			PhotoId = @PhotoId

			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
			
        throw;
    END CATCH

end