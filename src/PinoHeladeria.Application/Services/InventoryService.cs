using AutoMapper;
using AutoMapper.Configuration.Annotations;
using PinoHeladeria.Application.DTOs.InventoryDto;
using PinoHeladeria.Application.Exceptions;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Application.Services_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinoHeladeria.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(
            IInventoryRepository inv, 
            IMapper map,
            IUnitOfWork Uow
            )
        {
            _inventoryRepository = inv;
            _mapper = map;
            _unitOfWork = Uow;
        }

        public async Task<IEnumerable<InventoryDto>> GetAllAsync()
        {
            var response = await  _inventoryRepository.GetAllAsync();
            if (response == null)
                throw new NoContentException("No hay Inventario registrado en este momento");
            
            return _mapper.Map<IEnumerable<InventoryDto>>( response); 
        }

        public async Task<InventoryDto>GetByIdAsync(int id)
        {
            var response = await _inventoryRepository.GetByIdAsync(id);
            if (response == null)
                throw new NotFoundException($"No se encontro la instancia de inventario con el id {id}");

            return _mapper.Map<InventoryDto>( response );
        }

        public async Task<InventoryDto>GetByProductNameAsync(string name)
        {
            var response = await _inventoryRepository.GetByProductNameAsync(name);
            if (response == null)
                throw new NotFoundException($"El inventario del producto con nombre: {name} no se encontro o no existe");

            return _mapper.Map<InventoryDto>(response);
        }

        public async Task<InventoryDto>GetByProductIdAsync(int id)
        {
            var response = await _inventoryRepository.GetByProductIdAsync(id);
            if (response == null)
                throw new NotFoundException($"el inventario del producto con id: {id} no se encontro o no existe");

            return _mapper.Map<InventoryDto>(response);
        }

        public async Task<IEnumerable<InventoryDto>>GetByStockFilterAsync(int filter)
        {
            var errors = new List<string>();
            if (filter <= 0)
                errors.Add("Ingrese un numero valido este no puede ser igual a 0");
                
            if (errors.Any())
                throw new ErrorValidationException(errors);

            var response = await _inventoryRepository.GetByStockFilterAsync(filter);
            if(response == null)
                throw new NoContentException($"No hay inventarios con stock menor que el filtro proporiconado: {filter}");

            return _mapper.Map<IEnumerable<InventoryDto>>(response);

        }

    }
}
