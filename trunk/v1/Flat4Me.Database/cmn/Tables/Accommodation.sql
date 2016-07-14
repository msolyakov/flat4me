create table [cmn].[Accommodation]
(
	[AccommodationId]	int identity(1,1)	not null,
	[UserId]			int					not null,
	[IsDeleted]			bit default(0)		not null,
	[IsPublished]		bit default(0)		not null,-- Mean visible for booking
	[IsApproved]		bit default(0)		not null,-- Approved by administrator
	
	constraint [Accommodation_PK] primary key clustered ([AccommodationId])
)
go

create index [Accommodation_IX] on [cmn].[Accommodation] ([UserId])
go