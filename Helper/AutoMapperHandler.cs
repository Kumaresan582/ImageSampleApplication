using AutoMapper;
using Entity.Model;
using Repo.MapperModel;
using System;

namespace Helper
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            CreateMap<Customer, CustomerMapper>()
                .ForMember(dest => dest.Statusname, opt => opt.MapFrom(src => (src.IsActive != null && src.IsActive.Value) ? "Active" : "Inactive"))
                .ReverseMap();
        }
    }
}
