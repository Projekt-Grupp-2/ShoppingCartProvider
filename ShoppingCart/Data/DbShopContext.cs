using Microsoft.EntityFrameworkCore;
using ShoppingCart.Entity;

namespace ShoppingCart.Data
{
    public class DbShopContext : DbContext

    {


        public DbShopContext(DbContextOptions<DbShopContext> dbContextOptions) : base(dbContextOptions)
        {


        }


        public DbSet<Shopping> Shoppings { get; set; }

        public DbSet<CartItem> CartItems { get; set; }







        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Shopping>()
    .HasMany(s => s.Items)       // Shopping kan ha många CartItems
    .WithOne(ci => ci.Shopping)  // Varje CartItem har en Shopping
    .HasForeignKey(ci => ci.ShoppingId);  // ShoppingId är främmande nyckel i CartItem



     


        }



    }
}
