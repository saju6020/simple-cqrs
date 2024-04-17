using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core.Commands;
using Platform.Infrastructure.Core.Validation;
using System.Text.Json.Nodes;

namespace GenericCommandWeb.Domain
{
    public class CommandExecutionService : ICommandExecutionService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextProvider _userContextProvider;
        private readonly IValidationService _validationService;
        private static readonly JsonNodeOptions jsonNodeOptions = new JsonNodeOptions { PropertyNameCaseInsensitive = true };
        public CommandExecutionService(IAuthorizationService authorizationService, IUserContextProvider userContextProvider) 
        {
            this._authorizationService = authorizationService;
            this._userContextProvider = userContextProvider;

        }
        public async Task<CommandResponse> ExecuteAsync(CommandDto commandDto)
        {
            CommandResponse commandResponse = new CommandResponse();
            var validationResult = await this._validationService.ValidateAnyObjectAsync(commandDto);

            if(validationResult.Errors.Count > 0)
            {
                commandResponse.ValidationResult = validationResult;              
                return commandResponse;
            }

            var userContext = this._userContextProvider.GetUserContext();
            var hasAccess = await this._authorizationService.HassAccess(commandDto.CommandType, userContext);
            if (hasAccess)
            {
                string commandBodyJson = System.Text.Json.JsonSerializer.Serialize(commandDto.Command);

                if (string.IsNullOrWhiteSpace(commandBodyJson))
                {
                    throw new ArgumentException(Constants.CommandObjectIsEmptyMessage);
                }

                JsonNode messageObject = JsonNode.Parse(commandBodyJson, jsonNodeOptions);
            }

            return commandResponse;
        }

        private JsonObject GetUserContextJson(UserContext userContext)
        {
            JsonArray rolesJsonArray = new JsonArray();

            foreach (string role in userContext.Roles)
            {
                rolesJsonArray.Add(role);
            }

            var userContextJson = new JsonObject
            {
                [nameof(userContext.UserId)] = userContext.UserId,
                [nameof(userContext.VerticalId)] = userContext.VerticalId,
                [nameof(userContext.TenantId)] = userContext.TenantId,
                [nameof(userContext.ServiceId)] = userContext.ServiceId,
                [nameof(userContext.Email)] = userContext.Email,
                [nameof(userContext.UserName)] = userContext.UserName,
                [nameof(userContext.PhoneNumber)] = userContext.PhoneNumber,
                [nameof(userContext.ClientId)] = userContext.ClientId,
                [nameof(userContext.Audience)] = userContext.Audience,
                [nameof(userContext.TokenIssuer)] = userContext.TokenIssuer,
                [nameof(userContext.LanguageCode)] = userContext.LanguageCode,
                [nameof(userContext.DisplayName)] = userContext.DisplayName,
                [nameof(userContext.DeviceId)] = userContext.DeviceId,
                [nameof(userContext.Roles)] = rolesJsonArray,
            };

            return userContextJson;
        }
    }
}
