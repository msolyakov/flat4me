create procedure [cmn].[sp_Map_GetLocationListByLandmarkId]
	@LandmarkId int
AS
begin
	select
		ad.LandmarkId,
		al.[LocationId],
		al.[AccommodationId],
		al.[FullAddress],
		al.[PointY], -- Широта
		al.[PointX], -- Долгота
		al.[IsConfirmed],
		al.[IsDeleted],
		ad.DistanceCode,
		ad.Distance
	from cmn.Map_AccommodationDistance ad with(nolock) inner join
		 cmn.Map_AccommodationLocation al with(nolock) on 
		   ( al.LocationId = ad.LocationId and al.IsConfirmed = 1 and al.IsDeleted = 0 )
	where ad.LandmarkId = @LandmarkId 
end
go