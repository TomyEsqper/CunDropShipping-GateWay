# Entity (Adapter): Contratos con el Mundo Exterior

Esta carpeta define la “Capa de Contrato” entre el Gateway y sus consumidores externos (web, móvil, integraciones). Aquí viven las clases que describen exactamente el JSON que el cliente envía y el JSON que el Gateway devuelve. Piensa en estas clases como DTOs (Data Transfer Objects): son modelos planos, sin lógica de negocio, diseñados para el transporte de datos a través de la red.

Objetivo principal:
- Estandarizar el formato de entrada y salida del Gateway.
- Aislar al resto de capas de detalles de transporte (nombres de campos, formatos, versiones de la API, etc.).

Qué tipos de clases se crean aquí
1) Clases de Petición (...Request.cs)
   - Definen el JSON que recibimos del cliente.
   - Representan el contrato de entrada de un endpoint específico.
   - Deben ser explícitas respecto a validaciones superficiales (por ejemplo, campos requeridos) mediante atributos si aplica, pero no incluyen reglas de negocio del dominio.
   - Ejemplo real en este proyecto: CreateFullOrderRequest.cs, utilizada por el endpoint de creación de una “Orden Completa”.

2) Clases de Respuesta (...Response.cs)
   - Definen el JSON que retornamos al cliente.
   - Hacen explícito qué campos exponemos y con qué nombres; pueden diferir de cómo el dominio/integración interna modela los datos.
   - Deben ser estables y versionables (si cambian, se crea una nueva versión del contrato).
   - Ejemplo real en este proyecto: CreateFullOrderResponse.cs, que representa la “bandeja final” que el Gateway entrega al cliente tras orquestar a los microservicios.

Analogía del Mesero
- Imagina que el Gateway es un Mesero. Las clases Request son la orden que el cliente escribe en la comanda (lo que pide). Las clases Response son la bandeja que el Mesero entrega al cliente (lo que se sirve). Aquí se define el formato exacto de esa comanda y de esa bandeja final.

Buenas prácticas
- Mantener estas clases simples, planas y serializables.
- No incluir lógica de negocio ni dependencias de infraestructura.
- Evitar filtrar detalles internos: exponer sólo lo que el cliente necesita.
- Alinear nombres y casing al estándar de la API (por ejemplo, camelCase en JSON si así está configurado).
- Versionar cuando sea necesario: romper compatibilidad exige una nueva versión del contrato (v1, v2...).

Relación con otras capas
- Adapter/Entity ↔ Application: el Adapter convierte entre estos DTOs de entrada/salida y los modelos/objetos que la capa de Application maneja para coordinar el caso de uso.
- Adapter/Entity ↔ Domain/Entity: generalmente no se usan directamente. La capa Adapter mapea los Request/Response hacia/desde modelos del dominio o agregados temporales que viven en Domain/Entity.

Checklist rápido al crear un nuevo contrato
- ¿El nombre de la clase sigue el patrón esperado? (p. ej., CreateXRequest, CreateXResponse)
- ¿Los campos y su documentación reflejan exactamente lo que el cliente debe enviar o recibirá?
- ¿Se han considerado valores por defecto y la compatibilidad hacia atrás?
- ¿Existe una prueba manual/automática de serialización del contrato en el endpoint correspondiente?
