using System;
using System.Collections.Generic;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

/// <summary>
/// DTO de entrada (Request) para el flujo "Crear una Orden Completa".
/// Representa el contrato JSON que envía el Cliente (web/móvil) al API Gateway.
/// </summary>
public class CreateFullOrderRequest
{
    /// <summary>
    /// Identificador del usuario que realiza la orden.
    /// </summary>
    public Guid IdUsuario { get; set; }

    /// <summary>
    /// Lista de ítems del pedido (producto y cantidad).
    /// </summary>
    public List<FullOrderItemRequest> Items { get; set; } = new();

    /// <summary>
    /// Dirección de envío completa (líneas, ciudad, país, código postal, etc.).
    /// </summary>
    public string? DireccionEnvio { get; set; }

    /// <summary>
    /// Método de pago seleccionado (p.ej. "CARD", "PAYPAL", "COD").
    /// </summary>
    public string? MetodoPago { get; set; }

    /// <summary>
    /// Campo opcional con notas del cliente para la orden.
    /// </summary>
    public string? Notas { get; set; }
}

/// <summary>
/// Ítem de la orden enviado por el cliente.
/// </summary>
public class FullOrderItemRequest
{
    /// <summary>
    /// Identificador del producto.
    /// </summary>
    public Guid IdProducto { get; set; }

    /// <summary>
    /// Cantidad solicitada del producto.
    /// </summary>
    public int Cantidad { get; set; }
}
