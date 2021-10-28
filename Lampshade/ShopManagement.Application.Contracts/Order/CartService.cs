namespace ShopManagement.Application.Contracts.Order
{
    public class CartService : ICartService
    {
        public Cart Cart { get; set; }
        public Cart Get()
        {
            return Cart;
        }

        public void Set(Cart cart)
        {
            Cart = cart;
        }
    }
}
