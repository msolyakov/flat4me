CREATE PROCEDURE [cmn].[sp_Auth_User_GetByLogin]
	@ProviderId		int, 
	@ProviderKey	nvarchar(128)
AS
BEGIN
	
	select 
		u.UserId,
		u.IsDeleted,
		
		u.Email,
		u.EmailConfirmed,

		u.PhoneNumber,
		u.PhoneNumberConfirmed,
		
		u.LockoutEndDateUtc,
		u.LockoutEnabled,
		u.AccessFailedCount,

		u.PasswordHash,
		u.SecurityStamp,

		u.FirstName,
		u.LastName,
		u.PhotoSmallPath,
		u.PhotoTinyPath

	from cmn.Auth_User u with(nolock) 
		inner join cmn.Auth_UserLogin	al with(nolock)	on u.UserId	= al.UserId		

	where al.ProviderId = @ProviderId
		and al.ProviderKey = @ProviderKey
		
END
