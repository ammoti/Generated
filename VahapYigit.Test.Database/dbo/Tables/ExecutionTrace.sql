CREATE TABLE [dbo].[ExecutionTrace] (
    [ExecutionTrace_Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [ExecutionTrace_Module]        VARCHAR (128) NOT NULL,
    [ExecutionTrace_ClassName]     VARCHAR (256) NOT NULL,
    [ExecutionTrace_MethodName]    VARCHAR (96)  NOT NULL,
    [ExecutionTrace_Tag]           VARCHAR (96)  NULL,
    [ExecutionTrace_MinDuration]   INT           NOT NULL,
    [ExecutionTrace_MaxDuration]   INT           NOT NULL,
    [ExecutionTrace_TotalDuration] BIGINT        NOT NULL,
    [ExecutionTrace_Counter]       BIGINT        NOT NULL,
    [ExecutionTrace_LastCall]      DATETIME2 (3) NOT NULL,
    CONSTRAINT [PK_ExecutionTrace_1] PRIMARY KEY CLUSTERED ([ExecutionTrace_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Total duration (in millisecond)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExecutionTrace', @level2type = N'COLUMN', @level2name = N'ExecutionTrace_TotalDuration';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Minimal duration (in millisecond)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExecutionTrace', @level2type = N'COLUMN', @level2name = N'ExecutionTrace_MinDuration';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Maximal duration (in millisecond)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ExecutionTrace', @level2type = N'COLUMN', @level2name = N'ExecutionTrace_MaxDuration';
GO


