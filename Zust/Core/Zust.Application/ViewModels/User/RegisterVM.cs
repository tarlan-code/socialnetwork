using System.ComponentModel.DataAnnotations;

namespace Zust.Application.ViewModels
{
    public class RegisterVM
    {
        [MinLength(3),MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }





    }
}
