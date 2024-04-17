namespace Core.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.UnitTest.Fakes;
    using Moq;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core;
    using Platform.Infrastructure.Core.Accessors;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Domain;
    using Platform.Infrastructure.Core.Events;
    using Platform.Infrastructure.Core.Queries;
    using Xunit;

    public class DispatcherTests
    {
        private IDispatcher sut;

        private Mock<ICommandSender> commandSender;
        private Mock<IEventPublisher> eventPublisher;
        private Mock<IQueryProcessor> queryDispatcher;
        private Mock<IBusMessageDispatcher> busMessageDispatcher;

        private CreateSomething createSomething;
        private SomethingCreated somethingCreated;
        private GetSomething getSomething;
        private QueryResponse<Something> something;
        private CreateAggregate createAggregate;
        private CommandResponse commandResponse;
        private IUserContextProvider userContextProvider;
        private IEnumerable<IEvent> events;

        public DispatcherTests()
        {
            this.InitModels();
            this.InitCommandSender();
            this.InitEventPublisher();
            this.InitDispatcher();

            this.sut = new Dispatcher(
                this.commandSender.Object,
                this.eventPublisher.Object,
                this.queryDispatcher.Object,
                this.busMessageDispatcher.Object,
                this.userContextProvider);
        }

        private void InitDispatcher()
        {
            this.queryDispatcher = new Mock<IQueryProcessor>();
            this.queryDispatcher
                .Setup(x => x.ProcessAsync(this.getSomething))
                .Returns(Task.FromResult(this.something));
            this.queryDispatcher
                .Setup(x => x.Process(this.getSomething))
                .Returns(this.something);

            this.busMessageDispatcher = new Mock<IBusMessageDispatcher>();
        }

        private void InitEventPublisher()
        {
            this.eventPublisher = new Mock<IEventPublisher>();
            this.eventPublisher
                .Setup(x => x.PublishAsync(this.somethingCreated))
                .Returns(Task.CompletedTask);
        }

        private void InitCommandSender()
        {
            this.commandSender = new Mock<ICommandSender>();
            this.commandSender
                .Setup(x => x.SendAsync(this.createAggregate))
                .Returns(Task.FromResult(this.commandResponse));

            this.commandSender
                .Setup(x => x.Send(this.createAggregate));
        }

        private void InitModels()
        {
            this.createSomething = new CreateSomething();
            this.somethingCreated = new SomethingCreated();
            this.getSomething = new GetSomething();
            this.something = new QueryResponse<Something>();
            this.createAggregate = new CreateAggregate();
            this.events = new List<IEvent> { this.somethingCreated };
            this.commandResponse = new CommandResponse
            {
                Result = "Result",
            };

            this.userContextProvider = new UserContextProvider(new ContextAccessor());

        }

        [Fact]
        public async Task SendsCommandAsync()
        {
            await this.sut.SendAsync(this.createAggregate);
            this.commandSender.Verify(x => x.SendAsync(this.createAggregate), Times.Once);
        }

        [Fact]
        public async Task SendsCommandWithResultAsync()
        {
            await this.sut.SendAsync(this.createAggregate);
            this.commandSender.Verify(x => x.SendAsync(this.createAggregate), Times.Once);
        }

        //[Fact]
        //public void SendsCommand()
        //{
        //    this.sut.Send(this.createAggregate);
        //    this.commandSender.Verify(x => x.Send(this.createAggregate), Times.Once);
        //}

        //[Fact]
        //public void SendsCommandWithResult()
        //{
        //    this.sut.Send(this.createAggregate);
        //    this.commandSender.Verify(x => x.Send(this.createAggregate), Times.Once);
        //}

        [Fact]
        public async Task PublishesEventAsync()
        {
            await this.sut.PublishAsync(this.somethingCreated);
            this.eventPublisher.Verify(x => x.PublishAsync(this.somethingCreated), Times.Once);
        }

        [Fact]
        public void PublishesEvent()
        {
            this.sut.PublishAsync(this.somethingCreated);
            this.eventPublisher.Verify(x => x.PublishAsync(this.somethingCreated), Times.Once);
        }

        [Fact]
        public async Task GetsResultAsync()
        {
            await this.sut.GetResultAsync(this.getSomething);
            this.queryDispatcher.Verify(x => x.ProcessAsync(this.getSomething), Times.Once);
        }

        [Fact]
        public void GetsResult()
        {
            this.sut.GetResult(this.getSomething);
            this.queryDispatcher.Verify(x => x.Process(this.getSomething), Times.Once);
        }

        [Fact]
        public async Task DispatchesBusMessageAsync()
        {
            await this.sut.SendBusMessageAsync(this.createAggregate);
            this.busMessageDispatcher.Verify(x => x.DispatchAsync(this.createAggregate), Times.Once);
        }
    }
}
