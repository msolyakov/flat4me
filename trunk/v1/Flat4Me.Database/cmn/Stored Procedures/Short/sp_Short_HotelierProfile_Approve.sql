create procedure [cmn].[sp_Short_HotelierProfile_Approve]
	@UserId			int,
	@IsApproved		bit
AS
BEGIN

	SET XACT_ABORT ON;
	SET NOCOUNT ON;

    DECLARE @startTranCount int;

    BEGIN TRY
        SET @startTranCount = @@Trancount

        IF @startTranCount = 0
            BEGIN TRANSACTION;

		-- BEGIN SCRIPT
	
        update cmn.Short_HotelierProfile set 
			IsApproved = @IsApproved
		where
			UserId = @UserId

		-- END SCRIPT
			
        IF @startTranCount = 0
            COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 AND @startTranCount = 0
            ROLLBACK TRANSACTION;
			
        throw;
    END CATCH
	

END