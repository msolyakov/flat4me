CREATE PROCEDURE [cmn].[sp_Auth_UserClaim_Add]
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
			
        insert into cmn.Auth_UserClaim (UserId, [Type], [Value])
        values	(@UserId, @Type, @Value)		  	

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
