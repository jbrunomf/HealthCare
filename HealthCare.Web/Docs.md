**Documenta��o T�cnica: Arquitetura do Projeto HealthCare**

**1. Vis�o Geral**

O projeto HealthCare � uma aplica��o web desenvolvida utilizando ASP.NET Core com o padr�o MVC (Model-View-Controller). 
Ele utiliza o framework .NET 8.0 e � estruturado para ser modular, escal�vel e de f�cil manuten��o. A aplica��o � composta por v�rias camadas que seguem os princ�pios de separa��o de responsabilidades.

---

**2. Tecnologias Utilizadas**

- Linguagem de Programa��o: C# 12.0

- Framework: ASP.NET Core (MVC e Razor Pages)

- Banco de Dados: SQL Server

- ORM: Entity Framework Core

- Autentica��o e Autoriza��o: ASP.NET Identity

- Gerenciamento de Depend�ncias: Inje��o de Depend�ncia nativa do ASP.NET Core

- Infraestrutura de Globaliza��o: Suporte a culturas espec�ficas (com configura��o para evitar o modo de globaliza��o-invariante)

- Hospedagem: Cont�ineres Docker (opcional)

---

**3. Desenho da Arquitetura**

**3.1. Camadas da Arquitetura**

A arquitetura do projeto segue o padr�o N-Camadas, com as seguintes divis�es principais:

**3.1.1. Camada de Apresenta��o (Presentation Layer)**

- Descri��o: Respons�vel por interagir com os usu�rios finais. Cont�m as Views (Razor Pages) e os Controllers que gerenciam as requisi��es HTTP.

- Principais Tecnologias:

  - Razor Pages para renderiza��o de UI.

  - Controllers para gerenciar rotas e l�gica de apresenta��o.

- Responsabilidades:

  - Renderizar p�ginas HTML.

  - Processar entradas do usu�rio.

  - Invocar servi�os da camada de aplica��o.

**3.1.2. Camada de Aplica��o (Application Layer)**

- Descri��o: Cont�m a l�gica de neg�cios e orquestra��o de opera��es. Essa camada interage com a camada de dados para realizar opera��es no banco.

- Principais Tecnologias:

  - Servi�os (Services) registrados no cont�iner de inje��o de depend�ncia.

- Responsabilidades:

  - Implementar regras de neg�cio.

  - Coordenar opera��es entre a camada de apresenta��o e a camada de dados.

**3.1.3. Camada de Dados (Data Layer)**

- Descri��o: Gerencia o acesso ao banco de dados utilizando o Entity Framework Core.

- Principais Tecnologias:

  - Entity Framework Core para mapeamento objeto-relacional (ORM).

  - SQL Server como banco de dados relacional.

- Responsabilidades:

  - Executar consultas e comandos no banco de dados.

  - Gerenciar transa��es.

  - Mapear entidades de dom�nio para tabelas do banco.

**3.1.4. Camada de Infraestrutura (Infrastructure Layer)**

- Descri��o: Cont�m configura��es e servi�os auxiliares, como autentica��o, autoriza��o e suporte a globaliza��o.

- Principais Tecnologias:

  - ASP.NET Identity para autentica��o e autoriza��o.

  - Configura��o de globaliza��o e suporte a culturas espec�ficas.

- Responsabilidades:

  - Gerenciar usu�rios e permiss�es.

  - Configurar suporte a diferentes culturas e idiomas.

---

**3.2. Fluxo de Dados**

1. O usu�rio interage com a interface (View) enviando uma requisi��o HTTP.

2. O Controller processa a requisi��o e invoca os servi�os da camada de aplica��o.

3. A camada de aplica��o executa a l�gica de neg�cios e interage com a camada de dados para acessar ou persistir informa��es.

4. A camada de dados utiliza o Entity Framework Core para realizar opera��es no banco de dados.

5. Os dados s�o retornados para a camada de aplica��o, que os envia para o Controller.

6. O Controller retorna uma resposta HTTP para o cliente, renderizando a View com os dados processados.

---

**3.3. Diagrama de Arquitetura**

+-----------------------------+
|      Apresenta��o (UI)      |
|-----------------------------|
|  Razor Pages, Controllers   |
+-------------+---------------+
              |
              v
+-----------------------------+
|      Camada de Aplica��o    |
|-----------------------------|
|  Servi�os, Regras de Neg�cio|
+-------------+---------------+
              |
              v
+-----------------------------+
|         Camada de Dados     |
|-----------------------------|
|  Entity Framework Core,     |
|  Reposit�rios, SQL Server   |
+-------------+---------------+
              |
              v
+-----------------------------+
|     Camada de Infraestrutura|
|-----------------------------|
|  ASP.NET Identity,          |
|  Configura��es de Cultura   |
+-----------------------------+

---

**4. Configura��o de Cont�iner**

O projeto pode ser executado em cont�ineres Docker. A configura��o do cont�iner inclui:

- Imagem Base: mcr.microsoft.com/dotnet/aspnet:8.0

- Vari�veis de Ambiente:

  - DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false para suporte a culturas espec�ficas.

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

**5. Padr�es e Boas Pr�ticas**

- Inje��o de Depend�ncia: Todas as depend�ncias s�o registradas e resolvidas pelo cont�iner de DI do ASP.NET Core.

- Separa��o de Responsabilidades: Cada camada tem uma responsabilidade clara, facilitando a manuten��o e escalabilidade.

- Globaliza��o e Localiza��o: Configurado para suportar m�ltiplas culturas, com fallback para a cultura padr�o (en-us).