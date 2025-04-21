namespace ChatApi.DTO.Results
{
    public class LoginResults
    {
        public bool _isSuccess { get; init; }
        public string message { get; init; } = string.Empty;
        public string token { get; init; } = string.Empty;
        public bool notFound { get; init; } = false;
    }
}
