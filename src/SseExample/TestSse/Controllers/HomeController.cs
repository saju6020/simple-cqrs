using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestSse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;
        private readonly ISseHolder sse;


        public HomeController(ILogger<HomeController> logger, ISseHolder sse)
        {
            this.logger = logger;
            this.sse = sse;
        }



        // GET: api/<HomeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HomeController>
        [HttpPost]
        public async Task<string> Post([FromBody] SseMessage? message)
        {
            if (string.IsNullOrEmpty(message?.Id) ||
            message?.Message == null)
            {
                return "No messages";
            }
            await this.sse.SendMessageAsync(message);
            return "";
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
