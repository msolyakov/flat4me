create procedure [cmn].[sp_Short_HotelierProfile_GetUnapprovedList]
AS
BEGIN

	select 
		hp.UserId, 
		au.Email,
		au.PhoneNumber,
		au.LastName,
		au.FirstName,
		au.PhotoTinyPath,

		hp.CityId, 
		c.Name as CityName		
	
	from cmn.Short_HotelierProfile hp with(nolock)
		join cmn.Auth_User au with(nolock) on hp.UserId = au.UserId
		join cmn.City c with(nolock) on hp.CityId = c.CityId
	
	where IsApproved = 0

END