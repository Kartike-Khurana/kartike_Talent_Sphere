using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileStorageService> _logger;

        private static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".pdf", ".doc", ".docx" };

        private static readonly Dictionary<string, string> ContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"]  = "application/pdf",
            [".doc"]  = "application/msword",
            [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        public FileStorageService(IWebHostEnvironment env, ILogger<FileStorageService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> SaveResumeAsync(IFormFile file, int candidateId)
        {
            if (file is null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            if (file.Length > MaxFileSizeBytes)
                throw new ArgumentException($"File exceeds the 5 MB limit (uploaded: {file.Length / 1024 / 1024} MB).");

            var ext = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(ext))
                throw new ArgumentException($"Unsupported file type '{ext}'. Allowed: pdf, doc, docx.");

            // Build storage path:  wwwroot/uploads/resumes/{candidateId}/
            var relativeFolder = Path.Combine("uploads", "resumes", candidateId.ToString());
            var absoluteFolder = Path.Combine(_env.WebRootPath, relativeFolder);

            Directory.CreateDirectory(absoluteFolder);

            var fileName   = $"{Guid.NewGuid()}{ext}";
            var physicalPath = Path.Combine(absoluteFolder, fileName);

            await using var stream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write);
            await file.CopyToAsync(stream);

            // Return the URI the client can use to download (forward slashes for URL)
            return $"/uploads/resumes/{candidateId}/{fileName}";
        }

        public void DeleteFile(string relativeUri)
        {
            try
            {
                var physical = GetPhysicalPath(relativeUri);
                if (physical is not null && File.Exists(physical))
                    File.Delete(physical);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not delete file at URI {Uri}", relativeUri);
            }
        }

        public string? GetPhysicalPath(string relativeUri)
        {
            if (string.IsNullOrWhiteSpace(relativeUri)) return null;

            // Strip leading slash and convert to OS path separators
            var relativePath = relativeUri.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physical = Path.Combine(_env.WebRootPath, relativePath);

            // Prevent path-traversal attacks
            var root = Path.GetFullPath(_env.WebRootPath);
            var candidate = Path.GetFullPath(physical);
            return candidate.StartsWith(root, StringComparison.OrdinalIgnoreCase) ? candidate : null;
        }

        public static string? GetContentType(string relativeUri)
        {
            var ext = Path.GetExtension(relativeUri);
            return ContentTypes.TryGetValue(ext, out var ct) ? ct : null;
        }
    }
}
