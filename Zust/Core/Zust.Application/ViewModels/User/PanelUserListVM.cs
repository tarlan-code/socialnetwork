using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zust.Application.ViewModels
{
    public class PanelUserListVM
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public DateTime DeletedDate { get; set; }

    }
}
