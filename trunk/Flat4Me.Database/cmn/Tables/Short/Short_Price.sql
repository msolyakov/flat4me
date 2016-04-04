create table [cmn].[Short_Price]
(
	[PriceId]				int identity(1,1)	not null,
	[AccommodationId]		int					not null,

	[DurationDays]			tinyint				not null,
	[Amount]				int					not null,

	constraint [Price_PK] primary key clustered ([PriceId]),

	constraint [Short_Price_Accommodation_FK] foreign key ([AccommodationId]) 
		references [cmn].[Accommodation] ([AccommodationId])
)
go

create index [Short_Price_AccommodationId_IX] on [cmn].[Short_Price] ([AccommodationId])
go