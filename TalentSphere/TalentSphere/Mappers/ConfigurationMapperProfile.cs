using AutoMapper;
using TalentSphere.Models;
using TalentSphere.DTOs;
using TalentSphere.Utils;

namespace TalentSphere.Mappers
{
    public class ConfigurationMapperProfile : Profile
    {
        public ConfigurationMapperProfile()
        {

            // ComplianceRecord mappings
            CreateMap<CreateComplianceRecordDTO, ComplianceRecord>()
                .ReverseMap();

            CreateMap<UpdateComplianceRecordDTO, ComplianceRecord>()
                .ReverseMap();

            CreateMap<ComplianceRecord, ComplianceRecordResponseDTO>()
                .ReverseMap();


            // Audit mappings
            CreateMap<CreateAuditDTO, Audit>()
                .ReverseMap();

            CreateMap<UpdateAuditDTO, Audit>()
                .ReverseMap();

            CreateMap<Audit, AuditResponseDTO>()

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

            // User mappings
            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => PasswordHasher.Hash(src.PasswordHash)));

            // Update mapping: map only provided (non-null and non-whitespace for strings) fields
            // NOTE: Avoid chaining across calls that may return void; assign to local variable first to prevent CS0023.
            var updateUserMap = CreateMap<UpdateUserDTO, User>();
            // Use the 4-argument Condition overload to be compatible with AutoMapper signatures
            updateUserMap.ForAllMembers(opt => opt.Condition((src, dest, srcMember, context) =>
                srcMember != null && (!(srcMember is string) || !string.IsNullOrWhiteSpace((string)srcMember))
            ));
            // Rely on the ForAllMembers condition to skip null/whitespace strings; map/hash password when present
            updateUserMap.ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => PasswordHasher.Hash(src.PasswordHash)));

            // Employee mappings
            CreateMap<CreateEmployeeDTO, Employee>()
                .ReverseMap();

            // Update mapping for Employee: map only provided fields (non-null and non-whitespace for strings)
            var updateEmployeeMap = CreateMap<UpdateEmployeeDTO, Employee>();
            updateEmployeeMap.ForAllMembers(opt => opt.Condition((src, dest, srcMember, context) =>
                srcMember != null && (!(srcMember is string) || !string.IsNullOrWhiteSpace((string)srcMember))
            ));

            // Employee response mapping
            CreateMap<Employee, EmployeeResponseDto>()
                .ReverseMap();

            // User mappings
            CreateMap<User, UserResponseDto>()
                .ReverseMap();

            // EmployeeDocument mappings
            CreateMap<CreateEmployeeDocumentDTO, EmployeeDocument>()
                .ReverseMap();

            // EmployeeDocument -> Response DTO
            CreateMap<EmployeeDocument, EmployeeDocumentResponseDto>()
                .ReverseMap();

            // AuditLog -> Response DTO
            CreateMap<AuditLog, AuditLogResponseDto>()
                .ReverseMap();

            // UserRole mappings
            CreateMap<CreateUserRoleDTO, UserRole>()
                .ReverseMap();

            // UserRole -> Response DTO
            CreateMap<UserRole, UserRoleResponseDto>()
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
