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

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Ativo', N'DataAlteracao', N'DataCadastro', N'Genero', N'PrecoBase', N'Descricao', N'Nome') AND [object_id] = OBJECT_ID(N'[Jogos]'))
    SET IDENTITY_INSERT [Jogos] ON;
INSERT INTO [Jogos] ([Id], [Ativo], [DataAlteracao], [DataCadastro], [Genero], [PrecoBase], [Descricao], [Nome])
VALUES ('0b01cdf5-5421-4dfa-a40a-1df159b24571', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 6, 19.9, N'Gerencie sua própria empresa de caminhões e domine as estradas norte-americanas neste clássico da simulação.', N'18 Wheels of Steel: Haulin'''),
('137ac0a2-d703-4fd7-a4fd-67b1a7bd21c4', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 6, 61.99, N'Experimente a lendária liberdade americana dirigindo caminhões icônicos através de paisagens deslumbrantes e marcos históricos dos Estados Unidos.', N'American Truck Simulator'),
('4362aab0-06e5-455f-a19e-d4f585bde400', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 3, 125.0, N'Construa, automatize e gerencie fábricas complexas em um planeta alienígena infinito para lançar um foguete ao espaço.', N'Factorio'),
('4ee09524-2c09-4283-a633-6e8ddd0b8d15', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 1, 249.9, N'Um jogo de ação e aventura em mundo aberto ambientado em um continente de Pywel, focado em sobrevivência e mercenários.', N'Crimson Desert'),
('52a756f6-392c-47e8-ad07-4c1ee260ed2f', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 4, 73.99, N'Desafie o deus dos mortos enquanto você abre caminho para fora do Submundo neste dungeon crawler roguelike.', N'Hades'),
('58564834-2f6e-4d98-b3b3-f3c2ce2da0b0', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 4, 229.9, N'Levante-se, Maculado, e seja guiado pela graça para portar o poder do Anel Príncipio e se tornar um Lorde Príncipio nas Terras Intermédias.', N'Elden Ring'),
('8138c73c-fc4c-42c9-a316-ff7874069510', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 2, 45.99, N'Um simulador de sobrevivência espacial onde você gerencia seus colonos e os ajuda a cavar, construir e manter uma base asteroide subterrânea.', N'Oxygen Not Included'),
('942b8596-a092-45eb-8302-f9eabc1637dc', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 11, 199.99, N'Red Dead Redemption 2 é uma história épica de honra e lealdade no alvorecer dos tempos modernos.', N'Red Dead Redemption 2'),
('a79a3378-3fe4-4bba-94be-440b097694ac', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 4, 129.99, N'Torne-se um caçador de monstros profissional e embarque em uma aventura épica para encontrar a criança da profecia em um mundo aberto vasto.', N'The Witcher 3: Wild Hunt'),
('fb1d25ba-9e7f-44be-a716-f03914ee8430', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 14, 24.99, N'Você herdou a antiga fazenda do seu avô. Com ferramentas de segunda mão e algumas moedas, você parte para começar sua nova vida.', N'Stardew Valley'),
('fd0d99fe-0196-414b-b103-5a371c5e804b', CAST(1 AS bit), '2026-05-02T00:00:00.0000000', '2026-05-02T00:00:00.0000000', 6, 61.99, N'Viaje pela Europa como o rei da estrada, um caminhoneiro que entrega cargas importantes em distâncias impressionantes.', N'Euro Truck Simulator 2');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Ativo', N'DataAlteracao', N'DataCadastro', N'Genero', N'PrecoBase', N'Descricao', N'Nome') AND [object_id] = OBJECT_ID(N'[Jogos]'))
    SET IDENTITY_INSERT [Jogos] OFF;

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

