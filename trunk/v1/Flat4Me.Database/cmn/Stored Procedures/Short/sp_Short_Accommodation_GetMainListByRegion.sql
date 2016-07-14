CREATE PROCEDURE [cmn].[sp_Short_Accommodation_GetMainListByRegion]
	@CityId int,
	@LowerLeftY float,
	@LowerLeftX float,
	@UpperRightY float,
	@UpperRightX float
AS
begin
	-- 1. Выбираем все точки в границах прямоугольника с заданными координатами
	select
		al.[LocationId],
		al.[AccommodationId],
		al.[FullAddress],
		al.[PointY], -- Широта
		al.[PointX] -- Долгота
	into #_LocationsByRegion
	from cmn.Map_AccommodationLocation al with(nolock)
	where ( @LowerLeftY < al.[PointY] and al.[PointY] < @UpperRightY )
	  and ( @LowerLeftX < al.[PointX] and al.[PointX] < @UpperRightX )
	  and al.IsConfirmed = 1 and al.IsDeleted = 0 

	-- 2. Выбираем квартиры и джойним с локациями региона
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
		#_LocationsByRegion location 

		inner join cmn.Accommodation acc with(nolock)
			on location.AccommodationId = acc.AccommodationId

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

	where 
		acc.IsDeleted = 0
		and accb.CityId = @CityId
		and acc.IsPublished	= 1
		and acc.IsApproved = 1
end
