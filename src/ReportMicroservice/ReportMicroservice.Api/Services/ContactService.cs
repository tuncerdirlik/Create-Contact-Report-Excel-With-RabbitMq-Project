using ReportMicroservice.Api.Models;
using ReportMicroservice.Api.Services.Contracts;

namespace ReportMicroservice.Api.Services
{
    public class ContactService : IContactService
    {
        private readonly HttpClient _httpClient;

        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDto<List<ContactDto>>> GetAsync()
        {
            return await _httpClient.GetFromJsonAsync<ResponseDto<List<ContactDto>>>("http://localhost:5083/api/contacts");
        }
    }
}
