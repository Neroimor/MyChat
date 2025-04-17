using ChatApi.DTO.UserDTO;

namespace ChatApi.Services.RegisterServices
{
    public interface IRegistrServices
    {
        public Task<string> RegistrationAsync(UserRegistration userRegistration);
        public Task<string> LoginAsync(UserLogin userLogin);
        public Task<string> LogoutAsync(string token);
        public Task<string> VerificationEmailAsync(string code);
        public Task<string> ForgotPasswordAsync(string email);
        public Task<string> ResetPasswordAsync(string code, string newPassword);

    }
}
