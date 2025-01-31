namespace Persistance.DTOs.Accounts
{
    public class ApplicationUserDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string RoleName { get; set; }
        public string Address { get; set; }
    }
}
