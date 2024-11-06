namespace ShoppingCart.Entity
{
    public class Shopping
    {



        public Guid ShoppingId { get; set; }
        public Guid UserId { get; set; }    // Kopplar kundvagnen till en användare
        public DateTime CreatedDate { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>(); // Lista av produkter i kundvagnen






    }
}
