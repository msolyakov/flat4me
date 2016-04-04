create procedure [cmn].[sp_Map_GetLandmarkList]
	@CityId int
AS
begin
	select
		[LandmarkId],
		[CityId],
		[ShortName],
		[FullName],
		[FullAddress],
		[PointY],
		[PointX]
	from cmn.Map_CityLandmark cl with(nolock)
	where cl.CityId = @CityId
end
go