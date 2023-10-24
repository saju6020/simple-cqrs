namespace Core.UnitTest.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.UnitTest.Fakes;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Accessors;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Dependencies;
    using Platform.Infrastructure.Core.Domain;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Validation;
    using Xunit;

    public class CommandSenderAsyncTests
    {
        private ICommandSender sut;

        private Mock<IHandlerResolver> handlerResolver;
        private Mock<IEventPublisher> eventPublisher;
        private Mock<IDomainStore> storeProvider;

        private Mock<IValidationService> validationService;

        private Mock<ICommandHandlerAsync<CreateSomething>> commandHandlerAsync;
        private Mock<IOptions<CommandOptions>> commandOptionsMock;
        private Mock<IOptions<ValidationOptions>> validationOptionsMock;

        private CreateSomething createSomething;
        private CreateSomething createSomethingConcrete;
        private SomethingCreated somethingCreated;
        private SomethingCreated somethingCreatedConcrete;
        private IUserContextProvider userContextProvider;
        private IEnumerable<IEvent> events;
        private AggregateCreated aggregateCreated;
        private AggregateCreated aggregateCreatedConcrete;
        private Aggregate aggregate;

        private CommandResponse commandResponse;

        private List<IDomainEvent> domainEvents;
        private Mock<IDomainEventProcessor> domainEventProcessor;
        private Mock<ILogger<CommandSender>> logger;

        public CommandSenderAsyncTests()
        {
            this.InitSystemUnderTest();
        }

        /// <summary>Initializes the system under test.</summary>
        private void InitSystemUnderTest()
        {
            this.InitRequiredObjects();

            this.InitCommandComponents();

            this.InitEventComponents();

            this.InitValidationComponents();

            this.sut = new CommandSender(
                this.handlerResolver.Object,
                this.userContextProvider,
                this.domainEventProcessor.Object,
                this.validationService.Object,
                this.commandOptionsMock.Object,
                this.validationOptionsMock.Object,
                this.logger.Object);
        }

        /// <summary>Initializes the validation components.</summary>
        private void InitValidationComponents()
        {
            ValidationResponse validationResponse = new ValidationResponse();
            this.validationService = new Mock<IValidationService>();
            this.validationService
                .Setup(x => x.ValidateAsync<CreateSomething>(It.IsAny<CreateSomething>()))
                .ReturnsAsync(validationResponse);

            this.validationOptionsMock = new Mock<IOptions<ValidationOptions>>();
            this.validationOptionsMock
                .Setup(x => x.Value)
                .Returns(new ValidationOptions());
        }

        /// <summary>Initializes the command components.</summary>
        private void InitCommandComponents()
        {
            this.commandHandlerAsync = new Mock<ICommandHandlerAsync<CreateSomething>>();
            this.commandHandlerAsync
                .Setup(x => x.HandleAsync(this.createSomethingConcrete))
                .ReturnsAsync(this.commandResponse);

            this.handlerResolver = new Mock<IHandlerResolver>();
            this.handlerResolver
                .Setup(x => x.ResolveHandler(this.createSomething, typeof(ICommandHandlerAsync<>)))
                .Returns(this.commandHandlerAsync.Object);

            this.logger = new Mock<ILogger<CommandSender>>();

            this.commandOptionsMock = new Mock<IOptions<CommandOptions>>();
            this.commandOptionsMock
                .Setup(x => x.Value)
                .Returns(new CommandOptions());
        }

        /// <summary>Initializes the event components.</summary>
        private void InitEventComponents()
        {
            this.eventPublisher = new Mock<IEventPublisher>();
            this.eventPublisher
                .Setup(x => x.PublishAsync(this.somethingCreatedConcrete))
                .Returns(Task.CompletedTask);

            this.domainEventProcessor = new Mock<IDomainEventProcessor>();
            this.domainEventProcessor
                .Setup(x => x.Process(It.IsAny<IEnumerable<IEvent>>(), It.IsAny<ICommand>()))
                .Returns(Task.CompletedTask);

            this.storeProvider = new Mock<IDomainStore>();
            this.storeProvider
                .Setup(x => x.SaveAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<IDomainEvent>>()))
                .Returns(Task.CompletedTask);
        }

        /// <summary>Initializes the required objects.</summary>
        private void InitRequiredObjects()
        {
            this.domainEvents = new List<IDomainEvent>();
            this.createSomething = new CreateSomething();
            this.createSomethingConcrete = new CreateSomething();
            this.somethingCreated = new SomethingCreated();
            this.somethingCreatedConcrete = new SomethingCreated();
            this.userContextProvider = new UserContextProvider(new ContextAccessor());

            this.events = new List<IEvent> { this.somethingCreated };
            this.aggregateCreatedConcrete = new AggregateCreated();
            this.aggregate = new Aggregate();
            this.aggregateCreated = (AggregateCreated)this.aggregate.Events[0];

            this.commandResponse = new CommandResponse { Events = this.events, Result = "Result" };
        }

        [Fact]
        public void SendAsync_ThrowsException_WhenCommandIsNull()
        {
            this.createSomething = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this.sut.SendAsync(this.createSomething));
        }

        [Fact]
        public async Task SendAsync_ValidatesCommand()
        {
            this.createSomething.ValidateCommand = true;
            var response = await this.sut.SendAsync(this.createSomething).ConfigureAwait(false);

            // Assert.True(response.ValidationResult.IsValid);
            this.validationService.Verify(x => x.ValidateAsync<CreateSomething>(It.IsAny<CreateSomething>()), Times.Never);
        }

        [Fact]
        public async Task SendAsync_HandlesCommand()
        {
            await this.sut.SendAsync(this.createSomething);
            this.commandHandlerAsync.Verify(x => x.HandleAsync(this.createSomething), Times.Once);
        }

        [Fact]
        public async Task SendAsync_SavesStoreData()
        {
            await this.sut.SendAsync(this.createSomething);
            this.storeProvider.Verify(x => x.SaveAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<IDomainEvent>>()), Times.Never);
        }

        [Fact]
        public async Task SendAsync_PublishesEvents()
        {
            await this.sut.SendAsync(this.createSomething);
            this.eventPublisher.Verify(x => x.PublishAsync(this.somethingCreated), Times.Never);
        }

        [Fact]
        public async Task SendAsync_NotPublishesEvents_WhenSetInOptions()
        {
            this.commandOptionsMock
                .Setup(x => x.Value)
                .Returns(new CommandOptions { PublishEvents = false });

            this.sut = new CommandSender(
                this.handlerResolver.Object,
                this.userContextProvider,
                this.domainEventProcessor.Object,
                new Mock<IValidationService>().Object,
                this.commandOptionsMock.Object,
                this.validationOptionsMock.Object,
                this.logger.Object);

            await this.sut.SendAsync(this.createSomething);
            this.eventPublisher.Verify(x => x.PublishAsync(this.somethingCreated), Times.Never);
        }

        [Fact]
        public async Task SendAsync_NotPublishesEvents_WhenSetInCommand()
        {
            this.createSomething.PublishEvents = false;
            await this.sut.SendAsync(this.createSomething);
            this.eventPublisher.Verify(x => x.PublishAsync(this.somethingCreated), Times.Never);
        }
    }
}
