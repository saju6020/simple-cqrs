namespace Platform.Infrastructure.Core.Commands
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Dependencies;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Validation;

    /// <summary>Command sender class to produce all kind of methods related to send commands.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Commands.ICommandSender" />
    public class CommandSender : ICommandSender
    {
        private readonly IDomainEventProcessor eventProcessor;
        private readonly IHandlerResolver handlerResolver;
        private readonly IValidationService validationService;
        private readonly IOptions<CommandOptions> commandOptions;
        private readonly IOptions<ValidationOptions> validationOptions;
        private IUserContextProvider userContextProvider;
        private ILogger logger;

        private bool ValidateCommand(ICommand command) => command.ValidateCommand ?? this.validationOptions.Value.ValidateAllCommands;

        private bool PublishEvents(ICommand command) => command.PublishEvents ?? this.commandOptions.Value.PublishEvents;

        public CommandSender(
            IHandlerResolver handlerResolver,
            IUserContextProvider userContextProvider,
            IDomainEventProcessor eventProcessor,
            IValidationService validationService,
            IOptions<CommandOptions> commandOptions,
            IOptions<ValidationOptions> validationOptions,
            ILogger<CommandSender> logger)
        {
            this.handlerResolver = handlerResolver;
            this.userContextProvider = userContextProvider;
            this.eventProcessor = eventProcessor;
            this.validationService = validationService;
            this.commandOptions = commandOptions;
            this.validationOptions = validationOptions;
            this.logger = logger;
        }

        public async Task<CommandResponse> SendAsync(ICommand command)
        {
            CommandResponse commandResponse = new CommandResponse();
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            this.SetUserContext(command);

            if (this.ValidateCommand(command))
            {
                commandResponse.ValidationResult = await this.validationService.ValidateAsync(command).ConfigureAwait(false);
                if (commandResponse.ValidationResult != null && !commandResponse.ValidationResult.IsValid)
                {
                    return commandResponse;
                }
            }

            var handler = this.handlerResolver.ResolveHandler(command, typeof(ICommandHandlerAsync<>));
            var handleMethod = handler.GetType().GetMethod("HandleAsync", new[] { command.GetType() });
            var response = await (Task<CommandResponse>)handleMethod.Invoke(handler, new object[] { command });

            if (response == null)
            {
                return null;
            }

            if (response.Events == null)
            {
                return new CommandResponse(response.ValidationResult != null ? response.ValidationResult : new ValidationResponse(), response.Result);
            }

            await this.eventProcessor.Process(response.Events, command);

            return new CommandResponse(response.ValidationResult != null ? response.ValidationResult : new ValidationResponse(), response.Result);
        }

        private void SetUserContext(ICommand command)
        {
            if (command.UserContext == null)
            {
                command.SetUserContext(this.userContextProvider.GetUserContext());
            }
        }

        public CommandResponse Send(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            this.SetUserContext(command);

            if (this.ValidateCommand(command))
            {
                this.validationService.Validate(command);
            }

            var handler = this.handlerResolver.ResolveHandler(command, typeof(ICommandHandler<>));
            var handleMethod = handler.GetType().GetMethod("Handle", new[] { command.GetType() });
            var response = (CommandResponse)handleMethod.Invoke(handler, new object[] { command });

            if (response == null)
            {
                return null;
            }

            if (response.Events == null)
            {
                return new CommandResponse(response.ValidationResult, response.Result);
            }

            this.eventProcessor.Process(response.Events, command).GetAwaiter().GetResult();

            return new CommandResponse(response.ValidationResult, response.ValidationResult);
        }
    }
}
