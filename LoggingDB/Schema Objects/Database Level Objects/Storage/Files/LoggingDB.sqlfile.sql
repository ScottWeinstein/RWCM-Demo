ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [LoggingDB], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10.DEF\MSSQL\DATA\LoggingDB.mdf', SIZE = 2304 KB, FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];

