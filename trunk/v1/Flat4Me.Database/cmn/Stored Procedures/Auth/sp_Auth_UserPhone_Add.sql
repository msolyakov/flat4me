CREATE PROCEDURE [cmn].[sp_Auth_UserPhone_Add]
	@UserId			int, 
	@PhoneNumber	nvarchar(128)
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
			
        insert into cmn.Auth_UserPhone (UserId, PhoneNumber, PhoneNumberConfirmed)
        values	(@UserId, @PhoneNumber, 0)

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
