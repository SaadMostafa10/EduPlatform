using AutoMapper;
using Domain.Models.Identity;
using Domain.Models.Lessons;
using Shared.Dtos.AuthDtos;
using Shared.Dtos.LessonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<ApplicationUser, AuthResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Grade, GradeDto>();

            //
            CreateMap<Lesson, LessonResponseDto>()
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.Grade.Name));

            CreateMap<CreateLessonRequestDto, Lesson>();

            CreateMap<UpdateLessonRequestDto, Lesson>();
        }
    }
}
