create table [cmn].[Currency]
(
	[CurrencyId]	int identity(1,1)	not null,
	[Code]			char(3)				not null,-- ISO code

	constraint [Currency_PK] primary key clustered ([CurrencyId])
)
