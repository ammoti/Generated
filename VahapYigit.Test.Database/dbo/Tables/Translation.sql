CREATE TABLE [dbo].[Translation] (
	[Translation_Id] BIGINT IDENTITY (1,1) NOT NULL,
	[Translation_Key] VARCHAR(256) NOT NULL,
	[Translation_Value_EN] VARCHAR (1024) NOT NULL,
	CONSTRAINT [PK_Translation_Id] PRIMARY KEY CLUSTERED ([Translation_Id] ASC),
	CONSTRAINT [UQ_Translation_Key] UNIQUE NONCLUSTERED ([Translation_Key] ASC)
);
