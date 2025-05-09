namespace MyCourier.Models.ViewModels
{
    public class AgentCreateUserViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; } = "user";
    }
}
