CREATE TABLE [dbo].[User] (
    [User_Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [User_Username]           VARCHAR (16)  NOT NULL,
    [User_Password]           VARCHAR (40)	NOT NULL,
    [User_Email]              VARCHAR (256)	NOT NULL,
    [User_PasswordQuestion]   VARCHAR (48)	NOT NULL,
    [User_PasswordResponse]   VARCHAR (32)	NOT NULL,
    [User_RegistrationDate]   DATETIME2(0)	NOT NULL,
    [User_LastConnectionDate] DATETIME2(0)	NULL,
    [User_Culture]            VARCHAR (2)	NOT NULL DEFAULT 'EN',
    CONSTRAINT [PK_User_Id] PRIMARY KEY CLUSTERED ([User_Id] ASC),
    CONSTRAINT [IQ_User_Email] UNIQUE NONCLUSTERED ([User_Email] ASC),
    CONSTRAINT [IQ_User_Username] UNIQUE NONCLUSTERED ([User_Username] ASC)
);

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The Culture must be 2 characters long' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'User_Culture'
GO