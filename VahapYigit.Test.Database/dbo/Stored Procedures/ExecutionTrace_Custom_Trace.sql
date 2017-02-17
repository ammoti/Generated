CREATE PROCEDURE [dbo].[ExecutionTrace_Custom_Trace]
(
	@TvpExecutionTrace AS TvpExecutionTrace READONLY,

	@CtxUser BIGINT = NULL,
	@CtxCulture VARCHAR(2) = N'EN',
	@CtxWithContextSecurity BIT = N'True'
)
AS
BEGIN

	SET NOCOUNT ON
	
	DECLARE
		@v_Module VARCHAR(128),
		@v_ClassName VARCHAR(256),
		@v_MethodName VARCHAR(96),
		@v_Tag VARCHAR(96),
		@v_Duration INT,
		@v_LastCall DATETIME2(3)

	DECLARE TvpCursor CURSOR FAST_FORWARD FOR 
	SELECT
		[Module],
		[ClassName],
		[MethodName],
		[Tag],
		[Duration],
		[LastCall]
	FROM
		@TvpExecutionTrace

	OPEN TvpCursor
	FETCH NEXT FROM TvpCursor INTO
		@v_Module,
		@v_ClassName,
		@v_MethodName,
		@v_Tag,
		@v_Duration,
		@v_LastCall

	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE @v_Id INT = (
			SELECT ExecutionTrace_Id
			FROM ExecutionTrace WITH(NOLOCK)
			WHERE ExecutionTrace_Module = @v_Module
				AND  ExecutionTrace_ClassName = @v_ClassName
				AND  ExecutionTrace_MethodName = @v_MethodName
				AND  (@v_Tag IS NULL OR @v_Tag = ExecutionTrace_Tag))

		IF @v_Id IS NOT NULL
		BEGIN

			UPDATE ExecutionTrace
			SET ExecutionTrace_MinDuration = (CASE WHEN @v_Duration < ExecutionTrace_MinDuration THEN @v_Duration ELSE ExecutionTrace_MinDuration END),
				ExecutionTrace_MaxDuration = (CASE WHEN @v_Duration > ExecutionTrace_MaxDuration THEN @v_Duration ELSE ExecutionTrace_MaxDuration END),
				ExecutionTrace_TotalDuration = ExecutionTrace_TotalDuration + @v_Duration,
				ExecutionTrace_Counter = ExecutionTrace_Counter + 1,
				ExecutionTrace_LastCall = @v_LastCall
			WHERE
				ExecutionTrace_Id = @v_Id

		END
		ELSE
		BEGIN

			EXEC ExecutionTrace_Save
				0,
				@v_Module,
				@v_ClassName,
				@v_MethodName,
				@v_Tag,
				@v_Duration,
				@v_Duration,
				@v_Duration,
				1,
				@v_LastCall,
				@CtxUser,
				@CtxCulture,
				@CtxWithContextSecurity

		END

		FETCH NEXT FROM TvpCursor INTO
			@v_Module,
			@v_ClassName,
			@v_MethodName,
			@v_Tag,
			@v_Duration,
			@v_LastCall

	END

	CLOSE TvpCursor
	DEALLOCATE TvpCursor

END