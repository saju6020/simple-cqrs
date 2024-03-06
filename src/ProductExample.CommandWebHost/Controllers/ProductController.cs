using Commands;
using Dtos;
using Microsoft.AspNetCore.Mvc;
using Platform.Infrastructure.Core.Bus;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductExample.CommandWebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IBusMessageDispatcher _busMessageDispatcher;
        public ProductController(
            ILogger<ProductController> logger,
            IBusMessageDispatcher busMessageDispatcher) {
            this._logger = logger;
            this._busMessageDispatcher = busMessageDispatcher;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
            
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task Post([FromBody] ProductDto productDto)
        {

            var productId = Guid.NewGuid();
            CreateProductCommand command = new CreateProductCommand()
            {
                ProductId = productId,
                Title = productDto.Title,
                Description = productDto.Description,
                CorrelationId = productId
            };

            await this._busMessageDispatcher.SendAsync(command);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
