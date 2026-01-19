


using AutoMapper;
using PinoHeladeria.Application.DTOs.SuppliersDto;
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
    public class SupplierService : ISuppliersService
    {

        private readonly ISuppliersRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public SupplierService(ISuppliersRepository repo, IMapper map, IUnitOfWork work)
        {
            _repo = repo;
            _mapper = map;
            _unitOfWork = work;
        }

        public async Task<IEnumerable<SuppliersDto>> GetAllSuppliersAsync()
        {
            var suppliers = await _repo.GetAllAsync();
            if (!suppliers.Any())
                throw new NoContentException("No hay proveedores registrados");

            return _mapper.Map<IEnumerable<SuppliersDto>>(suppliers);
        }

        public async Task<SuppliersDto> GetByIdAsync(int id)
        {
            var supplier = await _repo.GetByIdAsync(id);
            if (supplier == null)
                throw new NotFoundException($"El proveedor con id {id} no fue encontrado");
            return _mapper.Map<SuppliersDto>(supplier);
        }

        public async Task<SuppliersDto>GetByNameAsync(string name)
        {
            var supplier = await _repo.GetByNameAsync(name);
            if (supplier == null)
                throw new NotFoundException($"El proveedor con nombre {name} no fue encontrado");
            return _mapper.Map<SuppliersDto>(supplier);
        }

       
        public async Task<SuppliersDto>AddAsync(CreateSupplierDto supplier)
        {
            var existingSupplier = await _repo.GetByNameAsync(supplier.SuplierName);
            if(existingSupplier != null)
                throw new ConflictException($"El proveedor con nombre {supplier.SuplierName} ya existe");

            var supplierEntity = _mapper.Map<Suppliers>(supplier);

            var AddedSup = await _repo.AddAsync(supplierEntity);
            if (AddedSup != null)
            {
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<SuppliersDto>(AddedSup);
            }

            throw new DataBaseException("Ocurrio un error inesperado");
                
            
        }

        public async Task<SuppliersDto>UpdateAsync(int id, UpdateSupplierDto supplierDto)
        {
            var existingSupplier = await _repo. GetByIdWithTrackingAsync(id);
            if (existingSupplier == null)
                throw new NotFoundException($"El proveedor con id {id} no fue encontrado");
            existingSupplier.SuplierName = supplierDto.SuplierName;
            existingSupplier.Location = supplierDto.Location;
            existingSupplier.Phone = supplierDto.Phone;
            existingSupplier.IsActive = supplierDto.IsActive;
            var conflictedName = await _repo.GetByNameAsync(supplierDto.SuplierName);
            if(conflictedName != null)
                throw new ConflictException($"El proveedor con nombre:{supplierDto.SuplierName} ya existe");  
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SuppliersDto>(existingSupplier);
        }

        public async Task<SuppliersDto> UpdateStatusAsync(int id, UpdateStatusSupDto status)
        {
            var existingSupplier = await _repo.GetByIdWithTrackingAsync(id);
            if(existingSupplier == null)
                throw new NotFoundException($"El proveedor con id {id} no fue encontrado");

            if(existingSupplier.IsActive == status.IsActive)
            {
                var error = new List<string>();
                error.Add($"El estado del proveedor con id {id} ya es {(status.IsActive ? "activo" : "inactivo")}");
                throw new ErrorValidationException(error);
            }
            existingSupplier.IsActive = status.IsActive;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SuppliersDto>(existingSupplier);

        }

        public async Task<IEnumerable<SuppliersDto>> GetActiveAsync()
        {
            var activeSuppliers = await _repo.GetActiveAsync();
            if (!activeSuppliers.Any())
                throw new NoContentException("No hay proveedores activos");
            return _mapper.Map<IEnumerable<SuppliersDto>>(activeSuppliers);
        }


    }
}
