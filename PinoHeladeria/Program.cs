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
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<MyAppDbContext>());
// Registrar AutoMapper usando una configuración explícita
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CategoryProfiles).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<CategoryProfiles>());
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(SuppliersProfile).Assembly));
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<SuppliersProfile>());
builder.Services.AddScoped<ISuppliersRepository, SuppliersRepository>();
builder.Services.AddScoped<ISuppliersService, SupplierService>();




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


