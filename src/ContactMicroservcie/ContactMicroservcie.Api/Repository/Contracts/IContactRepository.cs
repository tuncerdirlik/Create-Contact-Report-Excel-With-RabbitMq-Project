using ContactMicroservcie.Api.Models.Dtos;

namespace ContactMicroservcie.Api.Repository.Contracts
{
    public interface IContactRepository
    {
        Task<IEnumerable<ContactDto>> GetAll();
    }
}
