CREATE TABLE [dbo].[ProcessLog] (
    [ProcessLog_Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [ProcessLog_Date]          DATETIME2(3)     NOT NULL,
    [ProcessLog_ProcedureName] VARCHAR (96)  NOT NULL,
    [ProcessLog_Data]          VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ProcessLog_Id] PRIMARY KEY CLUSTERED ([ProcessLog_Id] ASC)
);

