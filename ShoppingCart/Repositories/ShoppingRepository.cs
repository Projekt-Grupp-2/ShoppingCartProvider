using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Dto;
using ShoppingCart.Entity;

namespace ShoppingCart.Repositories
{
    public class ShoppingRepository : IShopping
    {

        private readonly DbShopContext dbShopContext;

        public ShoppingRepository(DbShopContext dbShopContext)
        {
            this.dbShopContext = dbShopContext;
        }

        public async Task<Shopping> CreateAsync(Shopping shopping)
        {
            await dbShopContext.Shoppings.AddAsync(shopping);
            await dbShopContext.SaveChangesAsync();

            return shopping;
        }

        public async Task<Shopping?> DeleteAsync(Guid id)
        {

            var existingShop = await dbShopContext.Shoppings.Include(s => s.Items).FirstOrDefaultAsync(x => x.ShoppingId == id);

            if (existingShop == null)
            {
                return null;

            }

            dbShopContext.Shoppings.Remove(existingShop);

            await dbShopContext.SaveChangesAsync();



            return existingShop;
        }

        public async Task<Shopping?> DeleteItemAsync(Guid cartItemId)
        {

            var existingItem = await dbShopContext.CartItems.Include(c => c.Shopping).FirstOrDefaultAsync(x => x.CartItemId == cartItemId);

            if (existingItem == null)
            {
                return null;  // Om produkten inte hittas, returnera null
            }

            // Spara referensen till kundvagnen (Shopping) innan du tar bort produkten
            var shoppingCart = existingItem.Shopping;

            // Ta bort produkten från CartItems-tabellen
            dbShopContext.CartItems.Remove(existingItem);

            await dbShopContext.SaveChangesAsync();

            // Returnera hela kundvagnen (Shopping)
            return shoppingCart;
        }

        public async Task<List<Shopping>> GetAllAsync()
        {
            return await dbShopContext.Shoppings.Include(s => s.Items).ToListAsync();
        }

        public async Task<Shopping?> GetByIdAsync(Guid id)
        {
            var existingShop = await dbShopContext.Shoppings.Include(s => s.Items).FirstOrDefaultAsync(x => x.ShoppingId == id);

            return existingShop;

        }

        public async Task<Shopping?> GetUserIdAsync(Guid id)
        {
            var existingShop = await dbShopContext.Shoppings.Include(s => s.Items).FirstOrDefaultAsync(x => x.UserId == id);

            return existingShop;


        }

        public async Task<Shopping?> UpdateAsync(Guid id, Shopping shopping)
        {

            

            var existingProduct = await dbShopContext.Shoppings.Include(s => s.Items).FirstOrDefaultAsync(x => x.ShoppingId == id);


    
          if (existingProduct == null)
            {

                return null;
            }
           


            foreach (var item in shopping.Items)
            {
                var existingItem = existingProduct.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (existingItem != null)
                {
                    // Uppdatera om produkten redan finns i kundvagnen
                    existingItem.Quantity = item.Quantity;
                    existingItem.Price = item.Price;
                }
                else
                {
                    // Lägg till nya produkter som inte redan finns i kundvagnen
                    existingProduct.Items.Add(item);
                }
            }


            var itemsToRemove = existingProduct.Items
       .Where(existingItem => !shopping.Items.Any(i => i.CartItemId == existingItem.CartItemId))
       .ToList();


            foreach (var item in itemsToRemove)
            {
                existingProduct.Items.Remove(item);
            }


            


            // Spara alla ändringar
            await dbShopContext.SaveChangesAsync();

            return existingProduct;  // Returnera den uppdaterade kundvagnen
        }













        public async Task<Shopping> GetOrCreateShoppingCartForUser(Guid userId)
        {
            // Hämta den befintliga kundvagnen för användaren om den finns
            var shoppingCart = await dbShopContext.Shoppings
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            // Om ingen kundvagn finns för användaren, skapa en ny
            if (shoppingCart == null)
            {
                shoppingCart = new Shopping
                {
                    ShoppingId = Guid.NewGuid(),
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    Items = new List<CartItem>()
                };

                // Lägg till den nya kundvagnen i databasen
                dbShopContext.Shoppings.Add(shoppingCart);
                await dbShopContext.SaveChangesAsync();
            }

            return shoppingCart;
        }








    }
}
