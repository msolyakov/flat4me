create table [cmn].[Map_AccommodationLocation]
(
	[LocationId]		bigint identity		not null,
	[AccommodationId]	int 				not null,
	[FullAddress]		nvarchar(512)		not null,
	[PointY]			float				not null, -- Широта
	[PointX]			float				not null, -- Долгота
	[IsConfirmed]		bit					null default 0,
	[IsDeleted]			bit					null default 0,

	constraint [Map_AccommodationLocation_PK] primary key clustered ([LocationId]),

	constraint [Map_AccommodationLocation_AccommodationId_FK] foreign key ([AccommodationId]) 
		references [cmn].[Accommodation] ( [AccommodationId] )
)
go

CREATE INDEX [Map_AccommodationLocation_IX] on [cmn].[Map_AccommodationLocation] 
	([AccommodationId], [IsConfirmed], [IsDeleted])
go