using ContactMicroservcie.Api.Controllers;
using ContactMicroservcie.Api.Models.Dtos;
using ContactMicroservcie.Api.Repository.Contracts;
using Moq;

namespace ContactReportProject.UnitTests
{
    public class ContactMicroservcieTests
    {
        private readonly Mock<IContactRepository> _mockContactRepository;
        private List<ContactDto> lstContats;

        public ContactMicroservcieTests()
        {
            lstContats = new List<ContactDto>
            {
               new ContactDto
               {
                   Id= 1,
                   first_name = "James",
                   last_name = "Butt",
                   company_name = "Benton, John B Jr",
                   address = "6649 N Blue Gum St",
                   city = "New Orleans",
                   county = "Orleans",
                   state = "LA",
                   zip = "70116",
                   phone1 = "504-621-8927",
                   phone2 = "504-845-1427",
                   email = "jbutt@gmail.com",
                   web = "http://www.bentonjohnbjr.com"
               }
            };

            _mockContactRepository = new Mock<IContactRepository>();
            _mockContactRepository.Setup(k => k.GetAll()).ReturnsAsync(lstContats);
        }

        [Fact]
        public async Task Return_Success_When_Gettin_All_Contacts()
        {
            var controller = new ContactsController(_mockContactRepository.Object);
            var response = await controller.Get();

            Assert.True(response.IsSuccess);
            Assert.IsAssignableFrom<IEnumerable<ContactDto>>(response.Result);
        }
    }
}
