USE [MyCompare1]
GO

/****** Object:  Table [dbo].[Directories]    Script Date: 12/04/2019 21:43:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Directories](
	[ProcessId] [uniqueidentifier] NOT NULL,
	[Path] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


