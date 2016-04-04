create procedure [cmn].[sp_Short_Accommodation_GetMainList]
	@CityId int
AS
begin
	select 
		acc.AccommodationId,
		accb.[Name],		
		primaryPhoto.SmallPath as PhotoSmallPath,		

		minPrice.Amount as MinPriceAmount,

		location.LocationId as LocationId,
		location.FullAddress as FullAddress,
		location.PointY as PointY,
		location.PointX as PointX

	from 
		cmn.Accommodation acc with(nolock)

		inner join cmn.AccommodationBase accb with(nolock)
			on acc.AccommodationId = accb.AccommodationId

		inner join cmn.Short_Accommodation accs with(nolock)
			on acc.AccommodationId = accs.AccommodationId

		inner join cmn.Photo primaryPhoto with(nolock)
			on acc.AccommodationId = primaryPhoto.AccommodationId 
				and primaryPhoto.IsPrimary = 1
				and primaryPhoto.IsApproved = 1
				and primaryPhoto.IsDeleted = 0
				and primaryPhoto.SmallPath is not null

		cross apply cmn.f_Short_Price_GetMinByAccommodation (acc.AccommodationId) minPrice
		
		cross apply cmn.f_Map_GetLocationByAccomodation (acc.AccommodationId) location

	where 
		acc.IsDeleted = 0
		and accb.CityId = @CityId
		and acc.IsPublished	= 1
		and acc.IsApproved = 1		
end