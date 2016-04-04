CREATE PROCEDURE [cmn].[sp_Auth_UserLogin_Remove]
	@UserId			int,
	@ProviderId		int, 
	@ProviderKey	nvarchar(128)
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
			
        delete from cmn.Auth_UserLogin where 
			@UserId = @UserId 
			and @ProviderId = @ProviderId 
			and ProviderKey = @ProviderKey        

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
