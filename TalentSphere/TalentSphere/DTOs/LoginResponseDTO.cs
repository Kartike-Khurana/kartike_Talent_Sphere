namespace TalentSphere.DTOs
{
    public class LoginResponseDTO
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
