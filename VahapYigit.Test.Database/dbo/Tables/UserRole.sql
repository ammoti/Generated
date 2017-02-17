CREATE TABLE [dbo].[UserRole] (
    [UserRole_Id]          BIGINT IDENTITY (1, 1) NOT NULL,
    [UserRole_IdUser]      BIGINT NOT NULL,
    [UserRole_IdRole]      BIGINT NOT NULL,
    CONSTRAINT [PK_UserRole_Id] PRIMARY KEY CLUSTERED ([UserRole_Id] ASC),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([UserRole_IdRole]) REFERENCES [dbo].[Role] ([Role_Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserRole_IdUser]) REFERENCES [dbo].[User] ([User_Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [IQ_UserRole_IdUser_IdRole] UNIQUE NONCLUSTERED ([UserRole_IdUser] ASC, [UserRole_IdRole] ASC) 
);

