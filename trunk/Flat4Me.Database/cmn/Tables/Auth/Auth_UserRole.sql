create table [cmn].[Auth_UserRole]
(
	[UserId]	int		not null,
	[RoleId]	int		not null,	

	constraint [Auth_UserRole_PK] primary key clustered ([UserId], [RoleId]),

	constraint [Auth_UserRole_User_FK] foreign key ([UserId])
		references [cmn].[Auth_User]([UserId]),

	constraint [Auth_UserRole_Role_FK] foreign key ([RoleId])
		references [cmn].[Auth_Role]([RoleId]),
)
go

create index [Auth_UserRole_UserId_IX] on [cmn].[Auth_UserRole] ([UserId])
go