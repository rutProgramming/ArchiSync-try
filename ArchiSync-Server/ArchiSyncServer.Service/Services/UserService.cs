using AutoMapper;
using ArchiSyncServer.core;
using ArchiSyncServer.core.DTOs;
using ArchiSyncServer.core.Entities;
using ArchiSyncServer.core.Iservices;
using ArchiSyncServer.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ArchiSyncServer.Core.Entities;

namespace ArchiSyncServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRepositoryManager _repositoryManager;

        public UserService(IUserRepository userRepository, IMapper mapper, IUserRolesRepository userRolesRepository, IRolesRepository rolesRepository, IRepositoryManager repositoryManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userRolesRepository = userRolesRepository;
            _rolesRepository = rolesRepository;
            _repositoryManager = repositoryManager;
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }

            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDto, string roleName)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "User data cannot be null.");
            }

            var existingUser = await _userRepository.GetUserByUsernameAsync(userDto.UserName);
            if (existingUser != null)
            {
                throw new ArgumentException("User already exists.");
            }

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = userDto.PasswordHash; //HashPassword(userDto.PasswordHash);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            var createdUser = await _userRepository.CreateAsync(user);
            var role =await _rolesRepository.GetRoleByNameAsync(roleName);
            var createdUserRole = await _userRolesRepository.CreateAsync(new UserRoles() { User = createdUser, Role = role });
            if (createdUserRole == null || createdUser == null)
            {
                throw new ArgumentException("User creation failed.");
            }

            await _repositoryManager.SaveAsync();
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task UpdateUserAsync(int id, UserDTO userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            user.Username = userDto.UserName;
            user.Email = userDto.Email;
            if (!string.IsNullOrEmpty(userDto.PasswordHash))
            {
                user.PasswordHash = HashPassword(userDto.PasswordHash);
            }
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserAsync(id);
            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserRoles> Authenticate(string userName, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userName);
            ///|| !VerifyPassword(user.PasswordHash, password)
            if (user == null )
                return null;

            var userRole = await _userRolesRepository.GetRoleByUsernameAsync(user.Username);

            if (userRole == null)
                return null;

            return userRole;
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            return hashedPassword.Equals(HashPassword(password));
        }
    }
}
