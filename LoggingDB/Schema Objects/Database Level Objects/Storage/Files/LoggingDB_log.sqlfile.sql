ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [LoggingDB_log], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10.DEF\MSSQL\DATA\LoggingDB_log.ldf', SIZE = 1024 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);

