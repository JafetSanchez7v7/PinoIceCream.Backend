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
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _context;
       

        public CategoryService(ICategoryRepository repo, IMapper map,IUnitOfWork context)
        {
            _categoryRepo = repo;
            _mapper = map;
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>>GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllCategoriesAsync();
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
            var category = await _categoryRepo.FindCatAsync(categoryId);
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
            var category = await _categoryRepo.FindByNameAsync(categoryName);
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

            
            var existingCategory = await _categoryRepo.FindByNameAsync(category.CategoryName);
            if (existingCategory != null)
            {
                throw new ConflictException($"La categoria con nombre {category.CategoryName} ya existe");
            }
            
            //mapeo de dto a entidad
            var categoryEntity = _mapper.Map<Categories>(category);

            var addedCategory = await _categoryRepo.AddAsync(categoryEntity);
            if (addedCategory != null)
            {
                await _context.SaveChangesAsync();
                return _mapper.Map<CategoryDto>(addedCategory);
            }

            throw new DataBaseException("Error al crear la categoria");



        }

        public async Task<CategoryDto> UpdateAsync(int categoryId, UpdateCategoryDto dto)
        {
            var category = await _categoryRepo.FindAsTrackingAsync(categoryId);
            if (category == null)
            {
                throw new NotFoundException($"La categoria con Id {categoryId} no existe");
            }

            //logica
            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;
            
            var existingCategory = await _categoryRepo.FindByNameAsync(dto.CategoryName);
            if (existingCategory != null)
            {
                throw new ConflictException($"La categoria con nombre {dto.CategoryName} ya existe");
            }
            
            
                await _context.SaveChangesAsync();
                return _mapper.Map<CategoryDto>(category);
          
        }

        public async Task<CategoryDto> UpdateStatusAsync(int categoryId, UpdateStatusCatDto dto)
        {
            var category = await _categoryRepo.FindAsTrackingAsync(categoryId);
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
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>>GetActiveCategoriesAsync()
        {
            var categories = await _categoryRepo.GetActiveCategoriesAsync();
            if (!categories.Any())
                throw new NoContentException("No hay Categorias Activas Registradas");

           
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);

        }


    }

}
