**Documentação Técnica: Arquitetura do Projeto HealthCare**

**1. Visão Geral**

O projeto HealthCare é uma aplicação web desenvolvida utilizando ASP.NET Core com o padrão MVC (Model-View-Controller). 
Ele utiliza o framework .NET 8.0 e é estruturado para ser modular, escalável e de fácil manutenção. A aplicação é composta por várias camadas que seguem os princípios de separação de responsabilidades.

---

**2. Tecnologias Utilizadas**

- Linguagem de Programação: C# 12.0

- Framework: ASP.NET Core (MVC e Razor Pages)

- Banco de Dados: SQL Server

- ORM: Entity Framework Core

- Autenticação e Autorização: ASP.NET Identity

- Gerenciamento de Dependências: Injeção de Dependência nativa do ASP.NET Core

- Infraestrutura de Globalização: Suporte a culturas específicas (com configuração para evitar o modo de globalização-invariante)

- Hospedagem: Contêineres Docker (opcional)

---

**3. Desenho da Arquitetura**

**3.1. Camadas da Arquitetura**

A arquitetura do projeto segue o padrão N-Camadas, com as seguintes divisões principais:

**3.1.1. Camada de Apresentação (Presentation Layer)**

- Descrição: Responsável por interagir com os usuários finais. Contém as Views (Razor Pages) e os Controllers que gerenciam as requisições HTTP.

- Principais Tecnologias:

  - Razor Pages para renderização de UI.

  - Controllers para gerenciar rotas e lógica de apresentação.

- Responsabilidades:

  - Renderizar páginas HTML.

  - Processar entradas do usuário.

  - Invocar serviços da camada de aplicação.

**3.1.2. Camada de Aplicação (Application Layer)**

- Descrição: Contém a lógica de negócios e orquestração de operações. Essa camada interage com a camada de dados para realizar operações no banco.

- Principais Tecnologias:

  - Serviços (Services) registrados no contêiner de injeção de dependência.

- Responsabilidades:

  - Implementar regras de negócio.

  - Coordenar operações entre a camada de apresentação e a camada de dados.

**3.1.3. Camada de Dados (Data Layer)**

- Descrição: Gerencia o acesso ao banco de dados utilizando o Entity Framework Core.

- Principais Tecnologias:

  - Entity Framework Core para mapeamento objeto-relacional (ORM).

  - SQL Server como banco de dados relacional.

- Responsabilidades:

  - Executar consultas e comandos no banco de dados.

  - Gerenciar transações.

  - Mapear entidades de domínio para tabelas do banco.

**3.1.4. Camada de Infraestrutura (Infrastructure Layer)**

- Descrição: Contém configurações e serviços auxiliares, como autenticação, autorização e suporte a globalização.

- Principais Tecnologias:

  - ASP.NET Identity para autenticação e autorização.

  - Configuração de globalização e suporte a culturas específicas.

- Responsabilidades:

  - Gerenciar usuários e permissões.

  - Configurar suporte a diferentes culturas e idiomas.

---

**3.2. Fluxo de Dados**

1. O usuário interage com a interface (View) enviando uma requisição HTTP.

2. O Controller processa a requisição e invoca os serviços da camada de aplicação.

3. A camada de aplicação executa a lógica de negócios e interage com a camada de dados para acessar ou persistir informações.

4. A camada de dados utiliza o Entity Framework Core para realizar operações no banco de dados.

5. Os dados são retornados para a camada de aplicação, que os envia para o Controller.

6. O Controller retorna uma resposta HTTP para o cliente, renderizando a View com os dados processados.

---

**3.3. Diagrama de Arquitetura**

+-----------------------------+
|      Apresentação (UI)      |
|-----------------------------|
|  Razor Pages, Controllers   |
+-------------+---------------+
              |
              v
+-----------------------------+
|      Camada de Aplicação    |
|-----------------------------|
|  Serviços, Regras de Negócio|
+-------------+---------------+
              |
              v
+-----------------------------+
|         Camada de Dados     |
|-----------------------------|
|  Entity Framework Core,     |
|  Repositórios, SQL Server   |
+-------------+---------------+
              |
              v
+-----------------------------+
|     Camada de Infraestrutura|
|-----------------------------|
|  ASP.NET Identity,          |
|  Configurações de Cultura   |
+-----------------------------+

---

**4. Configuração de Contêiner**

O projeto pode ser executado em contêineres Docker. A configuração do contêiner inclui:

- Imagem Base: mcr.microsoft.com/dotnet/aspnet:8.0

- Variáveis de Ambiente:

  - DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false para suporte a culturas específicas.

- Dockerfile:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthCare.Web/HealthCare.Web.csproj", "HealthCare.Web/"]
COPY ["HealthCare.Business/HealthCare.Business.csproj", "HealthCare.Business/"]
COPY ["HealthCare.Data/HealthCare.Data.csproj", "HealthCare.Data/"]
RUN dotnet restore "./HealthCare.Web/HealthCare.Web.csproj"
COPY . .
WORKDIR "/src/HealthCare.Web"
RUN dotnet build "./HealthCare.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthCare.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthCare.Web.dll"]

```

---

**5. Padrões e Boas Práticas**

- Injeção de Dependência: Todas as dependências são registradas e resolvidas pelo contêiner de DI do ASP.NET Core.

- Separação de Responsabilidades: Cada camada tem uma responsabilidade clara, facilitando a manutenção e escalabilidade.

- Globalização e Localização: Configurado para suportar múltiplas culturas, com fallback para a cultura padrão (en-us).