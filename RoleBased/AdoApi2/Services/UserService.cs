using AdoApi2.Helper;
using AdoApi2.Repositories.Implemenetation;
using AdoApi2.Repositories.Interfaces;
using PocoClasses;
using PocoClasses.Dto;

namespace AdoApi2.Services
{
    public class UserService(IUserRepository repo, EmailService emailService)
    {
        private readonly IUserRepository _repo = repo;

        public Task<List<User>> GetUsers() => _repo.GetUsers();
        public Task<User> GetUserById(Guid id) => _repo.GetUserById(id);
        public Task CreateUser(User user) => _repo.CreateUser(user);
        public Task UpdateUser(User user) => _repo.UpdateUser(user);
        public Task DeleteUser(Guid id) => _repo.DeleteUser(id);

        public Task UpdateProfilePicture(Guid userId, string filePath)
        {
            return _repo.UpdateProfilePicture(userId, filePath);
        }
        public async Task CreateUserByAdmin(User user)
        {
            var tempPassword = GenerateTemporaryPassword();

            user.Id = Guid.NewGuid();
            user.Password = PasswordHelper.Hash(tempPassword);
            user.MustChangePassword = true;

            await _repo.CreateUser(user);

            var body = $@"
            <h3>Welcome, {user.Name}</h3>
            <p>Your account has been created.</p>
            <p><strong>Email:</strong> {user.Email}</p>
            <p><strong>Temporary Password:</strong> {tempPassword}</p>
            <p>Please login and change your password immediately.</p>";

            await emailService.SendEmailAsync( user.Email, "Your account has been created", body);
        }

        public async Task<bool> ChangePassword(Guid userId, ChangePasswordDto dto)
        {
            var user = await _repo.GetUserById(userId);

            if (user == null)
                return false;

            var passwordHash = PasswordHelper.Hash(dto.NewPassword);

            await _repo.UpdatePassword( userId, passwordHash, false);

            return true;
        }

        private static string GenerateTemporaryPassword()
        {
            return $"Temp@{Guid.NewGuid().ToString("N")[..8]}1";
        }
    }
}
