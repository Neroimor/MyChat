namespace ChatApi.DTO.UserDTO
{
    public record class UserRegistration
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string AvatarImgBase64 { get; init; } = string.Empty;

    }
}
