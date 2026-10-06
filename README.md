# VisionCare Backend

API REST de VisionCare, proyecto final del curso Seminario de Investigacion Academica (Ingenieria de Software, UPC, 2025). VisionCare ayuda a las personas a gestionar sus medicamentos: reconoce el texto de la etiqueta de un medicamento a partir de una foto, lo interpreta con un modelo de lenguaje y permite programar recordatorios de toma.

El backend da servicio a la app movil VisionCare (Flutter) y esta desplegado en Azure App Service con despliegue continuo desde GitHub Actions.

## Funcionalidades

- Registro, inicio de sesion, refresh token y cierre de sesion con JWT (token por header o cookie HttpOnly)
- Contrasenas con hash BCrypt
- Reconocimiento de texto en imagenes de medicamentos con Azure AI Vision (OCR)
- Interpretacion del texto reconocido con OpenAI (GPT)
- Gestion de medicamentos por usuario y de sus horarios de toma

## Arquitectura

Monolito modular organizado por bounded contexts siguiendo Domain Driven Design. Cada contexto separa sus capas:

```
VisionCareCore/
├── User/         Autenticacion, usuarios, JWT, BCrypt
├── HealthCare/   Medicamentos y horarios de toma
├── Vision/       Reconocimiento de imagenes con Azure AI Vision
├── OpenAI/       Integracion con GPT
└── Shared/       AppDbContext, repositorio base, Unit of Work, convenciones de EF Core
```

| Capa | Responsabilidad |
|---|---|
| Domain | Entidades, agregados, interfaces de repositorios y servicios |
| Application | Command services y query services (separacion de lecturas y escrituras) |
| Infrastructure | EF Core sobre SQL Server, clientes de APIs externas, hashing y tokens |
| Interfaces | Controllers REST, recursos y assemblers; ACL entre contextos |

## Endpoints principales

| Metodo | Ruta | Descripcion |
|---|---|---|
| POST | /amsac/v1/authentication/sign-up | Registro de usuario |
| POST | /amsac/v1/authentication/sign-in | Inicio de sesion, devuelve JWT |
| POST | /amsac/v1/authentication/refresh-token | Renueva el token |
| POST | /amsac/v1/authentication/sign-out | Cierra la sesion |
| GET | /amsac/v1/AuthUser/me | Usuario autenticado |
| POST | /vc/v1/vision/recognize-image | OCR de la foto de un medicamento |
| POST | /vc/v1/gpt/test | Procesa texto con GPT |
| POST | /api/Medicine | Registra un medicamento |
| GET | /api/Medicine/user/{userId} | Medicamentos de un usuario |
| POST | /api/medicine-time | Programa un horario de toma |
| GET | /api/medicine-time/by-medicine/{medicineId} | Horarios de un medicamento |
| DELETE | /api/medicine-time/{id} | Elimina un horario |

La documentacion completa esta disponible en Swagger al ejecutar la API.

## Stack

- .NET 8, ASP.NET Core Web API
- Entity Framework Core 8 con SQL Server
- JWT Bearer, BCrypt.Net
- Azure AI Vision (Image Analysis), OpenAI
- Swagger (Swashbuckle)
- Azure App Service, GitHub Actions con login federado (OIDC)

## Como ejecutarlo

Requisitos: .NET 8 SDK y una instancia de SQL Server.

1. Configura los valores sensibles con user-secrets (nunca en el repositorio):

```bash
cd VisionCareCore
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<cadena de conexion>"
dotnet user-secrets set "TokenSettings:Secret" "<clave de al menos 32 caracteres>"
```

   Ademas, las claves de Azure AI Vision y OpenAI en las secciones que usa appsettings.json.

2. Ejecuta la API:

```bash
dotnet run --project VisionCareCore
```

3. Abre Swagger en la URL que muestra la consola.

## Despliegue

El workflow `.github/workflows/production_visioncarebackend.yml` compila y publica la API en Azure App Service en cada push a la rama `production`, autenticandose con credenciales federadas de Azure (sin secretos de larga duracion en el repositorio).

## Equipo

- Bruno Palomino ([@BrunoPa23](https://github.com/BrunoPa23))
- William Riega ([@Ziggrk](https://github.com/Ziggrk))
