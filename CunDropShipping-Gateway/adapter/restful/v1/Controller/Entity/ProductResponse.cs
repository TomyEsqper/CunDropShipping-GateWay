namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

public class ProductRequest
{
    public int IdProduct { get; set; }
    public string nameProduct { get; set; }
    public string Description { get; set; }
    public decimal price { get; set; }
    public int stockQuantity { get; set; }
}