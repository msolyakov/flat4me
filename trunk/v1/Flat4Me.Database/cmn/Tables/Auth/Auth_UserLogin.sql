CREATE TABLE [cmn].[Auth_UserLogin]
(
	[UserId]		int				not null,
	[ProviderId]	int				not null,
	[ProviderKey]	nvarchar(128)	not null,	

	constraint [Auth_UserLogin_PK] primary key clustered ([ProviderId] ASC, [ProviderKey] ASC, [UserId] ASC),

	constraint [Auth_UserLogin_UserId_FK] foreign key([UserId])
		references [cmn].[Auth_User] ([UserId])
			on delete cascade
)
GO

CREATE NONCLUSTERED INDEX [Auth_UserLogin_UserId_IX] ON [cmn].[Auth_UserLogin]([UserId] ASC)
go