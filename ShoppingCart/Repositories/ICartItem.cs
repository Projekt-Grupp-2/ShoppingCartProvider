using ShoppingCart.Entity;

namespace ShoppingCart.Repositories
{
    public interface ICartItem
    {

        Task<CartItem?> GetByIdAsyncCart(Guid id);

        Task<CartItem?> DeleteAsyncCart(Guid id);

        Task<List<CartItem>> GetAllAsyncCart();
        Task<CartItem> CreateAsync(CartItem cartItem);


        Task<CartItem?> UpdateAsyncCart(Guid id, CartItem cartItem);




    }
}
