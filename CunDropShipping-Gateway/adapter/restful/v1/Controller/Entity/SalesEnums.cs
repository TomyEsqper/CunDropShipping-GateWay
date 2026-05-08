namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

public enum PaymentStatus
{
    PROCESSING,
    COMPLETED,
    FAILED
}

public enum OrderPaymentStatus
{
    PENDING,
    PAID,
    CANCELLED,
    REFUNDED
}

public enum EscrowStatus
{
    PENDING,
    HELD,
    RELEASED,
    REFUNDED
}
