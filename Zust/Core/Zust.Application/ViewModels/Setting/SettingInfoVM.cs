using System.ComponentModel.DataAnnotations;

namespace Zust.Application.ViewModels
{
    public class SettingInfoVM
    {
        [MinLength(3), MaxLength(25)]
        public string Name { get; set; }
        [MinLength(3), MaxLength(30)]
        public string? Surname { get; set; }
        [MaxLength(30)]
        public string? Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public bool IsConfirmedUpdateEmail { get; set; }
        public int? GenderId { get; set; }
        public int? RelationId { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public PrivacyVM? privacyVM { get; set;}
    }
}
