-- ===============================================================
-- == Расстояние от квартиры до места пребывания.
-- == Хранится код удаленности, примерно соответствующий следующим значениям:
-- ==   0 - 8 мин. пешком; 
-- ==   1 - 15 мин. пешком; 
-- ==   2 - 20 мин. на транспорте и пешком.
-- ===============================================================
create table [cmn].[Map_AccommodationDistance]
(
	[LandmarkId]	bigint	not null,
	[LocationId]	bigint	not null,
	[DistanceCode]	tinyint not null, -- 0 - ~8 мин. пешком; 1 - ~15 мин. пешком; 2 - ~20 мин. на транспорте и пешком.
	[Distance]		bigint	not null, -- Расстояние по прямой в метрах

	constraint [Map_AccommodationDistance_PK] primary key clustered ([LandmarkId], [LocationId]),

	constraint [Map_AccommodationDistance_LandmarkId_FK] foreign key ([LandmarkId]) 
		references [cmn].[Map_CityLandmark] ([LandmarkId]),

	constraint [Map_AccommodationDistance_LocationId_FK] foreign key ([LocationId]) 
		references [cmn].[Map_AccommodationLocation] ([LocationId]),
)
go

CREATE INDEX [Map_AccommodationDistance_IX] on [cmn].[Map_AccommodationDistance] 
	([LandmarkId])
go