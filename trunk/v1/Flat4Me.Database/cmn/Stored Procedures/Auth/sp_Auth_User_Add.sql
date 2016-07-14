CREATE PROCEDURE [cmn].[sp_Auth_User_Add]
	-- Standart ASP.NET Identity params
	@Email				nvarchar(256),

	@PhoneNumber		nvarchar(256),

	@SecurityStamp		nvarchar(max),
	@PasswordHash		nvarchar(max),	  
	
	-- Custome business params
	@FirstName			nvarchar(256),
	@LastName			nvarchar(256)
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
			
        INSERT INTO cmn.[Auth_User] (Email, PasswordHash, SecurityStamp, PhoneNumber, FirstName, LastName)
        VALUES	(@Email, @PasswordHash, @SecurityStamp, @PhoneNumber, @FirstName, @LastName)
																		  	
		SELECT cast(scope_identity() as int) as UserId			  	

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
