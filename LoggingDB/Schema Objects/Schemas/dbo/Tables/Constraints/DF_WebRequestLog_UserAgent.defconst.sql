ALTER TABLE [dbo].[WebRequestLog]
    ADD CONSTRAINT [DF_WebRequestLog_UserAgent] DEFAULT ('') FOR [UserAgent];

