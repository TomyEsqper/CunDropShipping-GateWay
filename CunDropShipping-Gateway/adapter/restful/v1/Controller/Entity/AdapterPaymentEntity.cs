namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

public class AdapterPaymentEntity
{
    public Guid PaymentId { get; set; }
    public Guid OrderId { get; set; }
    public string PaymentProvider { get; set; } = string.Empty;
    public string TransactionReference { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
}
