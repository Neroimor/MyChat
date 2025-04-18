using Castle.Core.Logging;
using ChatApi.DTO.Results;
using ChatApi.DTO.UserDTO;
using ChatApi.Services.ConvertingData;
using ChatApi.Services.DataBase;
using ChatApi.Services.FileManagment;
using ChatApi.Services.RegisterServices.Interface;
using ChatApi.Services.RegisterServices.Realization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestApi
{
    public class TestRegister
    {
        private readonly IRegistrServices _registrServices;
        private readonly AppDBContext _appDBContext;
        private readonly Mock<ILogger<IRegistrServices>> _moqLogger;
        private readonly JwtService _jwtService;
        private readonly Argon2PasswordHasher _argon2PasswordHasher;
        private readonly IConvertingImage _convertingImage;
        private readonly IFileManagement _fileManagement;
        private readonly IGenerateCode _generateCode;
        public TestRegister()
        {
            _moqLogger = new Mock<ILogger<IRegistrServices>>();
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "testDataBase")
                .Options;
            _appDBContext = new AppDBContext(options);

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["Jwt:Secret"]).Returns("skfkdjfkdsjffdjkoxmaemczzxdqdmxdaslxkd123");
            mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("ThisApp");
            mockConfig.Setup(c => c["Jwt:Audience"]).Returns("ThisAppUsers");
            mockConfig.Setup(c => c["Jwt:ExpirationMinutes"]).Returns("60");

            _jwtService = new JwtService(mockConfig.Object);
            _argon2PasswordHasher = new Argon2PasswordHasher();
            _convertingImage = new ConvertingImage();
            _fileManagement = new FileManagement();
            _generateCode = new GeneratorCode();


            _registrServices = new RegistrServices( _moqLogger.Object, _appDBContext,
                _jwtService, _argon2PasswordHasher,_convertingImage,_fileManagement,_generateCode);
        }

        [Fact]
        public async Task TestRegisterationIsSuccessAsync()
        {
            var resultReg = new ResultsRegister()
            {
                message = "User registered",
                _isSuccess = true,
                token = string.Empty
            };
            var user = RegistrationFish();

            var result = await _registrServices.RegistrationAsync(user);

            Assert.Equal(result, resultReg);
        }
        [Fact]
        public async Task TestRegisterationNotIsSuccessAsync()
        {
            var resultReg = new ResultsRegister()
            {
                message = "The user is not registered",
                _isSuccess = false,
                token = string.Empty
            };
            var user = RegistrationFish();

            var result = await _registrServices.RegistrationAsync(user);

            Assert.Equal(result, resultReg);
        }

        [Fact]
        public async Task TestRegisterationNotIsSuccessAsync2()
        {
            var resultReg = new ResultsRegister()
            {
                message = "The user is already present",
                _isSuccess = false,
                token = string.Empty
            };
            var user = RegistrationFish();

            var result = await _registrServices.RegistrationAsync(user);

            Assert.Equal(result, resultReg);
        }

        private UserRegistration RegistrationFish()
        {
            var user = new UserRegistration()
            {
                Name = "TestName",
                Email = "TestEmail@test.test",
                Password = "TestPassword",

            };

            return user;
        }
    }
}
