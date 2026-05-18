# siscomat-backend

[![Deploy backend](https://github.com/siscomat/siscomat-backend/actions/workflows/deploy.yml/badge.svg?branch=master)](https://github.com/siscomat/siscomat-backend/actions/workflows/deploy.yml)

API REST de SISCOMAT desarrollada en ASP.NET Core, organizada en capas siguiendo el patrón Controlador-Servicio-Repositorio.

## Índice

- [Requisitos](#requisitos)
- [Estructura del repositorio](#estructura-del-repositorio)
- [Configuración](#configuraci%C3%B3n)
- [Ejecución local](#ejecuci%C3%B3n-local)
- [Ejecución con Docker Compose](#ejecuci%C3%B3n-con-docker-compose)
- [Documentación](#documentaci%C3%B3n)
- [GitHub Actions](#github-actions)
- [Contribuidores](#contribuidores)

## Requisitos

- [Git](https://git-scm.com/)
- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [PostgreSQL 16](https://www.postgresql.org/download/)

> Si no se requiere ejecutar este proyecto de forma nativa, ver [Ejecución con Docker Compose](#ejecuci%C3%B3n-con-docker-compose).

## Estructura del repositorio

```
siscomat-backend/
├── Api/
│   ├── Controllers/               # Controladores de la API
│   ├── appsettings.json           # Configuración del proyecto (incluído en el repo)
│   ├── appsettings.Development.json
│   └── Program.cs                 # Punto de entrada de la aplicación
├── Siscomat.Core/                 # Modelos y contratos del dominio
├── Siscomat.Repository/           # Capa de acceso a datos
├── Siscomat.Services/             # Lógica de negocio
├── Siscomat.Tests/                # Pruebas automatizadas
├── docker-compose.yml             # Configuración de Docker para desarrollo
├── Dockerfile                     # Imagen de Docker
└── siscomat.sln                   # Solución de Visual Studio
```

## Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/siscomat/siscomat-backend.git
cd siscomat-backend
```

### 2. Revisar variables de configuración

El backend se configura mediante `Api/appsettings.json`, incluido en el repositorio con valores predeterminados para desarrollo local. Las credenciales de producción se manejan por separado y nunca se exponen en el repositorio.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost; Database=siscomat_db; Username=postgres; Password=pwd"
  },
  "FrontendSettings": {
    "PortalPublicoUrl": "http://localhost:5173",
    "PanelAdminUrl": "http://localhost:5174"
  },
  "MicroservicioSettings": {
    "Url": "http://localhost:8000",
    "ApiKey": "siscomat_token_seguro_2026"
  },
  "Storage": {
    "PlantillasPath": "plantillas"
  }
}
```

Modificar estos valores si se requiere conectar a una base de datos con credenciales distintas.

## Ejecución local

> Se asume que .NET SDK 10 y PostgreSQL 16 están instalados.

Desde la raíz del repositorio, restaurar las dependencias:

```bash
dotnet restore
```

Luego, desde el directorio `Api/`, aplicar las migraciones y levantar el servidor:

```bash
cd Api
dotnet ef database update
dotnet run
```

La API estará disponible en `http://localhost:8086`.

> Las funciones que dependan del microservicio no estarán disponibles hasta que este también esté corriendo. Ver [siscomat-microservicio](https://github.com/siscomat/siscomat-microservicio).

### Pruebas automatizadas

El proyecto cuenta con pruebas automatizadas en NUnit. Para ejecutarlas desde la línea de comandos:

```bash
dotnet test
```

Como alternativa, desde Visual Studio 2022 es posible ejecutarlas visualmente haciendo clic derecho sobre el proyecto `Siscomat.Tests` y seleccionando **Run Tests**.

> Las pruebas también se ejecutan automáticamente en cada push o pull request a `master`.

## Ejecución con Docker Compose

El Docker Compose del backend incluye dos servicios: la API y la base de datos PostgreSQL. Permite levantar uno o ambos sin instalar .NET ni PostgreSQL de forma nativa.

**Requisito:** tener [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y corriendo.

Desde la raíz del repositorio, ejecutar el comando según lo que se necesite:

```bash
docker compose up --build        # ambos servicios
docker compose up --build api    # solo la API
docker compose up --build db     # solo la base de datos
```

| Servicio | URL |
|---|---|
| API | http://localhost:8086 |
| Base de datos | localhost:5432 |

Levantar solo la base de datos es útil cuando se trabaja activamente en el backend: el compose crea el usuario y la base de datos automáticamente, evitando configuraciones manuales y permitiendo recrearla libremente. Si en cambio se trabaja en el frontend y se necesita el backend para pruebas de integración, levantar ambos servicios evita tener que instalar .NET y PostgreSQL de forma nativa.

> Para desarrollo activo en el backend se recomienda la ejecución local, ya que permite acceder a los logs, el linter y el recargado automático.

## Documentación

El backend expone una interfaz interactiva generada con Swagger. Con el proyecto corriendo localmente, está disponible en:

`http://localhost:5056/swagger/index.html`

Desde ahí es posible consultar todos los endpoints, sus parámetros y probarlos directamente desde el navegador.

## GitHub Actions

### `deploy.yml` — Deploy backend

Se ejecuta en cada push o pull request a `master`. Restaura dependencias, compila el proyecto y ejecuta las pruebas. Si todo pasa, despliega el backend al servidor de producción mediante SSH.

## Contribuidores

[![Contribuidores](https://contrib.rocks/image?repo=siscomat/siscomat-backend)](https://github.com/siscomat/siscomat-backend/graphs/contributors)
