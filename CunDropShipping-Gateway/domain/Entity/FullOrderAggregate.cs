using System;
using System.Collections.Generic;

namespace CunDropShipping_Gateway.domain.Entity;

/// <summary>
/// Molde interno (no persistente) que el Gateway utiliza para combinar datos
/// provenientes de múltiples microservicios durante el flujo
/// "Crear una Orden Completa".
/// </summary>
public class FullOrderAggregate
{
    // Origen: Order-API
    public Guid IdOrden { get; set; }
    public Guid IdUsuario { get; set; }

    // Origen: Product-API (enriquecimiento de ítems)
    public List<FullOrderItem> Items { get; set; } = new();

    // Origen: Shipment-API
    public ShipmentInfo? Envio { get; set; }

    // Origen: Payment-API (o cálculo propio del gateway)
    public PaymentSummary Totales { get; set; } = new();

    // Origen: User-API (enriquecimiento de datos del cliente)
    public string? NombreCliente { get; set; }
}

/// <summary>
/// Ítem interno de la orden con datos enriquecidos.
/// </summary>
public class FullOrderItem
{
    public Guid IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Importe => Math.Round(PrecioUnitario * Cantidad, 2);
    public string? NombreProducto { get; set; }
}

/// <summary>
/// Información de envío agregada desde Shipment-API.
/// </summary>
public class ShipmentInfo
{
    public string? NumeroDeSeguimiento { get; set; }
    public DateTimeOffset? FechaEntregaEstimada { get; set; }
    public decimal CostoEnvio { get; set; }
}

/// <summary>
/// Resumen de pagos/totales utilizado internamente por el servicio del gateway.
/// </summary>
public class PaymentSummary
{
    public decimal Subtotal { get; set; }
    public decimal Impuestos { get; set; }
    public decimal CostoEnvio { get; set; }
    public decimal Total => Math.Round(Subtotal + Impuestos + CostoEnvio, 2);
}
