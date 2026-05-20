namespace PocoClasses
{
    public class Permission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string PageName { get; set; } = string.Empty; // Dashboard, Users
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
    }
}
