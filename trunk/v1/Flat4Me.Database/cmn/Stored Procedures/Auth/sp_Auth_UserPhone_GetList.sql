CREATE PROCEDURE [cmn].[sp_Auth_UserPhone_GetList]
	@UserId	int
AS
BEGIN
    select 
		aup.UserPhoneId,
		aup.PhoneNumber, 
		aup.PhoneNumberConfirmed		
	
	from cmn.Auth_UserPhone aup with(nolock)

	where aup.UserId = @UserId
END
