CREATE PROCEDURE [cmn].[sp_Map_GetLocationListByRegion]
	@LowerLeftY float,
	@LowerLeftX float,
	@UpperRightY float,
	@UpperRightX float
AS
begin
	-- Depricated. При поиске будет использоваться sp_Short_Accommodation_GetMainListByRegion
	-- Выбираем все точки в границах прямоугольника с заданными координатами
	select
		al.[LocationId],
		al.[AccommodationId],
		al.[FullAddress],
		al.[PointY], -- Широта
		al.[PointX], -- Долгота
		al.[IsConfirmed],
		al.[IsDeleted]
	from cmn.Map_AccommodationLocation al with(nolock)
	where ( @LowerLeftY < al.[PointY] and al.[PointY] < @UpperRightY )
	  and ( @LowerLeftX < al.[PointX] and al.[PointX] < @UpperRightX )
	  and al.IsConfirmed = 1 and al.IsDeleted = 0 
end
go