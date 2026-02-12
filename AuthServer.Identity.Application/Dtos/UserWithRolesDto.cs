namespace AuthServer.Identity.Application.Dtos
{
    public class UserWithRolesDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
    }
}