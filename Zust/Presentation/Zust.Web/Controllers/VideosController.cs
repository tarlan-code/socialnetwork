using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zust.Application.Abstractions.Services;
using Zust.Application.ViewModels;
using Zust.Domain.Entities;

namespace Zust.Web.Controllers
{
    [Authorize]
    [Authorize(Roles = "Member")]

    public class VideosController : Controller
    {
        IPostService _postService;
        IMapper _mapper;

        public VideosController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<PostMedia> videos = await _postService.GetAllVideos();
            videos = videos.Where(p => p.Media.Substring(p.Media.LastIndexOf(".")) == ".mp4").ToList();
            List<VideoVM> videoVMs = new();
            videos.ForEach(v => videoVMs.Add(_mapper.Map<VideoVM>(v)));
            return View(videoVMs);
        }
    }
}
