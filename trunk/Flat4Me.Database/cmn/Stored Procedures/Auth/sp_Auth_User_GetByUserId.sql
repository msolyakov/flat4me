﻿CREATE PROCEDURE [cmn].[sp_Auth_User_GetByUserId]
	@UserId int
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
	where u.UserId = @UserId
		
END
