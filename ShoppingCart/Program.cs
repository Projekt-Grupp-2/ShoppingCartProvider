using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddDbContext<DbShopContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingDbConnectionString")));


builder.Services.AddScoped<IShopping, ShoppingRepository>();



builder.Services.AddScoped<ICartItem, CartItemRepository>();



builder.Services.AddCors(o => o.AddPolicy("yes", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("yes");
app.UseAuthorization();

app.MapControllers();

app.Run();
