using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Platform.Infrastructure.Core.Commands;
using SimpleCQRS.UAM.Database;
using SimpleCQRS.UAM.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCQRS.UAM.Domain.CommandHandlers
{
    public class CreateTokenCommandHandler : ICommandHandlerAsync<CreateTokenCommand>
    {
        private readonly UserManager<User> _userManager;
        
        public CreateTokenCommandHandler(UserManager<User> userManager) 
        {
            this._userManager = userManager;
        }

        public async Task<CommandResponse> HandleAsync(CreateTokenCommand command)
        {
           // this._userManager.veri
            throw new NotImplementedException();
        }
    }
}
