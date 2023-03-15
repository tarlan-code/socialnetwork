using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface ICommentService
    {
        Task<bool> AddComment(Comment comment);
        Task<Comment?> GetComment(int? id);
        Task<List<Comment>> GetReplyComments(int? id);
        Task<int> GetCommentCount(int? postId);
        Task<List<Comment>> GetCommentsAsync(int postId, int skip=0, int take=3);
    }
}
