# API CRUD com .NET 9, Identity e PostgreSQL

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql)
![JWT](https://img.shields.io/badge/JWT-Authentication-D63AFF?style=for-the-badge&logo=jsonwebtokens)
![Swagger](https://img.shields.io/badge/Swagger-API_Docs-85EA2D?style=for-the-badge&logo=swagger)

## 📖 Visão Geral

Este projeto é uma **API RESTful** robusta e segura, construída com **.NET 9.0**, que implementa operações **CRUD** (Create, Read, Update, Delete) completas. A aplicação utiliza **ASP.NET Core Identity** para autenticação e autorização baseada em tokens JWT, **Entity Framework Core** para interação com um banco de dados **PostgreSQL**, e **AutoMapper** para o mapeamento de objetos.

A configuração do ambiente, especialmente do PostgreSQL, foi detalhada para uma **instalação local sem o uso de Docker**, oferecendo um controle mais granular sobre o ambiente de desenvolvimento. 

## ✨ Funcionalidades

* **Autenticação e Autorização:** Sistema completo de registro e login com **ASP.NET Core Identity**. 
* **Segurança com JWT:** Endpoints protegidos com tokens **JSON Web Tokens**. 
* **Controle de Acesso por Papel (Roles):** Endpoints com acesso restrito a papéis específicos, como "Admin" e "User". 
* **Operações CRUD Completas:** Endpoints para criar, ler, atualizar e deletar recursos (`Produto`). 
* **Mapeamento com AutoMapper:** Transformação segura e eficiente entre entidades do banco e DTOs. 
* **Padrão DTO:** Uso de **Data Transfer Objects** para definir contratos claros e seguros para a API. 
* **Validação de Dados:** Validação de entrada nos DTOs para garantir a integridade dos dados. 
* **Migrations com EF Core:** Gerenciamento de versionamento do esquema do banco de dados de forma automatizada. 
* **Gerenciamento de Segredos:** Proteção de dados sensíveis (chaves e strings de conexão) com a ferramenta `Secret Manager` em ambiente de desenvolvimento. 
* **Tratamento de Erros Centralizado:** Middleware para capturar exceções e retornar respostas de erro padronizadas. 
* **Configuração de CORS:** Permite que a API seja consumida de forma segura por aplicações front-end de diferentes origens. 

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Finalidade |
| :--- | :--- |
| **.NET 9.0** | Framework de desenvolvimento.  |
| **ASP.NET Core Web API** | Construção da API RESTful.  |
| **Entity Framework Core** | ORM para interação com o banco de dados.  |
| **PostgreSQL** | Banco de dados objeto-relacional.  |
| **Npgsql** | Provedor de dados .NET para PostgreSQL.  |
| **ASP.NET Core Identity** | Sistema de autenticação e gerenciamento de usuários.  |
| **AutoMapper** | Mapeamento de objetos.  |
| **Swagger (OpenAPI)** | Documentação interativa da API.  |

## 🚀 Começando

Siga os passos abaixo para configurar e executar o projeto em seu ambiente local.

### Pré-requisitos

* **[.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)**
* **Servidor PostgreSQL** instalado localmente.

### Guia de Instalação

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/erickromao/AspNetCore-Identity-Crud 
    ```

2.  **Crie o Banco de Dados:**
    Conecte-se ao seu servidor PostgreSQL e execute os seguintes comandos SQL:
    ```sql
    CREATE DATABASE loja_teste;
    ```
    

3.  **Configure os Segredos da Aplicação:**
    Use a ferramenta `Secret Manager` para armazenar suas credenciais de forma segura. Na raiz do projeto, execute:
    ```bash
    # Inicialize a ferramenta de segredos
    dotnet user-secrets init

    # Adicione sua string de conexão
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=nome_banco_api;Username=nome_usuario_api;Password=senha_forte_api"

    # Adicione sua chave secreta para o JWT (use uma chave longa e segura)
    dotnet user-secrets set "JWT:Secret" "SUA_CHAVE_SECRETA"
    ```
    

4.  **Aplique as Migrações do Banco de Dados:**
    Este comando criará todas as tabelas necessárias no banco de dados.
    ```bash
    dotnet ef database update
    ```
    

5.  **Execute a Aplicação:**
    ```bash
    dotnet run
    ```
    A API estará em execução. Abra seu navegador e acesse `https://localhost:<PORTA>/swagger` para ver a documentação e testar os endpoints.

## Endpoints da API

### Autenticação (`/api/Auth`)

| Verbo | Rota | Descrição |
| :--- | :--- |:---|
| `POST`| `/register`| Registra um novo usuário. |
| `POST`| `/login` | Autentica um usuário e retorna um token JWT.  |

### Produtos (`/api/Produtos`)

*Requer token de autenticação no cabeçalho `Authorization: Bearer <token>`.*

| Verbo | Rota | Descrição | Autorização |
| :--- | :--- | :--- |:--- |
| `GET` | `/` | Lista todos os produtos. | Autenticado |
| `GET` | `/{id}` | Busca um produto pelo ID. | Autenticado |
| `POST`| `/` | Cria um novo produto. | `Admin` ou `Manager` |
| `PUT` | `/{id}` | Atualiza um produto existente. | `Admin` ou `Manager` |
| `DELETE`| `/{id}` | Deleta um produto. | `Admin` |
