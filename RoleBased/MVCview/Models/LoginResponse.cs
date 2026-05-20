namespace MVCview.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public bool MustChangePassword { get; set; }
        public List<PermissionDto> permissions { get; set; }
    }
}
