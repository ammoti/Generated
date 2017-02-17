﻿-- ------------------------------------------------------------------------------ 
-- <auto-generated> 
-- This code was generated by LayerCake Generator v3.7.1.
-- http://www.layercake-generator.net
-- 
-- Changes to this file may cause incorrect behavior AND WILL BE LOST IF 
-- the code is regenerated. 
-- </auto-generated> 
-- ------------------------------------------------------------------------------

CREATE PROCEDURE ProcessErrorLog_Save
(
	@ProcessErrorLog_Id BIGINT = 0,
	@ProcessErrorLog_Date DATETIME2(3),
	@ProcessErrorLog_ProcedureName VARCHAR(96),
	@ProcessErrorLog_ErrorMessage VARCHAR(MAX),
	@ProcessErrorLog_ErrorSeverity INT,
	@ProcessErrorLog_ErrorState INT,
	@ProcessErrorLog_Data VARCHAR(MAX),

	@CtxUser BIGINT = NULL,
	@CtxCulture VARCHAR(2) = N'EN',
	@CtxWithContextSecurity BIT = N'True'
)
AS
BEGIN

	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRY
		BEGIN TRANSACTION;

			DECLARE @V_ID BIGINT = @ProcessErrorLog_Id;

			IF @ProcessErrorLog_Id < 1
			BEGIN
				INSERT INTO [ProcessErrorLog]
				(
					 [ProcessErrorLog].[ProcessErrorLog_Date]
					,[ProcessErrorLog].[ProcessErrorLog_ProcedureName]
					,[ProcessErrorLog].[ProcessErrorLog_ErrorMessage]
					,[ProcessErrorLog].[ProcessErrorLog_ErrorSeverity]
					,[ProcessErrorLog].[ProcessErrorLog_ErrorState]
					,[ProcessErrorLog].[ProcessErrorLog_Data]
				)
				VALUES
				(
					 @ProcessErrorLog_Date
					,@ProcessErrorLog_ProcedureName
					,@ProcessErrorLog_ErrorMessage
					,@ProcessErrorLog_ErrorSeverity
					,@ProcessErrorLog_ErrorState
					,@ProcessErrorLog_Data
				);

				SET @V_ID = SCOPE_IDENTITY();

			END
			ELSE
			BEGIN

				DECLARE @V_IS_LOCKED BIT = N'False';

				IF (@V_IS_LOCKED = N'False')
				BEGIN

					IF (EXISTS(	SELECT	TOP 1 1
								FROM	[sys].[tables] WITH(NOLOCK)
								WHERE	[name] = N'ProcessErrorLog_LOGS'))
					BEGIN

						DECLARE @V_LOG_QUERY NVARCHAR(MAX) = 
							  N' INSERT INTO [ProcessErrorLog_LOGS]'
							+ N' ('
							+ N'  [ProcessErrorLog_LOGS].[ProcessErrorLog_Id]'
							+ N' ,[ProcessErrorLog_LOGS].[ProcessErrorLog_Date]'
							+ N' ,[ProcessErrorLog_LOGS].[ProcessErrorLog_ProcedureName]'
							+ N' ,[ProcessErrorLog_LOGS].[ProcessErrorLog_ErrorMessage]'
							+ N' ,[ProcessErrorLog_LOGS].[ProcessErrorLog_ErrorSeverity]'
							+ N' ,[ProcessErrorLog_LOGS].[ProcessErrorLog_ErrorState]'
							+ N' ,[ProcessErrorLog_LOGS].[ProcessErrorLog_Data]'
							+ N' )'
							+ N' SELECT'
							+ N'  [ProcessErrorLog].[ProcessErrorLog_Id]'
							+ N' ,[ProcessErrorLog].[ProcessErrorLog_Date]'
							+ N' ,[ProcessErrorLog].[ProcessErrorLog_ProcedureName]'
							+ N' ,[ProcessErrorLog].[ProcessErrorLog_ErrorMessage]'
							+ N' ,[ProcessErrorLog].[ProcessErrorLog_ErrorSeverity]'
							+ N' ,[ProcessErrorLog].[ProcessErrorLog_ErrorState]'
							+ N' ,[ProcessErrorLog].[ProcessErrorLog_Data]'
							+ N' FROM'
							+ N' [ProcessErrorLog] WITH(NOLOCK)'
							+ N' WHERE'
							+ N' [ProcessErrorLog].[ProcessErrorLog_Id] = @ProcessErrorLog_Id;';

						EXECUTE sp_executesql @V_LOG_QUERY, N'@ProcessErrorLog_Id BIGINT', @ProcessErrorLog_Id;
					END

					UPDATE
						[ProcessErrorLog]
					SET
						 [ProcessErrorLog].[ProcessErrorLog_Date] = @ProcessErrorLog_Date
						,[ProcessErrorLog].[ProcessErrorLog_ProcedureName] = @ProcessErrorLog_ProcedureName
						,[ProcessErrorLog].[ProcessErrorLog_ErrorMessage] = @ProcessErrorLog_ErrorMessage
						,[ProcessErrorLog].[ProcessErrorLog_ErrorSeverity] = @ProcessErrorLog_ErrorSeverity
						,[ProcessErrorLog].[ProcessErrorLog_ErrorState] = @ProcessErrorLog_ErrorState
						,[ProcessErrorLog].[ProcessErrorLog_Data] = @ProcessErrorLog_Data
					WHERE
						[ProcessErrorLog_Id] = @ProcessErrorLog_Id;

				END

			END

		COMMIT TRANSACTION;

		SELECT	*
		FROM	[ProcessErrorLog] WITH(NOLOCK)
		WHERE	[ProcessErrorLog].[ProcessErrorLog_Id] = @V_ID;

		RETURN (@@ROWCOUNT);

	END TRY
	BEGIN CATCH

		IF XACT_STATE() != 0
		BEGIN
			ROLLBACK TRANSACTION;
		END

	END CATCH
END