CREATE PROCEDURE [dbo].[ProcessErrorLog_Custom_Purge]
(
    @CtxUser BIGINT = NULL,					-- Not used
    @CtxCulture VARCHAR(2) = N'EN',			-- Not used
    @CtxWithContextSecurity BIT = N'True'	-- Not used
)
AS
BEGIN
    SET NOCOUNT ON;
    TRUNCATE TABLE [ProcessErrorLog];
    SELECT @@ROWCOUNT;
END
