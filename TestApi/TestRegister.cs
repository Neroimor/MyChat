using Castle.Core.Logging;
using ChatApi.DTO.Options;
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
using Microsoft.Extensions.Options;
using Moq;
using System.Linq;

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
        private readonly AvatarOptions _avatarOptions;
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

            var baseDir = AppContext.BaseDirectory;
            var projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));
            var filePath = Path.Combine(projectRoot, "ChatApi", "wwwroot", "assets", "notImage.png");
            var avatarOptions = new AvatarOptions
            {
                DefaultAvatarRelativePath = filePath
            };

            // Используем Options.Create для создания IOptions<AvatarOptions>
            var avatarOptionsAccessor = Options.Create(avatarOptions);

            _registrServices = new RegistrServices(_moqLogger.Object, _appDBContext,
                _jwtService, _argon2PasswordHasher, _convertingImage, _fileManagement, _generateCode, avatarOptionsAccessor);
        }


        [Fact]
        public async Task TestRegistrationIsSuccessAsync()
        {
            // Arrange
            var expected = new ResultsRegister
            {
                message = "User registered",
                _isSuccess = true,
                token = string.Empty
            };
            var userReg = RegistrationFish();

            var actual = await _registrServices.RegistrationAsync(userReg);

            Assert.Equal(expected.message, actual.message);
            Assert.True(actual._isSuccess);
            Assert.Equal(expected.token, actual.token);
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
            UserRegistration user = null!;

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

            await _registrServices.RegistrationAsync(user);
            var result = await _registrServices.RegistrationAsync(user);

            Assert.Equal(result, resultReg);

        }

        [Fact]
        public async Task TestLoginIsSuccessAsync()
        {
            var user = RegistrationFish();
            await RegisterUserAsync(user);
            var expected = new ResultsRegister
            {
                message = "User logged in",
                _isSuccess = true,
                token = string.Empty
            };
            var login = LoginFish();
            var result = await _registrServices.LoginAsync(login);
            Assert.Equal(expected.message, result.message);
            Assert.True(result._isSuccess);
            Assert.NotEmpty(result.token);
        }

        [Fact]
        public async Task TestLoginNotIsSuccessAsync()
        {

            var expected = new ResultsRegister
            {
                message = "User not found",
                _isSuccess = false,
                token = string.Empty
            };
            var login = LoginFish();
            var user = await _appDBContext.Users.Where(x => x.Email == login.Email).FirstOrDefaultAsync();
            if (user is not null)
            {
                _appDBContext.Remove(user);
                await _appDBContext.SaveChangesAsync();
            }


            var result = await _registrServices.LoginAsync(login);
            Assert.Equal(expected.message, result.message);
            Assert.False(result._isSuccess);
            Assert.Empty(result.token);
        }
        [Fact]
        public async Task TestLoginNotIsSuccessAsync2()
        {
            var user = RegistrationFish();
            await RegisterUserAsync(user);
            var expected = new ResultsRegister
            {
                message = "Invalid password",
                _isSuccess = false,
                token = string.Empty
            };
            var login = new UserLogin()
            {
                Email = "TestEmail@test.test",
                Password = "WrangPassword",

            };
            var result = await _registrServices.LoginAsync(login);
            Assert.Equal(expected.message, result.message);
            Assert.False(result._isSuccess);
            Assert.Empty(result.token);
        }


        private async Task RegisterUserAsync(UserRegistration user)
        {
            var userReg = RegistrationFish();
            await _registrServices.RegistrationAsync(userReg);
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
        private UserLogin LoginFish()
        {
            var user = new UserLogin()
            {
                Email = "TestEmail@test.test",
                Password = "TestPassword",

            };

            return user;
        }
    }
}
