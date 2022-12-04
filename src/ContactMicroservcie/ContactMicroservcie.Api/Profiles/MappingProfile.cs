using AutoMapper;
using ContactMicroservcie.Api.Models;
using ContactMicroservcie.Api.Models.Dtos;

namespace ContactMicroservcie.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactDto>().ReverseMap();
        }
    }
}
