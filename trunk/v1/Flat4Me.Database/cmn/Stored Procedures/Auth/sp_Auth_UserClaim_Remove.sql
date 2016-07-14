CREATE PROCEDURE [cmn].[sp_Auth_UserClaim_Remove]
	@UserId	int, 
	@Type	nvarchar(128), 
	@Value	nvarchar(128)
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
			
        delete from cmn.Auth_UserClaim  where 
			UserId = @UserId 
			and [Type]	= @Type
			and [Value] = @Value

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
