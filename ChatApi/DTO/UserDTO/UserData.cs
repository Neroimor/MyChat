namespace ChatApi.DTO.UserDTO
{
    public record class UserData
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string password { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public bool IsVerified { get; init; } = false;
        public string VerificationCode { get; init; } = string.Empty;
        public string AvatarImgBase64 { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public List<Message> Messages { get; init; } = new();
    }
}
