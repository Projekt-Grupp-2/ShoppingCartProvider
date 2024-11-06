using ShoppingCart.Entity;

namespace ShoppingCart.Repositories
{
    public interface IShopping
    {

        Task<List<Shopping>> GetAllAsync();
        Task<Shopping> CreateAsync(Shopping shopping);




        Task<Shopping?> UpdateAsync(Guid id, Shopping shopping);

        Task<Shopping?> GetByIdAsync(Guid id);

        Task<Shopping?> DeleteAsync(Guid id);


        Task<Shopping?> DeleteItemAsync(Guid cartItemId);



        Task<Shopping?> GetUserIdAsync(Guid id);


        Task<Shopping> GetOrCreateShoppingCartForUser(Guid userId);



    }
}
