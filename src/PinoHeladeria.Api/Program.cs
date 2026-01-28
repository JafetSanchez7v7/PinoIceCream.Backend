using Microsoft.EntityFrameworkCore;
using PinoHeladeria.API.MIddleWares;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Application.MappingsProfiles;
using PinoHeladeria.Application.Services;
using PinoHeladeria.Application.Services_Interfaces;
using PinoHeladeria.Infrastucture.AppDbContext;
using PinoHeladeria.Infrastucture.Repositories;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MyAppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//SERVICIOS Y REPOSITORIOS
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISuppliersRepository, SuppliersRepository>();
builder.Services.AddScoped<ISuppliersService, SupplierService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddScoped<ICustomersService, CustomerService>();
//Autorizacion y usuarios
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

//Transaccionales Servicios Compra venta y atomicidad
builder.Services.AddScoped<IPurchasesRepository, PurchasesRepository>();
builder.Services.AddScoped<IPurchasesService, PurchasesService>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MyAppDbContext>());
// Registrar Perfiles de mapeo
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CategoryProfiles).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<CategoryProfiles>());
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(SuppliersProfile).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<SuppliersProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ProductProfiles).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProductProfiles>());
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(InventoryProfile).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<InventoryProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<PurchasesProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(PurchasesProfile).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<SalesProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(SalesProfile).Assembly));
builder.Services.AddAutoMapper(cfg=> cfg.AddProfile<UsersProfile>());
builder.Services.AddAutoMapper(cfg=> cfg.AddMaps(typeof (UsersProfile).Assembly));





var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Heladería API")
               .WithTheme(ScalarTheme.Mars) // Tiene temas oscuros geniales
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });

}
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();


