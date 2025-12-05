# Estructura del Proyecto - NFL Fantasy API

## Árbol de Directorios Completo

```
📁 ClaudeDisenoFinalTrabajo/
│
├── 📄 README.md
├── 📄 .gitignore
│
├── 📁 docs/                                        (Documentación)
│
├── 📁 backend/                                     ━━━━━ BACKEND (.NET) ━━━━━
│   │
│   ├── 📄 NFLFantasyAPI.sln                       (Solución principal)
│   │
│   └── 📁 src/
│       │
│       ├── 📁 Migrations/                         (Migraciones antiguas - legacy)
│       │   ├── 20251109230042_cambiarTemporada.cs
│       │   ├── 20251109230042_cambiarTemporada.Designer.cs
│       │   ├── 20251109235649_AgregarTablaJugadores.cs
│       │   ├── 20251109235649_AgregarTablaJugadores.Designer.cs
│       │   └── ApplicationDbContextModelSnapshot.cs
│       │
│       ├── 📁 NFLFantasyAPI.CrossCutting/         ━━━ CAPA: Cross-Cutting Concerns ━━━
│       │   ├── 📄 NFLFantasyAPI.CrossCutting.csproj
│       │   ├── 📄 README.md
│       │   ├── 📄 ServiceResult.cs                (Objeto de resultado para servicios)
│       │   │
│       │   ├── 📁 Configuration/
│       │   │   ├── FileServerSettings.cs          (Configuración de servidor de archivos)
│       │   │   └── JwtSettings.cs                 (Configuración JWT)
│       │   │
│       │   └── 📁 Interface/
│       │       └── IDbContextProvider.cs          (Interfaz para proveer DbContext)
│       │
│       ├── 📁 NFLFantasyAPI.Persistence/          ━━━ CAPA: Persistencia / Data Access ━━━
│       │   ├── 📄 NFLFantasyAPI.Persistence.csproj
│       │   ├── 📄 README.md
│       │   ├── 📄 Class1.cs
│       │   │
│       │   ├── 📁 Context/
│       │   │   └── ApplicationDbContext.cs        (DbContext de Entity Framework)
│       │   │
│       │   ├── 📁 Models/                         (Modelos de base de datos)
│       │   │   ├── Usuario.cs
│       │   │   ├── Liga.cs
│       │   │   ├── Temporada.cs
│       │   │   ├── Semana.cs
│       │   │   ├── EquipoNFL.cs
│       │   │   ├── EquipoFantasy.cs
│       │   │   ├── JugadorNFL.cs
│       │   │   ├── NoticiaJugador.cs
│       │   │   └── BatchResult.cs                 (Resultado de procesamiento batch)
│       │   │
│       │   ├── 📁 Interfaces/                     (Interfaces de repositorios)
│       │   │   ├── IUsuarioRespository.cs
│       │   │   ├── ILigaRepository.cs
│       │   │   ├── ITemporadaRepository.cs
│       │   │   ├── IEquipoNFLRepository.cs
│       │   │   ├── IEquipoFantasyRespository.cs
│       │   │   ├── IJugadorRepository.cs
│       │   │   └── INoticiaJugadorRepository.cs
│       │   │
│       │   ├── 📁 Repositories/                   (Implementación de repositorios)
│       │   │   ├── UsuarioRepository.cs
│       │   │   ├── LigaRepository.cs
│       │   │   ├── TemporadaRepository.cs
│       │   │   ├── EquipoNFLRepository.cs
│       │   │   ├── EquipoFantasyRepository.cs
│       │   │   ├── JugadorRepository.cs
│       │   │   └── NoticiaJugadorRepository.cs
│       │   │
│       │   └── 📁 Migrations/                     (Migraciones de EF Core)
│       │       ├── 20251112053650_MigracionFinal.cs
│       │       ├── 20251112053650_MigracionFinal.Designer.cs
│       │       └── ApplicationDbContextModelSnapshot.cs
│       │
│       ├── 📁 NFLFantasyAPI.Logic/                ━━━ CAPA: Lógica de Negocio ━━━
│       │   ├── 📄 NFLFantasyAPI.Logic.csproj
│       │   ├── 📄 Class1.cs
│       │   ├── 📄 DbContextProvide.cs             (Proveedor de DbContext)
│       │   │
│       │   ├── 📁 DTOs/                           (Data Transfer Objects)
│       │   │   ├── AuthDTOs.cs                    (DTOs de autenticación)
│       │   │   ├── UsuarioResponseDto.cs
│       │   │   ├── Desbloquearcuentadto.cs
│       │   │   ├── LigaDTOs.cs
│       │   │   ├── CrearLigaDto.cs
│       │   │   ├── TemporadaDTO.cs
│       │   │   ├── SemanaDTO.cs
│       │   │   ├── EquipoNFLDTOs.cs
│       │   │   ├── EquipoFantasyDTOs.cs
│       │   │   ├── JugadorDtos.cs
│       │   │   ├── JugadorBatchDto.cs             (DTOs para batch de jugadores)
│       │   │   ├── NoticiaJugadorDTOs.cs
│       │   │   └── ErrorResponseDto.cs
│       │   │
│       │   ├── 📁 Exceptions/                     ✨ NUEVO - Excepciones personalizadas
│       │   │   ├── ValidationException.cs
│       │   │   ├── JugadorNotFoundException.cs
│       │   │   ├── JugadorDuplicadoException.cs
│       │   │   ├── EquipoNFLNotFoundException.cs
│       │   │   ├── InvalidFileException.cs
│       │   │   └── BatchProcessingException.cs
│       │   │
│       │   ├── 📁 Validators/                     ✨ NUEVO - Validadores centralizados
│       │   │   └── JugadorValidator.cs            (Validaciones de jugadores)
│       │   │
│       │   ├── 📁 Interfaces/                     (Interfaces de servicios)
│       │   │   ├── IAuthService.cs
│       │   │   ├── ILigaService.cs
│       │   │   ├── ITemporadaService.cs
│       │   │   ├── IEquipoNFLService.cs
│       │   │   ├── IEquipoFantasyService.cs
│       │   │   ├── IJugadorService.cs
│       │   │   ├── INoticiaJugadorService.cs
│       │   │   └── IBatchFileProcessingService.cs ✨ NUEVO
│       │   │
│       │   └── 📁 Service/                        (Implementación de servicios)
│       │       ├── AuthService.cs                 (Autenticación y usuarios)
│       │       ├── JWTService.cs                  (Generación de tokens JWT)
│       │       ├── LigaService.cs
│       │       ├── TemporadaService.cs
│       │       ├── EquipoNFLService.cs
│       │       ├── EquipoFantasyService.cs
│       │       ├── JugadorService.cs              ♻️ REFACTORIZADO
│       │       ├── NoticiaJugadorService.cs
│       │       └── BatchFileProcessingService.cs  ✨ NUEVO - Manejo de archivos
│       │
│       └── 📁 NFLFantasyAPI.Presentation/         ━━━ CAPA: Presentación / API ━━━
│           ├── 📄 NFLFantasyAPI.Presentation.csproj
│           ├── 📄 README.md
│           ├── 📄 Program.cs                      ♻️ ACTUALIZADO - DI registrada
│           ├── 📄 appsettings.json
│           ├── 📄 appsettings.Development.json
│           │
│           ├── 📁 Controllers/                    (Controladores API REST)
│           │   ├── AuthController.cs              (Endpoints de autenticación)
│           │   ├── LigaController.cs
│           │   ├── TemporadaController.cs
│           │   ├── EquipoNFLController.cs
│           │   ├── EquipoFantasyController.cs
│           │   ├── JugadorController.cs           (Endpoints de jugadores)
│           │   └── NoticiaJugadorController.cs
│           │
│           ├── 📁 Properties/
│           │   └── launchSettings.json
│           │
│           └── 📁 logs/                           (Archivos de log - Serilog)
│               ├── nfl-fantasy-20251126.txt
│               ├── nfl-fantasy-20251126_001.txt
│               └── nfl-fantasy-20251126_002.txt
│
└── 📁 frontend/                                    ━━━━━ FRONTEND (Angular) ━━━━━
    │
    ├── 📄 README.md
    ├── 📄 package.json
    ├── 📄 package-lock.json
    ├── 📄 angular.json
    ├── 📄 tsconfig.json
    ├── 📄 tsconfig.app.json
    ├── 📄 tsconfig.spec.json
    │
    └── 📁 src/
        │
        ├── 📄 index.html                          (HTML principal)
        ├── 📄 main.ts                             (Bootstrap de Angular)
        ├── 📄 styles.css                          (Estilos globales)
        │
        ├── 📁 app/                                (Módulo principal)
        │   ├── app.ts                             (Componente raíz)
        │   ├── app.html
        │   ├── app.css
        │   ├── app.spec.ts
        │   ├── app.config.ts                      (Configuración de la app)
        │   └── app.routes.ts                      (Rutas de la aplicación)
        │
        ├── 📁 models/                             (Modelos TypeScript)
        │   └── item.ts
        │
        ├── 📁 guards/                             (Guards de autenticación)
        │   ├── auth.guard.ts                      (Protección de rutas)
        │   ├── auth.guard.spec.ts
        │   └── admin.guard.ts
        │
        ├── 📁 services/                           (Servicios Angular)
        │   ├── api.ts                             (Servicio base API)
        │   ├── api.spec.ts
        │   ├── authservice.ts                     (Servicio de autenticación)
        │   ├── authservice.spec.ts
        │   ├── jwt.interceptor.ts                 (Interceptor JWT)
        │   ├── liga.service.ts
        │   ├── temporada.service.ts
        │   ├── equipo-nfl.service.ts
        │   ├── equipo-fantasy.service.ts
        │   ├── jugadores.service.ts
        │   └── noticia-jugador.service.ts
        │
        ├── 📁 loginwidgets/                       (Componentes de login)
        │   ├── 📁 login/
        │   │   ├── login.ts
        │   │   ├── login.html
        │   │   ├── login.css
        │   │   └── login.spec.ts
        │   │
        │   └── 📁 register/
        │       ├── register.ts
        │       ├── register.html
        │       ├── register.css
        │       └── register.spec.ts
        │
        ├── 📁 perfil/                             (Componente de perfil)
        │   ├── perfil.ts
        │   ├── perfil.html
        │   └── perfil.css
        │
        └── 📁 mainpage/                           (Componentes principales - post-login)
            │
            ├── mainpage.component.ts              (Layout principal)
            ├── mainpage.html
            ├── mainpage.css
            │
            ├── 📁 sidenav/                        (Menú lateral)
            │   ├── sidenav.ts
            │   ├── sidenav.html
            │   ├── sidenav.css
            │   └── sidenav.spec.ts
            │
            ├── 📁 liga/                           (Gestión de ligas)
            │   ├── liga.ts
            │   ├── liga.html
            │   ├── liga.css
            │   └── liga.spec.ts
            │
            ├── 📁 crear-liga/
            │   ├── crear-liga.ts
            │   ├── crear-liga.html
            │   └── crear-liga.css
            │
            ├── 📁 buscar-unirse-liga/
            │   ├── buscar-unirse-liga.ts
            │   ├── buscar-unirse-liga.html
            │   └── buscar-unirse-liga.css
            │
            ├── 📁 temporada/
            │   ├── temporada.ts
            │   ├── temporada.html
            │   ├── temporada.css
            │   └── temporada.spec.ts
            │
            ├── 📁 equipos-nfl-list/               (Lista de equipos NFL)
            │   ├── equipos-nfl-list.component.ts
            │   ├── equipos-nfl-list.component.html
            │   └── equipos-nfl-list.component.css
            │
            ├── 📁 equipos-nfl-form/               (Formulario equipos NFL)
            │   ├── equipos-nfl-form.component.ts
            │   ├── equipos-nfl-form.component.html
            │   └── equipos-nfl-form.component.css
            │
            ├── 📁 equipos-fantasy-list/           (Lista de equipos Fantasy)
            │   ├── equipos-fantasy-list.ts
            │   ├── equipos-fantasy-list.html
            │   ├── equipos-fantasy-list.css
            │   └── equipos-fantasy-list.spec.ts
            │
            ├── 📁 equipos-fantasy-form/           (Formulario equipos Fantasy)
            │   ├── equipos-fantasy-form.ts
            │   ├── equipos-fantasy-form.html
            │   ├── equipos-fantasy-form.css
            │   └── equipos-fantasy-form.spec.ts
            │
            ├── 📁 jugadores/                      (Lista de jugadores)
            │   ├── jugadores.component.ts
            │   ├── jugadores.component.html
            │   └── jugadores.component.css
            │
            ├── 📁 creacion-manual-de-jugador/     (Crear jugador manual)
            │   ├── form-jugador.ts
            │   ├── form-jugador.html
            │   └── form-jugador.css
            │
            ├── 📁 jugadores-batch/                (Gestión batch de jugadores)
            │   ├── jugadores-batch.ts
            │   ├── jugadores-batch.html
            │   ├── jugadores-batch.css
            │   └── jugadores-batch.spec.ts
            │
            ├── 📁 jugador-batch-upload/           (Upload batch jugadores)
            │   ├── jugador-batch-upload.ts
            │   ├── jugador-batch-upload.html
            │   └── jugador-batch-upload.css
            │
            ├── 📁 noticia-jugador/                (Crear/editar noticias)
            │   ├── noticia-jugador.ts
            │   ├── noticia-jugador.html
            │   ├── noticia-jugador.css
            │   └── noticia-jugador.spec.ts
            │
            └── 📁 ver-noticias/                   (Ver lista de noticias)
                ├── ver-noticias.ts
                ├── ver-noticias.html
                ├── ver-noticias.css
                └── ver-noticias.spec.ts
```

## Arquitectura del Backend (Capas)

```
┌─────────────────────────────────────────────────────────────┐
│  NFLFantasyAPI.Presentation (API Layer)                    │
│  - Controllers (REST API endpoints)                        │
│  - Program.cs (Configuración, DI, Middleware)              │
└────────────────────┬────────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────────┐
│  NFLFantasyAPI.Logic (Business Logic Layer)                │
│  - Services (Lógica de negocio)                            │
│  - Validators ✨ (Validaciones centralizadas)              │
│  - Exceptions ✨ (Excepciones personalizadas)              │
│  - DTOs (Data Transfer Objects)                            │
└────────────────────┬────────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────────┐
│  NFLFantasyAPI.Persistence (Data Access Layer)             │
│  - Repositories (Acceso a datos)                           │
│  - Models (Entidades de DB)                                │
│  - DbContext (Entity Framework Core)                       │
└─────────────────────────────────────────────────────────────┘
                     │
┌────────────────────▼────────────────────────────────────────┐
│  NFLFantasyAPI.CrossCutting (Shared Concerns)              │
│  - ServiceResult (Respuestas uniformes)                    │
│  - Configuration (Settings compartidos)                    │
└─────────────────────────────────────────────────────────────┘
```

## Arquitectura del Frontend (Angular)

```
┌─────────────────────────────────────────┐
│  Core (App, Guards, Interceptors)      │
└───────────────┬─────────────────────────┘
                │
┌───────────────▼─────────────────────────┐
│  Services (API Communication)           │
└───────────────┬─────────────────────────┘
                │
┌───────────────▼─────────────────────────┐
│  Components (UI)                        │
│  - Login/Register                       │
│  - Mainpage (Post-auth features)        │
│  - Ligas, Equipos, Jugadores, Noticias │
└─────────────────────────────────────────┘
```

## Archivos Clave de la Refactorización

### Nuevos
- `backend/src/NFLFantasyAPI.Logic/Exceptions/*.cs` (6 archivos)
- `backend/src/NFLFantasyAPI.Logic/Validators/JugadorValidator.cs`
- `backend/src/NFLFantasyAPI.Logic/Service/BatchFileProcessingService.cs`
- `backend/src/NFLFantasyAPI.Logic/Interfaces/IBatchFileProcessingService.cs`

### Modificados
- `backend/src/NFLFantasyAPI.Logic/Service/JugadorService.cs` (Refactorizado)
- `backend/src/NFLFantasyAPI.Presentation/Program.cs` (DI actualizada)

---

**Última actualización:** 2025-11-26
**Refactorización:** Separación de responsabilidades en módulo de jugadores
