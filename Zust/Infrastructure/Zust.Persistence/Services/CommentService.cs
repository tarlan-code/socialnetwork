using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class CommentService : ICommentService
    {
        IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            bool result = await _unitOfWork.GetWriteRepository<Comment>().AddAsync(comment);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<Comment?> GetComment(int? id)
        {
            if (id is null) return null;

            return await _unitOfWork.GetReadRepository<Comment>().GetAsync(c => c.Id == id);
        }


        public async Task<List<Comment>> GetReplyComments(int? id)
        {
            if (id is null) return null;
            return  _unitOfWork.GetReadRepository<Comment>().GetAll(c => c.RepliedId == id,c=>c.AppUser).ToList();
        }

        public async Task<int> GetCommentCount(int? postId)
        {
            if (postId is null) return 0;
            return await _unitOfWork.GetReadRepository<Comment>().CountAsync(c => c.PostId == postId);
        }

        public async Task<List<Comment>> GetCommentsAsync(int postId,int skip = 0, int take=3)
            =>_unitOfWork.GetReadRepository<Comment>().GetAll(c => c.PostId == postId && c.RepliedId == null,c=>c.AppUser).OrderByDescending(c => c.Date).Skip(skip).Take(take).ToList();
        
    }
}
