create table [cmn].[Auth_Provider]
(
	[ProviderId]	int identity(1,1)	not null,
	[Name]			nvarchar(128)		not null,

	constraint [Auth_Provider_PK] primary key clustered ([ProviderId]),
	constraint [Auth_Provider_Name_UN] unique nonclustered ([Name] ASC),
)
go

create index [Auth_Provider_Name_IX] on [cmn].[Auth_Provider] ([Name])
go