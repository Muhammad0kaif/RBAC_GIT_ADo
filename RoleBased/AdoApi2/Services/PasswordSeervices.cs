using AdoApi2.Helper;
using AdoApi2.Repositories.Interfaces;


namespace AdoApi2.Services
{
    public class PasswordService : IPasswordService
    {
        public string GenerateTemporaryPassword()
        {
            return $"Temp@{Guid.NewGuid().ToString("N")[..8]}1";
        }

        public string HashPassword(string password)
        {
            return PasswordHelper.Hash(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return PasswordHelper.Verify(password, hash);
        }
    }
}