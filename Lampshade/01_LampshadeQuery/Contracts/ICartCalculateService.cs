using ShopManagement.Application.Contracts.Order;
using System.Collections.Generic;

namespace _01_LampshadeQuery.Contracts
{
    public interface ICartCalculateService
    {
        Cart ComputeCart(List<CartItem> cartItems);
    }
}