-- ===============================================================
-- == Требуется для определения значений DistanceCode в таблице 
-- == AccommodationDistance, в зависимости от города.
-- == Для Самары предлагается три градации - 500, 1000 и 2000 м.
-- ===============================================================
create table [cmn].[Map_CityDistanceCode]
(
	[CityId]		int 	not null,
	[DistanceCode]	tinyint not null, -- 0 - ~8 мин. пешком; 1 - ~15 мин. пешком; 2 - ~20 мин. на транспорте и пешком.
	[Distance]		bigint	not null, -- Расстояние по прямой в метрах

	constraint [Map_CityDistanceCode_PK] primary key clustered ([CityId], [DistanceCode]),

	constraint [Map_CityDistanceCode_CityId_FK] foreign key ([CityId]) 
		references [cmn].[City] ([CityId])
)
go