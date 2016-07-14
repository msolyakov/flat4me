create procedure [cmn].[sp_City_GetAll]
AS
begin
	select
		ct.CityId,
		ct.Name,
		ct.Url,
		rg.Name as RegionName,
		cn.Name as CountryName
	
	from cmn.City ct with(nolock)

	inner join cmn.Region rg with(nolock)
		on ct.RegionId = rg.RegionId and rg.IsDeleted = 0

	inner join cmn.Country cn with(nolock)
		on rg.CountryId = cn.CountryId and cn.IsDeleted = 0

	where
		ct.IsDeleted = 0
end