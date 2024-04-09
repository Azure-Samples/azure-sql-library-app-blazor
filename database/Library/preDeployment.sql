-- This file contains SQL statements that will be executed before the build script.
-- App user
---------------------------------------------------------------------------
DECLARE @role NVARCHAR(100);
DECLARE @password NVARCHAR(100);

IF '$(environment)' = 'development'
BEGIN
    SET @role = 'db_owner';
    SET @password = '$(devPassword)'; -- Development environment password SQLCMD variable
END
ELSE
BEGIN
    SET @role = 'db_datareader, db_datawriter';
    SET @password = '$(prodPassword)'; -- Production environment password SQLCMD variable
END

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'LibraryUser')
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = 'CREATE USER LibraryUser WITH PASSWORD = ''' + @password + ''';' +
               'ALTER ROLE [' + @role + '] ADD MEMBER LibraryUser;' +
               'GRANT EXECUTE TO LibraryUser;'; 

    EXEC sp_executesql @sql;
END
ELSE
BEGIN
    PRINT 'Library user already exists.';
END