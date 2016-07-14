CREATE FUNCTION [cmn].[f_Short_Price_GetMaxByAccommodation]
(
	@AccommodationId int
)
RETURNS TABLE
AS
RETURN
	select top 1
		Amount, DurationDays
    from cmn.Short_Price with (nolock)    
    where AccommodationId = @AccommodationId
    order by Amount desc