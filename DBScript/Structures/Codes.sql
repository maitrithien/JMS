IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[Codes]') AND TYPE IN (N'U'))

/****** Object:  Table [dbo].[Codes]    Script Date: 17/04/2015 10:52:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Codes](
	[CodeID] [nvarchar](50) NOT NULL,
	[CodeName] [nvarchar](100) NOT NULL,
	[CodeDescription] [nvarchar](250) NULL,
	[CodeGroupID] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserID] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Codes] ADD  CONSTRAINT [DF_Codes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Codes] ADD  CONSTRAINT [DF_Codes_CreatedUserID]  DEFAULT (N'ADMINISTRATOR') FOR [CreatedUserID]
GO


