create table [cmn].[Auth_UserClaim]
(
	[UserClaimId]	int identity(1,1)	not null,
	[UserId]		int					not null,
	[Type]			nvarchar(128)		not null,
	[Value]			nvarchar(128)		not null,

	constraint [Auth_UserClaim_PK] primary key clustered ([UserClaimId])
)
go

create index [Auth_UserClaim_UserId_IX] on [cmn].[Auth_UserClaim] ([UserId])
go