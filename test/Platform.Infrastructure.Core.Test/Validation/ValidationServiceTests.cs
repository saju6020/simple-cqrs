namespace Core.UnitTest.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.UnitTest.Fakes;
    using Moq;
    using Platform.Infrastructure.Core.Validation;
    using Xunit;

    public class ValidationServiceTests
    {
        private readonly Mock<IValidationProvider> validationProviderMock;
        private IValidationService sut;
        private CreateAggregate createAggregate;
        private ValidationResponse validationResult;

        public ValidationServiceTests()
        {
            this.createAggregate = new CreateAggregate();
            this.validationResult = new ValidationResponse()
            {
                Errors = new List<ValidationError>(),
            };

            this.validationProviderMock = new Mock<IValidationProvider>();
            this.validationProviderMock
                .Setup(x => x.ValidateAsync(this.createAggregate))
                .ReturnsAsync(this.validationResult);
            this.validationProviderMock
                .Setup(x => x.Validate(this.createAggregate))
                .Returns(this.validationResult);

            this.sut = new ValidationService(this.validationProviderMock.Object);
        }

        [Fact]
        public void ValidateAsync_ThrowsException_WhenCommandIsNull()
        {
            this.createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this.sut.ValidateAsync(this.createAggregate));
        }

        [Fact]
        public void Validate_ThrowsException_WhenCommandIsNull()
        {
            this.createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this.sut.ValidateAsync(this.createAggregate));
        }

        [Fact]
        public async Task ValidateAsync_CallsProvider()
        {
            await this.sut.ValidateAsync(this.createAggregate);
            this.validationProviderMock.Verify(x => x.ValidateAsync(this.createAggregate), Times.Once);
        }

        [Fact]
        public void Validate_CallsProvider()
        {
            this.sut.Validate(this.createAggregate);
            this.validationProviderMock.Verify(x => x.Validate(this.createAggregate), Times.Once);
        }

        [Fact]
        public void ValidateAsync_ThrowsException_WhenValidationFails()
        {
            this.validationResult = new ValidationResponse()
            {
                Errors = new List<ValidationError>()
                {
                     new ValidationError
                    {
                        PropertyName = "Something",
                        ErrorMessage = "Blah blah blah...",
                    },
                },
            };

            this.validationProviderMock
                .Setup(x => x.ValidateAsync(this.createAggregate))
                .ReturnsAsync(this.validationResult);

            this.sut = new ValidationService(this.validationProviderMock.Object);

            Assert.ThrowsAsync<Exception>(async () => await this.sut.ValidateAsync(this.createAggregate));
        }

        [Fact]
        public void Validate_ThrowsException_WhenValidationFails()
        {
            this.validationResult = new ValidationResponse()
            {
                Errors = new List<ValidationError>
                {
                    new ValidationError
                    {
                        PropertyName = "Something",
                        ErrorMessage = "Blah blah blah...",
                    },
                },
            };

            this.validationProviderMock
                .Setup(x => x.Validate(this.createAggregate))
                .Returns(this.validationResult);

            this.sut = new ValidationService(this.validationProviderMock.Object);

            Assert.Throws<Exception>(() => this.sut.Validate(this.createAggregate));
        }
    }
}
