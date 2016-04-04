create table [cmn].[City] 
(
    [CityId]		int identity(1,1)	not null,
    [RegionId]		int					not null,
    [TimeZoneId]	int					not null,
    [IsDeleted]		bit default(0)		not null,
    [Name]			nvarchar(50)		not null,
	[Url]			varchar(20)			not null, -- Url address for routing	
	[PointY]		float				not null, -- Широта
	[PointX]		float				not null, -- Долгота
	[Zoom]			int default(12)		not null, -- Масштаб карты по умолчанию. Может отличаться для разных городов.

    constraint [City_PK] primary key clustered ([CityId]),
    
	constraint [City_Region_FK] foreign key ([RegionId]) 
		references [cmn].[Region] ([RegionId]),

    constraint [City_TimeZone_FK] foreign key ([TimeZoneId]) 
		references [cmn].[TimeZone] ([TimeZoneId]),


	constraint [City_Url_UN] unique nonclustered ([Url] asc),
)
go

create index [City_Name_IX] on [cmn].[City] ([Name])
go

create index [City_Url_IX] on [cmn].[City] ([Url])
go