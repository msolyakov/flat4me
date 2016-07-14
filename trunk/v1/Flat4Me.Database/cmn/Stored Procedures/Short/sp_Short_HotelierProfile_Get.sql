create procedure [cmn].[sp_Short_HotelierProfile_Get]
	@UserId int
AS
BEGIN

	select 
		UserId, 
		CityId, 
		IsApproved, 
		
		CheckinFrom, 
		CheckinTo, 
		CheckoutFrom, 
		CheckoutTo,					

		HasAirportTransfer,			
		EstimatedAirportTransferCost,	

		HasTrainTransfer,				
		EstimatedTrainTransferCost		
	
	from cmn.Short_HotelierProfile with(nolock)
	
	where UserId = @UserId

END