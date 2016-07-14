CREATE PROCEDURE [cmn].[sp_Auth_User_SetPhoto]
	@UserId					int,
	-- Custome business params
	@PhotoSmallPath			nvarchar(1024),
	@PhotoTinyPath			nvarchar(1024)
AS
BEGIN

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;
		
		-- BEGIN SCRIPT
		
		update cmn.Auth_User set
			PhotoSmallPath	= @PhotoSmallPath,
			PhotoTinyPath	= @PhotoTinyPath
		where
			UserId = @UserId

		-- END SCRIPT
																		  		
        IF @startTranCount = 0											  	
            COMMIT TRANSACTION;											  	
    END TRY																  
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
		
        throw;
    END CATCH

END
