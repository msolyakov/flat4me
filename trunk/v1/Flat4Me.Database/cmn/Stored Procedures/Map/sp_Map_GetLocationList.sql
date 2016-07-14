create procedure [cmn].[sp_Map_GetLocationList]
	@AccommodationId int
AS
begin
	select
		[LocationId],
		[AccommodationId],
		[FullAddress],
		[PointY], -- Широта
		[PointX], -- Долгота
		[IsConfirmed],
		[IsDeleted]
	from cmn.Map_AccommodationLocation al with(nolock)
	where al.AccommodationId = @AccommodationId
	  and al.IsDeleted = 0
end
go