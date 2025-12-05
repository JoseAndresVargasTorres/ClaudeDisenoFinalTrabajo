# PROMPT PARA GENERAR CONTEXTO

Este es el prompt que usarás en el video para mencionar que generaste el contexto completo del proyecto.

---

## PROMPT (Para mencionar en el video):

```
Necesito que me ayudes a implementar pruebas unitarias en mi proyecto. Para que entiendas bien el contexto, necesito que analices toda la estructura de mi proyecto.

CONTEXTO DEL PROYECTO:
- Proyecto: New Generation NFL Fantasy (plataforma de fantasy football para la NFL)
- Backend: C# .NET 9 con arquitectura en capas (Presentation, Logic, Persistence, CrossCutting)
- Frontend: Angular 20 con TypeScript
- Estoy en el Sprint 4 que requiere implementar pruebas unitarias (5% del sprint)

OBJETIVO:
Necesito implementar pruebas unitarias para:
1. Servicios del backend (especialmente JugadorService y AuthService)
2. Servicios del frontend (especialmente Authservice)
3. Seguir mejores prácticas de testing
4. Asegurar buena cobertura de código

INFORMACIÓN QUE NECESITO QUE ANALICES:
- La arquitectura completa del proyecto (4 capas del backend)
- Los servicios principales y sus dependencias
- Los frameworks de testing ya instalados (xUnit, Moq en backend; Jasmine, Karma en frontend)
- Los métodos específicos que necesito probar
- Las excepciones personalizadas que se usan
- Los DTOs y modelos involucrados
- La estructura de archivos del proyecto

Por favor, analiza todos los archivos del proyecto y dame un resumen completo del contexto necesario para implementar las pruebas unitarias correctamente. Incluye información sobre:
- Dependencias de cada servicio
- Casos de prueba que debo cubrir
- Cómo estructurar los archivos de prueba
- Patrones y mejores prácticas a seguir
```

---

## CÓMO USARLO EN EL VIDEO:

**Qué decir:**
"Lo primero que hice fue pedirle a Cursor que analizara todo mi proyecto para entender el contexto completo. Le di este prompt [mostrar prompt] y Cursor generó un documento completo con toda la información necesaria: la arquitectura, las dependencias de cada servicio, los frameworks disponibles, y los casos de prueba que debía cubrir. Esto me ahorró mucho tiempo porque no tuve que explicarle todo manualmente."

**Archivo a mostrar en Cursor:**
- `docs/ActividadDifusion/Contexto_Completo_Pruebas_Unitarias.md` - abrir y mostrar el contenido generado



