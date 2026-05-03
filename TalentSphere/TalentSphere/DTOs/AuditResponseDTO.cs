namespace TalentSphere.DTOs
{
    public class AuditResponseDTO
    {
        public int AuditID { get; set; }
        public string Description { get; set; }
        public DateTime AuditDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
