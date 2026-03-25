using System;
using System.Threading.Tasks;
using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Enums;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace TalentSphere.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> CreateUserAsync(User user)
        {
            // Check if email already exists
            var existingUser = await _repository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists. Please use a different email.");
            }

            user.PasswordHash = BC.HashPassword(user.PasswordHash);

            var added = await _repository.AddAsync(user);

            await _repository.SaveChangesAsync();
            var userDetail = _mapper.Map<UserResponseDto>(added);
            return userDetail;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDTO dto)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            // Use AutoMapper to apply only provided fields (mapping profile controls null/whitespace behavior)
            _mapper.Map(dto, user);

            user.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return false;

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            // Find user by email
            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email");
            }

            // Verify password
            bool isPasswordValid = BC.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new InvalidOperationException("Invalid password");
            }

            return user;
        }
    }
}
