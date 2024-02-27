using Aggregates;
using Commands;
using Dtos;
using Microsoft.Extensions.Logging;
using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core.Commands;
using Platform.Infrastructure.Core.Domain;
using Platform.Infrastructure.Cqrs.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductExample.CommandHandler
{
    internal class CreateProducctCommandHandler : ICommandHandlerAsync<CreateProductCommand>
    {
        private readonly ILogger<CreateProducctCommandHandler> _logger;
        private readonly IAggregateRootRepository<ProductAggregate> aggregateRootRepository;
        private readonly IUserContextProvider _userContextProvider;

        public CreateProducctCommandHandler(
            ILogger<CreateProducctCommandHandler> logger, 
            IUserContextProvider userContextProvider,
            IAggregateRootRepository<ProductAggregate> aggregateRootRepository)
        {
            _logger = logger;
            this.aggregateRootRepository = aggregateRootRepository;
            this._userContextProvider = userContextProvider;
        }

        public async Task<CommandResponse> HandleAsync(CreateProductCommand command)
        {
            this._logger.LogInformation($"Inside HandleAsynch method with correlation id {command.CorrelationId}");

            var productAggreagate = new ProductAggregate();
            productAggreagate.CreateProduct(GetProductDto(command), this._userContextProvider.GetUserContext());
            await this.aggregateRootRepository.SaveAsync(productAggreagate);

            return new CommandResponse();

        }

        private ProductDto GetProductDto(CreateProductCommand command)
        {
            return new ProductDto
            {
                Description = command.Description,
                Title = command.Title,
                ProductId = command.ProductId
            };
        }
    }
}
