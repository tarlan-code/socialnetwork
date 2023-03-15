using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zust.Application.Abstractions.Services
{
    public interface IPathService
    {
        public string UsersFolder { get; }
        public string HtmlFolder { get; }
    }
}
