CREATE TYPE [dbo].[TvpExecutionTrace] AS TABLE
(
	[Module] [VARCHAR](128) NOT NULL,
	[ClassName] [VARCHAR](256) NOT NULL,
	[MethodName] [VARCHAR](96) NOT NULL,
	[Tag] [VARCHAR](96),
	[Duration] [INT] NOT NULL,
	[LastCall] [DATETIME2](3) NOT NULL
);