using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Platform.Infrastructure.Core.Commands;
using Platform.Infrastructure.Core.Validation;
using SimpleCQRS.UAM.Database;
using SimpleCQRS.UAM.Domain.Commands;
using System.Collections.Generic;

namespace SimpleCQRS.UAM.Domain.CommandHandlers
{
    public class CreateUserCommandHandler : ICommandHandlerAsync<CreateUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(UserManager<User> userManager, IMapper mapper)
        {
            this._userManager = userManager;
            this._mapper = mapper;
        }


        public async Task<CommandResponse> HandleAsync(CreateUserCommand command)
        {
            CommandResponse commandResponse = new CommandResponse();
            var user = this._mapper.Map<User>(command);
            var userId = Guid.NewGuid();
            
            UserRole userRole = new UserRole();
            userRole.RoleId = command.RoleId;
            userRole.UserId = userId;

            user.Id = userId;
            user.CreatedAt = DateTime.UtcNow;
            user.UserRoles = new List<UserRole>
            {
                userRole
            };

            var identityResult = await this._userManager.CreateAsync(user);

            commandResponse.ValidationResult = new ValidationResponse();

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {                    
                    commandResponse.ValidationResult.AddError(error.Description, error.Code);
                }
            }

            commandResponse.Result = identityResult.Succeeded;

            return commandResponse;
        }
    }
}
