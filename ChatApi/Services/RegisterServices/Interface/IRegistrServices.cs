using ChatApi.DTO.Results;
using ChatApi.DTO.UserDTO;

namespace ChatApi.Services.RegisterServices.Interface
{
    public interface IRegistrServices
    {
        public Task<ResultsRegister> RegistrationAsync(UserRegistration userRegistration);
        public Task<LoginResults> LoginAsync(UserLogin userLogin);
        public Task<ResultsRegister> VerificationEmailAsync(string code);
        public Task<ResultsRegister> ForgotPasswordAsync(string email);
        public Task<ResultsRegister> ResetPasswordAsync(string code, string newPassword);

    }
}
