create procedure [cmn].[sp_Map_GetLocationListByCityId]
	@CityId int
AS
begin
	select
		al.[LocationId],
		al.[AccommodationId],
		al.[FullAddress],
		al.[PointY], -- Широта
		al.[PointX], -- Долгота
		al.[IsConfirmed],
		al.[IsDeleted]
	from cmn.Map_AccommodationLocation al with(nolock) inner join
		 cmn.AccommodationBase ab with(nolock) on 
		   ( al.AccommodationId = ab.AccommodationId and al.IsConfirmed = 1 and al.IsDeleted = 0 )
	where ab.CityId = @CityId 
end
go