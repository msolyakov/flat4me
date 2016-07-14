-- ===============================================================
-- == Справочник мест, рядом с которыми планируется пребывание.
-- == Например - Ж/д вокзал, Клиника Бранчевского, Самарский Мед.институт.
-- ===============================================================
create table [cmn].[Map_CityLandmark]
(
	[LandmarkId]	bigint identity		not null,
	[CityId]		int 				not null,
	[ShortName]		nvarchar(100)		not null,
	[FullName]		nvarchar(256)		not null,
	[FullAddress]	nvarchar(512)		not null,
	[PointY]		float				not null, -- Широта
	[PointX]		float				not null, -- Долгота

	constraint [Map_CityLandmark_PK] primary key clustered ([LandmarkId]),

	constraint [Map_CityLandmark_CityId_FK] foreign key ([CityId]) 
		references [cmn].[City] ( [CityId] )
)
go

CREATE INDEX [Map_CityLandmark_IX] on [cmn].[Map_CityLandmark] 
	([CityId])
go
