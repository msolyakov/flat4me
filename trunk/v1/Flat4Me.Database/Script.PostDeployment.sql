/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


-- INSERT CONSTANTS

begin tran

-- Geography data
declare @RussiaId int;
insert into [cmn].[Country] ([Name], [PhoneCode])
	values(N'Россия', '7')
select @RussiaId = SCOPE_IDENTITY()

declare @SamaraRegionId int;
insert into [cmn].[Region] ([CountryId], [Name])
	values(@RussiaId, N'Самарская область')
select @SamaraRegionId = SCOPE_IDENTITY()

declare @gmt4Id int;
insert into [cmn].[TimeZone](UTCOffset)
	values(4)
select @gmt4Id = SCOPE_IDENTITY()

declare @SamaraCityId int;
insert into [cmn].[City] (RegionId, TimeZoneId, [Name], [Url], PointY, PointX, Zoom)
	values(@SamaraRegionId, @gmt4Id, N'Самара', 'samara', 53.199933, 50.251801, 12)
select @SamaraCityId = SCOPE_IDENTITY()

insert into [cmn].[City] (RegionId, TimeZoneId, [Name], [Url], PointY, PointX, Zoom)
	values(@SamaraRegionId, @gmt4Id, N'Тольятти', 'tolyatti', 53.508714, 49.41918, 11)

-- [Distance Codes]
insert into [cmn].[Map_CityDistanceCode] (CityId, DistanceCode, Distance)
	values(@SamaraCityId, 0, 500);
insert into [cmn].[Map_CityDistanceCode] (CityId, DistanceCode, Distance)
	values(@SamaraCityId, 1, 1000);
insert into [cmn].[Map_CityDistanceCode] (CityId, DistanceCode, Distance)
	values(@SamaraCityId, 2, 2000);


-- Auth_Role
declare @AdminRoleId int;
insert into [cmn].[Auth_Role] (Name) values('admin')
insert into [cmn].[Auth_Role] (Name) values('hotelier')
insert into [cmn].[Auth_Role] (Name) values('guest')

--select @AdminRoleId = SCOPE_IDENTITY()


---- Auth_User
--declare @AleksandrId int;
--insert into [cmn].[Auth_User] (IsDeleted, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, LockoutEnabled, SecurityStamp)
--	values(0, N'imaleksandr@Outlook.com', 1, N'9277607222', 1, 1, N'234rgfgre')
--select @AleksandrId = scope_identity()

---- Auth_UserRole
--insert into cmn.Auth_UserRole (UserId, RoleId) values (@AleksandrId, @AdminRoleId)

--declare @MikhailId int;
--insert into [cmn].[User] (CityId, [IsActivated], Email, [Password], [Name], [CreatedOn], [LastVisitOn], [ProfilePhotoPath], [IsApproved])
--	values(@SamaraCityId, 1, 'mikhail.solyakov@gmail.com', '1qaz@WSX3edc', N'Михаил', getdate(), getdate(), 'ProfilePhotoPath', 1)
--select @MikhailId = scope_identity()

--declare @ElenaId int;
--insert into [cmn].[User] (CityId, [IsActivated], Email, [Password], [Name], [CreatedOn], [LastVisitOn], [ProfilePhotoPath], [IsApproved])
--	values(@SamaraCityId, 1, 'imelena@outlook.com', '1qaz@WSX3edc', N'Елена', getdate(), getdate(), 'ProfilePhotoPath', 1)
--select @ElenaId = scope_identity()

--declare @AnnaId int;
--insert into [cmn].[User] (CityId, [IsActivated], Email, [Password], [Name], [CreatedOn], [LastVisitOn], [ProfilePhotoPath], [IsApproved])
--	values(@SamaraCityId, 1, 'anna-solyakova@yandex.ru', '1qaz@WSX3edc', N'Анна', getdate(), getdate(), 'ProfilePhotoPath', 1)
--select @AnnaId = scope_identity()


-- Short_UserSetting
--insert into cmn.Short_UserSetting (UserId, ContactPhone, ArrivalFrom, ArrivalTo, DepartureFrom, DepartureTo, [HasAirportTransfer], [EstimatedAirportTransferCost])
--values (@AleksandrId, '9277607222', '13:00', '19:00', '9:00', '12:00', 1, 1200)

--insert into cmn.Short_UserSetting (UserId, ContactPhone, ArrivalFrom, ArrivalTo, DepartureFrom, DepartureTo, [HasAirportTransfer], [EstimatedAirportTransferCost])
--values (@MikhailId, '9272614833', '13:00', '19:00', '9:00', '12:00', 1, 1200)

--insert into cmn.Short_UserSetting (UserId, ContactPhone, ArrivalFrom, ArrivalTo, DepartureFrom, DepartureTo, [HasAirportTransfer], [EstimatedAirportTransferCost])
--values (@ElenaId, '9179541430', '13:00', '19:00', '9:00', '12:00', 1, 1200)

--insert into cmn.Short_UserSetting (UserId, ContactPhone, ArrivalFrom, ArrivalTo, DepartureFrom, DepartureTo, [HasAirportTransfer], [EstimatedAirportTransferCost])
--values (@AnnaId, '9276017416', '13:00', '19:00', '9:00', '12:00', 1, 1200)


-- [AccommodationType]
insert into [cmn].[AccommodationType] (Code)
	values('apartment')
insert into [cmn].[AccommodationType] (Code)
	values('house')
insert into [cmn].[AccommodationType] (Code)
	values('villa')



-- [cmn].[Currency]
insert into [cmn].[Currency] (Code)
	values('rub')
insert into [cmn].[Currency] (Code)
	values('eur')
insert into [cmn].[Currency] (Code)
	values('usd')


commit tran
-- Post-Deployment Script END