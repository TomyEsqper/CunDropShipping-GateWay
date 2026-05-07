# 🗺️ Hoja de Ruta Técnica (Roadmap) - CunDropShipping Gateway

Este documento detalla el progreso y las mejoras técnicas pendientes para llevar el Gateway de un estado funcional a un nivel "Professional Grade" (listo para producción).

**Estado Actual:**
✅ **Arquitectura:** Async/Await implementado en todas las capas (Infrastructure -> Domain -> Controller).
✅ **Rendimiento:** Problema N+1 resuelto mediante `IMemoryCache` para la agregación de Productos + Categorías.
✅ **Calidad de Código:** Advertencias de compilación (Warnings) resueltas y manejo de nulos (`Nullable`) estandarizado.
✅ **Manejo de Errores:** Middleware Global (`ExceptionMiddleware`) implementado y categorización de errores HTTP configurada.

---

## 1. 🛡️ Resiliencia y Tolerancia a Fallos (PRÓXIMO PASO - Prioridad Alta)
**El Problema:**
Actualmente, si el microservicio de Productos o Categorías se cae o responde lento, el Gateway se queda esperando hasta que ocurre un timeout, consumiendo recursos y bloqueando al usuario.

**La Solución: Polly**
Implementar patrones de resiliencia usando la librería **Polly**.
*   **Reintentos (Retry):** "Si la petición falla por un error transitorio (ej. red), reintenta 3 veces con una espera exponencial".
*   **Cortacircuitos (Circuit Breaker):** "Si el servicio falla 5 veces seguidas, deja de llamarlo durante 30 segundos y devuelve un error inmediato al usuario".
*   **Timeout:** "Si el servicio tarda más de 2 segundos, cancela la petición".

---

## 2. 🚦 Manejo Global de Errores (COMPLETADO ✅)
**Estado:**
✅ Implementado mediante `ExceptionMiddleware.cs` y registrado en `Program.cs`.

**Funcionalidad:**
El middleware intercepta todas las excepciones no controladas y devuelve respuestas JSON estandarizadas según el tipo de error:
*   `KeyNotFoundException` -> 404 Not Found
*   `UnauthorizedAccessException` -> 401 Unauthorized
*   `ArgumentException` -> 400 Bad Request
*   `HttpRequestException` -> 502 Bad Gateway
*   `Exception` (genérico) -> 500 Internal Server Error

---

## 3. 👁️ Observabilidad y Logs (Prioridad Media)
**El Problema:**
El uso de `Console.WriteLine` es insuficiente para entornos de producción (nube), ya que los logs se pierden o son difíciles de buscar.

**La Solución: Serilog + Structured Logging**
Implementar **Serilog**.
*   **Logs Estructurados:** Guardar los logs como objetos JSON, no como texto plano. Esto permite búsquedas como `Select * Where StatusCode = 500`.
*   **Sinks:** Enviar los logs a archivos, bases de datos, o sistemas de monitoreo (como Seq, ELK Stack o Application Insights).
*   **Tracing:** Poder seguir una petición desde que entra al Gateway hasta que pasa por los microservicios (Correlation ID).

---

## 4. 🚀 Escalabilidad con Caché Distribuido (Prioridad Baja/Futura)
**El Problema:**
Actualmente usamos `IMemoryCache`, que guarda los datos en la memoria RAM del servidor donde corre el Gateway.
*   Si escalamos el Gateway a 2 o más instancias (balanceo de carga), la caché **no se comparte**. La instancia A no sabe lo que la instancia B tiene en caché.
*   Si el servidor se reinicia, la caché se pierde.

**La Solución: Redis**
Cambiar `IMemoryCache` por `IDistributedCache` usando **Redis**.
*   Redis es un almacén de datos en memoria externo.
*   Todas las instancias del Gateway consultan el mismo Redis.
*   Permite persistencia y mayor capacidad que la RAM de un solo servidor.

---

## 📝 Resumen de Próximos Pasos Recomendados

1.  **Configurar Polly:** Vital para que el sistema no colapse bajo carga o fallos de terceros.
2.  **Configurar Serilog:** Para tener visibilidad real de lo que ocurre en producción.
