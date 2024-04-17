using GenericCommandWeb.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Infrastructure.Core.Bus;
using Platform.Infrastructure.Core.Commands;
using Platform.Infrastructure.Core.Validation;
using Platform.Infrastructure.EndpointRoleFeatureMap.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GenericCommandWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(ApiProtectionType.Protected)]
    [Produces("application/json")]
    public class CommandController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IBusMessageDispatcher _busMessageDispatcher;
        public CommandController(IValidationService validationService, IBusMessageDispatcher busMessageDispatcher) { 
            this._validationService = validationService;
            this._busMessageDispatcher = busMessageDispatcher;
        }
        // GET: api/<CommandController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CommandController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CommandController>
        [HttpPost]
        public async void SendAsync([FromBody] CommandDto clientCommand)
        {            
            var validationResult = await this._validationService.ValidateAnyObjectAsync<CommandDto>(clientCommand);
            //var command = new Command();
        }

        // PUT api/<CommandController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommandController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
