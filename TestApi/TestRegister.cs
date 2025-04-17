using Castle.Core.Logging;
using ChatApi.Services.DataBase;
using ChatApi.Services.RegisterServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestApi
{
    public class TestRegister
    {
        private readonly RegistrServices _registrServices;
        private readonly AppDBContext _appDBContext;
        private readonly Mock<ILogger<RegistrServices>> _moqLogger;
        private readonly JwtService _jwtService;
        public TestRegister()
        {
            _moqLogger = new Mock<ILogger<RegistrServices>>();
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

            _registrServices = new RegistrServices( _moqLogger.Object, _appDBContext, _jwtService);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
