create procedure [cmn].[sp_City_GetByUrl]
	@Url varchar(20)
AS
begin
	select ct.CityId
		,ct.RegionId
		,ct.TimeZoneId
		,ct.IsDeleted
		,ct.Name
		,ct.Url
		,ct.PointY
		,ct.PointX
		,ct.Zoom
	from cmn.City ct with(nolock)
	where
		ct.IsDeleted = 0
		and Url= @Url
end