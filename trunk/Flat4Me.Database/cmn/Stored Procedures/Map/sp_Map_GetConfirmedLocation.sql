create procedure [cmn].[sp_Map_GetConfirmedLocation]
	@AccommodationId int
AS
begin
	select * from [cmn].[f_Map_GetLocationByAccomodation](@AccommodationId)
end
go