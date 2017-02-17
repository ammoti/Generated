CREATE TABLE [dbo].[Role] (
    [Role_Id]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [Role_CodeRef]     VARCHAR (24) NOT NULL,
    [Role_Name]        VARCHAR (24) NOT NULL,
    CONSTRAINT [PK_Role_Id] PRIMARY KEY CLUSTERED ([Role_Id] ASC),
    CONSTRAINT [IQ_Role_CodeRef] UNIQUE NONCLUSTERED ([Role_CodeRef] ASC),
    CONSTRAINT [IQ_Role_Name] UNIQUE NONCLUSTERED ([Role_Name] ASC) 
);

