# EcoRoad Back-End

## Descripción

EcoRoad es una plataforma orientada a la gestión y supervisión
ambiental de proyectos de infraestructura vial.

Este repositorio contiene el backend de la aplicación, encargado
de procesar las solicitudes del frontend, aplicar las reglas de
negocio y gestionar la persistencia de información.

## Tecnologías utilizadas

* C# y ASP.NET Core: desarrollo de la API REST.
* Entity Framework Core: acceso y gestión de datos.
* MySQL: sistema de gestión de base de datos.
* JWT: autenticación y autorización.
* OpenAPI y Scalar: documentación de la API.
* Docker: contenerización de la aplicación.

## Arquitectura del backend

El proyecto está organizado en módulos según las
responsabilidades de cada dominio.

### Módulos principales

|Módulo|Responsabilidad|
|-|-|
|Projects|Gestión de proyectos de infraestructura.|
|Monitoring|Registro y consulta de mediciones ambientales.|
|Assets|Gestión de sensores y activos.|
|Alerting|Evaluación y gestión de alertas ambientales.|
|Incidents|Seguimiento de incidentes.|
|Compliance|Generación de información para reportes de cumplimiento.|
|Iam|Gestión de usuarios, autenticación y autorización.|
|Commercial|Gestión de planes y suscripciones.|
|Requests|Componentes relacionados con las solicitudes del sistema.|
|Shared|Componentes compartidos entre módulos.|

## Estructura general

* `EcoRoad-platform/`: código fuente del backend.
* `EcoRoad-platform.sln`: solución de .NET.
* `Dockerfile`: configuración para construir el contenedor.
* `global.json`: configuración de la versión del SDK de .NET.
* `docs/`: documentación complementaria.

## Requisitos de desarrollo

Para trabajar con el backend se requiere:

1. Un SDK de .NET compatible con `global.json`.
2. Un entorno de desarrollo como JetBrains Rider.
3. Git para el control de versiones.
4. Una instancia de MySQL de desarrollo para las operaciones que
requieran persistencia.

## Restauración y compilación

Desde la carpeta raíz del repositorio, ejecutar:

```bash
dotnet restore EcoRoad-platform.sln
dotnet build EcoRoad-platform.sln --no-restore
```

Estos comandos permiten restaurar las dependencias y comprobar
la compilación del proyecto.

## Configuración de la base de datos

La conexión con MySQL se configura mediante los mecanismos
de configuración de ASP.NET Core.

La variable de entorno correspondiente es:

`ConnectionStrings\_\_DefaultConnection`

Las credenciales y cadenas de conexión reales no deben
publicarse en el repositorio.

Importante: el inicio de la aplicación puede ejecutar migraciones
de Entity Framework Core. Por ello, las pruebas locales deben
utilizar una base de datos de desarrollo aislada.

## Integración con el frontend

El backend proporciona los endpoints REST utilizados por
el frontend de EcoRoad.

La comunicación se realiza mediante solicitudes HTTP,
respetando los contratos de las API existentes.

Repositorio del frontend:

https://github.com/Aplicaciones-Web-1ASI0730-2620-8130/EcoRoad-FrontEnd

## Control de versiones

El desarrollo del proyecto se realiza mediante Git y GitHub.

Los cambios deben verificarse antes de integrarse a la rama
de desarrollo, procurando mantener la compatibilidad con
el frontend y la base de datos.
