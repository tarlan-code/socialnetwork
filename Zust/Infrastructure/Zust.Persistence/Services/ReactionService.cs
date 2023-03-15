using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class ReactionService : IReactionService
    {
        IUnitOfWork _unitOfWork;

        public ReactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Reaction> GetReaction(string name)
        => await _unitOfWork.GetReadRepository<Reaction>().GetAsync(r => r.IconName == name);

        public async Task<PostReaction> GetPostReaction(string userId,int postId)
            =>await _unitOfWork.GetReadRepository<PostReaction>().GetAsync(pr=>pr.AppUserId == userId && pr.PostId == postId,p=>p.Reaction);

        public async Task<bool> DeletePostReaction(PostReaction postreaction)
        {
            bool result = _unitOfWork.GetWriteRepository<PostReaction>().Delete(postreaction);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<int> GetReactionsCount(string userId)
         =>await _unitOfWork.GetReadRepository<PostReaction>().CountAsync(r => r.AppUserId == userId);
            
         
    }
}
