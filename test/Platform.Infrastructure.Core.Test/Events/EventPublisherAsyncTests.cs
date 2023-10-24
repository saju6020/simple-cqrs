namespace Core.UnitTest.Events
{
    using System;
    using System.Threading.Tasks;
    using Core.UnitTest.Fakes;
    using Moq;
    using Platform.Infrastructure.Common.Security;
    using Platform.Infrastructure.Core.Bus;
    using Platform.Infrastructure.Core.Dependencies;
    using Platform.Infrastructure.Core.Events;
    using Xunit;

    public class EventPublisherAsyncTests
    {
        private readonly IEventPublisher sut;

        private readonly Mock<IHandlerResolver> handlerResolver;
        private readonly Mock<IBusMessageDispatcher> busMessageDispatcher;
        private readonly UserContext userContext;

        private readonly Mock<IEventHandlerAsync<SomethingCreated>> eventHandler1;
        private SomethingCreated somethingCreated;

        public EventPublisherAsyncTests()
        {
            this.somethingCreated = new SomethingCreated();

            this.userContext = new UserContext()
            {
                UserId = Guid.Parse("5dfd1019-8616-4745-8d1d-d70d570bf1e7"),
            };

            this.eventHandler1 = new Mock<IEventHandlerAsync<SomethingCreated>>();
            this.eventHandler1
                .Setup(x => x.HandleAsync(this.somethingCreated))
                .Returns(Task.CompletedTask);

            this.handlerResolver = new Mock<IHandlerResolver>();
            this.handlerResolver
                .Setup(x => x.ResolveHandler<IEventHandlerAsync<SomethingCreated>>())
                .Returns(this.eventHandler1.Object);

            this.busMessageDispatcher = new Mock<IBusMessageDispatcher>();
            this.busMessageDispatcher
                .Setup(x => x.DispatchAsync(this.somethingCreated))
                .Returns(Task.CompletedTask);

            this.sut = new EventPublisher(this.busMessageDispatcher.Object, this.userContext, this.handlerResolver.Object);
        }

        [Fact]
        public void PublishAsync_ThrowsException_WhenEventIsNull()
        {
            this.somethingCreated = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this.sut.PublishAsync(this.somethingCreated));
        }

        [Fact]
        public async Task PublishAsync_PublishesFirstEvent()
        {
            await this.sut.PublishAsync(this.somethingCreated);
            this.eventHandler1.Verify(x => x.HandleAsync(this.somethingCreated), Times.Once);
        }

        [Fact]
        public async Task PublishAsync_DispatchesEventToServiceBus()
        {
            await this.sut.PublishAsync(this.somethingCreated);
            this.busMessageDispatcher.Verify(x => x.DispatchAsync(It.IsAny<IMessage>()), Times.Once);
        }
    }
}
