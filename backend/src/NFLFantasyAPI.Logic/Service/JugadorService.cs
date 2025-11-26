using NFLFantasyAPI.CrossCutting;
using NFLFantasyAPI.Logic.DTOs;
using NFLFantasyAPI.Persistence.Models;
using NFLFantasyAPI.Persistence.Interfaces;
using NFLFantasyAPI.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using NFLFantasyAPI.CrossCutting.Configuration;


namespace NFLFantasyAPI.Logic.Services
{
    public class JugadorService : IJugadorService
    {
        private readonly IJugadorRepository _jugadorRepository;
        private readonly IEquipoNFLRepository _equipoNFLRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<JugadorService> _logger;

        private readonly FileServerSettings _fileServerSettings;


        public JugadorService(
            IJugadorRepository jugadorRepository,
            IEquipoNFLRepository equipoNFLRepository,
            IWebHostEnvironment environment,
            ILogger<JugadorService> logger,
            IOptions<FileServerSettings> fileServerSettings)
        {
            _jugadorRepository = jugadorRepository;
            _equipoNFLRepository = equipoNFLRepository;
            _environment = environment;
            _logger = logger;
            _fileServerSettings = fileServerSettings.Value;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var jugadores = await _jugadorRepository.GetAllAsync();

            var dto = jugadores.Select(j => new JugadorListDto
            {
                Id = j.Id,
                Nombre = j.Nombre,
                Posicion = j.Posicion,
                NombreEquipoNFL = j.EquipoNFL.Nombre,
                ThumbnailUrl = j.ThumbnailUrl,
                Estado = j.Estado
            }).ToList();

            return ServiceResult.Ok(dto);
        }

        public async Task<ServiceResult> SubirImagenAsync(int id, IFormFile imagen)
{
    try
    {
        // 1. Validar que el jugador existe
        var jugador = await _jugadorRepository.GetByIdAsync(id);
        if (jugador == null)
            return ServiceResult.BadRequest("Jugador no encontrado");

        // 2. Validar el archivo
        if (imagen == null || imagen.Length == 0)
            return ServiceResult.BadRequest("El archivo está vacío");

        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
        if (!allowedTypes.Contains(imagen.ContentType.ToLower()))
            return ServiceResult.BadRequest("Solo se permiten imágenes JPEG o PNG");

        if (imagen.Length > 5 * 1024 * 1024) // 5 MB
            return ServiceResult.BadRequest("El tamaño máximo permitido es 5 MB");

        // 3. Crear directorio si no existe
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "jugadores");
        Directory.CreateDirectory(uploadsFolder);

        // 4. Generar nombre único y guardar archivo
        var extension = Path.GetExtension(imagen.FileName);
        var fileName = $"{id}_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imagen.CopyToAsync(stream);
        }

        // 5. Actualizar la URL en la base de datos
        jugador.ImagenUrl = $"{_fileServerSettings.BaseUrl}/uploads/jugadores/{fileName}";
        await _jugadorRepository.SaveChangesAsync();

        _logger.LogInformation("Imagen subida exitosamente para el jugador {Id}", id);

        return ServiceResult.Ok(new 
        { 
            mensaje = "Imagen subida exitosamente",
            imagenUrl = jugador.ImagenUrl 
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al subir imagen del jugador {Id}", id);
        return ServiceResult.Error("Error interno del servidor al subir la imagen");
    }
}

public async Task<ServiceResult> SubirThumbnailAsync(int id, IFormFile thumbnail)
{
    try
    {
        // 1. Validar que el jugador existe
        var jugador = await _jugadorRepository.GetByIdAsync(id);
        if (jugador == null)
            return ServiceResult.BadRequest("Jugador no encontrado");

        // 2. Validar el archivo
        if (thumbnail == null || thumbnail.Length == 0)
            return ServiceResult.BadRequest("El archivo está vacío");

        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
        if (!allowedTypes.Contains(thumbnail.ContentType.ToLower()))
            return ServiceResult.BadRequest("Solo se permiten imágenes JPEG o PNG");

        if (thumbnail.Length > 2 * 1024 * 1024) // 2 MB para thumbnails
            return ServiceResult.BadRequest("El tamaño máximo permitido es 2 MB");

        // 3. Crear directorio si no existe
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "jugadores", "thumbnails");
        Directory.CreateDirectory(uploadsFolder);

        // 4. Generar nombre único y guardar archivo
        var extension = Path.GetExtension(thumbnail.FileName);
        var fileName = $"{id}_thumb_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await thumbnail.CopyToAsync(stream);
        }

        // 5. Actualizar la URL en la base de datos
        jugador.ThumbnailUrl = $"{_fileServerSettings.BaseUrl}/uploads/jugadores/thumbnails/{fileName}";
        await _jugadorRepository.SaveChangesAsync();

        _logger.LogInformation("Thumbnail subido exitosamente para el jugador {Id}", id);

        return ServiceResult.Ok(new 
        { 
            mensaje = "Thumbnail subido exitosamente",
            thumbnailUrl = jugador.ThumbnailUrl 
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al subir thumbnail del jugador {Id}", id);
        return ServiceResult.Error("Error interno del servidor al subir el thumbnail");
    }
}

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            var jugador = await _jugadorRepository.GetByIdAsync(id);
            if (jugador == null)
                return ServiceResult.BadRequest("Jugador no encontrado");

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

        public async Task<ServiceResult> CreateAsync(CrearJugadorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Posicion) ||
                dto.EquipoNFLId <= 0)
                return ServiceResult.BadRequest("Todos los campos requeridos deben ser proporcionados");

            if (!await _jugadorRepository.EquipoExistsAsync(dto.EquipoNFLId))
                return ServiceResult.BadRequest("El equipo NFL especificado no existe");

            if (await _jugadorRepository.ExistsInEquipoAsync(dto.Nombre, dto.EquipoNFLId))
                return ServiceResult.BadRequest("Ya existe un jugador con ese nombre en el equipo especificado");

            var jugador = new Jugador
            {
                Nombre = dto.Nombre.Trim(),
                Posicion = dto.Posicion.Trim(),
                EquipoNFLId = dto.EquipoNFLId,
                ImagenUrl = dto.ImagenUrl?.Trim(),
                ThumbnailUrl = dto.ThumbnailUrl?.Trim(),
                Estado = "Activo",
                FechaCreacion = DateTime.UtcNow
            };

            await _jugadorRepository.AddAsync(jugador);

            return ServiceResult.Ok(new { mensaje = "Jugador creado correctamente", jugador.Id });
        }

        public async Task<ServiceResult> UpdateAsync(int id, ActualizarJugadorDto dto)
        {
            var jugador = await _jugadorRepository.GetByIdAsync(id);
            if (jugador == null)
                return ServiceResult.BadRequest("Jugador no encontrado");

            if (dto.EquipoNFLId.HasValue && !await _jugadorRepository.EquipoExistsAsync(dto.EquipoNFLId.Value))
                return ServiceResult.BadRequest("El equipo NFL especificado no existe");

            if (!string.IsNullOrWhiteSpace(dto.Nombre))
            {
                var equipoId = dto.EquipoNFLId ?? jugador.EquipoNFLId;
                if (await _jugadorRepository.ExistsInEquipoAsync(dto.Nombre, equipoId))
                    return ServiceResult.BadRequest("Ya existe un jugador con ese nombre en el equipo");
            }

            jugador.Nombre = dto.Nombre ?? jugador.Nombre;
            jugador.Posicion = dto.Posicion ?? jugador.Posicion;
            jugador.EquipoNFLId = dto.EquipoNFLId ?? jugador.EquipoNFLId;
            jugador.ImagenUrl = dto.ImagenUrl ?? jugador.ImagenUrl;
            jugador.ThumbnailUrl = dto.ThumbnailUrl ?? jugador.ThumbnailUrl;
            jugador.Estado = dto.Estado ?? jugador.Estado;
            jugador.FechaActualizacion = DateTime.UtcNow;

            await _jugadorRepository.UpdateAsync(jugador);

            return ServiceResult.Ok(new { mensaje = "Jugador actualizado correctamente" });
        }

        public async Task<ServiceResult> DeleteAsync(int id, bool permanente)
        {
            var jugador = await _jugadorRepository.GetByIdAsync(id);
            if (jugador == null)
                return ServiceResult.BadRequest("Jugador no encontrado");

            if (permanente)
            {
                await _jugadorRepository.DeleteAsync(jugador);
                return ServiceResult.Ok(new { mensaje = "Jugador eliminado permanentemente" });
            }

            jugador.Estado = "Inactivo";
            jugador.FechaActualizacion = DateTime.UtcNow;
            await _jugadorRepository.UpdateAsync(jugador);
            return ServiceResult.Ok(new { mensaje = "Jugador desactivado correctamente" });
        }

        public async Task<ServiceResult> GetByEquipoAsync(int equipoId)
        {
            if (!await _jugadorRepository.EquipoExistsAsync(equipoId))
                return ServiceResult.BadRequest("Equipo NFL no encontrado");

            var jugadores = await _jugadorRepository.GetByEquipoAsync(equipoId);

            var dto = jugadores.Select(j => new JugadorListDto
            {
                Id = j.Id,
                Nombre = j.Nombre,
                Posicion = j.Posicion,
                NombreEquipoNFL = j.EquipoNFL.Nombre,
                ThumbnailUrl = j.ThumbnailUrl,
                Estado = j.Estado
            }).ToList();

            return ServiceResult.Ok(dto);
        }

        public async Task<ServiceResult> GetByPosicionAsync(string posicion)
        {
            var jugadores = await _jugadorRepository.GetByPosicionAsync(posicion);
            var dto = jugadores.Select(j => new JugadorListDto
            {
                Id = j.Id,
                Nombre = j.Nombre,
                Posicion = j.Posicion,
                NombreEquipoNFL = j.EquipoNFL.Nombre,
                ThumbnailUrl = j.ThumbnailUrl,
                Estado = j.Estado
            }).ToList();

            return ServiceResult.Ok(dto);
        }

        public async Task<JugadorBatchResultDto> ProcessBatchFileAsync(IFormFile file)
        {
            var result = new JugadorBatchResultDto
            {
                Exito = false,
                TotalProcesados = 0,
                TotalExitosos = 0,
                TotalErrores = 0
            };

            byte[]? fileContent = null;

            try
            {
                // 1. Validar que el archivo no esté vacío
                if (file == null || file.Length == 0)
                {
                    result.Mensaje = "El archivo está vacío o no es válido";
                    result.Errores.Add(new JugadorBatchErrorDto
                    {
                        Error = "Archivo vacío o no válido"
                    });
                    return result;
                }

                // Guardar el contenido del archivo para luego moverlo
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileContent = memoryStream.ToArray();
                }

                // 2. Leer y parsear el archivo JSON
                JugadorBatchRequestDto? batchRequest;
                try
                {
                    using var stream = new MemoryStream(fileContent);
                    batchRequest = await JsonSerializer.DeserializeAsync<JugadorBatchRequestDto>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                catch (JsonException ex)
                {
                    result.Mensaje = "Error al parsear el archivo JSON: formato inválido";
                    result.Errores.Add(new JugadorBatchErrorDto
                    {
                        Error = $"Formato JSON inválido: {ex.Message}"
                    });
                    var failedPath = await SaveFileToProcessedFolderAsync(file.FileName, fileContent, false);
                    result.ArchivoMovidoA = failedPath;
                    return result;
                }

                if (batchRequest == null || batchRequest.Jugadores == null || !batchRequest.Jugadores.Any())
                {
                    result.Mensaje = "El archivo JSON no contiene jugadores válidos";
                    result.Errores.Add(new JugadorBatchErrorDto
                    {
                        Error = "No se encontraron jugadores en el archivo"
                    });
                    var failedPath = await SaveFileToProcessedFolderAsync(file.FileName, fileContent, false);
                    result.ArchivoMovidoA = failedPath;
                    return result;
                }

                result.TotalProcesados = batchRequest.Jugadores.Count;

                // 3. Validar TODOS los jugadores antes de crear cualquiera
                var validationErrors = await ValidateAllPlayersAsync(batchRequest.Jugadores);

                if (validationErrors.Any())
                {
                    // Si hay errores, NO crear ningún jugador (todo-o-nada)
                    result.Mensaje = $"Se encontraron {validationErrors.Count} errores. No se creó ningún jugador (operación todo-o-nada)";
                    result.TotalErrores = validationErrors.Count;
                    result.Errores = validationErrors.Select(e => new JugadorBatchErrorDto
                    {
                        Id = e.PlayerId,
                        Nombre = e.PlayerName,
                        Error = e.ErrorMessage
                    }).ToList();

                    var failedPath = await SaveFileToProcessedFolderAsync(file.FileName, fileContent, false);
                    result.ArchivoMovidoA = failedPath;
                    return result;
                }

                // 4. Si NO hay errores, crear TODOS los jugadores en una transacción
                var createdPlayers = await CreateAllPlayersInTransactionAsync(batchRequest.Jugadores);

                if (createdPlayers.Any())
                {
                    result.Exito = true;
                    result.TotalExitosos = createdPlayers.Count;
                    result.Mensaje = $"Se crearon exitosamente {createdPlayers.Count} jugadores";
                    result.JugadoresCreados = createdPlayers.Select(j => new JugadorCreatedDto
                    {
                        Id = j.Id,
                        Nombre = j.Nombre,
                        Posicion = j.Posicion,
                        NombreEquipoNFL = j.EquipoNFL?.Nombre ?? "N/A"
                    }).ToList();

                    var successPath = await SaveFileToProcessedFolderAsync(file.FileName, fileContent, true);
                    result.ArchivoMovidoA = successPath;
                }
                else
                {
                    result.Mensaje = "No se pudieron crear los jugadores";
                    result.Errores.Add(new JugadorBatchErrorDto
                    {
                        Error = "Error desconocido al crear jugadores"
                    });
                    var failedPath = await SaveFileToProcessedFolderAsync(file.FileName, fileContent, false);
                    result.ArchivoMovidoA = failedPath;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al procesar el archivo batch de jugadores");
                result.Mensaje = $"Error inesperado: {ex.Message}";
                result.Errores.Add(new JugadorBatchErrorDto
                {
                    Error = $"Error del sistema: {ex.Message}"
                });

                if (file != null && fileContent != null)
                {
                    try
                    {
                        var failedPath = await SaveFileToProcessedFolderAsync(file.FileName, fileContent, false);
                        result.ArchivoMovidoA = failedPath;
                    }
                    catch (Exception moveEx)
                    {
                        _logger.LogError(moveEx, "Error al mover archivo después de un fallo en el procesamiento");
                    }
                }

                return result;
            }
        }

        private async Task<List<BatchValidationError>> ValidateAllPlayersAsync(List<JugadorBatchItemDto> jugadores)
        {
            var errors = new List<BatchValidationError>();

            // Posiciones válidas de la NFL
            var posicionesValidas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "QB", "RB", "WR", "TE", "K", "DEF", "OL", "DL", "LB", "DB", "FB", "P", "LS"
            };

            // Validar IDs duplicados dentro del batch
            var idsEnBatch = new Dictionary<int, int>(); // ID -> contador de apariciones
            foreach (var jugador in jugadores)
            {
                if (idsEnBatch.ContainsKey(jugador.Id))
                {
                    idsEnBatch[jugador.Id]++;
                }
                else
                {
                    idsEnBatch[jugador.Id] = 1;
                }
            }

            var idsDuplicados = idsEnBatch.Where(kvp => kvp.Value > 1).Select(kvp => kvp.Key).ToList();
            if (idsDuplicados.Any())
            {
                foreach (var id in idsDuplicados)
                {
                    var jugadoresConId = jugadores.Where(j => j.Id == id).ToList();
                    foreach (var jugador in jugadoresConId)
                    {
                        errors.Add(new BatchValidationError
                        {
                            PlayerId = jugador.Id,
                            PlayerName = jugador.Nombre,
                            ErrorMessage = $"El ID {id} aparece {idsEnBatch[id]} veces en el archivo. Cada ID debe ser único",
                            ErrorType = "duplicate"
                        });
                    }
                }
            }

            // Obtener todos los IDs de equipos NFL en una sola pasada
            var equipoIds = jugadores.Select(j => j.EquipoNFLId).Distinct().ToList();

            // Validar equipos existentes
            var equiposExistentes = await _equipoNFLRepository.GetAllAsync();
            var equiposExistentesIds = equiposExistentes
                .Where(e => equipoIds.Contains(e.Id))
                .Select(e => e.Id)
                .ToHashSet();

            // Obtener jugadores existentes en los equipos relevantes
            var jugadoresExistentes = new List<Jugador>();
            foreach (var equipoId in equipoIds)
            {
                var jugadoresEquipo = await _jugadorRepository.GetByEquipoAsync(equipoId);
                jugadoresExistentes.AddRange(jugadoresEquipo);
            }

            // Validar cada jugador del batch
            for (int i = 0; i < jugadores.Count; i++)
            {
                var jugador = jugadores[i];

                // Validar que el ID sea positivo
                if (jugador.Id <= 0)
                {
                    errors.Add(new BatchValidationError
                    {
                        PlayerId = jugador.Id,
                        PlayerName = jugador.Nombre ?? "Sin nombre",
                        ErrorMessage = $"El ID debe ser un número positivo mayor a 0 (valor actual: {jugador.Id})",
                        ErrorType = "validation"
                    });
                    continue;
                }

                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(jugador.Nombre))
                {
                    errors.Add(new BatchValidationError
                    {
                        PlayerId = jugador.Id,
                        PlayerName = "Sin nombre",
                        ErrorMessage = "El nombre es requerido",
                        ErrorType = "validation"
                    });
                    continue;
                }

                if (string.IsNullOrWhiteSpace(jugador.Posicion))
                {
                    errors.Add(new BatchValidationError
                    {
                        PlayerId = jugador.Id,
                        PlayerName = jugador.Nombre,
                        ErrorMessage = "La posición es requerida",
                        ErrorType = "validation"
                    });
                    continue;
                }

                // Validar que la posición sea válida
                if (!posicionesValidas.Contains(jugador.Posicion.Trim()))
                {
                    errors.Add(new BatchValidationError
                    {
                        PlayerId = jugador.Id,
                        PlayerName = jugador.Nombre,
                        ErrorMessage = $"La posición '{jugador.Posicion}' no es válida. Posiciones válidas: {string.Join(", ", posicionesValidas)}",
                        ErrorType = "validation"
                    });
                    continue;
                }

                if (jugador.EquipoNFLId <= 0)
                {
                    errors.Add(new BatchValidationError
                    {
                        PlayerId = jugador.Id,
                        PlayerName = jugador.Nombre,
                        ErrorMessage = "El ID del equipo NFL debe ser mayor a 0",
                        ErrorType = "validation"
                    });
                    continue;
                }

                // Validar URL de imagen si está presente
                if (!string.IsNullOrWhiteSpace(jugador.ImagenUrl))
                {
                    if (!Uri.TryCreate(jugador.ImagenUrl, UriKind.Absolute, out var uriResult) ||
                        (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    {
                        errors.Add(new BatchValidationError
                        {
                            PlayerId = jugador.Id,
                            PlayerName = jugador.Nombre,
                            ErrorMessage = $"La URL de imagen '{jugador.ImagenUrl}' no tiene un formato válido",
                            ErrorType = "validation"
                        });
                        continue;
                    }
                }

                // Validar que el equipo NFL existe
                if (!equiposExistentesIds.Contains(jugador.EquipoNFLId))
                {
                    errors.Add(new BatchValidationError
                    {
                        PlayerId = jugador.Id,
                        PlayerName = jugador.Nombre,
                        ErrorMessage = $"El equipo NFL con ID {jugador.EquipoNFLId} no existe",
                        ErrorType = "not_found"
                    });
                    continue;
                }

                // Validar duplicados en la base de datos
                var existeDuplicado = jugadoresExistentes.Any(j =>
                    string.Equals(j.Nombre.Trim(), jugador.Nombre.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    j.EquipoNFLId == jugador.EquipoNFLId);

                if (existeDuplicado)
                {
                    errors.Add(new BatchValidationError
                    {
                        PlayerId = jugador.Id,
                        PlayerName = jugador.Nombre,
                        ErrorMessage = $"Ya existe un jugador con el nombre '{jugador.Nombre}' en el equipo NFL especificado",
                        ErrorType = "duplicate"
                    });
                    continue;
                }

                // Validar duplicados dentro del mismo batch (nombre + equipo)
                var duplicadoEnBatch = jugadores
                    .Where((j, idx) => idx != i) // Excluir el jugador actual
                    .Any(j => string.Equals(j.Nombre.Trim(), jugador.Nombre.Trim(), StringComparison.OrdinalIgnoreCase)
                                && j.EquipoNFLId == jugador.EquipoNFLId);

                if (duplicadoEnBatch)
                {
                    errors.Add(new BatchValidationError
                    {
                        PlayerId = jugador.Id,
                        PlayerName = jugador.Nombre,
                        ErrorMessage = $"El jugador '{jugador.Nombre}' aparece duplicado en el archivo para el mismo equipo",
                        ErrorType = "duplicate"
                    });
                }
            }

            return errors;
        }

        /// <summary>
        /// Crea todos los jugadores en una única transacción (todo-o-nada)
        /// </summary>
        private async Task<List<Jugador>> CreateAllPlayersInTransactionAsync(List<JugadorBatchItemDto> jugadores)
        {
            var createdPlayers = new List<Jugador>();

            try
            {
                // Iniciar la transacción dentro del repositorio (debe exponer un método BeginTransactionAsync)
                using var transaction = await _jugadorRepository.BeginTransactionAsync();

                foreach (var jugadorDto in jugadores)
                {
                    var jugador = new Jugador
                    {
                        Nombre = jugadorDto.Nombre.Trim(),
                        Posicion = jugadorDto.Posicion.Trim(),
                        EquipoNFLId = jugadorDto.EquipoNFLId,
                        ImagenUrl = jugadorDto.ImagenUrl?.Trim(),
                        ThumbnailUrl = jugadorDto.ImagenUrl?.Trim(), // Se autogenera del ImagenUrl
                        Estado = "Activo",
                        FechaCreacion = DateTime.UtcNow
                    };

                    await _jugadorRepository.AddAsync(jugador);
                    createdPlayers.Add(jugador);
                }

                // Guardar todos los cambios
                await _jugadorRepository.SaveChangesAsync();

                // Confirmar la transacción
                await transaction.CommitAsync();

                // Cargar los equipos NFL para el reporte
                foreach (var jugador in createdPlayers)
                {
                    jugador.EquipoNFL = await _equipoNFLRepository.GetByIdAsync(jugador.EquipoNFLId);
                }

                _logger.LogInformation($"Se crearon exitosamente {createdPlayers.Count} jugadores en batch");

                return createdPlayers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear jugadores en transacción, realizando rollback");

                // Si ocurre un error, intentar rollback
                await _jugadorRepository.RollbackTransactionAsync();

                throw;
            }
        }


        /// <summary>
        /// Guarda el archivo procesado en la carpeta correspondiente con el formato requerido
        /// Formato: {timestamp}_{NombreOriginal}.json
        /// </summary>
        private async Task<string> SaveFileToProcessedFolderAsync(string originalFileName, byte[] fileContent, bool success)
        {
            try
            {
                // Crear carpeta de archivos procesados si no existe
                var processedFolder = Path.Combine(_environment.WebRootPath, "processed", "jugadores");
                if (!Directory.Exists(processedFolder))
                {
                    Directory.CreateDirectory(processedFolder);
                    _logger.LogInformation($"Creada carpeta de archivos procesados: {processedFolder}");
                }

                // Generar nombre del archivo procesado según el formato: <timestamp>_<nombre_original>.json
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                var extension = Path.GetExtension(originalFileName);

                // Nombre con timestamp y prefijo de resultado para mejor organización
                var resultado = success ? "Exito" : "Fallo";
                var newFileName = $"{timestamp}_{resultado}_{fileNameWithoutExtension}{extension}";
                var fullPath = Path.Combine(processedFolder, newFileName);

                // Guardar el archivo físicamente en disco
                await File.WriteAllBytesAsync(fullPath, fileContent);

                _logger.LogInformation($"Archivo guardado exitosamente en: {fullPath}");

                return newFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar archivo en carpeta de procesados");
                // En caso de error, retornar el nombre original con timestamp
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                return $"{timestamp}_Error_{originalFileName}";
            }
        }

    }
}
