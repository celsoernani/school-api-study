SchoolApi

API .NET 8 com EF Core e SQL Server (Azure SQL Edge) para CRUD de Instituição e Aluno.

Pré-requisitos

- .NET 8 SDK (instalado em ~/.dotnet neste setup)
- Docker (via Colima em Apple Silicon)

Subir Docker/Colima e banco

```bash
eval "$(`/opt/homebrew/bin/brew shellenv)"
colima start --cpu 2 --memory 4 --disk 20 --vm-type=vz --arch aarch64
export DOCKER_HOST=unix://$HOME/.colima/docker.sock
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=Your_password123' \
  -p 1433:1433 --name sql1 -d --platform linux/arm64 \
  mcr.microsoft.com/azure-sql-edge:latest
```

Configurar e rodar API

```bash
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$HOME/.dotnet:$HOME/.dotnet/tools:$PATH"
cd "$HOME/school-api"
dotnet ef database update
ASPNETCORE_URLS="http://localhost:5182;https://localhost:7262" \
  dotnet run --project SchoolApi.csproj
```

Testes rápidos (curl)

```bash
curl -sS http://localhost:5182/swagger/index.html | head -n 1
curl -sS http://localhost:5182/institutions | jq .
curl -sS -X POST http://localhost:5182/institutions \
  -H 'Content-Type: application/json' \
  -d '{
    "name":"Escola Modelo",
    "address":"Rua A, 10",
    "city":"São Paulo",
    "state":"SP",
    "postalCode":"01000-000",
    "phone":"+55 11 90000-0000"
  }'
curl -sS -X POST http://localhost:5182/students \
  -H 'Content-Type: application/json' \
  -d '{
    "firstName":"João",
    "lastName":"Pereira",
    "dateOfBirth":"2011-09-01T00:00:00Z",
    "peopleInHousehold":5,
    "notes":"",
    "institutionId":1
  }'
```

Observação: A seed cria uma instituição e dois alunos se o banco estiver vazio.
# school-api-study
