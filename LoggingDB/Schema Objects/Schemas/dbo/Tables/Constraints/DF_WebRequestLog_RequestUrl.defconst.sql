ALTER TABLE [dbo].[WebRequestLog]
    ADD CONSTRAINT [DF_WebRequestLog_RequestUrl] DEFAULT ('') FOR [RequestUrl];

