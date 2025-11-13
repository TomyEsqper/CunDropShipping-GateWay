using System;
using System.Collections.Generic;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

/// <summary>
/// DTO de salida (Response) para el flujo "Crear una Orden Completa".
/// Representa el contrato JSON que el API Gateway devuelve al Cliente.
/// </summary>
public class CreateFullOrderResponse
{
    /// <summary>
    /// Identificador de la orden creada en el ecosistema.
    /// </summary>
    public Guid IdOrden { get; set; }

    /// <summary>
    /// Número de seguimiento generado por el servicio de envíos (si aplica).
    /// </summary>
    public string? NumeroDeSeguimiento { get; set; }

    /// <summary>
    /// Fecha estimada de entrega, si está disponible.
    /// </summary>
    public DateTimeOffset? FechaEntregaEstimada { get; set; }

    /// <summary>
    /// Resumen de totales calculados para la orden.
    /// </summary>
    public OrderTotalsResponse Totales { get; set; } = new();

    /// <summary>
    /// Mensaje informativo para el cliente (p.ej., confirmaciones o warnings).
    /// </summary>
    public string? Mensaje { get; set; }
}

/// <summary>
/// Totales de la orden para la respuesta al cliente.
/// </summary>
public class OrderTotalsResponse
{
    public decimal Subtotal { get; set; }
    public decimal CostoEnvio { get; set; }
    public decimal Impuestos { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemSummaryResponse> Items { get; set; } = new();
}

/// <summary>
/// Resumen de ítem incluido en la orden creada.
/// </summary>
public class OrderItemSummaryResponse
{
    public Guid IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Importe { get; set; }
    public string? NombreProducto { get; set; }
}
