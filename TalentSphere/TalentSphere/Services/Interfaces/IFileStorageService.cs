namespace TalentSphere.Services.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>Saves file to disk. Returns the stored relative URI (e.g. /uploads/resumes/1/abc.pdf).</summary>
        Task<string> SaveResumeAsync(IFormFile file, int candidateId);

        /// <summary>Deletes a previously saved file by its relative URI. No-ops if the file doesn't exist.</summary>
        void DeleteFile(string relativeUri);

        /// <summary>Returns the absolute physical path for a relative URI, or null if the file doesn't exist.</summary>
        string? GetPhysicalPath(string relativeUri);
    }
}
