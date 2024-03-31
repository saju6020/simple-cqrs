using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core.Commands;
using Platform.Infrastructure.Core.Validation;

namespace GenericCommandWeb.Domain
{
    public class CommandExecutionService : ICommandExecutionService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextProvider _userContextProvider;
        private readonly IValidationService _validationService;
        public CommandExecutionService(IAuthorizationService authorizationService, IUserContextProvider userContextProvider) 
        {
            this._authorizationService = authorizationService;
            this._userContextProvider = userContextProvider;

        }
        public async Task<CommandResponse> ExecuteAsync(ClientCommand command)
        {
            CommandResponse commandResponse = new CommandResponse();
            var validationResult = await this._validationService.ValidateAnyObjectAsync(command);

            if(validationResult.Errors.Count > 0)
            {
                commandResponse.ValidationResult = validationResult;              
                return commandResponse;
            }

            var hasAccess = await this._authorizationService.HassAccess(command.CommandType, this._userContextProvider.GetUserContext());
            if (hasAccess)
            {

            }

            return commandResponse;
        }
    }
}
