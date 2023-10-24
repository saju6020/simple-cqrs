using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualBasic;
using Platform.Infrastructure.Core;
using Platform.Infrastructure.Core.Commands;
using SimpleCQRS.UAM.Common;
using SimpleCQRS.UAM.Domain.Commands;
using SimpleCQRS.UAM.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UAM.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IDispatcher _dispatcher;

        public AuthController(IMapper mapper, IDispatcher dispatcher)
        {
            this._mapper = mapper;
            this._dispatcher = dispatcher;
        }

        // POST api/<AuthController>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDetailsDto userDetailsDto)
        {
            if (userDetailsDto == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var command = this._mapper.Map<CreateUserCommand>(userDetailsDto);
            command.RoleId = Guid.Parse(UAMConstants.USER_ROLE_ID);

            var response = await this._dispatcher.SendAsync(command);
           
           if (!response.ValidationResult.IsValid)
            {              
                return new BadRequestObjectResult(new { Message = "User Registration Failed", response.ValidationResult });
            }

            return Ok(new { Message = "User Reigstration Successful" });
        }

        // POST api/<AuthController>
        [HttpPost]
        [Route("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] UserDetailsDto userDetailsDto)
        {
            if (userDetailsDto == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var command = this._mapper.Map<CreateUserCommand>(userDetailsDto);
            command.RoleId = Guid.Parse(UAMConstants.ADMIN_ROLE_ID);

            var response = await this._dispatcher.SendAsync(command);

            if (!response.ValidationResult.IsValid)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed", response.ValidationResult });
            }

            return Ok(new { Message = "User Reigstration Successful" });
        }
    }
}

