CREATE PROCEDURE [cmn].[sp_Map_GetCityDistanceCodeList]
AS
	SELECT [CityId], 
	       [DistanceCode], 
		   [Distance]
	FROM [cmn].[Map_CityDistanceCode]
GO