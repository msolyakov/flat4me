CREATE TABLE [cmn].[Short_Reservation]
(
	[ReservationId]		int identity(1,1)	not null,
	[AccommodationId]	int					not null,
	[UserId]			int					not null,
	[CreatedOnUtc]		datetime			not null,
	[IsCanceled]		bit default(0)		not null,

	[Checkin]			date				not null,-- Checkin
	[Checkout]			date				not null,-- Checkout
	[Guests]			tinyint				not null,
	[EstimatedAmount]	int					not null,
	[Children]			tinyint				null,

	constraint [Short_Reservation_PK] primary key clustered ([ReservationId]),

	constraint [Short_Reservation_Accommodation_FK] foreign key ([AccommodationId]) 
		references [cmn].[Accommodation] ([AccommodationId]),

	constraint [Short_Reservation_Auth_User_FK] foreign key ([UserId]) 
		references [cmn].[Auth_User] ([UserId]),
)
go

create index [Short_Reservation_AccommodationId_IX] on [cmn].[Short_Reservation] ([AccommodationId])
go

create index [Short_Reservation_UserId_IX] on [cmn].[Short_Reservation] ([UserId])
go