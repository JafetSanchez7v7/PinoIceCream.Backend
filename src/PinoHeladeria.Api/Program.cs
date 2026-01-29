using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PinoHeladeria.API.MIddleWares;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Application.MappingsProfiles;
using PinoHeladeria.Application.Services;
using PinoHeladeria.Application.Services_Interfaces;
using PinoHeladeria.Infrastucture.AppDbContext;
using PinoHeladeria.Infrastucture.Repositories;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
        ClockSkew = TimeSpan.Zero // El token expira al segundo exacto
    };
});

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Pino Heladería API";
        document.Components ??= new();
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Introduce tu token JWT"
        });
        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
        return Task.CompletedTask;
    });
});
//Rate limiter 
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("LoginPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5;
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Respuesta personalizada cuando alguien se pasa del límite
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Rate limiting

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

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRateLimiter();

app.MapControllers();

    app.Run();


