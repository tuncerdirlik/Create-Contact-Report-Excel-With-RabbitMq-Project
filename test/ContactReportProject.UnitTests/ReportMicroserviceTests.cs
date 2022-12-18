using ReportMicroservice.Api.Repositories.Contracts;
using ReportMicroservice.Api.Services.Contracts;
using ReportMicroservice.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ReportMicroservice.Api.Models;
using Microsoft.AspNetCore.Http;
using ReportMicroservice.Api.Controllers;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Connections;

namespace ContactReportProject.UnitTests
{
    public class ReportMicroserviceTests
    {
        private readonly Mock<IContactService> _contactService;
        private readonly Mock<IUnitOfWork> _uow;
        
        private ResponseDto<List<ContactDto>> responseDto;

        private Mock<IRabbitMqPublisher> _rabbitMqPublisher;

        public ReportMicroserviceTests()
        {
            responseDto = new ResponseDto<List<ReportMicroservice.Api.Models.ContactDto>>
            {
                IsSuccess = true,
                Result = new List<ReportMicroservice.Api.Models.ContactDto>
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
                }
            };

            _contactService = new Mock<IContactService>();
            _uow = new Mock<IUnitOfWork>();
            
            var reportFileRepository = new Mock<IReportFileRepository>();
            _uow.Setup(k => k.ReportFileRepository).Returns(reportFileRepository.Object);

            _contactService.Setup(k => k.GetAsync()).ReturnsAsync(responseDto);

            _rabbitMqPublisher = new Mock<IRabbitMqPublisher>();
            _rabbitMqPublisher.Setup(k => k.Publish(It.IsAny<RabbitMqPublishModel>()));
        }

        [Fact]
        public async Task Return_Success_When_Gettin_All_Contacts()
        {
            var controller = new ContactsController(_contactService.Object, _rabbitMqPublisher.Object, _uow.Object);
            var response = await controller.Get();

            _uow.Verify(k => k.SaveAsync(),Times.Once());
            _rabbitMqPublisher.Verify(k => k.Publish(It.IsAny<RabbitMqPublishModel>()),Times.Once());
        }
    }
}
