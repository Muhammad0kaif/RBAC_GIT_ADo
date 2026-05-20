namespace PocoClasses.PocoClasses
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiryDate { get; set; }

        public bool IsRevoked { get; set; }

        public Guid UserId { get; set; }   // ✅ KEEP ONLY THIS

        public User? User { get; set; }
    }
}