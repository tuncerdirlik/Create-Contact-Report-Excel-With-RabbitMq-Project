using ReportMicroservice.Api.Models;

namespace ReportMicroservice.Api.Services.Contracts
{
    public interface IContactService
    {
        Task<ResponseDto<List<ContactDto>>> GetAsync();
    }
}
