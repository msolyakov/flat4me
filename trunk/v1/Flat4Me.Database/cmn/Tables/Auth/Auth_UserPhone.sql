create table [cmn].[Auth_UserPhone]
(
	[UserPhoneId]			int identity(1,1)	not null,
	[UserId]				int					not null,		

	[PhoneNumber]			nvarchar(256)		not null,
	[PhoneNumberConfirmed]	bit default(0)		not null,

	constraint [Auth_UserPhone_PK] primary key clustered ([UserPhoneId]),

	constraint [Auth_UserPhone_Auth_User_FK] foreign key ([UserId]) 
		references [cmn].[Auth_User] ([UserId]),
)
go

create index [Auth_UserPhone_UserId_IX] on [cmn].[Auth_UserPhone] ([UserId])
go