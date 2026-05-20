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
        Task UpdatePassword(Guid userId, string passwordHash, bool mustChangePassword);
    }
}