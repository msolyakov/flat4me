create table [cmn].[AccommodationType]
(
	[AccommodationTypeId]	int identity(1,1)	not null,
	[Code]					varchar(10)			not null,

	constraint [AccommodationType_PK] primary key clustered ([AccommodationTypeId]),
	constraint [AccommodationType_Code_UN] unique nonclustered ([Code] ASC)
)