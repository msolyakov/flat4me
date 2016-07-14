create procedure [cmn].[sp_Short_Price_GetList]
	@AccommodationId int
AS
BEGIN

    select 
		PriceId, DurationDays, Amount 
	from 
		cmn.Short_Price with(nolock)		
    where 
		AccommodationId = @AccommodationId

END