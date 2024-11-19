using AutoMapper;
using CloudQuick.Data;
using CloudQuick.Models;

namespace CloudQuick.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
              // CreateMap<Student, StudentDTo>();
               //CreateMap<StudentDTo, Student>();

            CreateMap<StudentDTo, Student>().ReverseMap();
        }
    }
}
