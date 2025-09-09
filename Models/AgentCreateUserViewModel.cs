using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mycourier.Models;
using MyCourier.Models; // Ensure this points to where your User model is

namespace MyCourier.Models.ViewModels
{
    public class AgentCreateUserViewModel
    {
        public int Id { get; set; }  // <-- Add this for update scenarios

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserType { get; set; } = "user";

        // List of existing users to show in the CreateUser page
        public List<User> ExistingUsers { get; set; } = new List<User>();
    }
}
