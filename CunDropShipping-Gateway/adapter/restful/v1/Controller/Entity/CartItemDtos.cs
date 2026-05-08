namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

public class CartItemResponse
{
    public int CartItemId { get; set; }
    public Guid CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }
}

public class UpsertCartItemRequest
{
    public Guid CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateCartItemQuantityRequest
{
    public int Quantity { get; set; }
}
