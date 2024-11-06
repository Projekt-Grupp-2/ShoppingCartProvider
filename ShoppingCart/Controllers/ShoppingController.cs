using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Dto;
using ShoppingCart.Entity;
using ShoppingCart.Repositories;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {

        private readonly DbShopContext dbShopContext;
        private readonly IShopping shoppingRepo;

        public ShoppingController(DbShopContext dbShopContext, IShopping shoppingRepo)
        {
            this.dbShopContext = dbShopContext;
            this.shoppingRepo = shoppingRepo;

        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {



            var shoppingsDomain = await shoppingRepo.GetAllAsync();

            var shoppingsDto = new List<ShoppingDto>();

            foreach (var shoppingDomain in shoppingsDomain)
            {
                shoppingsDto.Add(new ShoppingDto
                {
                    ShoppingId = shoppingDomain.ShoppingId,
                    UserId = shoppingDomain.UserId,
                    Items = shoppingDomain.Items.Select(i => new CartItemDto
                    {
                        CartItemId = i.CartItemId,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()
                });


            }

            return Ok(shoppingsDto);



        }




        [HttpGet]
        [Route("GetById/{id:Guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var shoppingDomain = await shoppingRepo.GetByIdAsync(id);

            if (shoppingDomain == null)
            {
                return NotFound();
            }


            var shoppingDto = new ShoppingDto
            {
                ShoppingId = shoppingDomain.ShoppingId,
                UserId = shoppingDomain.UserId,

                Items = shoppingDomain.Items.Select(i => new CartItemDto
                {
                    CartItemId = i.CartItemId,
                    ShoppingId = i.ShoppingId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };


            return Ok(shoppingDto);

        }




      








        [HttpGet]
        [Route("GetUserId/{id:Guid}")]

        public async Task<IActionResult> GetUserId([FromRoute] Guid id)
        {
            var shoppingDomain = await shoppingRepo.GetUserIdAsync(id);

            if (shoppingDomain == null)
            {
                return NotFound();
            }


            var shoppingDto = new ShoppingDto
            {
                ShoppingId = shoppingDomain.ShoppingId,
                UserId = shoppingDomain.UserId,

                Items = shoppingDomain.Items.Select(i => new CartItemDto
                {
                    CartItemId = i.CartItemId,
                    ShoppingId = i.ShoppingId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };


            return Ok(shoppingDto);

        }











        [HttpGet]
        [Route("AddOrUpdate/{id:Guid}")]

        public async Task<IActionResult>AddOrUpdate([FromRoute]Guid id, ShoppingDto shopping)
        {
            var shoppingDomain = await shoppingRepo.GetUserIdAsync(id);

            if (shoppingDomain != null)
            {
                var shoppingDto = new ShoppingDto
                {
                    ShoppingId = shoppingDomain.ShoppingId,
                    UserId = shoppingDomain.UserId,

                    Items = shoppingDomain.Items.Select(i => new CartItemDto
                    {
                        CartItemId = i.CartItemId,
                        ShoppingId = i.ShoppingId,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()
                };


                return Ok(shoppingDto);
            }


            var shoppingDomainModel = new Shopping
            {
                ShoppingId = Guid.NewGuid(),
                UserId = shopping.UserId,
                CreatedDate = DateTime.UtcNow,
                Items = shopping.Items.Select(i => new CartItem
                {
                    CartItemId = Guid.NewGuid(),
                    ShoppingId = i.ShoppingId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };








            shoppingDomainModel = await shoppingRepo.CreateAsync(shoppingDomainModel);


            return Ok(shoppingDomainModel);


        }





        [HttpPost]
        [Route("Update/{id:Guid}")]      

        public async Task<IActionResult> Update([FromRoute]Guid id,[FromBody]ShoppingDto shoppingDto)
        {

            


            // Hämta den existerande shopping-uppgiften från databasen
            var shoppingDomain = await shoppingRepo.GetUserIdAsync(id);

            if (shoppingDomain == null)
            {

                return NotFound();


            } 

            shoppingDomain.UserId = shoppingDto.UserId;
             shoppingDomain.Items = shoppingDto.Items.Select(i => new CartItem
              {

                  ProductId = i.ProductId,
                  Quantity = i.Quantity,
                  Price = i.Price
              }).ToList();



            

            // Uppdatera shopping-datan i databasen
            var updatedShoppingDomain = await shoppingRepo.UpdateAsync(shoppingDomain.ShoppingId, shoppingDomain);

           

            //Felet är här, de sker ingen uppdatering.


            // Returnera det uppdaterade objektet som svar
            return Ok(updatedShoppingDomain);



        }


















        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ShoppingDto addShoppingDto)
        {
            var shoppingDomainModel = new Shopping
            {
                ShoppingId = addShoppingDto.ShoppingId,
                UserId = addShoppingDto.UserId,
                CreatedDate = DateTime.UtcNow,
                Items = addShoppingDto.Items.Select(i => new CartItem
                {
                    CartItemId = i.CartItemId,
                    ShoppingId = i.ShoppingId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };


        





            shoppingDomainModel = await shoppingRepo.CreateAsync(shoppingDomainModel);


            var shoppingDto = new ShoppingDto
            {
                ShoppingId = shoppingDomainModel.ShoppingId,
                UserId = shoppingDomainModel.UserId,
              
              Items = shoppingDomainModel.Items.Select(i => new CartItemDto
                {
                    CartItemId = i.CartItemId,
                    ShoppingId = i.ShoppingId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };




        
            return CreatedAtAction(nameof(GetById), new { id = shoppingDto.ShoppingId }, shoppingDto);
        }


        






        [HttpPost]
        [Route("AddOrUpdateCart")]
        public async Task<IActionResult> AddOrUpdateCart([FromBody] ShoppingDto shoppingDto)
        {
            if (shoppingDto == null || shoppingDto.Items == null || shoppingDto.Items.Count == 0)
            {
                return BadRequest("Inga produkter att lägga till i kundvagnen.");
            }

            // Hämta eller skapa användarens kundvagn
            var shoppingCart = await shoppingRepo.GetOrCreateShoppingCartForUser(shoppingDto.UserId);

            // Uppdatera eller lägg till nya produkter i kundvagnen
            foreach (var item in shoppingDto.Items)
            {
                var existingItem = shoppingCart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

                if (existingItem != null)
                {
                    // Uppdatera kvantiteten och priset för befintlig produkt
                    existingItem.Quantity = item.Quantity;
                    existingItem.Price = item.Price;
                }
                else
                {
                    // Lägg till en ny produkt om den inte redan finns
                    shoppingCart.Items.Add(new CartItem
                    {
                        CartItemId = item.CartItemId != Guid.Empty ? item.CartItemId : Guid.NewGuid(),
                        ShoppingId = item.ShoppingId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }
            }

            // Uppdatera kundvagnen i databasen
            var updatedCart = await shoppingRepo.UpdateAsync(shoppingCart.ShoppingId, shoppingCart);
            if (updatedCart == null)
            {
                return NotFound("Kundvagn hittades inte.");
            }

            // Returnera den uppdaterade kundvagnen som svar
            var updatedShoppingDto = new ShoppingDto
            {
                ShoppingId = updatedCart.ShoppingId,
                UserId = updatedCart.UserId,
                Items = updatedCart.Items.Select(i => new CartItemDto
                {
                    CartItemId = i.CartItemId,
                    ShoppingId = i.ShoppingId,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            return Ok(updatedShoppingDto);
        }































    }

}

