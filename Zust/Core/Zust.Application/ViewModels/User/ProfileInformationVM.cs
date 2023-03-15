using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zust.Application.ViewModels
{
    public class ProfileInformationVM
    {
        [MinLength(3), MaxLength(25)]
        public string Name { get; set; }
        [MinLength(3), MaxLength(30)]
        public string? Surname { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(30)]
        public string Username { get; set; }
        public string ProfilImage { get; set; }
        public string BannerImage { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }
        [Phone]
        public string PhoneNo { get; set; }
        public string Gender { get; set; }
        public string Relation { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [MaxLength(1000)]
        public string? About { get; set; }
        [MaxLength(10000)]
        public string? EducationOrWorks { get; set; }
        public int FriendsCount { get; set; }
        public int LikeCount { get; set; }
    }
}
