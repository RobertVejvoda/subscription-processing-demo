USE ODSDataMart
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32) NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231230201009_InitialCreate'
)
BEGIN
CREATE SEQUENCE [dbo].[CustomerNumbers] AS int START WITH 100 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231230201009_InitialCreate'
)
BEGIN
CREATE SEQUENCE [dbo].[SubscriptionNumbers] AS int START WITH 100 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231230201009_InitialCreate'
)
BEGIN
CREATE TABLE [dbo].[Customer] (
    [Id] varchar(32) NOT NULL,
    [CustomerNo] int NOT NULL DEFAULT (NEXT VALUE FOR CustomerNumbers),
    [FirstName] nvarchar(32) NOT NULL,
    [LastName] nvarchar(64) NOT NULL,
    [Email] varchar(64) NOT NULL,
    [State] varchar(32) NOT NULL,
    [BirthDate] date NOT NULL,
    [LastUpdatedOn] datetime2(3) NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    [TotalLoanAmount] decimal(12,2) NOT NULL,
    [TotalInsuredAmount] decimal(12,2) NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231230201009_InitialCreate'
)
BEGIN
CREATE TABLE [dbo].[Subscription] (
    [Id] varchar(32) NOT NULL,
    [SubscriptionNo] int NOT NULL DEFAULT (NEXT VALUE FOR SubscriptionNumbers),
    [State] varchar(32) NOT NULL,
    [LoanAmount] decimal(14,2) NOT NULL,
    [InsuredAmount] decimal(14,2) NOT NULL,
    [ProductId] nvarchar(max) NOT NULL,
    [ReceivedOn] datetime2 NOT NULL,
    [LastUpdatedOn] datetime2(3) NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    [UnderwritingResult] nvarchar(max) NULL,
    [Message] nvarchar(max) NULL,
    [ProcessInstanceKey] nvarchar(max) NOT NULL,
    [CustomerId] varchar(32) NOT NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_Subscription_Customer_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231230201009_InitialCreate'
)
BEGIN
CREATE INDEX [IX_Subscription_CustomerId] ON [dbo].[Subscription] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231230201009_InitialCreate'
)
BEGIN
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231230201009_InitialCreate', N'8.0.0');
END;
GO

COMMIT;
GO
