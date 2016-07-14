CREATE TABLE [cmn].[Auth_User]
(
	[UserId]					int identity(1,1)	not null,
	[IsDeleted]					bit default(0)		not null,

	[Email]						nvarchar(256)		not null,-- LOGIN	
	[EmailConfirmed]			bit default(0)		not null,
	
	[PhoneNumber]				nvarchar(256)		not null,
	[PhoneNumberConfirmed]		bit default(0)		not null,
	
	[LockoutEndDateUtc]			datetime			null,
	[LockoutEnabled]			bit default(1)		not null,
	[AccessFailedCount]			tinyint	default(0)	not null,
		
	[SecurityStamp]				nvarchar(max)		not null,-- Use for ASP.NET Identity (A random value that should change whenever a users credentials have changed (password changed, login removed))
	[PasswordHash]				nvarchar(max)		null,-- Empty when external login use (facebook, vk)
	
	[FirstName]					nvarchar(256)		not null,-- Custome F4Me Field. Mean, it doesn't relate to ASP.Identity
	[LastName]					nvarchar(256)		not null,-- Custome F4Me Field. Mean, it doesn't relate to ASP.Identity
	[PhotoSmallPath]			nvarchar(1024)		null,-- Custome F4Me Field. Mean, it doesn't relate to ASP.Identity. Insert/Update separatly
	[PhotoTinyPath]				nvarchar(1024)		null,-- Custome F4Me Field. Mean, it doesn't relate to ASP.Identity. Insert/Update separatly

	constraint [Auth_User_PK] primary key clustered ([UserId]),

	constraint [Auth_User_Email_UN] unique nonclustered ([Email] ASC)
)
go

create index [Auth_User_Email_IX] on [cmn].[Auth_User] ([Email])
go