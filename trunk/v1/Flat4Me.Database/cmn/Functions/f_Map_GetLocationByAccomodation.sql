CREATE FUNCTION [cmn].[f_Map_GetLocationByAccomodation]
(
	@AccommodationId int
)
RETURNS TABLE
AS
RETURN
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
	  and al.IsConfirmed = 1
	  and al.IsDeleted = 0
GO
