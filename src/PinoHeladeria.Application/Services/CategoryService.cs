using AutoMapper;
using PinoHeladeria.Application.DTOs.CategoryDtos;
using PinoHeladeria.Application.Exceptions;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Application.Services_Interfaces;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _context;
       

        public CategoryService(ICategoryRepository repo, IMapper map,IUnitOfWork context)
        {
            _mapper = map;
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>>GetAllCategoriesAsync()
        {
            var categories = await _context.CategoriesI.GetAllCategoriesAsync();
            if(!categories.Any())
            {
                throw new NoContentException("No hay Categorias existentes");
            }
            else
            {
                return _mapper.Map<IEnumerable<CategoryDto>>(categories);
            }
               
        }
        public async Task<CategoryDto>FindCatAsync(int categoryId)
        {
            var category = await _context.CategoriesI.FindCatAsync(categoryId);
            if (category == null)
            {
                throw new NotFoundException($"La categoria con Id {categoryId} no fue encontrada");
            }
            else
            {
                return _mapper.Map<CategoryDto>(category);
            }
        }

        public async Task<CategoryDto>FindByNameAsync(string categoryName)
        {
            var category = await _context.CategoriesI.FindByNameAsync(categoryName);
            if (category == null)
            {
                throw new NotFoundException($"La categoria con nombre {categoryName} no fue encontrada");
            }
            else
            {
                return _mapper.Map<CategoryDto>(category);
            }
        }

        public async Task<CategoryDto> AddAsync(CreateCategoryDto category)
        {

            
            var existingCategory = await _context.CategoriesI.FindByNameAsync(category.CategoryName);
            if (existingCategory != null)
            {
                throw new ConflictException($"La categoria con nombre {category.CategoryName} ya existe");
            }
            
            //mapeo de dto a entidad
            var categoryEntity = _mapper.Map<Categories>(category);

            var addedCategory = await _context.CategoriesI.AddAsync(categoryEntity);
            if (addedCategory != null)
            {
                await _context.SaveChangesAsync();
                return _mapper.Map<CategoryDto>(addedCategory);
            }

            throw new DataBaseException("Error al crear la categoria");



        }

        public async Task<CategoryDto> UpdateAsync(int categoryId, UpdateCategoryDto dto)
        {
            var category = await _context.CategoriesI.FindAsTrackingAsync(categoryId);
            if (category == null)
            {
                throw new NotFoundException($"La categoria con Id {categoryId} no existe");
            }

            //logica
            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;
            
            var existingCategory = await _context.CategoriesI.FindByNameAsync(dto.CategoryName);
            if (existingCategory != null && existingCategory.CategoryId == categoryId )
            {
                throw new ConflictException($"La categoria con nombre {dto.CategoryName} ya existe");
            }
            
                
            string oldName = category.CategoryName;
            await _context.CategoriesI.UpdateAsync(category, oldName);
            
            return _mapper.Map<CategoryDto>(category);
          
        }

        public async Task<CategoryDto> UpdateStatusAsync(int categoryId, UpdateStatusCatDto dto)
        {
            var category = await _context.CategoriesI.FindAsTrackingAsync(categoryId);
            if (category == null)
            {
                throw new NotFoundException($"La categoria con Id {categoryId} no existe");
            }

            if(category.IsActive == dto.IsActive)
            {
                var error = new List<string>();
                error.Add($"La categoria con Id {categoryId} ya tiene el estado {(dto.IsActive ? "activo" : "inactivo")}");
                throw new ErrorValidationException(error);
            }
            //logica
            category.IsActive = dto.IsActive;
            await _context.CategoriesI.UpdateAsync(category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>>GetActiveCategoriesAsync()
        {
            var categories = await _context.CategoriesI.GetActiveCategoriesAsync();
            if (!categories.Any())
                throw new NoContentException("No hay Categorias Activas Registradas");

           
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);

        }


    }

}
