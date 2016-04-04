CREATE PROCEDURE [cmn].[sp_Auth_UserRole_GetList]
	@UserId	int
AS
BEGIN
    select ar.Name		
	
	from cmn.Auth_UserRole aur with(nolock)
		join cmn.Auth_Role ar with(nolock)
			on aur.RoleId = ar.RoleId

	where aur.UserId = @UserId
END
