using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FiapCloundGames.API.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Primeira_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jogos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrecoBase = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Perfil = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MotivoDesativacao = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromocaoJogos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JogoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorPromocao = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromocaoJogos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromocaoJogos_Jogos_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bibliotecas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JogoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bibliotecas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bibliotecas_Jogos_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bibliotecas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PrecoTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidoJogos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JogoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PedidoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoJogos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoJogos_Jogos_JogoId",
                        column: x => x.JogoId,
                        principalTable: "Jogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidoJogos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Jogos",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataCadastro", "Genero", "PrecoBase", "Descricao", "Nome" },
                values: new object[,]
                {
                    { new Guid("0b01cdf5-5421-4dfa-a40a-1df159b24571"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 19.90m, "Gerencie sua própria empresa de caminhões e domine as estradas norte-americanas neste clássico da simulação.", "18 Wheels of Steel: Haulin'" },
                    { new Guid("137ac0a2-d703-4fd7-a4fd-67b1a7bd21c4"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 61.99m, "Experimente a lendária liberdade americana dirigindo caminhões icônicos através de paisagens deslumbrantes e marcos históricos dos Estados Unidos.", "American Truck Simulator" },
                    { new Guid("4362aab0-06e5-455f-a19e-d4f585bde400"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 125.00m, "Construa, automatize e gerencie fábricas complexas em um planeta alienígena infinito para lançar um foguete ao espaço.", "Factorio" },
                    { new Guid("4ee09524-2c09-4283-a633-6e8ddd0b8d15"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 249.90m, "Um jogo de ação e aventura em mundo aberto ambientado em um continente de Pywel, focado em sobrevivência e mercenários.", "Crimson Desert" },
                    { new Guid("52a756f6-392c-47e8-ad07-4c1ee260ed2f"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 73.99m, "Desafie o deus dos mortos enquanto você abre caminho para fora do Submundo neste dungeon crawler roguelike.", "Hades" },
                    { new Guid("58564834-2f6e-4d98-b3b3-f3c2ce2da0b0"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 229.90m, "Levante-se, Maculado, e seja guiado pela graça para portar o poder do Anel Príncipio e se tornar um Lorde Príncipio nas Terras Intermédias.", "Elden Ring" },
                    { new Guid("8138c73c-fc4c-42c9-a316-ff7874069510"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 45.99m, "Um simulador de sobrevivência espacial onde você gerencia seus colonos e os ajuda a cavar, construir e manter uma base asteroide subterrânea.", "Oxygen Not Included" },
                    { new Guid("942b8596-a092-45eb-8302-f9eabc1637dc"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 199.99m, "Red Dead Redemption 2 é uma história épica de honra e lealdade no alvorecer dos tempos modernos.", "Red Dead Redemption 2" },
                    { new Guid("a79a3378-3fe4-4bba-94be-440b097694ac"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 129.99m, "Torne-se um caçador de monstros profissional e embarque em uma aventura épica para encontrar a criança da profecia em um mundo aberto vasto.", "The Witcher 3: Wild Hunt" },
                    { new Guid("fb1d25ba-9e7f-44be-a716-f03914ee8430"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 24.99m, "Você herdou a antiga fazenda do seu avô. Com ferramentas de segunda mão e algumas moedas, você parte para começar sua nova vida.", "Stardew Valley" },
                    { new Guid("fd0d99fe-0196-414b-b103-5a371c5e804b"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 61.99m, "Viaje pela Europa como o rei da estrada, um caminhoneiro que entrega cargas importantes em distâncias impressionantes.", "Euro Truck Simulator 2" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "DataAlteracao", "DataCadastro", "MotivoDesativacao", "Perfil", "Email", "Nome", "Senha" },
                values: new object[] { new Guid("aea0b4f3-d220-4c8d-aba8-d868be7ca593"), true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, "admin@fiapcloudgames.com.br", "Administrador Sistema", "$2a$11$Soy4TsNUDtuazT6CJulPleFnp82cF5BkICiOmF9sk19x0X6pMAic." });

            migrationBuilder.CreateIndex(
                name: "IX_Bibliotecas_JogoId",
                table: "Bibliotecas",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Bibliotecas_UsuarioId_JogoId",
                table: "Bibliotecas",
                columns: new[] { "UsuarioId", "JogoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PedidoJogos_JogoId",
                table: "PedidoJogos",
                column: "JogoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoJogos_PedidoId",
                table: "PedidoJogos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PromocaoJogos_JogoId",
                table: "PromocaoJogos",
                column: "JogoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bibliotecas");

            migrationBuilder.DropTable(
                name: "PedidoJogos");

            migrationBuilder.DropTable(
                name: "PromocaoJogos");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Jogos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
