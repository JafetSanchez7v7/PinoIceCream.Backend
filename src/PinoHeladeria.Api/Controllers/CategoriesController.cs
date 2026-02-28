using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PinoHeladeria.Application.DTOs;
using PinoHeladeria.Application.DTOs.CategoryDtos;
using PinoHeladeria.Application.Services_Interfaces;
using System.Runtime.InteropServices;

namespace PinoHeladeria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Salesman" )]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            var totalCount = new
            {
                TotalAmount = $"Numero de Categorias:{response.Count()}"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Categorias obtenidas con exito",totalCount);
            return Ok(ApiResponse);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Salesman")]


        public async Task<IActionResult> GetById(int id)
        {
            var response = await _categoryService.FindCatAsync(id);
            string details = $"Categoria con id: {response.CategoryId} Encontrada";
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Categoria obtenida con exito", details);
            return Ok(ApiResponse);
        }

        [HttpGet("byname/{name}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetByName(string name)
        {
            var response = await _categoryService.FindByNameAsync(name);
            var meta = new
            {
                detail = $"Categoria con nombre: {response.CategoryName} Encontrada"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Categoria obtenida con exito", meta);
            return Ok(ApiResponse);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> Create([FromBody] CreateCategoryDto categoryDto)
        {
            var response = await _categoryService.AddAsync(categoryDto);
            var meta = new
            {
                detail = $"Categoria con id: {response.CategoryId} Creada"
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(201, response, "Categoria creada con exito", meta);
            return CreatedAtAction(nameof(GetById), new { id = response.CategoryId }, ApiResponse);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto categoryDto)
        {
            var response = await _categoryService.UpdateAsync(id, categoryDto);
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Categoria actualizada con exito");
            return Ok(ApiResponse);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusCatDto statusDto)
        {
            var response = await _categoryService.UpdateStatusAsync(id, statusDto);
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Estado de la categoria actualizado con exito");
            return Ok(ApiResponse);
        }

        [HttpGet("Active")]
        [Authorize(Roles = "Admin, Salesman")]

        public async Task<IActionResult> GetActiveCategories()
        {
            var response = await _categoryService.GetActiveCategoriesAsync();
            var meta = new
            {
                totalCount = response.Count()
            };
            var ApiResponse = HelpersOfAppResp.ApiResponseMaker.Create(200, response, "Categorias activas obtenidas con exito", meta);
            return Ok(ApiResponse);
               

        }
    }
}
