using System.Text.RegularExpressions;

namespace AdoApi2.Helper
{
    public static class PasswordPolicyHelper
    {
        public static bool IsValid(string password, out string error)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                error = "Password is required";
                return false;
            }

            if (password.Length < 8)
            {
                error = "Password must be at least 8 characters";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                error = "Password must contain at least one uppercase letter";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                error = "Password must contain at least one number";
                return false;
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""{}|<>_\-+=]"))
            {
                error = "Password must contain at least one special character";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }
}