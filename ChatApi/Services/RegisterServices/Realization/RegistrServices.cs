using ChatApi.DTO.Results;
using ChatApi.DTO.UserDTO;
using ChatApi.Services.ConvertingData;
using ChatApi.Services.DataBase;
using ChatApi.Services.FileManagment;
using ChatApi.Services.RegisterServices.Interface;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Services.RegisterServices.Realization
{
    public class RegistrServices : IRegistrServices
    {
        private readonly AppDBContext _context;
        private readonly ILogger<IRegistrServices> _logger;
        private readonly JwtService _jwtService;
        private readonly Argon2PasswordHasher _argon2PasswordHasher;
        private readonly IConvertingImage _convertingImage;
        private readonly IFileManagement _fileManagement;
        private readonly IGenerateCode _generateCode;
        public RegistrServices(ILogger<IRegistrServices> logger, AppDBContext context, 
            JwtService jwtService, Argon2PasswordHasher argon2PasswordHasher, 
            IConvertingImage convertingImage, IFileManagement fileManagement,
            IGenerateCode generateCode)
        {
            _logger = logger;
            _context = context;
            _jwtService = jwtService;
            _argon2PasswordHasher = argon2PasswordHasher;
            _convertingImage = convertingImage;
            _fileManagement = fileManagement;
            _generateCode = generateCode;
        }
        public async Task<ResultsRegister> RegistrationAsync(UserRegistration userRegistration)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userRegistration.Email);

                if (user is not null)
                {

                    _logger.LogError("User is already present");
                    return CreateResponse("The user is already present", false, string.Empty);
                }

                var hashedPassword = await HashPasswordArgonAsync(userRegistration.Password);

                string imageBase64 = await ImageAvatarBase64Async(userRegistration.AvatarImgBase64);

                var newUser = CreateUserData(userRegistration, hashedPassword, imageBase64);
                await _context.Users.AddAsync(newUser);
                return CreateResponse("User registered", true, string.Empty);
            }
            catch (Exception ex)
            {

                _logger.LogError("Error in RegistrationAsync: {Message}", ex.Message);
                return CreateResponse("The user is not registered", false,string.Empty); ;
            }


        }

        private UserData CreateUserData(UserRegistration userRegistration, string hashedPassword, string imageBase64)
        {
            return new UserData
            {
                Name = userRegistration.Name,
                Email = userRegistration.Email,
                Password = hashedPassword,
                Role = "User",
                IsVerified = false,
                VerificationCode = _generateCode.GenerateCode(6),
                AvatarImgBase64 = imageBase64,
                CreatedAt = DateTime.UtcNow,
                Messages = new List<Message>(),
            };
        }



        private async Task<string> ImageAvatarBase64Async(string imageBase64)
        {
            bool isBase64 = await _convertingImage.IsCheckBase64(imageBase64);
            if (isBase64)
                return imageBase64;

            var img = await _fileManagement.ReadFileAsync(@"wwwroot/assets/notImage.png");
            return await _convertingImage.ConvertImageToBase64(img);
        }


        private ResultsRegister CreateResponse(string message, bool isSuccess, string token)
        {
            return new ResultsRegister
            {
                message = message,
                _isSuccess = isSuccess,
                token = token
            };
        }



        private Task<string> HashPasswordArgonAsync(string password)
        {
            return Task.Run(() => _argon2PasswordHasher.HashPassword(password));
        }

        public Task<ResultsRegister> ForgotPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ResultsRegister> LoginAsync(UserLogin userLogin)
        {
            throw new NotImplementedException();
        }

        public Task<ResultsRegister> LogoutAsync(string token)
        {
            throw new NotImplementedException();
        }


        public Task<ResultsRegister> ResetPasswordAsync(string code, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<ResultsRegister> VerificationEmailAsync(string code)
        {
            throw new NotImplementedException();
        }
    }
}
