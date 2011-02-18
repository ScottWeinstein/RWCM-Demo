ALTER TABLE [dbo].[WebRequestLog]
    ADD CONSTRAINT [DF_WebRequestLog_UserName] DEFAULT ('') FOR [UserName];

