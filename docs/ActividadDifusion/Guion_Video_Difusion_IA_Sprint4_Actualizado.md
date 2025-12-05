# GUIÓN: VIDEO DE DIFUSIÓN - USO DE IA EN INGENIERÍA DE SOFTWARE
## Proyecto: New Generation NFL Fantasy - Sprint 4
## Funcionalidad: Implementación de Pruebas Unitarias (5%)

**Duración:** 5-7 minutos | **IA:** Cursor | **Stack:** C# .NET 9 + Angular 20

---

## 🎬 SECCIÓN 1: INTRODUCCIÓN Y CONTEXTO (1 min)

### Qué decir:
"Hola, soy José Andrés Vargas Torres. Les mostraré cómo usé inteligencia artificial, específicamente Cursor, para implementar pruebas unitarias en mi proyecto 'New Generation NFL Fantasy' - una plataforma de fantasy football para la NFL. Backend en C# .NET 9, frontend en Angular 20. En el Sprint 4 debía implementar pruebas unitarias, lo cual representa el 5% del sprint. Como estudiante sin experiencia previa en testing, usé IA en tres formas: para generar documentación del contexto, para generar código de pruebas, y para entender conceptos como el patrón AAA y mocking."

### Archivos a mostrar en Cursor:
- `README.md` (raíz) - mostrar estructura del proyecto
- `docs/Especificacion/S12025-Objetivos Sprint 4.pdf` - mostrar objetivo de pruebas unitarias
- Explorador mostrando `backend/` y `frontend/`

---

## 🎬 SECCIÓN 2: CONCEPTOS DE IA APLICADOS (1 min)

### Qué decir:
"Usé IA en tres formas específicas: Primero, generación de documentación: le pedí a Cursor que analizara todo mi proyecto y generara un documento con el contexto completo - arquitectura, dependencias, frameworks disponibles. Segundo, generación de código: Cursor generó las pruebas unitarias completas con xUnit y Moq. Tercero, mejor entendimiento de conceptos: Cursor me explicó el patrón AAA, cómo hacer mocking, y diferencias entre pruebas unitarias e integración. Esto me permitió aprender mientras implementaba."

### Archivos a mostrar en Cursor:
- `docs/ActividadDifusion/Contexto_Completo_Pruebas_Unitarias.md` - mostrar documento generado por IA
- `backend/src/NFLFantasyAPI.Logic/Service/JugadorService.cs` - mostrar servicio a probar
- Terminal: mostrar que no hay pruebas aún

---

## 🎬 SECCIÓN 3: PROCESO PASO A PASO (3-4 min)

### PASO 1: GENERACIÓN DE DOCUMENTACIÓN (30 seg)

**Qué decir:**
"Primero usé IA para generar documentación. Le pedí a Cursor que analizara todo mi proyecto y creara un documento con el contexto completo. Esto me ahorró tiempo explicando manualmente."

**Archivos a mostrar en Cursor:**
- `docs/ActividadDifusion/Prompt_Generar_Contexto.md` - mostrar el prompt usado
- `docs/ActividadDifusion/Contexto_Completo_Pruebas_Unitarias.md` - abrir y mostrar contenido generado
- Explicar: "Este documento tiene arquitectura, dependencias, frameworks, casos de prueba - todo lo necesario"

**PROMPT USADO (mencionar, no ejecutar):**
```
[Mostrar prompt del archivo Prompt_Generar_Contexto.md]
```

### PASO 2: ENTENDIMIENTO DE CONCEPTOS (30 seg)

**Qué decir:**
"Segundo, usé IA para entender conceptos. No sabía qué framework usar ni cómo estructurar pruebas. Cursor me explicó xUnit y Moq, y me enseñó el patrón AAA."

**Archivos a mostrar en Cursor:**
- Chat de Cursor: mostrar pregunta sobre frameworks
- Respuesta de Cursor explicando xUnit, Moq y patrón AAA
- `backend/src/NFLFantasyAPI.Presentation/README.md` - verificar que xUnit y Moq están instalados

**PROMPT:**
```
¿Qué framework de testing recomiendas para C# .NET 9? ¿Cómo funciona el patrón AAA (Arrange-Act-Assert)? ¿Qué es Moq y cómo se usa para mocking?
```

### PASO 3: GENERACIÓN DE CÓDIGO (1.5 min)

**Qué decir:**
"Tercero, usé IA para generar código. Seleccioné el método GetByIdAsync y pedí a Cursor que genere las pruebas. Cursor generó código completo con explicaciones."

**Archivos a mostrar en Cursor:**
- `backend/src/NFLFantasyAPI.Logic/Service/JugadorService.cs` - líneas 62-92 (método GetByIdAsync)
- Seleccionar método completo y abrir chat (Ctrl+L)
- Mostrar código de prueba generado por Cursor en el chat
- Cursor creando archivo `JugadorServiceTests.cs` automáticamente

**PROMPT (seleccionar GetByIdAsync y usar Ctrl+L):**
```
Genera pruebas unitarias para este método usando xUnit y Moq. Debe probar: 1) caso exitoso cuando jugador existe, 2) caso de error cuando jugador no existe (JugadorNotFoundException), 3) verificar mapeo correcto del DTO. Explica cada sección del patrón AAA.
```

**Qué decir mientras muestra el código:**
"Miren el código que Cursor generó. Incluye: Arrange - configuración de mocks con Moq, Act - ejecución del método, Assert - verificaciones. Cursor también explicó por qué cada parte es necesaria. Esto es generación de código asistida por IA."

**Archivos a mostrar en Cursor:**
- Archivo `JugadorServiceTests.cs` generado - mostrar código completo
- Chat de Cursor con explicación del patrón AAA
- Terminal: ejecutar `dotnet test` y mostrar pruebas pasando exitosamente

---

### PASO 4: ITERACIÓN Y MEJORAS (30 seg)

**Qué decir:**
"Cursor recuerda el contexto de la conversación. Iteré para mejorar: pedí explicaciones sobre cómo hacer pruebas más legibles, agregar casos límite, y reducir duplicación. También me explicó la diferencia entre pruebas unitarias e integración - esto es mejor entendimiento de conceptos."

**PROMPT:**
```
Las pruebas funcionan. ¿Cómo hacerlas más legibles? ¿Qué casos límite debo agregar (ej: EquipoNFL null)? ¿Cómo reducir duplicación con setup compartido? También, explícame la diferencia entre pruebas unitarias e integración.
```

**Archivos a mostrar en Cursor:**
- Respuesta de Cursor explicando mejoras y conceptos
- Archivo refactorizado con setup compartido
- Terminal: `dotnet test` mostrando más pruebas pasando

---

### PASO 5: FRONTEND - GENERACIÓN DE CÓDIGO (1 min)

**Qué decir:**
"En frontend usé IA para generar y mejorar código de pruebas. Angular ya tiene Jasmine y Karma. Cursor mejoró mis pruebas existentes y generó nuevas."

**PROMPT:**
```
Tengo authservice.spec.ts. Revísalo y mejóralo. Agrega pruebas para checkSessionValidity que verifica expiración de tokens. Explica cómo mockear HttpClient y Router correctamente.
```

**Archivos a mostrar en Cursor:**
- `frontend/src/services/authservice.spec.ts` - antes (mostrar código existente)
- Chat de Cursor con mejoras sugeridas
- `frontend/src/services/authservice.spec.ts` - después (mostrar código mejorado)
- Terminal: `ng test` mostrando todas las pruebas pasando

---

## 🎬 SECCIÓN 4: RESULTADOS Y APRENDIZAJES (1 min)

### Qué decir:
"Resumen de cómo usé IA: Primero, generación de documentación - Cursor creó un documento completo con todo el contexto. Segundo, generación de código - Cursor generó pruebas unitarias completas para backend y frontend. Tercero, mejor entendimiento - Cursor me explicó patrón AAA, mocking, y diferencias entre tipos de pruebas.

Resultados: ✅ Pruebas implementadas para JugadorService y AuthService, ✅ Mejoras en pruebas del frontend, ✅ Aprendí conceptos profesionales de testing, ✅ Reduje tiempo de desarrollo significativamente, ✅ Mejoré calidad del código.

Aprendizajes clave: 1) IA es herramienta educativa, no reemplazo del aprendizaje, 2) Mejor prompt genera mejor respuesta, 3) Debes entender el código generado, no solo copiarlo, 4) La iteración es clave - pregunta, prueba, mejora, 5) Documentar el proceso ayuda al aprendizaje.

Consejo final: Usen IA como compañero de estudio. Para generar código, documentación, y entender conceptos. Pero siempre entiendan lo que están usando."

### Archivos a mostrar en Cursor:
- Terminal: `dotnet test` - todas las pruebas del backend pasando
- Terminal: `ng test` - todas las pruebas del frontend pasando
- Explorador: mostrar archivos de prueba creados (JugadorServiceTests.cs, etc.)
- Resumen visual: mostrar los 3 conceptos de IA aplicados (documentación, código, conceptos)

---

## 📌 CHECKLIST

- [ ] Cursor abierto con proyecto cargado
- [ ] Chat listo (Ctrl+L)
- [ ] Terminal lista para `dotnet test` y `ng test`
- [ ] Archivos clave abiertos: JugadorService.cs, AuthService.cs, authservice.spec.ts

## 🎯 TIPS

- Habla claro, muestra código real, enfatiza aprendizaje
- Zoom en código, resalta líneas importantes
- Mantén 5-7 minutos

---

**Estudiante:** José Andrés Vargas Torres  
**Curso:** CE-1116 Diseño y Calidad de Productos Tecnológicos  
**Sprint 4:** Implementación de Pruebas Unitarias (5%)

