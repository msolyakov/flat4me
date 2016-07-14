create table [cmn].[Short_HotelierProfile]
(
	[UserId]						int				not null,
	[CityId]						int				not null,
	[IsApproved]					bit default(0)	not null,-- Moderated	

	[CheckinFrom]					time(0)			not null,
	[CheckinTo]						time(0)			null,		
	[CheckoutFrom]					time(0)			null,
	[CheckoutTo]					time(0)			not null,

	[HasAirportTransfer]			bit default(0)	not null,-- From airport		
	[EstimatedAirportTransferCost]	int				null,

	[HasTrainTransfer]				bit	default(0)	not null,-- From Train station	
	[EstimatedTrainTransferCost]	int				null,

	constraint [Short_HotelierProfile_PK] primary key clustered ([UserId]),

	constraint [Short_HotelierProfile_Auth_User_FK] foreign key ([UserId]) 
		references [cmn].[Auth_User] ([UserId]),

	constraint [Short_HotelierProfile_City_FK] foreign key ([CityId]) 
		references [cmn].[City] ([CityId]),	
)