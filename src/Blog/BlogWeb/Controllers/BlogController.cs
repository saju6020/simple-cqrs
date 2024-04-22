using AutoMapper;
using Blog.ORM.Models;
using Microsoft.AspNetCore.Mvc;
using Platform.Infrastructure.Core;
using Platform.Infrastructure.Core.Commands;
using SimpleCQRS.Blog.Domain.Commands;
using SimpleCQRS.Blog.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public BlogController(IDispatcher dispatcher, IMapper mapper)
        {
            this._dispatcher = dispatcher;
            this._mapper = mapper;
        }
        // GET: api/<BlogController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BlogController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BlogController>
        [HttpPost]
        public async Task<CommandResponse> Post([FromBody] BlogDto blogDto)
        {
            var createPostCommand = this._mapper.Map<CreatePostCommand>(blogDto);
            return await this._dispatcher.SendAsync(createPostCommand);
        }

        // PUT api/<BlogController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BlogController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
