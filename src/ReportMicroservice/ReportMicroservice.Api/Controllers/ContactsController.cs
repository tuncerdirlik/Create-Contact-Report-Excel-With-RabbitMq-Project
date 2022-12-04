using Microsoft.AspNetCore.Mvc;
using ReportMicroservice.Api.Models;
using ReportMicroservice.Api.Repositories.Contracts;
using ReportMicroservice.Api.Services;
using ReportMicroservice.Api.Services.Contracts;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportMicroservice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        private readonly IUnitOfWork _uow;

        public ContactsController(IContactService contactService, RabbitMqPublisher rabbitMqPublisher, IUnitOfWork uow)
        {
            _contactService = contactService;
            _rabbitMqPublisher = rabbitMqPublisher;
            _uow = uow;
        }

        // GET: api/<ContactsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _contactService.GetAsync();
            string fileName = DateTime.Now.ToString().Replace(".","-").Replace(" ","-").Replace(":","-");

            var reportFile = new ReportFile();
            reportFile.FileName = string.Concat(fileName, ".xlsx");
            reportFile.IsCompleted = false;

            await _uow.ReportFileRepository.AddAsync(reportFile);
            await _uow.SaveAsync();

            _rabbitMqPublisher.Publish(new RabbitMqPublishModel
            {
                ReportFile= reportFile,
                Contacts = items.Result
            });

            return Ok();
        }

        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file, int reportFileId, string reportFileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", reportFileName);

            using FileStream fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);

            var reportFile = await _uow.ReportFileRepository.GetAsync(reportFileId);
            if (reportFile != null)
            {
                reportFile.FilePath = string.Concat("http://localhost:5274/", "wwwroot/files/", reportFileName);
                reportFile.IsCompleted = true;

                await _uow.ReportFileRepository.UpdateAsync(reportFile);
                await _uow.SaveAsync();
            }

            return Ok();
        }


    }
}
