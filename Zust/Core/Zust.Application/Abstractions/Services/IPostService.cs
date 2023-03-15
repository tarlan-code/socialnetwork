using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IPostService
    {
        Task<bool> AddPostAsync(Post post);
        Task<List<Post>> GetUserPosts(string userId, int skip = 0);
        Task<Post> GetPost(int id);
        Task<bool> UpdatePost(Post post);
        Task<List<Post>> GetNewsFeedPosts(List<string> userIds,int skip = 0);
        Task<List<PostMedia>> GetAllVideos();
        Task<bool> DeletePost(Post post);
    }
}
