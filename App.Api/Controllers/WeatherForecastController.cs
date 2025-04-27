using App.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TestController : ControllerBase
    {
        
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Tests")]
        public IEnumerable<TestClass> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new TestClass
            {
                Name = "Anas",
                TestValue = true
            })
            .ToArray();
        }
    }
}
