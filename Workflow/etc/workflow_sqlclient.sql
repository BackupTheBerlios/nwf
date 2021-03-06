if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OS_CURRENTSTEP_PREV]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[OS_CURRENTSTEP_PREV]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OS_HISTORYSTEP_PREV]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[OS_HISTORYSTEP_PREV]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[OS_PROPERTYENTRY]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[OS_PROPERTYENTRY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[os_currentstep]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[os_currentstep]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[os_historyStep]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[os_historyStep]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[os_wfentry]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[os_wfentry]
GO

CREATE TABLE [dbo].[OS_CURRENTSTEP_PREV] (
	[ID] [int] NOT NULL ,
	[PREVIOUS_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OS_HISTORYSTEP_PREV] (
	[ID] [int] NOT NULL ,
	[PREVIOUS_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OS_PROPERTYENTRY] (
	[GLOBAL_KEY] [varchar] (255) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[ITEM_KEY] [varchar] (255) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[ITEM_TYPE] [smallint] NOT NULL ,
	[STRING_VALUE] [varchar] (255) COLLATE Chinese_PRC_CI_AS NULL ,
	[DATE_VALUE] [datetime] NULL ,
	[DATA_VALUE] [image] NULL ,
	[FLOAT_VALUE] [float] NULL ,
	[NUMBER_VALUE] [numeric](18, 0) NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[os_currentstep] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[action_Id] [int] NULL ,
	[caller] [char] (10) COLLATE Chinese_PRC_CI_AS NULL ,
	[finish_Date] [datetime] NULL ,
	[start_Date] [datetime] NULL ,
	[due_Date] [datetime] NULL ,
	[owner] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[status] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[step_id] [int] NULL ,
	[stepIndex] [int] NULL ,
	[entry_id] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[os_historyStep] (
	[id] [int] NOT NULL ,
	[action_Id] [int] NULL ,
	[caller] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[finish_Date] [datetime] NULL ,
	[start_Date] [datetime] NULL ,
	[due_Date] [datetime] NULL ,
	[owner] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[status] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,
	[step_Id] [int] NULL ,
	[stepIndex] [int] NULL ,
	[entry_id] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[os_wfentry] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[name] [varchar] (128) COLLATE Chinese_PRC_CI_AS NOT NULL ,
	[state] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OS_CURRENTSTEP_PREV] WITH NOCHECK ADD 
	 PRIMARY KEY  CLUSTERED 
	(
		[ID],
		[PREVIOUS_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[OS_HISTORYSTEP_PREV] WITH NOCHECK ADD 
	 PRIMARY KEY  CLUSTERED 
	(
		[ID],
		[PREVIOUS_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[OS_PROPERTYENTRY] WITH NOCHECK ADD 
	CONSTRAINT [PK__OS_PROPERTYENTRY__023D5A04] PRIMARY KEY  CLUSTERED 
	(
		[GLOBAL_KEY],
		[ITEM_KEY]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[os_currentstep] WITH NOCHECK ADD 
	CONSTRAINT [PK_os_currentstep] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[os_historyStep] WITH NOCHECK ADD 
	CONSTRAINT [PK_os_historyStep] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[os_wfentry] WITH NOCHECK ADD 
	CONSTRAINT [PK_os_wfentry] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[os_currentstep] ADD 
	CONSTRAINT [DF_os_currentstep_action_Id] DEFAULT (0) FOR [action_Id],
	CONSTRAINT [DF_os_currentstep_stepIndex] DEFAULT (0) FOR [stepIndex]
GO

ALTER TABLE [dbo].[os_historyStep] ADD 
	CONSTRAINT [DF_os_historyStep_stepIndex] DEFAULT (0) FOR [stepIndex]
GO

