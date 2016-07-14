create procedure [cmn].[sp_AccommodationBase_Add]
	@AccommodationId		int,

	@AccommodationTypeCode	varchar(10),
	@CityId					int,
	@Name					nvarchar(50),

	@Area					smallint,
	@StreetName				nvarchar(50),
	@HouseNumber			nvarchar(10),

	@RoomCount				tinyint,
	@BedroomCount			tinyint,
	@BathroomCount			tinyint,
	@LoungeRoomCount		tinyint
AS
BEGIN

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;
	declare @AccommodationTypeId int;

	-- Get accommodation type id
	select @AccommodationTypeId = AccommodationTypeId
	from cmn.AccommodationType with(nolock)
	where Code = @AccommodationTypeCode

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;


		insert into cmn.AccommodationBase
		(
			AccommodationId, AccommodationTypeId, CityId, 
			[Name], Area, StreetName, 
			HouseNumber, 
			RoomCount, BedroomCount, BathroomCount, 
			LoungeRoomCount
		) 
		values
		(
			@AccommodationId, @AccommodationTypeId, @CityId, 
			@Name, @Area, @StreetName, 
			@HouseNumber, 
			@RoomCount, @BedroomCount, @BathroomCount, 
			@LoungeRoomCount
		)
		

		IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
			
        throw;
    END CATCH

END