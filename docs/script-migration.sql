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
CREATE TABLE [Jogos] (
    [Id] uniqueidentifier NOT NULL,
    [Nome] nvarchar(100) NOT NULL,
    [Descricao] nvarchar(500) NOT NULL,
    [PrecoBase] decimal(18,2) NOT NULL,
    [Ativo] bit NOT NULL DEFAULT CAST(1 AS bit),
    [DataCadastro] datetime2 NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [Genero] int NOT NULL,
    CONSTRAINT [PK_Jogos] PRIMARY KEY ([Id])
);

CREATE TABLE [Usuarios] (
    [Id] uniqueidentifier NOT NULL,
    [Nome] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Senha] nvarchar(60) NOT NULL,
    [Perfil] int NOT NULL,
    [Ativo] bit NOT NULL,
    [DataCadastro] datetime2 NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [MotivoDesativacao] int NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);

CREATE TABLE [PromocaoJogos] (
    [Id] uniqueidentifier NOT NULL,
    [JogoId] uniqueidentifier NOT NULL,
    [ValorPromocao] decimal(18,2) NOT NULL,
    [Ativo] bit NOT NULL,
    [DataCadastro] datetime2 NOT NULL,
    [DataInicio] datetime2 NOT NULL,
    [DataFim] datetime2 NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    CONSTRAINT [PK_PromocaoJogos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PromocaoJogos_Jogos_JogoId] FOREIGN KEY ([JogoId]) REFERENCES [Jogos] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Bibliotecas] (
    [Id] uniqueidentifier NOT NULL,
    [UsuarioId] uniqueidentifier NOT NULL,
    [JogoId] uniqueidentifier NOT NULL,
    [DataCadastro] datetime2 NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    [Ativo] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Bibliotecas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bibliotecas_Jogos_JogoId] FOREIGN KEY ([JogoId]) REFERENCES [Jogos] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Bibliotecas_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Pedidos] (
    [Id] uniqueidentifier NOT NULL,
    [UsuarioId] uniqueidentifier NOT NULL,
    [Status] int NOT NULL,
    [PrecoTotal] decimal(18,2) NOT NULL DEFAULT 0.0,
    [DataCadastro] datetime2 NOT NULL,
    [DataAlteracao] datetime2 NOT NULL,
    CONSTRAINT [PK_Pedidos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Pedidos_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PedidoJogos] (
    [Id] uniqueidentifier NOT NULL,
    [JogoId] uniqueidentifier NOT NULL,
    [ValorUnitario] decimal(18,2) NOT NULL,
    [PedidoId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_PedidoJogos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PedidoJogos_Jogos_JogoId] FOREIGN KEY ([JogoId]) REFERENCES [Jogos] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PedidoJogos_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([Id]) ON DELETE CASCADE
);


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Ativo', N'DataAlteracao', N'DataCadastro', N'MotivoDesativacao', N'Perfil', N'Email', N'Nome', N'Senha') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] ON;
INSERT INTO [Usuarios] ([Id], [Ativo], [DataAlteracao], [DataCadastro], [MotivoDesativacao], [Perfil], [Email], [Nome], [Senha])
VALUES ('aea0b4f3-d220-4c8d-aba8-d868be7ca593', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', NULL, 1, N'admin@fiapcloudgames.com.br', N'Administrador Sistema', N'$2a$11$Soy4TsNUDtuazT6CJulPleFnp82cF5BkICiOmF9sk19x0X6pMAic.');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Ativo', N'DataAlteracao', N'DataCadastro', N'MotivoDesativacao', N'Perfil', N'Email', N'Nome', N'Senha') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] OFF;

CREATE INDEX [IX_Bibliotecas_JogoId] ON [Bibliotecas] ([JogoId]);

CREATE UNIQUE INDEX [IX_Bibliotecas_UsuarioId_JogoId] ON [Bibliotecas] ([UsuarioId], [JogoId]);

CREATE INDEX [IX_PedidoJogos_JogoId] ON [PedidoJogos] ([JogoId]);

CREATE INDEX [IX_PedidoJogos_PedidoId] ON [PedidoJogos] ([PedidoId]);

CREATE INDEX [IX_Pedidos_UsuarioId] ON [Pedidos] ([UsuarioId]);

CREATE INDEX [IX_PromocaoJogos_JogoId] ON [PromocaoJogos] ([JogoId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260504015100_Primeira_Migration', N'9.0.15');

COMMIT;
GO

