ALTER TABLE [dbo].[WebRequestLog]
    ADD CONSTRAINT [DF_WebRequestLog_Referer] DEFAULT ('') FOR [Referer];

