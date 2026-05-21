using PocoClasses;

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
    }
}