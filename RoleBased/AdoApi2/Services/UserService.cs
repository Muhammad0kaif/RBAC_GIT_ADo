using AdoApi2.Helper;
using AdoApi2.Repositories.Implemenetation;
using AdoApi2.Repositories.Interfaces;
using PocoClasses;
using PocoClasses.Dto;

namespace AdoApi2.Services
{
    public class UserService(IUserRepository repo, IEmailService emailService, IEmailTemplateService emailTemplateService, IPasswordService passwordService)
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

        public Task UpdateProfile(Guid userId, string name, string email)
        {
            return _repo.UpdateProfile(userId, name, email);
        }

        public Task UpdateUserByAdmin(Guid userId, string name, string email,int roleId)
        {
            return _repo.UpdateUserByAdmin(userId, name, email, roleId);
        }
        public async Task CreateUserByAdmin(User user)
        {
            var tempPassword = passwordService.GenerateTemporaryPassword();

            user.Id = Guid.NewGuid();
            user.Password = passwordService.HashPassword(tempPassword);
            user.MustChangePassword = true;

            await _repo.CreateUser(user);

            var loginUrl = "https://localhost:7196/Account/Login";

            var body = emailTemplateService.BuildNewUserEmail(user.Name, user.Email,tempPassword,loginUrl);


            await emailService.SendEmailAsync( user.Email, "Your account has been created", body);
        }

        public async Task<bool> ChangePassword(Guid userId, ChangePasswordDto dto)
        {
            var user = await _repo.GetUserById(userId);

            if (user == null)
                return false;

            if (!PasswordHelper.Verify(dto.OldPassword, user.Password))
                throw new Exception("Old password is incorrect");

            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("New password and confirm password do not match");

            if (!PasswordPolicyHelper.IsValid(dto.NewPassword, out string error))
                throw new Exception(error);

            var lastHashes = await _repo.GetLastPasswordHistory(userId);

            foreach (var oldHash in lastHashes)
            {
                if (PasswordHelper.Verify(dto.NewPassword, oldHash))
                {
                    throw new Exception("You cannot reuse your last 3 passwords");
                }
            }

            await _repo.InsertPasswordHistory(userId, user.Password);

            var passwordHash = PasswordHelper.Hash(dto.NewPassword);

            await _repo.UpdatePassword(
                userId,
                passwordHash,
                false);

            return true;
        }


    }
}
