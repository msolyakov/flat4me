create table [cmn].[Region] 
(
    [RegionId]  int identity(1,1)	not null,
    [CountryId] int					not null,
    [IsDeleted] BIT DEFAULT(0)		not null,    
    [Name]		NVARCHAR(50)		not null,

    constraint [Region_PK] primary key clustered ([RegionId]),

    constraint [Region_Country_FK] foreign key ([CountryId]) 
		references [cmn].[Country] ([CountryId])
);