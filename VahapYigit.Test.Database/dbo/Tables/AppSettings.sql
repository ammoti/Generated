CREATE TABLE [dbo].[AppSettings] (
    [AppSettings_Id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [AppSettings_Key]   VARCHAR (128)  NOT NULL,
    [AppSettings_Value] VARCHAR (1024) NOT NULL,
    [AppSettings_Description] VARCHAR(256) NULL, 
    CONSTRAINT [PK_AppSettings_Id] PRIMARY KEY CLUSTERED ([AppSettings_Id] ASC),
    CONSTRAINT [UQ_AppSettings_Key] UNIQUE NONCLUSTERED ([AppSettings_Key] ASC)
);

