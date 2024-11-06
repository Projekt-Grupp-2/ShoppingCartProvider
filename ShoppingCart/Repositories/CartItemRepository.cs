using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Entity;

namespace ShoppingCart.Repositories
{
    public class CartItemRepository : ICartItem
    {
        private readonly DbShopContext dbShopContext;

        public CartItemRepository(DbShopContext dbShopContext)
        {
            this.dbShopContext = dbShopContext;
        }

        public async Task<CartItem> CreateAsync(CartItem cartItem)
        {
            await dbShopContext.CartItems.AddAsync(cartItem);
            await dbShopContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem?> DeleteAsyncCart(Guid id)
        {
            var existingItem = await dbShopContext.CartItems.FirstOrDefaultAsync(x => x.CartItemId == id);

            if (existingItem == null)
            {
                return null;

            }

            dbShopContext.CartItems.Remove(existingItem);

            await dbShopContext.SaveChangesAsync();



            return existingItem;
        }

        public async Task<List<CartItem>> GetAllAsyncCart()
        {
            return await dbShopContext.CartItems.ToListAsync();
        }

        public async Task<CartItem?> GetByIdAsyncCart(Guid id)
        {
            var existingItem = await dbShopContext.CartItems.FirstOrDefaultAsync(x => x.CartItemId == id);

            return existingItem;
        }

        public async Task<CartItem?> UpdateAsyncCart(Guid id, CartItem cartItem)
        {
            var existingItem = await dbShopContext.CartItems.FirstOrDefaultAsync(x => x.CartItemId == id);

            if (existingItem == null)
            {
                return null;
            }

            existingItem.Quantity = cartItem.Quantity;
            existingItem.Price = cartItem.Price;


            await dbShopContext.SaveChangesAsync();
            return existingItem;

        }
    }
}
