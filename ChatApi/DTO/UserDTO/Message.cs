namespace ChatApi.DTO.UserDTO
{
    public record class Message
    {
        public int Id { get; init; }
        public int UserDataId { get; init; }
        public string text { get; init; } = string.Empty;
        public string[] imagesBase64 { get; init; } = Array.Empty<string>();
        public DateTime createdAt { get; init; }

        public UserData? UserData { get; init; }

    }
}
