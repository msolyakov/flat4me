/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
-- DELETE ONLY 'cmn' SCHEMA OBJECTS START
PRINT N'Deleting [cmn] schema objects...';
DECLARE @N CHAR(1)
SET @N = CHAR(10)
DECLARE @Stmt NVARCHAR(MAX)



-- procedures
SELECT
    @Stmt = ISNULL(@Stmt + @N, '') +
    'drop procedure [' + SCHEMA_NAME(schema_id) + '].[' + Name + ']'
FROM sys.procedures
WHERE SCHEMA_NAME(schema_id) IN ('cmn')



-- check constraints
SELECT
    @Stmt = ISNULL(@Stmt + @N, '') +
    'alter table [' + SCHEMA_NAME(schema_id) + '].[' + OBJECT_NAME(parent_object_id) + ']    drop constraint [' + Name + ']'
FROM sys.check_constraints
WHERE SCHEMA_NAME(schema_id) IN ('cmn')



-- functions
SELECT
    @Stmt = ISNULL(@Stmt + @N, '') +
    'drop function [' + SCHEMA_NAME(schema_id) + '].[' + Name + ']'
FROM sys.objects
WHERE type IN ('FN', 'IF', 'TF')
AND SCHEMA_NAME(schema_id) IN ('cmn')



-- views
SELECT
    @Stmt = ISNULL(@Stmt + @N, '') +
    'drop view [' + SCHEMA_NAME(schema_id) + '].[' + Name + ']'
FROM sys.views
WHERE SCHEMA_NAME(schema_id) IN ('cmn')



-- foreign keys
SELECT
    @Stmt = ISNULL(@Stmt + @N, '') +
    'alter table [' + SCHEMA_NAME(schema_id) + '].[' + OBJECT_NAME(parent_object_id) + '] drop constraint [' + Name + ']'
FROM sys.foreign_keys
WHERE SCHEMA_NAME(schema_id) IN ('cmn')



-- tables
SELECT
    @Stmt = ISNULL(@Stmt + @N, '') +
    'drop table [' + SCHEMA_NAME(schema_id) + '].[' + Name + ']'
FROM sys.tables
WHERE SCHEMA_NAME(schema_id) IN ('cmn')



-- user defined types
SELECT
    @Stmt = ISNULL(@Stmt + @N, '') +
    'drop type [' + SCHEMA_NAME(schema_id) + '].[' + Name + ']'
FROM sys.types
WHERE is_user_defined = 1
AND SCHEMA_NAME(schema_id) IN ('cmn')



EXEC sp_executesql @Stmt
-- DELETE ONLY 'cmn' SCHEMA OBJECTS END
-- Pre-Deployment Script END