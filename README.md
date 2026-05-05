# FIAP Cloud Games - Tech Challenge 1

Este projeto é uma API de um **Plataforma de venda de jogos digitais e gestão de servidores para partidas online**. 

Desenvolvida como parte do Tech Challenge da pós-graduação em **Arquitetura de Sistemas .NET** na **FIAP**. A solução foi desenhada com foco em escalabilidade, manutenibilidade e separação de responsabilidades.

## 🚀 Como Executar o Projeto

### Pré-requisitos
*   [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e rodando.
*   SDK do .NET 9 instalado.

### 1. Subir a Infraestrutura
Na raiz do projeto (onde está o arquivo `docker-compose.yml`), execute:
```bash
docker-compose up -d
````
Este comando inicializará o SQL Server na porta 1433 e o Redis na porta 6379

#### 2. Rodar a API
Pelo terminal na pasta da API ou pelo Visual Studio (F5):

```bash
dotnet run
```
As migrations e o Seed Data (populando o catálogo inicial de jogos) são executados automaticamente no startup.

---

## 🔐 Credenciais de Teste (Admin)

Para facilitar a avaliação das funcionalidades administrativas (como a criação de jogos, jogadores e promoções), utilize o usuário pré-configurado:

*   E-mail: admin@fiapcloudgames.com.br

*   Senha: SenhaAdmin@123

Nota: Realize o login no endpoint /login para obter o Token JWT e utilize o botão Authorize do Swagger para enviar o cabeçalho de autorização.

---

## 🛠️ Tecnologias e Frameworks

*   **Runtime:** .NET 9
*   **Web API:** ASP.NET Core com **Controllers**
*   **Arquitetura:** Clean Architecture & Domain-Driven Design (DDD)
*   **Banco de Dados:** SQL Server 2022 via Docker
*   **Cache:** Redis via Docker
*   **ORM:** Entity Framework Core
*   **Testes:** Testes Unitários com **xUnit**
*   **Documentação:** Swagger (OpenAPI)

---

## 🏗️ Estrutura da Solução

O projeto segue os princípios da **Clean Architecture**, garantindo que as regras de negócio sejam independentes de frameworks externos:

*   **Domain:** Contém as Entidades, Agregados, Value Objects e as interfaces dos Repositórios.
*   **Application:** Onde residem os Serviços de Aplicação e os **DTOs** (Requests/Responses).
*   **Infrastructure:** Implementação do acesso a dados (EF Core), Migrations, configuração do Redis e integração com provedores de identidade.
*   **API (Presentation):** Controllers responsáveis pela exposição dos endpoints, autenticação JWT e documentação.

---

## 🧠 Modelagem de Domínio (Event Storming)

A arquitetura deste sistema foi desenhada utilizando a metodologia **Event Storming** para identificar os contextos delimitados (*Bounded Contexts*), eventos de domínio e a linguagem ubíqua do projeto.

A dinâmica foi realizada através do **Miro** e serviu como base para a implementação dos Agregados e Entidades presentes na camada de `Domain`.

### Principais Contextos Identificados:
*   **Catálogo de Jogos:** Gestão de títulos, preços e estoque.
*   **Gestão de Usuários:** Controle de perfis (Jogadores e Administradores) e autenticação.
*   **Pedidos:** Processamento de compras e histórico transacional.
*   **Biblioteca do Jogador:**: Controle dos seus jogos adquiridos após pedido feito

### 🔗 Link do Board (Miro)
Você pode visualizar o desenho técnico completo e o fluxo de eventos através do link abaixo:
> [Acesse aqui o Event Storming no Miro](https://miro.com/app/board/uXjVGmb0dF4=/?share_link_id=834581073090)

---

## 🧪 Testes

Para garantir a integridade das regras de negócio, utilize o comando abaixo para rodar a suíte de testes unitários:

```bash
dotnet test
```

## 🔗 Endereços Úteis

*   Redis Insight: http://localhost:8001 (para monitorar o cache)
*   SQL Server: localhost,1433 (Login: sa / Senha definida no compose)

---
Desenvolvedor: Alex Oliveira Ferreira
