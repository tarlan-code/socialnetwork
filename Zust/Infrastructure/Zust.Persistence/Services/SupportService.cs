using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class SupportService : ISupportService
    {
        IUnitOfWork _unitOfWork;

        public SupportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddContact(Contact contact)
        {
            await _unitOfWork.GetWriteRepository<Contact>().AddAsync(contact);
            await _unitOfWork.SaveAsync();
        }


        public async Task<List<Contact>> GetContacts()
        => await _unitOfWork.GetReadRepository<Contact>().GetAll(null,c=>c.AppUser).ToListAsync();
        public async Task DeleteContact(Contact contact)
        {
            _unitOfWork.GetWriteRepository<Contact>().Delete(contact);
            await _unitOfWork.SaveAsync();
        }
        

        public async Task<Contact> GetContactAsync(int id)
        => await _unitOfWork.GetReadRepository<Contact>().GetAsync(c => c.Id == id);

        
    }
}
