CREATE TABLE [dbo].[WebRequestLog] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [UserAgent]       VARCHAR (MAX) NOT NULL,
    [RequestUrl]      VARCHAR (MAX) NOT NULL,
    [Referer]         VARCHAR (MAX) NOT NULL,
    [UserHostAddress] VARCHAR (226) NOT NULL,
    [UserName]        VARCHAR (50)  NOT NULL,
    [Timestamp]       DATETIME2 (7) NULL
);



