using AutoMapper;
using PinoHeladeria.Application.DTOs.UsersDtos;
using PinoHeladeria.Application.Exceptions;
using PinoHeladeria.Application.Interfaces;
using PinoHeladeria.Application.Services_Interfaces;
using PinoHeladeria.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PinoHeladeria.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUsersRepository repo, IMapper mapper,IUnitOfWork uni)
        {
            _repo = repo;
            _unitOfWork = uni;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>>GetAllAsync()
        {
            var response = await _repo.GetAllAsync();
            if (!response.Any())
                throw new NoContentException("No hay registros disponibles");

            return _mapper.Map<IEnumerable<UserDto>>(response);
        }

        public async Task<IEnumerable<UserDto>> GetActiveUsersAsync()
        {
            var response = await _repo.GetActiveUsersAsync();
            if (!response.Any())
                throw new NoContentException("No hay registros de usuarios activos disponibles");

            return _mapper.Map<IEnumerable<UserDto>>(response);
        }
        public async Task<UserDto>GetByIdAsync(int id)
        {
            var response = await _repo.GetByIdAsync(id);
            if(response == null)
                throw new NotFoundException($"El usuario con Id {id}, no se encontro");

            return _mapper.Map<UserDto>(response);


            
        }
        public async Task<UserDto> GetByNameAsync(string name)
        {
            var response = await _repo.GetByNameAsync(name);
            if (response == null)
                throw new NotFoundException($"El usuario con nombre {name}  no se encontro");

            return _mapper.Map<UserDto>(response);
        }
        public async Task<UserDto> AddAsync(CreateUserDto dto)
        {
            var existingUser = await _repo.GetByNameAsync(dto.UserName);
            if (existingUser != null)
                throw new ConflictException($"el usuario con nombre {dto.UserName} ya existe, por favor ingrese otro nombre");
            var entity = _mapper.Map<SsUsers>(dto);
            var newUser = await _repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserDto>(newUser);

        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var errors = new List<string>();
            var existingName = await _repo.GetByNameAsync(dto.UserName);
            if (existingName != null && existingName.UserId != id)
                throw new ConflictException($"El usuario con nombre {dto.UserName} ya esta ocupado por otro usuario");
            var userToUpdate = await _repo.GetToUpdate(id);
            if (userToUpdate == null)
            {
                errors.Add("usuario con id proporcionado no existe");
                throw new ErrorValidationException(errors);
            }
            var isValidpass = BCrypt.Net.BCrypt.Verify(dto.OldPassword, userToUpdate.PasswordHash);
            if (!isValidpass)
                throw new UnauthorizedException("la contraseña anterior es incorrecta, no se puede actualizar.");

            userToUpdate.UserName = dto.UserName;   
            userToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            userToUpdate.IsActive = dto.IsActive;

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserDto>(userToUpdate);
        }

        public async Task<UserDto>UpdateStatusAsync(int id, UpdateUserStatusDto dto)
        {
            var errors = new List<string>();
            var userToUpdate = await _repo.GetToUpdate(id);
            if(userToUpdate == null)
            {
                errors.Add("usuario con id proporcionado no existe");
                throw new ErrorValidationException(errors);
            }

            if(dto.IsActive == userToUpdate.IsActive)
            {
                errors.Add($"El estado del usuario con id {id} ya es {(dto.IsActive ? "activo" : "inactivo")}");
                throw new ErrorValidationException(errors);
            }

            userToUpdate.IsActive = dto.IsActive;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UserDto>(userToUpdate);

        }

        public async Task UpdatePassWordAsync(int id, UpdatePasswordDto dto)
        {
            var errors = new List<string>();
            var user = await _repo.GetToUpdate(id);
            if (user == null)
            {
                errors.Add("usuario con id proporcionado no existe");
                throw new ErrorValidationException(errors);
            }
            var verifyPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if(!verifyPassword)
                throw new UnauthorizedException("la contraseña anterior es incorrecta, no se puede actualizar.");

            var verifyNew = BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.PasswordHash);
            if (verifyNew)
            {
                errors.Add("la nueva Contraseña no puede ser igual a la anterior por favor cambie la contraseña");
                throw new ErrorValidationException(errors);
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
 