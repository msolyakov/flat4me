create table [cmn].[Auth_Role]
(
	[RoleId]	int identity(1,1)	not null,
	[Name]		nvarchar(128)		not null,

	constraint [Auth_Role_PK] primary key clustered ([RoleId]),
	constraint [Auth_Role_Name_UN] unique nonclustered ([Name] ASC),
)
go

create index [Auth_Role_Name_IX] on [cmn].[Auth_Role] ([Name])
go