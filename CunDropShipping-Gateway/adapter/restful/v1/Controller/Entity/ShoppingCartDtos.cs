namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

public class ShoppingCartResponse
{
    public Guid ShoppingCartId { get; set; }
    public Guid BuyerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CartItemResponse> CartItems { get; set; } = new();
}

public class CreateShoppingCartRequest
{
    public Guid BuyerId { get; set; }
}
