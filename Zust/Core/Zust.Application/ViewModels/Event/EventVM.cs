using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Zust.Application.ViewModels
{
    public class EventVM
    {
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(40)]
        public string Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
        public IFormFile Image { get; set; }
        public int EventCategoryId { get; set; }

    }
}
