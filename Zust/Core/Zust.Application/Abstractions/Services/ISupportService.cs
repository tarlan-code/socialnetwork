using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zust.Domain.Entities;

namespace Zust.Application.Abstractions.Services
{
    public interface ISupportService
    {
        Task AddContact(Contact contact);
        Task<List<Contact>> GetContacts();
        Task DeleteContact(Contact contact);
        Task<Contact> GetContactAsync(int id);
    }
}
