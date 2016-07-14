CREATE PROCEDURE [cmn].[sp_Auth_User_Update]
	@UserId					int,	
	-- Standart ASP.NET Identity params
	@Email					nvarchar(256),
	@EmailConfirmed			bit,	
	  
	@PhoneNumber			nvarchar(max),
	@PhoneNumberConfirmed	bit,

	@LockoutEndDateUtc		datetime,			
	@LockoutEnabled			bit,		
	@AccessFailedCount		tinyint,
		
	@SecurityStamp			nvarchar(max),
	@PasswordHash			nvarchar(max),
	-- Custome business params
	@FirstName				nvarchar(256),
	@LastName				nvarchar(256)
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
			Email					= @Email,
			EmailConfirmed			= @EmailConfirmed,

			PhoneNumber				= @PhoneNumber,
			PhoneNumberConfirmed	= @PhoneNumberConfirmed,

			LockoutEndDateUtc		= @LockoutEndDateUtc,
			LockoutEnabled			= @LockoutEnabled,
			AccessFailedCount		= @AccessFailedCount,

			PasswordHash			= @PasswordHash,
			SecurityStamp			= @SecurityStamp,

			FirstName				= @FirstName,
			LastName				= @LastName
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
