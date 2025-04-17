using ChatApi.DTO.UserDTO;
using ChatApi.Services.DataBase;

namespace ChatApi.Services.RegisterServices
{
    public class RegistrServices : IRegistrServices
    {
        private readonly AppDBContext _context;
        private readonly ILogger<RegistrServices> _logger;
        private readonly JwtService _jwtService;
        public RegistrServices(ILogger<RegistrServices> logger, AppDBContext context, JwtService jwtService)
        {
            _logger = logger;
            _context = context;
            _jwtService = jwtService;
        }
        public Task<string> RegistrationAsync(UserRegistration userRegistration)
        {
            throw new NotImplementedException();
        }
        public Task<string> LoginAsync(UserLogin userLogin)
        {
            throw new NotImplementedException();
        }
        public Task<string> ForgotPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }



        public Task<string> LogoutAsync(string token)
        {
            throw new NotImplementedException();
        }


        public Task<string> ResetPasswordAsync(string code, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<string> VerificationEmailAsync(string code)
        {
            throw new NotImplementedException();
        }
    }
}
