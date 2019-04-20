USE [MyCompare1]
GO

/****** Object:  Table [dbo].[FilesHash]    Script Date: 12/04/2019 21:44:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FilesHash](
	[ProcessId] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[Hash] [varchar](200) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


