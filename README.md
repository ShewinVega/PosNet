# PosNet

PosNet es una solución backend construida bajo el ecosistema de **.NET** (C#) cuyo objetivo central es mostrar una recopilación de buenas prácticas, arquitecturas y lecciones aprendidas de mi experiencia trabajando en diferentes proyectos.

Esta aplicación no es un proyecto estático; es un ecosistema vivo al cual **se le irán agregando nuevas características (features) constantemente**. Cada nueva iteración tiene el propósito de resolver un problema o demostrar un concepto de ingeniería de software con alta calidad.

## 🚀 Enfoque del Proyecto

El desarrollo de PosNet está guiado por pilares fundamentales de excelencia técnica:

- **Clean Architecture:** Se hace una clara separación de responsabilidades dividiendo el sistema en capas (Domain, UseCases, Infrastructure y API). Las dependencias fluyen siempre hacia el interior, hacia el núcleo del dominio, manteniendo la infraestructura y los detalles de entrega aislados.
- **Principios SOLID:** Cada componente, clase e interfaz está diseñado siguiendo los principios SOLID para asegurar que el sistema sea fácil de mantener, extender y probar a lo largo del tiempo.
- **Patrones de Diseño:** A lo largo de la implementación, se hace un uso extensivo de patrones de diseño estructurales, creacionales y de comportamiento (ej. *Repository*, *Factory*, *Dependency Injection*, *Options Pattern*, o enfoques de casos de uso estructurados) para resolver problemas recurrentes de manera elegante.

## 📂 Estructura del Proyecto

A continuación, se detalla la estructura actual de los principales proyectos en la solución:

```text
📂 PosNet (Solution)
 ├── 📂 PosNet.Domain           # Núcleo del negocio: Entidades, Constantes e Interfaces principales
 │    ├── 📂 Constants
 │    ├── 📂 Entities
 │    ├── 📂 Interfaces
 │    └── 📂 Shared
 ├── 📂 PosNet.UseCases         # Reglas de negocio de la aplicación
 │    ├── 📂 Common
 │    ├── 📂 Dtos
 │    ├── 📂 Interfaces
 │    └── 📂 Services
 ├── 📂 PosNet.Infrastructure   # Detalles de implementación, base de datos y utilidades externas
 │    ├── 📂 Authentication
 │    ├── 📂 Configuration
 │    ├── 📂 Middlewares
 │    ├── 📂 Migrations
 │    ├── 📂 Persistence
 │    ├── 📂 ProblemsDetail
 │    ├── 📂 Repositories
 │    └── 📂 Security
 └── 📂 PosNet.Api              # Punto de entrada y capa de presentación (Controladores, Routing)
      └── 📂 Controllers
```

## 🛠️ Cómo Iniciar y Levantar el Proyecto

Asegúrate de tener instalado el [SDK de .NET](https://dotnet.microsoft.com/download) correspondiente a la versión utilizada en este proyecto.

Abre tu terminal favorita (o la Consola del Administrador de Paquetes en Visual Studio) en el directorio raíz (`c:\Proyectos\PosNet`) y ejecuta los siguientes comandos:

1. **Restaurar las dependencias:**
   ```bash
   dotnet restore
   ```

2. **Compilar la solución:**
   ```bash
   dotnet build
   ```

3. **Ejecutar el proyecto principal (API):**
   ```bash
   dotnet run --project PosNet.Api/PosNet.Api.csproj
   ```
   *(El proyecto también cuenta con soporte para ser lanzado desde tu IDE favorito configurando el perfil de desarrollo).*