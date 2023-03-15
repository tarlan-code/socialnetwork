using System.ComponentModel.DataAnnotations;

namespace Zust.Application.ViewModels
{
    public class UpdatePasswordVM
    {
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
