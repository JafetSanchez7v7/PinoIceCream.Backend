using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.DTOs.ProductDtos;
using PinoHeladeria.Application.Services_Interfaces;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //DI
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        //Methods
        [HttpGet]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
            var meta = new
            {
                detail = "Lista de Productos obtenida",
                TotalAmount = products.Count()
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200,products,"Productos Obtenidos Exitosamente", meta);
            return Ok(apiResponse);
        }
        [HttpGet("ById/{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            var meta = new
            {
                detail = $"Producto con id: {product.ProductId} Encontrado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, product, "Producto Obtenido Exitosamente", meta);
            return Ok(apiResponse);
        }
        [HttpGet("ByName/{name}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetProductByName(string name)
        {
            var product = await _productService.GetByNameAsync(name);
            var meta = new
            {
                detail = $"Producto con nombre: {product.ProductName} Encontrado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, product, "Producto Obtenido Exitosamente", meta);
            return Ok(apiResponse);
        }
        [HttpPost("AddProducts")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var product = await _productService.AddAsync(productDto);
            var meta = new
            {
                detail = $"Producto con id: {product.ProductId} Creado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(201, product, "Producto Creado Exitosamente", meta);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, apiResponse);
        }
        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _productService.UpdateAsync(id, productDto);
            var meta = new
            {
                detail = $"Producto con id: {product.ProductId} Actualizado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, product, "Producto Actualizado Exitosamente", meta);
            return Ok(apiResponse);
        }
        [HttpPatch("Deactivate/{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> DeactivateProduct(int id, UpdateStatusDto dto)
        {
            var product = await _productService.UpdateStatusAsync(id,dto);
            var meta = new
            {
                detail = $"Producto con id: {product.ProductId} Actualizado"
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, product, $"Producto {(dto.IsActive ? "Activado" : "Desactivado")} exitosamente", meta);
            return Ok(apiResponse);
        }
        [HttpGet("ActiveProducts")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetActiveProducts()
        {
            var products = await _productService.GetActiveProductsAsync();
            var meta = new
            {
                detail = "Lista de Productos Activos obtenida",
                TotalAmount = products.Count()
            };
            var apiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, products, "Productos Activos Obtenidos Exitosamente", meta);
            return Ok(apiResponse);
        }
    }
}
