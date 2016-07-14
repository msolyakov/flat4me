create procedure [cmn].[sp_Photo_Approve]
	@PhotoId	int,
	@IsApproved	bit
AS
begin

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;

		-- Set one photo is primary
        update cmn.Photo set  
			IsApproved = @IsApproved
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