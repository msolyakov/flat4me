CREATE PROCEDURE [cmn].[sp_Auth_UserPhone_Get]
	@UserId			int,
	@PhoneNumber	nvarchar(128)
AS
BEGIN
    select 
		aup.UserPhoneId,
		aup.PhoneNumber, 
		aup.PhoneNumberConfirmed		
	
	from cmn.Auth_UserPhone aup with(nolock)

	where aup.UserId = @UserId
		and PhoneNumber = @PhoneNumber
END
