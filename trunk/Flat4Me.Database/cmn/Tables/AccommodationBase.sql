create table [cmn].[AccommodationBase]
(
	[AccommodationId]		int 			not null,

	[CityId]				int				not null,
	[AccommodationTypeId]	int				not null,	

	[Name]					nvarchar(50)	not null,
	[Area]					smallint		not null,-- Square meter
	[StreetName]			nvarchar(50)	not null,
	[HouseNumber]			nvarchar(10)	not null,

	[RoomCount]				tinyint			not null,
	[BedroomCount]			tinyint			not null,
	[BathroomCount]			tinyint			not null,
	[LoungeRoomCount]		tinyint			not null,

	constraint [AccommodationBase_PK] primary key clustered ([AccommodationId]),

	constraint [AccommodationBase_City_FK] foreign key ([CityId]) 
		references [cmn].[City] ([CityId]),

	constraint [AccommodationBase_Type_FK] foreign key ([AccommodationTypeId]) 
		references [cmn].[AccommodationType] ([AccommodationTypeId])
)
go

create index [AccommodationBase_CityId_IX] on [cmn].[AccommodationBase] ([CityId])
go