# Good Hamburger

API e client para gerenciamento de cardápio e pedidos de uma hamburgueria.

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

---

## Configuração do banco de dados

Após instalar o PostgreSQL, crie um banco com o nome desejado e atualize a connection string no arquivo `goodhamburger.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=goodhamburger;Username=postgres;Password=sua_senha"
}
```

As migrations são aplicadas automaticamente ao iniciar a API. O banco já é populado com produtos de exemplo via seed.

---

## Rodando o projeto

### Backend (API)

```bash
dotnet run --project goodhamburger.API
```

A API sobe em `http://localhost:5107`. A documentação interativa (Scalar) fica disponível em `http://localhost:5107/scalar` no ambiente de desenvolvimento.

### Client (Blazor)

```bash
dotnet run --project goodhamburger.Client
```

O client sobe em `http://localhost:5178` e consome a API automaticamente.

Para rodar ambos ao mesmo tempo, abra dois terminais separados.

---

## Rodando os testes

```bash
dotnet test goodhamburger.Tests
```

---

## Estrutura do projeto

```
goodhamburger.Domain          → Entidades, enums, interfaces de repositório
goodhamburger.Application     → Serviços de aplicação, DTOs, regras de negócio
goodhamburger.Infrastructure  → EF Core, repositórios, migrations
goodhamburger.API             → Controllers, configuração de DI
goodhamburger.Client          → Interface Blazor Server
goodhamburger.Tests           → Testes unitários (xUnit)
```

---

## Decisões técnicas

**DDD com Clean Architecture**
Optei por separar o projeto em camadas seguindo DDD para isolar melhor as regras de negócio da infraestrutura. Isso facilita a manutenção e torna mais simples expandir o sistema no futuro, seja adicionando novos domínios ou trocando componentes de infraestrutura sem impactar a lógica central.

**Sem middleware global de erros**
Decidi não usar um middleware de tratamento de erros para manter as exceptions simples e rastreáveis diretamente nos controllers. Cada endpoint captura apenas as exceções que fazem sentido para aquele contexto, retornando `400` para erros de negócio e `404` para recursos não encontrados.

**Entity Framework Core com migrations**
Utilizei EF Core para gerenciar o schema do banco via migrations e aproveitar o recurso de seed data, garantindo que o banco já inicie com produtos cadastrados sem precisar de scripts SQL manuais.

**Regra de desconto com HashSet**
A lógica de desconto foi implementada como uma lista de regras declarativas, onde cada regra define um conjunto de tipos de produto necessários e o percentual aplicado. Inicialmente implementei com um `switch`, mas a adição de novas condições tornava o código difícil de manter. A abordagem com `HashSet` e `IsSubsetOf` permite adicionar ou alterar uma regra com uma única linha, sem tocar no resto da lógica.

**xUnit para testes**
Escolhi xUnit pela experiência prévia com o pacote. Os testes cobrem as regras de desconto, validações de pedido e as operações de CRUD do serviço de orders, utilizando Moq para mockar os repositórios.
