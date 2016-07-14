CREATE PROCEDURE [cmn].[sp_Auth_Provider_Get]
	@Name nvarchar(128)
AS
BEGIN
	
	select ProviderId, Name		
	from cmn.Auth_Provider with(nolock) 
	where Name = @Name
		
END
