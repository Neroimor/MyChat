namespace ChatApi.DTO.Results
{
    public record class ResultsRegister
    {

        public bool _isSuccess { get;init; }
        public string message { get; init; } = string.Empty;
        public string token { get; init; } = string.Empty;

    }
}
