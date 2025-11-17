# 📘 Censudex – Clients Service

Servicio encargado de la **gestión de clientes dentro de la plataforma Censudex**.
Forma parte del ecosistema de microservicios y expone sus funcionalidades exclusivamente mediante **gRPC**, permitiendo que el **API Gateway** consulte, cree, actualice y desactive clientes de forma segura y eficiente.

Este servicio utiliza **PostgreSQL**, implementa validaciones sólidas mediante **FluentValidation**, mapeos limpios mediante **AutoMapper**, y sigue una arquitectura desacoplada basada en patrones de diseño como Repository y DTO Pattern.

---

## 🏛 Arquitectura y Patrón de Diseño

### Arquitectura: Microservicios + Comunicación Síncrona gRPC

El Clients Service implementa:

* Arquitectura de capas (Layered Architecture)
* Persistencia con **PostgreSQL**
* Comunicación síncrona mediante **gRPC**
* Mapeos con **AutoMapper**
* Validaciones con **FluentValidation**
* Separación por capas:

  * DTOs
  * Repositories
  * Validators
  * Models
  * Data Layer (EF Core)

---

## 🧩 Patrones de Diseño Implementados

| Patrón                   | Descripción                                                    |
| ------------------------ | -------------------------------------------------------------- |
| **Repository Pattern**   | Encapsula el acceso a datos y evita acoplamiento con EF Core   |
| **DTO Pattern**          | Controla los datos que entran/salen del servicio               |
| **Dependency Injection** | Manejo limpio de dependencias en cada capa                     |
| **AutoMapper**           | Mapea DTO ↔ Model ↔ gRPC Responses                             |
| **Validation Layer**     | FluentValidation asegura integridad antes de llegar al dominio |
| **gRPC Service Pattern** | Servicio binario eficiente, optimizado para Gateway            |

---

## 🚀 Tecnologías Utilizadas

* **Framework:** ASP.NET Core 9.0
* **Comunicación:** gRPC
* **Base de Datos:** PostgreSQL
* **ORM:** Entity Framework Core
* **Validaciones:** FluentValidation
* **Mapper:** AutoMapper
* **Contenedores:** Docker
* **Versionado:** Git

---

## 🗂 Modelo de Datos

### Entidad Client

```json
{
  "Id": "UUID v4",
  "FullName": "string",
  "Email": "string",
  "Username": "string",
  "PasswordHash": "string",
  "BirthDate": "DateOnly",
  "Address": "string",
  "PhoneNumber": "string",
  "IsActive": "bool",
  "CreatedAt": "DateTime",
  "Role": "string"
}
```

---

## 📡 Endpoints gRPC Disponibles

Puerto por defecto: **[https://localhost:7181](https://localhost:7181)**

| Método                  | Descripción                            |
| ------------------      | -------------------------------        |
| `CreateClient`          | Crear un nuevo cliente                 |
| `GetClientById`         | Obtener cliente mediante UUID          |
| `GetAllClients`         | Obtener lista de clientes              |
| `UpdateClient`          | Actualizar un cliente existente        |
| `DeactivateClient`      | Desactivar un cliente                  |
| `GetClientByIdentifier` | Obtener cliente según username o email |

---

## ✔ Validaciones con FluentValidation

Las siguientes reglas están implementadas:

### ClientCreateValidator

* Email obligatorio y formato válido
* Username mínimo 3 caracteres
* Password con seguridad mínima
* Teléfono con longitud válida
* BirthDate válida
* FullName obligatorio
* Email no duplicado (validación en repositorio)

### ClientUpdateValidator

* Validación flexible (solo los campos enviados)
* Revalidación de reglas básicas

Estas validaciones se ejecutan manualmente dentro del **ClientsGrpcService**.

---

## 🔁 Mapeos AutoMapper

### ClientCreateDto → Client

### Client → ClientDto

### Client → ClientResponse (gRPC)

Mapas definidos en **ClientProfile.cs**.

---

## 🛠 Instalación y Configuración

### 1. Crear archivo `.env`

Crea un archivo **.env** en la raíz del proyecto:

```
POSTGRES_HOST=localhost
POSTGRES_PORT=5432
POSTGRES_DB=clients_db
POSTGRES_USER=censudex
POSTGRES_PASSWORD=censudex123
```

También existe un archivo **.env.example** como referencia.

---

### 2. Levantar PostgreSQL

```bash
docker-compose up -d
```

Esto iniciará la base de datos requerida por el servicio.

---

### 3. Ejecutar migraciones (solo primera vez)

```bash
dotnet ef database update
```

---

### 4. Ejecutar el servicio

```bash
dotnet run
```

El servicio estará disponible en:

```
https://localhost:7181   (gRPC)
```

---

## 🔍 Probar gRPC en Postman

1. Abrir Postman → New → gRPC Request
2. URL:

```
https://localhost:7181
```

3. Importar archivo `clients.proto`
4. Elegir el método a probar
5. Click en **Invoke**

---

## 🧪 Ejemplos de Requests gRPC

### ✔ CreateClient

```json
{
  "fullName": "Yamir Castillo",
  "email": "yamir@example.com",
  "username": "yamircv",
  "birthDate": "2002-03-17",
  "address": "Street 123",
  "phoneNumber": "+56912345678",
  "password": "SuperSegura123!"
}
```

---

### ✔ GetClientById

```json
{
  "id": "5fa571dc-4b93-4ccf-acb5-f1f294d2863e"
}
```

---

### ✔ UpdateClient

```json
{
  "id": "5fa571dc-4b93-4ccf-acb5-f1f294d2863e",
  "fullName": "Nuevo Nombre",
  "phoneNumber": "+56999999999"
}
```

---

### ✔ DeactivateClient

```json
{
  "id": "5fa571dc-4b93-4ccf-acb5-f1f294d2863e"
}
```

---

### ✔ GetClientByIdentifier

```json
{
    "identifier": "yamircv"
}
```

---

## 📦 Estructura del Proyecto

```
Src/
 ├── Services/
 │   └── ClientsGrpcService.cs
 ├── Repositories/
 │   └── ClientRepository.cs
 ├── DTOs/
 │   ├── ClientDto.cs
 │   ├── ClientCreateDto.cs
 │   └── ClientUpdateDto.cs
 ├── Extensions/
 │   └── ClientExtensions.cs
 ├── Grpc/
 │   └── ClientsGrpcService.cs
 ├── Interfaces/
 │   └── IClientRepository.cs
 ├── Validators/
 │   ├── ClientCreateValidator.cs
 │   └── ClientUpdateValidator.cs
 ├── Profiles/
 │   ├── ClientProfile.cs
 │   └── GrpcClientProfile.cs
 ├── Models/
 │   └── Client.cs
 ├── Profiles/
 │   ├── ClientProfile.cs
 │   └── GrpcClientProfile.cs
 └── Data/
     ├── AppDbContext.cs
     ├── DataSeeder.cs
     └── Migrations/
Protos/
 └── clients.proto
```

---
