namespace ShoppingCart.Entity
{
    public class CartItem
    {

        public Guid CartItemId { get; set; }         // Unikt ID för varje produkt i kundvagnen
        public Guid ShoppingId { get; set; }     // Kopplar produkten till en specifik kundvagn
        public Guid ProductId { get; set; }          // ID för produkten som läggs till
        public int Quantity { get; set; }            // Antal av produkten
        public double Price { get; set; }           // Pris för produkten

        public Shopping Shopping { get; set; }  // Navigationsproperty för ShoppingCart




    }
}
