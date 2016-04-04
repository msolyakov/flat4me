CREATE PROCEDURE [cmn].[sp_Auth_UserClaim_GetList]
	@UserId	int
AS
BEGIN
			
	select [Type], [Value]
	from cmn.Auth_UserClaim with(nolock)
	where UserId = @UserId

END
