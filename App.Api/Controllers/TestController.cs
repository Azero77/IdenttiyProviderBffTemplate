using App.Shared;
using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestController : ControllerBase
    {
        
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Tests")]
        public IEnumerable<TestClass> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new TestClass
            {
                Name = "Anas",
                TestValue = true
            })
            .ToArray();
        }

        [HttpGet("AccessToken")]
        public async Task<UserToken> GetAccessToken()
        {
            UserToken token = await HttpContext.GetUserAccessTokenAsync();
            return token;
        }
    }
}
