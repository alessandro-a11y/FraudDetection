# FraudDetection API

API REST para detecção de fraudes em transações financeiras, construída com .NET 10 e arquitetura limpa.

## Tecnologias

- .NET 10
- PostgreSQL + Entity Framework Core
- ASP.NET Identity + JWT
- Serilog
- Clean Architecture

## Arquitetura

```
FraudDetection.API            → Controllers, configuração, entry point
FraudDetection.Application    → Use Cases, Services, DTOs
FraudDetection.Domain         → Entidades, Interfaces, Regras de negócio
FraudDetection.Infrastructure → DbContext, Repositórios, Identity
FraudDetection.Worker         → Processamento em background
```

## Funcionalidades

- Autenticação com JWT e roles (Admin/User)
- Análise de risco por motor de regras desacoplado
- Persistência no PostgreSQL com migrations automáticas
- Logs estruturados com Serilog (console + arquivo)

## Motor de Regras

Cada regra de fraude é isolada e implementa `IFraudRule`:

| Regra | Critério | Score |
|---|---|---|
| HighAmountRule | Valor acima de R$5.000 | +40 |
| HighAmountRule | Valor acima de R$10.000 | +30 |
| LocationMismatchRule | Localização fora do BR | +30 |
| VelocityRule | 3+ transações em 1 minuto | +20 |

Score final determina o nível de risco: LOW / MEDIUM / HIGH.

## Como rodar

**Pré-requisitos:** .NET 10, PostgreSQL

```bash
# Configurar connection string em appsettings.json
# Rodar migrations
dotnet ef database update --project FraudDetection.Infrastructure --startup-project FraudDetection.API

# Iniciar API
dotnet run --project FraudDetection.API
```

## Endpoints

```
POST /api/v1/auth/register   Registro de usuário
POST /api/v1/auth/login      Login e geração de token
POST /api/v1/transaction     Criar e analisar transação (requer JWT)
```

## Exemplo de uso

```bash
# Login
curl -X POST http://localhost:5184/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@email.com","password":"senha123"}'

# Criar transação
curl -X POST http://localhost:5184/api/v1/transaction \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"userId":"user@email.com","amount":12000,"location":"US"}'
```

## Níveis de risco

| Score | Nível |
|---|---|
| 0 - 30 | LOW |
| 31 - 70 | MEDIUM |
| 71+ | HIGH |
