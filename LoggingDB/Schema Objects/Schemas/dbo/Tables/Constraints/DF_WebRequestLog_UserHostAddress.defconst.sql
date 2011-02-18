ALTER TABLE [dbo].[WebRequestLog]
    ADD CONSTRAINT [DF_WebRequestLog_UserHostAddress] DEFAULT ('') FOR [UserHostAddress];

