using System.ComponentModel.DataAnnotations;

namespace Zust.Application.ViewModels
{
    public class DeleteAccountVM
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
