using AutoMapper;
using PinoHeladeria.Application.DTOs.RolesDtos;
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
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IRoleRepository repo, IMapper map, IUnitOfWork ofc)
        {
            _roleRepository = repo;
            _mapper = map;
            _unitOfWork = ofc;
        }


        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            if (!roles.Any())
                throw new NoContentException(" No roles found ");
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }
        public async Task<IEnumerable<RoleDto>> GetActiveRolesAsync()
        {
            var roles = await _roleRepository.GetActiveRoles();
            if (!roles.Any())
                throw new NoContentException(" No active roles found ");
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }
        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                throw new NotFoundException($" Role with id {id} not found ");
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<RoleDto> GetByNameAsync(string name)
        {
            var role = await _roleRepository.GetByNameAsync(name);
            if (role == null)
                throw new NotFoundException($" Role with name {name} not found ");
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<RoleDto> AddAsync(CreateRoleDto dto)
        {
            var errors = new List<string>();
            var existingRole = await _roleRepository.GetByNameAsync(dto.RoleName);
            if (existingRole != null)
            {
                errors.Add($" Role with name {dto.RoleName} already exists ");
                throw new ErrorValidationException(errors);
            }
            var roleEntity = _mapper.Map<Roles>(dto);
            var newRole = await _roleRepository.AddAsync(roleEntity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RoleDto>(newRole);
        }

        public async Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto)
        {
            var errors = new List<string>();
            var roleToUpdate = await _roleRepository.GetToUpdate(id);
            if (roleToUpdate == null)
            {
                throw new NotFoundException($" Role with id {id} not found ");
            }
            var existingRole = await _roleRepository.GetByNameAsync(dto.RoleName);
            if (existingRole != null && existingRole.RoleId != id)
            {
                errors.Add($" Role with name {dto.RoleName} already exists ");
                throw new ErrorValidationException(errors);
            }
            _mapper.Map(dto, roleToUpdate);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RoleDto>(roleToUpdate);

        }

        public async Task<RoleDto> UpdateStatusAsync(int id, UpdateRoleStatusDto dto)
        {
            var roleToUpdate = await _roleRepository.GetToUpdate(id);
            if(dto.IsActive == roleToUpdate.IsActive)
            {
                throw new ErrorValidationException(new List<string> { $" Role with id {id} already has IsActive set to {dto.IsActive} " });
            }
            if (roleToUpdate == null)
            {
                throw new NotFoundException($" Role with id {id} not found ");
            }
            _mapper.Map(dto, roleToUpdate);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RoleDto>(roleToUpdate);
        }

    }
}
