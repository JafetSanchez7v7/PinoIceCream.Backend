using AutoMapper;
using PinoHeladeria.Application.DTOs.CustomerDto;
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
    public class CustomerService : ICustomersService
    {
        private readonly ICustomersRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _context;
        public CustomerService(ICustomersRepository cus, IMapper map, IUnitOfWork con)
        {
            _mapper = map;
            _repo = cus;
            _context = con;
        }

        public async Task<IEnumerable<CustomerDto>>GetAllAsync()
        {
            var response = await _repo.GetAllAsync();
            if (response == null)
                throw new NoContentException("No hay Clientes Registrados Actualmente");
            return _mapper.Map<IEnumerable<CustomerDto>>(response);
        }
        public async Task<CustomerDto>GetByIdAsync(int id)
        {
            var response = await _repo.GetByIdAsync(id);
            if (response == null)
                throw new NotFoundException($"El cliente con id{id} no se encontro");
            return _mapper.Map<CustomerDto>(response);
        }
        
        public async Task<CustomerDto>GetByNameAsync(string name)
        {
            var response = await _repo.GetByNameAsync(name);
            if (response == null)
                throw new NotFoundException($"El cliente con nombre {name} no se encontro");
            return _mapper.Map<CustomerDto>(response);
        }
        public async Task<CustomerDto> AddAsync(CreateCustomerDto dto)
        {
            var existingCustomer = await _repo.GetByNameAsync(dto.CustomerName);
            if (existingCustomer != null)
                throw new ConflictException($"El cliente con nombre {dto.CustomerName} ya existe.");
            // Mapeo de DTO a entidad
            var customer = _mapper.Map<Customers>(dto);
            var response = await _repo.AddAsync(customer);
            if (response == null)
            {
                throw new DataBaseException("No se pudo crear el cliente intente mas tarde");
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerDto>(response);
        }
        public async Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync()
        {
            var response = await _repo.GetActiveCustomersAsync();
            if (response == null || !response.Any())
                throw new NoContentException("No hay Clientes Activos Registrados Actualmente");
            return _mapper.Map<IEnumerable<CustomerDto>>(response);
        }
        public async Task<CustomerDto>UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var response = await _repo.GetToUpdate(id);
            if (response == null)
                throw new NotFoundException($"El cliente con id {id} no se encontro");
            var existingCustomer = await _repo.GetByNameAsync(dto.CustomerName);
            if (existingCustomer != null)
                throw new ConflictException($"El cliente con nombre {dto.CustomerName} ya existe.");
            // Mapeo de DTO a entidad
            response.CustomerName = dto.CustomerName;
            response.CustomerDescription = dto.CustomerDescription;
            response.IsActive = dto.IsActive;
            // Persistencia 
            await _context.SaveChangesAsync();
            
            return _mapper.Map<CustomerDto>(response);

        }
        public async Task<CustomerDto>UpdateStatusAsync(int id, UpdateCustomerStatusDto dto)
        {
            var response = await _repo.GetToUpdate(id);
            if (response == null)
                throw new NotFoundException($"El cliente con id {id} no se encontro");
            if (response.IsActive == dto.IsActive)
                throw new ConflictException($"El estado del cliente con id {id} ya es {(dto.IsActive ? "activo" : "inactivo")}");
            response.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerDto>(response);



        }

    }

}
