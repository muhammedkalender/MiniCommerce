## Introduction

### 🧪 MiniCommerce Demo Project

This is a demo e-commerce API built with **.NET Core 9**, using a layered architecture and containerized with **Docker
Compose**.  
It includes user and order management, authentication, and background processing support.

### 🚀 Tech Stack

- **.NET Core 9**
- **PostgreSQL**
- **Redis**
- **RabbitMQ**
- **Docker & Docker Compose**
- **Swagger UI** for API documentation
- **NUnit & Moq**
- **Serilog**

### 🗂 Project Structure

```text
MiniCommerce
├── Docs
├── Logs
│   ├── api
│   ├── processor
├── Endpoints
│   ├── MiniCommerce.Api
│   └── MiniCommerce.Api.Tests
│
├── Tiers
│   ├── MiniCommerce.Application
│   ├── MiniCommerce.Domain
│   ├── MiniCommerce.Infrastructure
│   └── MiniCommerce.Infrastructure.Tests
│
├── Workers
│   ├── MiniCommerce.Processor
│   └── MiniCommerce.Processor.Tests
```

---

## 📦 Installation

> **Prerequisites:**
> - [.NET 9 SDK ( If wanna run on local and migrations )](https://dotnet.microsoft.com/download/dotnet/9.0)
> - [Docker and Docker Compose](https://www.docker.com/products/docker-desktop)

---

### 1️⃣ Clone the repository

```bash
git clone https://github.com/muhammedkalender/MiniCommerce.git
cd MiniCommerce
```

### 2️⃣ Run with Docker Compose

Make sure `docker-compose.yml` exists in the **solution root folder** (`MiniCommerce/`).

```bash
docker-compose up --build -d
```

### 3️⃣ Access the API

Once the containers are up, navigate to:

> - [Swagger UI](http://localhost:37000/swagger)
> - Import Postman Collection
>   - /Docs/Postman.Collection.json 
>   - /Docs/Postman.Environment.json 

#### 🔐 Default Credentials

These are used for Basic Authentication inside Swagger or other API tools:

| Username | Password |
|----------|----------|
| admin    | 1234     |

---

## ⚙️ Migration

If your `appsettings.json` contains:

```json
{
  "MockDb": true
}
```

You no need additional actions, with this migration its automatically migrate when first query in api.

If its false follow this steps :

İf `dotnet ef` command not works you should install with

```bash
dotnet tool install --global dotnet-ef
```

### 👤 Example User IDs

| # | User ID                                |
|---|----------------------------------------|
| 1 | `11111111-1111-1111-1111-111111111111` |
| 2 | `22222222-2222-2222-2222-222222222222` |
| 3 | `33333333-3333-3333-3333-333333333333` |
| 4 | `44444444-4444-4444-4444-444444444444` |

---

## 🔐 **Access / Credentials**

| Service        | Username | Password   | Port(s)          | Notes                                 |
|----------------|----------|------------|------------------|---------------------------------------|
| **API**        | `admin`  | `1234`     | `37300`          | Web Api                               |
| **PostgreSQL** | `user`   | `password` | `37310`          | Database: `mini_commerce`             |
| **RabbitMQ**   | `user`   | `password` | `37320`, `37321` | `37321` is the UI (Management Plugin) |
| **Redis**      | –        | –          | `37330`          | No auth configured                    |

> ⚠️ **Note:** All ports declared for access from localhost to  **Docker Container** `appsettings.json` has different host info.