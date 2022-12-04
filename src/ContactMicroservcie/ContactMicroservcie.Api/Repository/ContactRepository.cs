using AutoMapper;
using ContactMicroservcie.Api.DbContexts;
using ContactMicroservcie.Api.Models.Dtos;
using ContactMicroservcie.Api.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ContactMicroservcie.Api.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public ContactRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactDto>> GetAll()
        {
            var items = await _db.Contacts.ToListAsync();
            return _mapper.Map<IEnumerable<ContactDto>>(items); 
        }
    }
}
