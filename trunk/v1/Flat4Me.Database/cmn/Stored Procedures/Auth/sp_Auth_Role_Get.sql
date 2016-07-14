CREATE PROCEDURE [cmn].[sp_Auth_Role_Get]
	@Name	nvarchar(128)
AS
BEGIN
    select ar.RoleId
		
	from cmn.Auth_Role ar with(nolock)		

	where ar.Name = @Name
END
