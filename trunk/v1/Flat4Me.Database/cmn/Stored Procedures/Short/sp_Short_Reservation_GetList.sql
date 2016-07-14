create procedure [cmn].[sp_Short_Reservation_GetList]
	@AccommodationId	int,
	@CheckinStart		date = null
AS
begin
	
    select
		r.ReservationId,
		r.AccommodationId,
		r.UserId,
		r.CreatedOnUtc,
		r.IsCanceled,

		r.Checkin,
		r.Checkout,
		r.Guests,
		r.EstimatedAmount,
		r.Children,

		au.FirstName,
		au.LastName,
		au.Email,
		au.PhoneNumber	
	from 
		cmn.Short_Reservation r with(nolock)
		
		inner join cmn.Auth_User au with(nolock)
			on r.UserId = au.UserId
	where
		r.AccommodationId = @AccommodationId
		and au.IsDeleted = 0
		and au.EmailConfirmed = 1
		and au.PhoneNumberConfirmed = 1
		and (@CheckinStart is null or r.Checkin >= @CheckinStart)

end