using ContactMicroservcie.Api.Models.Dtos;
using ContactMicroservcie.Api.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactMicroservcie.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        protected ResponseDto _response;
        private IContactRepository _contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            this._response = new ResponseDto();
            _contactRepository = contactRepository;
        }

        // GET: api/<ContactsController>
        [HttpGet]
        public async Task<ResponseDto> Get()
        {
            try
            {
                var contacts = await _contactRepository.GetAll();
                _response.Result= contacts;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        
    }
}
