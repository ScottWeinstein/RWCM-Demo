ALTER TABLE [dbo].[WebRequestLog]
    ADD CONSTRAINT [DF_WebRequestLog_Timestamp] DEFAULT (getdate()) FOR [Timestamp];

