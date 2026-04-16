# 📦 Faturamento & Estoque - Arquitetura de Microsserviços 

Este projeto é uma solução completa Fullstack para emissão de Notas Fiscais e Controle de Estoque. Ele foi construído com foco estrito em **Resiliência**, **Idempotência**, e **Clean Architecture**, preparado para testes de estresse em um ambiente de Microsserviços.

## 🚀 Tecnologias e Arquitetura

O sistema emprega 2 microsserviços autônomos (.NET) trabalhando com um Client Frontend nativo (Angular):
- **Frontend**: Angular 17+ (Standalone Components), Tailwind CSS, RxJS, Formulários Reativos.
- **Faturamento (API)**: .NET 8, C#, Entity Framework Core, SQLite, **Polly (Resiliência & Retries)**.
- **Estoque (API)**: .NET 8, C#, Entity Framework Core, SQLite, **Controle de Concorrência Otimista (Optimistic Concurrency)**.
- **Infra / Deployment**: Docker & Docker Compose completo com volumes assíncronos.

---

## ⚙️ Como Executar na Máquina Local (Ambiente Dockerizado)

O projeto é "Plug and Play". Na sua máquina, você providenciará a criação dos bancos relacionais com rodagem automática das *Migrations* ao subir pela primeira vez:

1. Clone o repositório em sua máquina.
2. Com o Docker aberto, rode pelo terminal (na raiz do projeto onde está o `compose.yaml`):

```bash
docker compose up -d --build
```
Isso levantará três instâncias:
- **`frontend`** (acessível pelo browser via `http://localhost:4200`)
- **`estoque-api`** (porta 5027)
- **`faturamento-api`** (porta 5169)

Pronto! Os projetos e seus respectivos bancos de dados SQlite nativos já estarão aptos para o uso imediato pela UI padrão.

---

## 🛠️ Detalhes e Diferenciais Técnicos Implementados

Dado que este ambiente simula concorrência transacional entre bases separadas, foram introduzidos três importantes conceitos de Arquitetura de Dados:

### 1. Resiliência por Transient Errors (Polly)
Caso o microsserviço de Estoque subitamente estoure um *Timeout* ou engasgue durante uma *Internal Server Error* na hora do envio da nota (por interrupção de rede), o Gateway de `IEstoqueClient` do **Faturamento** usa o Retry Pattern do Polly para realizar auto-curar sem quebrar a tela do front-end. Lógicas de negócio (Erro 400 ou Id inválido) burlam estrategicamente o Retry, poupando a rede. 

### 2. Idempotência Entre Transações  
Ao enviar o fechamento da nota por evento originado da fila / rede do microsserviço primário, o *Header HTTP* injeta uma **Chave de Idempotência `X-Idempotency-Key`** gerida no banco relacional auxiliar e único do Estoque. Se o Retry for acionado repetidamente, o endpoint previne a emissão dupla e retem saldo (Blindando o conceito de Baixa Indevida por Retentativa).

### 3. Tratamento Contra Concorrência (Lost Updates)
Se dois usuários paralelos simultaneamente tentarem puxar a Nota Fiscal no último saldo=1 pendente, o Estoque detém proteção por meio de **Token de Concorrência Otimista** nativo do EF Core (`DbUpdateConcurrencyException`). Emitindo retorno via código HTTP 409 (Conflict), preservando a saúde financeira do Estoque sem locar toda a base com Locks Pessimistas.
