create table [cmn].[TimeZone] 
(
    [TimeZoneId]	int identity(1,1)	not null,
    [UTCOffset]     tinyint				not null,    

    constraint [TimeZone_PK] primary key clustered ([TimeZoneId])
);