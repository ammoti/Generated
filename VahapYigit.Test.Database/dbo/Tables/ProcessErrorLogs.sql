CREATE TABLE [dbo].[ProcessErrorLog] (
    [ProcessErrorLog_Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [ProcessErrorLog_Date]          DATETIME2(3)     NOT NULL,
    [ProcessErrorLog_ProcedureName] VARCHAR (96)  NOT NULL,
    [ProcessErrorLog_ErrorMessage]  VARCHAR (MAX) NULL,
    [ProcessErrorLog_ErrorSeverity] INT           NULL,
    [ProcessErrorLog_ErrorState]    INT           NULL,
    [ProcessErrorLog_Data]          VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ProcessErrorLog_Id] PRIMARY KEY CLUSTERED ([ProcessErrorLog_Id] ASC)
);

