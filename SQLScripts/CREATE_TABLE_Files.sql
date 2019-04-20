USE [MyCompare1]
GO

/****** Object:  Table [dbo].[Files]    Script Date: 12/04/2019 21:43:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Files](
	[ProcessId] [uniqueidentifier] NOT NULL,
	[Drive] [varchar](50) NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Extension] [nvarchar](150) NULL,
	[ContractIndex] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


