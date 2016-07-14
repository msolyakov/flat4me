create procedure [cmn].[sp_Short_Accommodation_GetMyList]
	@UserId int
AS
begin

	select 
		acc.AccommodationId,
		acc.IsApproved,
		acc.IsPublished,		
		accb.[Name],
		primaryPhoto.SmallPath as PhotoSmallPath,
		minPrice.Amount as MinPriceAmount

	from 
		cmn.Accommodation acc with(nolock)

		inner join cmn.AccommodationBase accb with(nolock)
			on acc.AccommodationId = accb.AccommodationId

		inner join cmn.Short_Accommodation accs with(nolock)
			on acc.AccommodationId = accs.AccommodationId

		inner join cmn.Photo primaryPhoto with(nolock)
			on acc.AccommodationId = primaryPhoto.AccommodationId 
			and primaryPhoto.IsPrimary = 1

		cross apply cmn.f_Short_Price_GetMinByAccommodation(acc.AccommodationId) as minPrice

	where 
		acc.UserId = @UserId
		and acc.IsDeleted = 0

end