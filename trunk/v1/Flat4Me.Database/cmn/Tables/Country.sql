create table [cmn].[Country] 
(
    [CountryId]		int identity(1,1)	not null,
    [IsDeleted]		bit default(0)		not null,    
    [Name]			nvarchar(50)		not null,
	[PhoneCode]		varchar(10)			not null,-- only number. For example for Russia values will be = 7

    constraint [Country_PK] primary key clustered ([CountryId])
);