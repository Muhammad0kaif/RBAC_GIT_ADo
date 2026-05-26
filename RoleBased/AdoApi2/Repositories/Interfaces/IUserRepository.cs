using PocoClasses;
using PocoClasses.Dto;

namespace AdoApi2.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        Task<User> GetUserById(Guid id);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(Guid id);
        Task UpdateProfilePicture(Guid userId, string filePath);
        Task UpdateProfile(Guid userId, string name, string email);
        Task UpdatePassword(Guid userId, string passwordHash, bool mustChangePassword);
        Task UpdateUserByAdmin(Guid userId, string name, string email, int roleId);
        Task InsertPasswordHistory(Guid userId, string passwordHash);
        Task<List<string>> GetLastPasswordHistory(Guid userId);
        Task<List<PasswordHistoryDto>> GetPasswordHistory(Guid userId);
    }
}