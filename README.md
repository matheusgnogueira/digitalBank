# 💳 DigitalBank API

Sistema de caixa de banco.

---

## 💪 Como executar o projeto localmente

### ✅ Requisitos

Antes de começar, você precisa ter instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/)

---

### 📦 Clonando o projeto

```bash
git clone https://github.com/matheusgnogueira/digitalBank.git
cd digitalBank
```

---

### ⚙️ Configuração do banco de dados

O projeto utiliza **SQLite** como banco de dados local.

A string de conexão já está configurada no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=DigitalBank.db"
}
```

O banco (`DigitalBank.db`) será criado automaticamente na raiz da API ao executar a aplicação.

---

### 🚀 Rodando a API

1. Navegue até a pasta da API:

```bash
cd DigitalBank.API
```

2. Restaure os pacotes NuGet:

```bash
dotnet restore
```

3. Rode a aplicação:

```bash
dotnet run
```

4. Acesse a documentação Swagger:

```
https://localhost:7053/swagger
```

> Verifique a porta real no seu `launchSettings.json` se for diferente.

---

### ✅ Executando os testes

Para rodar todos os testes automatizados:

```bash
dotnet test
```

---

### 🥪 Testes automatizados incluídos

- ✅ Testes de **Domínio**: regras de negócio das entidades `Conta` e `Transferencia`
- ✅ Testes de **Serviço**: lógica da aplicação (`ContaService`, `TransferenciaService`)
- ✅ Testes de **Integração**: API real com `WebApplicationFactory`
- ✅ Testes de **Validação**: DTOs com FluentValidation
- ✅ Testes de **Erro/Exceção**: duplicidade, conta inexistente, saldo insuficiente etc.

---

### 📌 Estrutura do projeto

```
DigitalBank.API          --> Camada de apresentação (Controllers, Swagger, Middlewares)
DigitalBank.Application  --> Camada de aplicação (DTOs, Interfaces, Services, Validations)
DigitalBank.Domain       --> Camada de domínio (Entidades e regras de negócio)
DigitalBank.Infra.Data   --> Acesso a dados com EF Core + SQLite
DigitalBank.Infra.IoC    --> Injeção de dependência
DigitalBank.Util         --> Enums, extensões, helpers
DigitalBank.Tests        --> Projeto de testes automatizados
```

---

## 🔄 Padrão de resposta da API

Todas as respostas seguem o formato abaixo:

```json
{
  "success": true,
  "message": "Conta criada com sucesso",
  "data": {
    // objeto retornado
  }
}
```

---

## 📌 Endpoints principais

| Método | Rota                         | Descrição                       |
|--------|------------------------------|----------------------------------|
| POST   | `/api/conta`                | Criação de conta                |
| GET    | `/api/conta`                | Listagem com filtros            |
| GET    | `/api/conta/{documento}`    | Buscar conta por documento      |
| PUT    | `/api/conta/inativar`       | Inativação de conta             |
| POST   | `/api/transferencia`        | Realizar transferência          |

Todos os endpoints estão disponíveis em:  
📌 [`/swagger`](https://localhost:7053/swagger)

---
