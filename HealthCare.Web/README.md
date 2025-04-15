# HealthCare.Web

## Vis�o Geral
HealthCare.Web � uma aplica��o web projetada para fornecer solu��es de gerenciamento de sa�de. Este projeto tem como alvo o .NET 8 e visa oferecer uma plataforma robusta, escal�vel e amig�vel para gerenciar servi�os de sa�de.

## Funcionalidades
- Gerenciamento de pacientes
- Agendamento de consultas
- Gerenciamento de registros m�dicos
- Autentica��o e autoriza��o de usu�rios

## Tecnologias
- .NET 8
- Razor Pages
- Entity Framework Core
- SQL Server
- Bootstrap
- xUnit

## Arquitetura do Projeto

### Camadas Principais
1. **Apresenta��o (Razor Pages)**  
   - Respons�vel pela interface do usu�rio e intera��o com o cliente.
   - Implementada utilizando Razor Pages para criar p�ginas din�micas e amig�veis.
   - Utiliza Bootstrap para estiliza��o e design responsivo.

2. **Aplica��o (Servi�os e Regras de Neg�cio)**  
   - Cont�m a l�gica de neg�cios e regras espec�ficas do dom�nio.
   - Implementa servi�os que s�o consumidos pela camada de apresenta��o.

3. **Dados (Entity Framework Core)**  
   - Gerencia o acesso ao banco de dados.
   - Utiliza o Entity Framework Core para mapeamento objeto-relacional (ORM).
   - Inclui reposit�rios e contextos para abstrair opera��es de banco de dados.

### Fluxo de Dados
1. O usu�rio interage com a interface na camada de apresenta��o.
2. As solicita��es s�o enviadas para os servi�os na camada de aplica��o.
3. A camada de aplica��o processa as solicita��es e interage com a camada de dados para recuperar ou persistir informa��es.
4. Os dados s�o retornados para a camada de apresenta��o, onde s�o exibidos ao usu�rio.

### Banco de Dados
- O projeto utiliza SQL Server como banco de dados relacional.
- O Entity Framework Core � usado para gerenciar migra��es e consultas.

### Testes
- Os testes s�o implementados utilizando xUnit.
- Cobrem as camadas de aplica��o e dados para garantir a qualidade e confiabilidade do c�digo.

## Come�ando

### Pr�-requisitos
- SDK do .NET 8
- SQL Server
- Editor de c�digo (Visual Studio, Visual Studio Code, etc.)

### Instala��o
1. Clone o reposit�rio:
2. Navegue at� o diret�rio do projeto:
3. Restaure as depend�ncias:
4. Inicialize o banco de dados:
5. Execute a aplica��o: dotnet run.
