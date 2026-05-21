namespace AdoApi2.Repositories.Interfaces
{
    public interface IPasswordService
    {
        string GenerateTemporaryPassword();

        string HashPassword(string password);

        bool VerifyPassword(string password, string hash);
    }
}
