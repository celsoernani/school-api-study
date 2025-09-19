SHELL := /bin/zsh

PROJECT_NAME := school-api
COMPOSE := docker compose

.PHONY: build up down logs ps restart clean api-shell db-shell migrate

build:
	$(COMPOSE) build

up:
	$(COMPOSE) up -d
	$(COMPOSE) ps

down:
	$(COMPOSE) down

logs:
	$(COMPOSE) logs -f --tail=200

ps:
	$(COMPOSE) ps

restart:
	$(COMPOSE) down
	$(COMPOSE) up -d --build

api-shell:
	$(COMPOSE) exec api /bin/bash || $(COMPOSE) exec api /bin/sh

db-shell:
	$(COMPOSE) exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P Your_password123 -Q "SELECT DB_NAME() AS CurrentDatabase;"

# Apply EF migrations inside the container (if dotnet-ef isn't present, it will use CLI)
migrate:
	$(COMPOSE) exec api dotnet ef database update || dotnet ef database update


