# CONTEXTO COMPLETO: IMPLEMENTACIÓN DE PRUEBAS UNITARIAS
## Proyecto: New Generation NFL Fantasy - Sprint 4

---

## 📋 INFORMACIÓN GENERAL DEL PROYECTO

**Proyecto:** New Generation NFL Fantasy  
**Tipo:** Plataforma de fantasy football para la NFL  
**Curso:** CE-1116 Diseño y Calidad de Productos Tecnológicos  
**Estudiante:** José Andrés Vargas Torres  
**Sprint:** Sprint 4  
**Funcionalidad objetivo:** Implementación de Pruebas Unitarias (5% del sprint)

**Stack Tecnológico:**
- **Backend:** C# .NET 9
- **Frontend:** Angular 20 con TypeScript
- **Base de datos:** PostgreSQL (Entity Framework Core)
- **Herramienta IA:** Cursor (IDE con IA integrada)

---

## 🏗️ ARQUITECTURA DEL BACKEND

El backend sigue una arquitectura en capas (Clean Architecture):

### Capa 1: Presentation (API Layer)
**Ubicación:** `backend/src/NFLFantasyAPI.Presentation/`
- **Responsabilidad:** Controllers REST API, configuración, middleware
- **Archivos clave:**
  - `Program.cs` - Configuración de la aplicación, DI, middleware
  - `Controllers/` - Endpoints REST (AuthController, JugadorController, etc.)
  - `appsettings.json` - Configuración de la aplicación

### Capa 2: Logic (Business Logic Layer)
**Ubicación:** `backend/src/NFLFantasyAPI.Logic/`
- **Responsabilidad:** Lógica de negocio, servicios, validaciones, DTOs
- **Estructura:**
  - `Service/` - Servicios de negocio (JugadorService, AuthService, etc.)
  - `Interfaces/` - Interfaces de servicios (IJugadorService, IAuthService, etc.)
  - `DTOs/` - Data Transfer Objects
  - `Validators/` - Validadores centralizados (JugadorValidator)
  - `Exceptions/` - Excepciones personalizadas (JugadorNotFoundException, etc.)

### Capa 3: Persistence (Data Access Layer)
**Ubicación:** `backend/src/NFLFantasyAPI.Persistence/`
- **Responsabilidad:** Acceso a datos, repositorios, modelos de base de datos
- **Estructura:**
  - `Repositories/` - Implementación de repositorios
  - `Interfaces/` - Interfaces de repositorios (IJugadorRepository, etc.)
  - `Models/` - Entidades de base de datos (Jugador, Usuario, Liga, etc.)
  - `Context/ApplicationDbContext.cs` - DbContext de Entity Framework

### Capa 4: CrossCutting (Shared Concerns)
**Ubicación:** `backend/src/NFLFantasyAPI.CrossCutting/`
- **Responsabilidad:** Componentes compartidos
- **Archivos clave:**
  - `ServiceResult.cs` - Objeto de resultado uniforme para servicios
  - `Configuration/` - Configuraciones compartidas (JwtSettings, FileServerSettings)

---

## 🎯 SERVICIOS PRINCIPALES A PROBAR

### 1. JugadorService
**Ubicación:** `backend/src/NFLFantasyAPI.Logic/Service/JugadorService.cs`  
**Interfaz:** `backend/src/NFLFantasyAPI.Logic/Interfaces/IJugadorService.cs`

**Dependencias inyectadas:**
- `IJugadorRepository` - Repositorio de jugadores
- `IEquipoNFLRepository` - Repositorio de equipos NFL
- `ILogger<JugadorService>` - Logger para registro de eventos
- `JugadorValidator` - Validador centralizado de jugadores
- `IBatchFileProcessingService` - Servicio de procesamiento de archivos batch
- `IWebHostEnvironment` - Entorno de hosting
- `IOptions<FileServerSettings>` - Configuración del servidor de archivos

**Métodos principales:**
- `GetAllAsync()` - Obtiene todos los jugadores
- `GetByIdAsync(int id)` - Obtiene un jugador por ID (tiene validación y manejo de excepciones)
- `CreateAsync(CrearJugadorDto dto)` - Crea un nuevo jugador
- `UpdateAsync(int id, ActualizarJugadorDto dto)` - Actualiza un jugador
- `DeleteAsync(int id, bool permanente)` - Elimina o desactiva un jugador
- `GetByEquipoAsync(int equipoId)` - Obtiene jugadores por equipo
- `GetByPosicionAsync(string posicion)` - Obtiene jugadores por posición
- `ProcessBatchFileAsync(IFormFile file)` - Procesa archivos batch de jugadores
- `SubirImagenAsync(int id, IFormFile imagen)` - Sube imagen de jugador
- `SubirThumbnailAsync(int id, IFormFile thumbnail)` - Sube thumbnail de jugador

**Excepciones personalizadas usadas:**
- `JugadorNotFoundException` - Cuando el jugador no existe
- `JugadorDuplicadoException` - Cuando el jugador ya existe
- `EquipoNFLNotFoundException` - Cuando el equipo NFL no existe
- `ValidationException` - Errores de validación
- `InvalidFileException` - Archivo inválido

**Ejemplo de método a probar (GetByIdAsync):**
```csharp
public async Task<ServiceResult> GetByIdAsync(int id)
{
    try
    {
        var jugador = await _jugadorRepository.GetByIdAsync(id);
        if (jugador == null)
            throw new JugadorNotFoundException(id);

        var dto = new JugadorResponseDto
        {
            Id = jugador.Id,
            Nombre = jugador.Nombre,
            Posicion = jugador.Posicion,
            EquipoNFLId = jugador.EquipoNFLId,
            NombreEquipoNFL = jugador.EquipoNFL.Nombre,
            CiudadEquipoNFL = jugador.EquipoNFL.Ciudad,
            ImagenUrl = jugador.ImagenUrl,
            ThumbnailUrl = jugador.ThumbnailUrl,
            Estado = jugador.Estado,
            FechaCreacion = jugador.FechaCreacion,
            FechaActualizacion = jugador.FechaActualizacion
        };

        return ServiceResult.Ok(dto);
    }
    catch (JugadorNotFoundException ex)
    {
        _logger.LogWarning(ex.Message);
        return ServiceResult.BadRequest(ex.Message);
    }
}
```

**Casos de prueba necesarios para GetByIdAsync:**
1. Caso exitoso: jugador existe y se retorna correctamente con todos los campos del DTO
2. Caso de error: jugador no existe (debe lanzar JugadorNotFoundException y retornar BadRequest)
3. Verificar mapeo correcto: todos los campos del DTO se mapean correctamente desde la entidad

### 2. AuthService
**Ubicación:** `backend/src/NFLFantasyAPI.Logic/Service/AuthService.cs`  
**Interfaz:** `backend/src/NFLFantasyAPI.Logic/Interfaces/IAuthService.cs`

**Dependencias inyectadas:**
- `IUsuarioRepository` - Repositorio de usuarios
- `ILogger<AuthService>` - Logger
- `IOptions<JwtSettings>` - Configuración JWT

**Métodos principales:**
- `RegisterAsync(RegistroDto dto)` - Registra un nuevo usuario
- `LoginAsync(LoginDto dto)` - Autentica un usuario y genera token JWT
- `DesbloquearCuentaAsync(string email)` - Desbloquea una cuenta bloqueada
- `GetUsuariosAsync()` - Obtiene todos los usuarios
- `GetUsuarioAsync(int id)` - Obtiene un usuario por ID
- `DeleteUsuarioAsync(int id)` - Elimina un usuario

---

## 🎨 ARQUITECTURA DEL FRONTEND

**Ubicación:** `frontend/`

**Estructura principal:**
- `src/app/` - Módulo principal de Angular
- `src/services/` - Servicios Angular (comunicación con API)
- `src/guards/` - Guards de autenticación
- `src/mainpage/` - Componentes principales (post-login)
- `src/loginwidgets/` - Componentes de login/registro

### Servicio principal a probar: Authservice
**Ubicación:** `frontend/src/services/authservice.ts`  
**Archivo de prueba existente:** `frontend/src/services/authservice.spec.ts`

**Dependencias:**
- `HttpClient` - Para llamadas HTTP
- `Router` - Para navegación

**Métodos principales:**
- `login(credentials)` - Realiza login y guarda datos en localStorage
- `register(registroDto)` - Registra un nuevo usuario
- `logout()` - Cierra sesión y limpia localStorage
- `isLoggedIn()` - Verifica si hay un usuario logueado
- `checkSessionValidity()` - Verifica si la sesión sigue válida (verifica expiración de tokens)

**Herramientas de testing disponibles:**
- Jasmine (framework de testing)
- Karma (test runner)
- HttpClientTestingModule (para mockear HTTP)
- RouterTestingModule (para mockear Router)

---

## 🛠️ FRAMEWORKS Y HERRAMIENTAS DE TESTING

### Backend (C# .NET 9)
**Frameworks ya instalados:**
- **xUnit** - Framework de testing para .NET (versión 2.9.3)
- **Moq** - Framework para crear mocks (versión 4.20.72)
- **Microsoft.NET.Test.Sdk** - SDK de testing (versión 18.0.0)
- **xunit.runner.visualstudio** - Runner para Visual Studio (versión 3.1.5)

**Ubicación de dependencias:**
- Verificar en: `backend/src/NFLFantasyAPI.Presentation/README.md`
- O en: `backend/src/NFLFantasyAPI.Presentation/NFLFantasyAPI.Presentation.csproj`

### Frontend (Angular 20)
**Frameworks ya configurados:**
- **Jasmine** - Framework de testing (incluido en Angular CLI)
- **Karma** - Test runner (incluido en Angular CLI)
- **HttpClientTestingModule** - Para mockear HttpClient
- **RouterTestingModule** - Para mockear Router

**Comando para ejecutar pruebas:**
- Backend: `dotnet test`
- Frontend: `ng test`

---

## 📦 DEPENDENCIAS Y MODELOS CLAVE

### ServiceResult
**Ubicación:** `backend/src/NFLFantasyAPI.CrossCutting/ServiceResult.cs`

Clase que encapsula resultados de servicios:
- `ServiceResult.Ok(data)` - Resultado exitoso
- `ServiceResult.BadRequest(message)` - Error de validación
- `ServiceResult.Error(message)` - Error interno

### DTOs principales
**Ubicación:** `backend/src/NFLFantasyAPI.Logic/DTOs/`

- `JugadorResponseDto` - DTO de respuesta de jugador
- `JugadorListDto` - DTO de lista de jugadores
- `CrearJugadorDto` - DTO para crear jugador
- `ActualizarJugadorDto` - DTO para actualizar jugador
- `LoginDto`, `RegistroDto` - DTOs de autenticación

### Excepciones personalizadas
**Ubicación:** `backend/src/NFLFantasyAPI.Logic/Exceptions/`

- `JugadorNotFoundException` - Jugador no encontrado
- `JugadorDuplicadoException` - Jugador duplicado
- `EquipoNFLNotFoundException` - Equipo NFL no encontrado
- `ValidationException` - Error de validación
- `InvalidFileException` - Archivo inválido
- `BatchProcessingException` - Error en procesamiento batch

---

## 🎯 OBJETIVO DEL SPRINT 4

**Funcionalidad:** Implementación de Pruebas Unitarias (5% del sprint)

**Requisitos:**
1. Implementar pruebas unitarias para servicios del backend (C#)
2. Implementar pruebas unitarias para servicios y componentes del frontend (Angular)
3. Seguir las mejores prácticas de testing
4. Asegurar buena cobertura de código
5. Probar casos de éxito, casos de error y casos límite

**Servicios prioritarios:**
- Backend: JugadorService, AuthService
- Frontend: Authservice (mejorar pruebas existentes)

---

## 📝 PATRÓN AAA (ARRANGE-ACT-ASSERT)

Todas las pruebas deben seguir el patrón AAA:

1. **Arrange (Preparar):**
   - Configurar mocks de dependencias (usando Moq)
   - Definir datos de prueba
   - Configurar el comportamiento esperado de los mocks

2. **Act (Actuar):**
   - Ejecutar el método que se está probando
   - Capturar el resultado

3. **Assert (Verificar):**
   - Verificar que el resultado es el esperado
   - Verificar que se llamaron los métodos correctos en los mocks
   - Verificar que se lanzaron las excepciones correctas

---

## 🔍 CASOS DE PRUEBA ESPECÍFICOS

### Para JugadorService.GetByIdAsync:

**Caso 1: Jugador existe (éxito)**
- Arrange: Mock de repositorio retorna un jugador válido con EquipoNFL
- Act: Llamar GetByIdAsync con ID válido
- Assert: 
  - ServiceResult es Ok
  - DTO contiene todos los campos correctos
  - Se mapearon correctamente todos los campos

**Caso 2: Jugador no existe (error)**
- Arrange: Mock de repositorio retorna null
- Act: Llamar GetByIdAsync con ID inexistente
- Assert:
  - ServiceResult es BadRequest
  - Se lanzó JugadorNotFoundException
  - Logger registró el warning

**Caso 3: EquipoNFL es null (caso límite)**
- Arrange: Mock de repositorio retorna jugador pero EquipoNFL es null
- Act: Llamar GetByIdAsync
- Assert: Manejar el caso apropiadamente (puede lanzar excepción o retornar error)

---

## 🎓 MEJORES PRÁCTICAS A SEGUIR

1. **Aislamiento:** Cada prueba debe ser independiente
2. **Nombres descriptivos:** Los nombres de las pruebas deben describir qué se prueba
3. **Setup compartido:** Usar constructores o métodos de setup para reducir duplicación
4. **Mocking apropiado:** Mockear solo dependencias externas, no el código bajo prueba
5. **Cobertura:** Probar casos de éxito, error y límite
6. **Legibilidad:** Las pruebas deben ser fáciles de leer y entender

---

## 📂 ESTRUCTURA DE ARCHIVOS DE PRUEBA

### Backend
**Ubicación sugerida:** `backend/src/NFLFantasyAPI.Logic.Tests/` (proyecto de pruebas separado)
O dentro del mismo proyecto: `backend/src/NFLFantasyAPI.Logic/Service/Tests/`

**Ejemplo de nombre:** `JugadorServiceTests.cs`

### Frontend
**Ubicación:** Junto al archivo fuente con extensión `.spec.ts`
**Ejemplo:** `authservice.spec.ts` (ya existe)

---

## 🚀 COMANDOS ÚTILES

```bash
# Backend - Ejecutar pruebas
dotnet test

# Backend - Ejecutar pruebas con cobertura (si está configurado)
dotnet test --collect:"XPlat Code Coverage"

# Frontend - Ejecutar pruebas
ng test

# Frontend - Ejecutar pruebas una vez (sin watch)
ng test --watch=false
```

---

## 📋 CHECKLIST DE IMPLEMENTACIÓN

- [ ] Crear proyecto de pruebas para backend (si no existe)
- [ ] Instalar/verificar dependencias: xUnit, Moq
- [ ] Crear pruebas para JugadorService.GetByIdAsync
- [ ] Crear pruebas para otros métodos de JugadorService
- [ ] Crear pruebas para AuthService
- [ ] Mejorar pruebas existentes de Authservice (frontend)
- [ ] Agregar pruebas para checkSessionValidity
- [ ] Verificar que todas las pruebas pasan
- [ ] Refactorizar pruebas para reducir duplicación
- [ ] Agregar más casos límite

---

**Última actualización:** Diciembre 2024  
**Para uso en:** Video de difusión - Sprint 4

