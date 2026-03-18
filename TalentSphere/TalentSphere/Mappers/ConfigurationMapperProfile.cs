using AutoMapper;
using TalentSphere.Models;
using TalentSphere.DTOs;
namespace TalentSphere.Mappers
{
    public class ConfigurationMapperProfile : Profile
    {
        public ConfigurationMapperProfile()
        {

            // ComplianceRecord mappings
            CreateMap<CreateComplianceRecordDTO, ComplianceRecord>()
           .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
           .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result))
           .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
           .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
           .ForMember(dest => dest.EmployeeID, opt => opt.MapFrom(src => src.EmployeeID))
           .ReverseMap();

            // Audit mappings
            CreateMap<CreateAuditDTO, Audit>()
                .ReverseMap();

            // Job mappings
            CreateMap<CreateJobDTO, Job>()
                .ReverseMap();

            // Interview mappings
            CreateMap<CreateInterviewDTO, Interview>()
                .ReverseMap();

            // Selection mappings
            CreateMap<CreateSelectionDTO, Selection>()
                .ReverseMap();

            //Application mappings
            CreateMap<CreateApplicationDTO, Application>()
                .ReverseMap();

            //Resume mappings
            CreateMap<CreateResumeDTO, Resume>()
                .ReverseMap();

            //Screening mappings
            CreateMap<CreateScreeningDTO, Screening>()
                .ReverseMap();
            //Report mappings
            CreateMap<CreateReportDTO, Report>()
                .ReverseMap();

            //Training mappings
            CreateMap<CreateTrainingDTO, Training>()
                            .ReverseMap();

            //Enrollment mappings
            CreateMap<CreateEnrollmentDTO, Enrollment>()
                .ReverseMap();

            //SuccessionPlan mappings
            CreateMap<CreateSuccessionPlanDTO, SuccessionPlan>()
                    .ReverseMap();

            

            
        }
    }
}
