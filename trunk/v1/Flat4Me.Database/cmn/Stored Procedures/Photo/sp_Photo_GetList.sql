create procedure [cmn].[sp_Photo_GetList]
	@AccommodationId	int,
	@OnlyPrimary		bit
AS
begin
	
    select
		PhotoId,			
		IsPrimary,
		IsApproved,
		LargePath,
		MediumPath,
		SmallPath,
		TinyPath
	from 
		cmn.Photo with(nolock)
    where 
		AccommodationId = @AccommodationId
		and IsDeleted = 0
		and (@OnlyPrimary = 0 or @OnlyPrimary = 1 and IsPrimary = 1)

end