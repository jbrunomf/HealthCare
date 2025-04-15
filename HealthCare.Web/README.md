# HealthCare.Web

## Visão Geral
HealthCare.Web é uma aplicação web projetada para fornecer soluções de gerenciamento de saúde. Este projeto tem como alvo o .NET 8 e visa oferecer uma plataforma robusta, escalável e amigável para gerenciar serviços de saúde.

## Funcionalidades
- Gerenciamento de pacientes
- Agendamento de consultas
- Gerenciamento de registros médicos
- Autenticação e autorização de usuários

## Tecnologias
- .NET 8
- Razor Pages
- Entity Framework Core
- SQL Server
- Bootstrap
- xUnit

## Arquitetura do Projeto

### Camadas Principais
1. **Apresentação (Razor Pages)**  
   - Responsável pela interface do usuário e interação com o cliente.
   - Implementada utilizando Razor Pages para criar páginas dinâmicas e amigáveis.
   - Utiliza Bootstrap para estilização e design responsivo.

2. **Aplicação (Serviços e Regras de Negócio)**  
   - Contém a lógica de negócios e regras específicas do domínio.
   - Implementa serviços que são consumidos pela camada de apresentação.

3. **Dados (Entity Framework Core)**  
   - Gerencia o acesso ao banco de dados.
   - Utiliza o Entity Framework Core para mapeamento objeto-relacional (ORM).
   - Inclui repositórios e contextos para abstrair operações de banco de dados.

### Fluxo de Dados
1. O usuário interage com a interface na camada de apresentação.
2. As solicitações são enviadas para os serviços na camada de aplicação.
3. A camada de aplicação processa as solicitações e interage com a camada de dados para recuperar ou persistir informações.
4. Os dados são retornados para a camada de apresentação, onde são exibidos ao usuário.

### Banco de Dados
- O projeto utiliza SQL Server como banco de dados relacional.
- O Entity Framework Core é usado para gerenciar migrações e consultas.

### Testes
- Os testes são implementados utilizando xUnit.
- Cobrem as camadas de aplicação e dados para garantir a qualidade e confiabilidade do código.

## Começando

### Pré-requisitos
- SDK do .NET 8
- SQL Server
- Editor de código (Visual Studio, Visual Studio Code, etc.)

### Instalação
1. Clone o repositório:
2. Navegue até o diretório do projeto:
3. Restaure as dependências:
4. Inicialize o banco de dados:
5. Execute a aplicação: dotnet run.
