namespace Persistance.DTOs.Accounts
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Address { get; set; }
    }
}
