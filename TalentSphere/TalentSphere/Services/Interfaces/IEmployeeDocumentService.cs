using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using TalentSphere.DTOs;
using TalentSphere.Models;

namespace TalentSphere.Services.Interfaces
{
    public interface IEmployeeDocumentService
    {
        Task<EmployeeDocumentResponseDto> CreateEmployeeDocumentAsync(CreateEmployeeDocumentDTO dto);
        Task<EmployeeDocumentResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeDocumentResponseDto>> GetAllAsync();
        Task<IEnumerable<EmployeeDocumentResponseDto>> GetByEmployeeIdAsync(int employeeId);
        Task<EmployeeDocumentResponseDto> UploadDocumentAsync(int employeeId, string docType, IFormFile file);
        Task<bool> SendDocumentReminderAsync(int employeeId);
        Task<EmployeeDocumentResponseDto> UpdateEmployeeDocumentAsync(int id, UpdateEmployeeDocumentDTO dto);
        Task<bool> DeleteEmployeeDocumentAsync(int id);
    }
}
