# 📌 Projeto: Integração com Azure Functions e CosmosDB

## 📖 Visão Geral
Este projeto utiliza **Azure Functions** para processar e armazenar dados na nuvem. Ele implementa funções para salvar arquivos no **Storage Account**, armazenar registros no **CosmosDB**, filtrar e listar registros. O objetivo é demonstrar uma arquitetura **serverless** escalável e eficiente na **Azure Cloud**.

## 🚀 Tecnologias Utilizadas
- **Azure Functions**
- **Azure Storage Account**
- **Azure CosmosDB**
- **.NET 6 / .NET 7**
- **C#**
- **Visual Studio Code / Visual Studio 2022**
- **Postman (para testes de API)**
- **Azure Portal / Azure CLI**

---

## 🏗️ Configuração da Infraestrutura na Nuvem

### 1️⃣ Criando a Storage Account
A Storage Account será usada para armazenar arquivos. Execute o seguinte comando no **Azure CLI**:
```sh
az storage account create --name <NOME_STORAGE> --resource-group <NOME_RESOURCE_GROUP> --location eastus --sku Standard_LRS
```
> Substitua `<NOME_STORAGE>` e `<NOME_RESOURCE_GROUP>` pelos valores desejados.

### 2️⃣ Criando o Banco de Dados CosmosDB
Criação de um **CosmosDB com API SQL**:
```sh
az cosmosdb create --name <NOME_COSMOSDB> --resource-group <NOME_RESOURCE_GROUP>
```
Criação de um **Container**:
```sh
az cosmosdb sql database create --account-name <NOME_COSMOSDB> --name <DATABASE_NAME> --resource-group <NOME_RESOURCE_GROUP>
az cosmosdb sql container create --account-name <NOME_COSMOSDB> --database-name <DATABASE_NAME> --name movies --partition-key-path "/id"
```

---

## 🔧 Implementação das Azure Functions

### 📌 1. Criando uma Azure Function para Salvar Arquivos no Storage Account

### 📌 2. Criando uma Azure Function para Salvar Registros no CosmosDB

### 📌 3. Criando uma Azure Function para Filtrar Registros no CosmosDB

### 📌 4. Criando uma Azure Function para Listar Registros do CosmosDB

---

## ✅ Testando as Azure Functions
Use **Postman** ou **cURL** para testar as funções.

### 🔹 **Salvar um Filme** (POST)
```sh
curl -X POST "https://<NOME_FUNCTION>.azurewebsites.net/api/SaveToCosmosDB" \
     -H "Content-Type: application/json" \
     -d '{"title": "Matrix", "year": 1999}'
```

### 🔹 **Filtrar Filmes** (GET)
```sh
curl -X GET "https://<NOME_FUNCTION>.azurewebsites.net/api/movies/Matrix"
```

### 🔹 **Listar Todos os Filmes** (GET)
```sh
curl -X GET "https://<NOME_FUNCTION>.azurewebsites.net/api/movies"
```

---

## 🔗 Referências
- [Documentação Oficial do Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/)
- [CosmosDB Documentation](https://learn.microsoft.com/en-us/azure/cosmos-db/)
