using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface IReactionService
    {
        Task<Reaction> GetReaction(string name);
        Task<PostReaction> GetPostReaction(string userId, int postId);
        Task<bool> DeletePostReaction(PostReaction postreaction);
        Task<int> GetReactionsCount(string userId);
        
    }
}
