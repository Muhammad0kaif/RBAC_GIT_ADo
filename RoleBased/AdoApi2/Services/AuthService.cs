using AdoApi2.Helper;
using AdoApi2.Repositories.Interfaces;
using BCrypt.Net;
using PocoClasses;
using PocoClasses.Dto;

using System.Security.Cryptography;

namespace AdoApi2.Services
{
    public class AuthService(IAuthRepository authRepo, JwtService jwtService)
    {
        private readonly IAuthRepository _authRepo = authRepo;
        private readonly JwtService _jwtService = jwtService;

        #region LOGIN

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            var user = await _authRepo.GetUserByEmail(dto.Email);

            if (user == null)
                return null;

            
            var isValid = PasswordHelper.Verify(dto.Password, user.Password);

            if (!isValid)
                return null;
            var role = await _authRepo.GetRoleById(user.RoleId);

            var token = _jwtService.GenerateToken(
                user.Id,
                user.RoleId,
                role.RoleName
            );
            var refreshToken = GenerateRefreshToken();

            await _authRepo.SaveRefreshToken(
                refreshToken,
                DateTime.UtcNow.AddDays(7),
                user.Id
            );
            var permissions = await _authRepo.GetPermissionsByRoleId(user.RoleId);
            return new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Role = role.RoleName,
                MustChangePassword = user.MustChangePassword,
                Permissions = permissions
            };
        }

        #endregion

        #region REGISTER

        public async Task<bool> Register(RegisterDto dto)
        {
            var existingUser = await _authRepo.GetUserByEmail(dto.Email);

            if (existingUser != null)
                return false;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = PasswordHelper.Hash(dto.Password),
                RoleId = 2
            };

            await _authRepo.RegisterUser(user);

            return true;
        }

        #endregion

        #region REFRESH TOKEN

        public async Task<AuthResponseDto> RefreshToken(string refreshToken)
        {
            var storedToken = await _authRepo.GetRefreshToken(refreshToken);

            if (storedToken == null || storedToken.IsRevoked)
                return null;

            if (storedToken.ExpiryDate < DateTime.UtcNow)
                return null;

            await _authRepo.RevokeRefreshToken(refreshToken);

            var user = await _authRepo.GetUserById(storedToken.UserId);
           
            var role = await _authRepo.GetRoleById(user.RoleId);
            if (role == null)
                return null;

            var newAccessToken = _jwtService.GenerateToken(
                user.Id,
                user.RoleId,
                role.RoleName
            );

            var newRefreshToken = GenerateRefreshToken();

            await _authRepo.SaveRefreshToken(
                newRefreshToken,
                DateTime.UtcNow.AddDays(7),
                storedToken.UserId
            );

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                UserId = storedToken.UserId,
                MustChangePassword = user.MustChangePassword,
                Role = role.RoleName
            };
        }

        #endregion

        #region HELPERS

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        #endregion

        #region Get Roles
        public async Task<List<Role>> GetRoles()
        {
            return await _authRepo.GetRoles();
        }
        #endregion

        #region Create User
        public async Task<bool> CreateUser(UserDto dto)
        {
            var existing = await _authRepo.GetUserByEmail(dto.Email);

            if (existing != null)
                return false;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Password = PasswordHelper.Hash(dto.Password),
                RoleId = dto.RoleId
            };

            await _authRepo.RegisterUser(user);

            return true;
        }

        #endregion
    }
}