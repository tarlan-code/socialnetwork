using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.Extensions;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class PostService : IPostService
    {
        IUnitOfWork _unitOfWork;
        IPathService _pathService;

        public PostService(IUnitOfWork unitOfWork, IPathService pathService)
        {
            _unitOfWork = unitOfWork;
            _pathService = pathService;
        }

        public async Task<bool> AddPostAsync(Post post)
        {
            bool result = await _unitOfWork.GetWriteRepository<Post>().AddAsync(post);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<Post> GetPost(int id)
        =>await  _unitOfWork.GetReadRepository<Post>().GetAsync(p => p.Id == id,p=>p.AppUser,p=>p.PostMedias);

        public async Task<List<Post>> GetUserPosts(string userId,int skip = 0)
            => await _unitOfWork.GetReadRepository<Post>().GetAll(p => p.AppUserId == userId, p => p.PostMedias, p => p.AppUser,p=>p.PostReactions).OrderByDescending(p=>p.Date).Skip(skip).Take(3).Include(p=>p.PostTags).ThenInclude(p=>p.AppUser).ToListAsync();

        public async Task<bool> UpdatePost(Post post)
        {
            bool result = _unitOfWork.GetWriteRepository<Post>().Update(post);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<List<Post>> GetNewsFeedPosts(List<string> userIds,int skip = 0)
            =>await _unitOfWork.GetReadRepository<Post>().GetAll(p => userIds.Contains(p.AppUserId),p=>p.AppUser, p => p.PostMedias,p=>p.PostReactions)
            .OrderByDescending(p=>p.Date).Skip(skip).Take(3).Include(p=>p.PostTags).ThenInclude(p=>p.AppUser).ToListAsync();

        public async Task<List<PostMedia>> GetAllVideos()
        =>await _unitOfWork.GetReadRepository<PostMedia>().GetAll()
            .Include(p=>p.Post).ThenInclude(p=>p.AppUser)
            .Include(p=>p.Post).ThenInclude(p=>p.Comments)
            .Include(p=>p.Post).ThenInclude(p=>p.PostReactions)
            .ToListAsync();

        public async Task<bool> DeletePost(Post post)
        {
            bool result =  _unitOfWork.GetWriteRepository<Post>().Delete(post);
            if(post.PostMedias is not null)
            {
                foreach (PostMedia item in post.PostMedias)
                {
                    item.Media.Delete(Path.Combine(_pathService.UsersFolder, post.AppUser.UserName, "medias"));
                }
            }
            await _unitOfWork.SaveAsync();
            return result;
        }
        
          
    }
}
