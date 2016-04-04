CREATE PROCEDURE [cmn].[sp_Auth_UserPhone_Confirm]
	@UserId					int, 
	@UserPhoneId			int,
	@PhoneNumberConfirmed	bit
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
			
        update cmn.Auth_UserPhone
		set	PhoneNumberConfirmed = @PhoneNumberConfirmed
        where
			UserId = @UserId
			and UserPhoneId = @UserPhoneId

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
