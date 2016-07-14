CREATE PROCEDURE [cmn].[sp_Auth_UserLogin_GetList]
	@UserId	int
AS
BEGIN

	select
		ap.Name as ProviderName,
		al.ProviderKey
	from cmn.Auth_UserLogin al with(nolock)
		inner join cmn.Auth_Provider ap with(nolock)
			on al.ProviderId = ap.ProviderId
	where
		al.UserId = @UserId

END
