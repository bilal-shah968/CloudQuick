using AutoMapper;
using CloudQuick.Data;
using CloudQuick.Models;
using Microsoft.IdentityModel.Tokens;

namespace CloudQuick.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // CreateMap<Student, StudentDTo>();
            //CreateMap<StudentDTo, Student>();

            //Config for different property names 
            //CreateMap<StudentDTo, Student>().ReverseMap().ForMember(n => n.Name, opt => opt.MapFrom(x => x.StudentName));

            //Confiq for ignorring some property names
            //CreateMap<StudentDTo, Student>().ReverseMap().ForMember(n => n.StudentName, opt => opt.Ignore());

            //Confiq for Transforming some property names
            CreateMap<StudentDTo, Student>().ReverseMap()

                .ForMember(n => n.Address, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Address) ? "No adress found" : n.Address));



           
            //CreateMap<StudentDTo, Student>().ReverseMap();

        }
    }
}
