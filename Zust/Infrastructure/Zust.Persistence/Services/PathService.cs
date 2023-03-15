using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zust.Application.Abstractions.Services;

namespace Zust.Persistence.Services
{
    public class PathService : IPathService
    {
        IWebHostEnvironment _env;

        public PathService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string UsersFolder => Path.Combine(_env.WebRootPath, "assets", "zust", "users");

        public string HtmlFolder => Path.Combine(_env.WebRootPath, "assets", "html");
    }
}
