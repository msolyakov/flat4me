create table [cmn].[Photo]
(
	[PhotoId]			int identity(1,1)	not null,
	[AccommodationId]	int					not null,
	[IsDeleted]			bit default(0)		not null,
	[IsPrimary]			bit	default(0)		not null,
	[IsApproved]		bit default(0)		not null,
		
	[LargePath]			varchar(1024)		null,
	[MediumPath]		varchar(1024)		null,
	[SmallPath]			varchar(1024)		null,
	[TinyPath]			varchar(1024)		null,	

	constraint [Photo_PK] primary key clustered ([PhotoId]),

	constraint [Photo_Accommodation_FK] foreign key([AccommodationId]) 
		references [cmn].[Accommodation] ([AccommodationId])
)
go

create index [Photo_Accommodation_IX] on [cmn].[Photo] ([AccommodationId])
go