-- Short-term accommodation
create table [cmn].[Short_Accommodation]
(
	[AccommodationId]				int				not null,

	[CurrencyId]					int				not null,

	[MinDurationDays]				tinyint			not null,
	
	[SleepsCount]					nvarchar(50)	not null,-- example: 2+2 2+1
	[MaxGuestsCount]				tinyint			not null,

	[HasAirConditioning]			bit	default(0)	not null,
	[HasWashingMachine]				bit	default(0)	not null,
	[HasRefrigerator]				bit	default(0)	not null,
	[HasKitchen]					bit	default(0)	not null,
	[HasElevator]					bit	default(0)	not null,
	[HasParking]					bit	default(0)	not null,
	[HasJacuzzi]					bit	default(0)	not null,
	[HasInternet]					bit	default(0)	not null,
	[HasWiFi]						bit	default(0)	not null,	
	[HasGasWaterHeater]				bit	default(0)	not null,
	[HasElectricWaterHeater]		bit	default(0)	not null,	
	[HasDish]						bit	default(0)	not null,
	[HasDishwasher]					bit	default(0)	not null,
	[HasMicrowave]					bit	default(0)	not null,
	[HasElectricKettle]				bit	default(0)	not null,
	[HasHairDryer]					bit	default(0)	not null,
	[HasIron]						bit	default(0)	not null,	
	[HasIntercom]					bit	default(0)	not null,
	[HasConcierge]					bit	default(0)	not null,
	[HasSecurity]					bit	default(0)	not null,
	[HasToiletries]					bit	default(0)	not null,
	[HasTV]							bit	default(0)	not null,
	[HasTVCable]					bit	default(0)	not null,
										
	[IsSmokingAllowed]				bit	default(0)	not null,
	[IsAnimalsAllowed]				bit	default(0)	not null,
	[IsPrivateAllowed]				bit	default(0)	not null,-- Sex =)
	[IsPhotoSessionAllowed]			bit	default(0)	not null,
	[IsHypoallergenic]				bit	default(0)	not null,
	
	-- Deposit
	[HasDeposit]					bit default(0)	not null,
	[IsDepositWhenReservation]		bit default(0)	not null, -- When client make reservaton he should pay deposit
	[Deposit]						int				null,

	[Furniture]						nvarchar(max)	not null,
	[Infrastructure]				nvarchar(max)	not null,

	constraint [Short_Accommodation_PK] primary key clustered ([AccommodationId]),

	constraint [Short_Accommodation_Accommodation_FK] foreign key ([AccommodationId]) 
		references [cmn].[Accommodation] ([AccommodationId]),

	constraint [Short_Accommodation_Currency_FK] foreign key ([CurrencyId]) 
		references [cmn].[Currency] ([CurrencyId])
)