namespace Core.UnitTest.Queries
{
    using System;
    using System.Threading.Tasks;
    using Core.UnitTest.Fakes;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Platform.Infrastructure.Core.Dependencies;
    using Platform.Infrastructure.Core.Queries;
    using Platform.Infrastructure.Core.Validation;
    using Xunit;

    public class QueryProcessorAsyncTests
    {
        private readonly Mock<IHandlerResolver> handlerResolver;
        private readonly Mock<IQueryHandlerAsync<GetSomething, Something>> queryHandler;
        private readonly QueryResponse<Something> something;
        private Mock<IOptions<ValidationOptions>> validationOptionsMock;
        private Mock<IValidationService> validationService;

        private Mock<ILogger<QueryProcessor>> logger;
        private IQueryProcessor sut;
        private GetSomething getSomething;

        public QueryProcessorAsyncTests()
        {
            this.getSomething = new GetSomething();
            this.something = new QueryResponse<Something>();

            this.logger = new Mock<ILogger<QueryProcessor>>();

            this.queryHandler = new Mock<IQueryHandlerAsync<GetSomething, Something>>();
            this.queryHandler
                .Setup(x => x.HandleAsync(this.getSomething))
                .Returns(Task.FromResult(this.something));

            this.handlerResolver = new Mock<IHandlerResolver>();
            this.handlerResolver
                .Setup(x => x.ResolveHandler<IQueryHandlerAsync<GetSomething, Something>>())
                .Returns(this.queryHandler.Object);

            this.InitValidationComponents();

            this.sut = new QueryProcessor(
                this.handlerResolver.Object,
                this.validationService.Object,
                this.validationOptionsMock.Object,
                this.logger.Object);
        }

        /// <summary>Initializes the validation components.</summary>
        private void InitValidationComponents()
        {
            this.validationService = new Mock<IValidationService>();
            this.validationService
                .Setup(x => x.ValidateQuery<GetSomething>(It.IsAny<GetSomething>()));

            this.validationOptionsMock = new Mock<IOptions<ValidationOptions>>();
            this.validationOptionsMock
                .Setup(x => x.Value)
                .Returns(new ValidationOptions());
        }

        [Fact]
        public void ProcessAsync_ThrowsException_WhenQueryIsNull()
        {
            this.getSomething = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this.sut.ProcessAsync(this.getSomething));
        }

        [Fact]
        public async Task ProcessAsync_ReturnResult()
        {
            var result = await this.sut.ProcessAsync(this.getSomething);
            Assert.Equal(this.something.Result, result.Result);
        }
    }
}
