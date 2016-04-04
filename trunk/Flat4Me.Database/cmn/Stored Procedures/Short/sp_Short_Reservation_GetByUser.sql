create procedure [cmn].[sp_Short_Reservation_GetByUser]
	@AccommodationId	int,
	@UserId				int,
	@Now				date
AS
begin
	
    select top(1)-- last user's reservation
		r.ReservationId,
		r.AccommodationId,
		r.UserId,
		r.CreatedOnUtc,
		r.IsCanceled,

		r.Checkin,
		r.Checkout,
		r.Guests,
		r.EstimatedAmount,
		r.Children	
	from 
		cmn.Short_Reservation r with(nolock)
	where 
		r.AccommodationId = @AccommodationId 
		and r.UserId = @UserId
		and r.IsCanceled = 0
		and r.Checkout >= @Now -- return only future and current reservation.
	order by
		r.ReservationId desc

end