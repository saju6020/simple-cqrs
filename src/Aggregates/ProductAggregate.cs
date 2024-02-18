using Aggregates.Validators;
using Dtos;
using Events;
using FluentValidation.Results;
using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core.Domain;
using Platform.Infrastructure.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aggregates
{
    internal class ProductAggregate : AggregateRoot
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public ProductAggregate(ProductDto productDto, UserContext userContext) : base(id)
        {
            ValidationResult validationResult = new CreateProductValidator().Validate(productDto);

            if (!validationResult.IsValid)
            {
                this.AddBusinessRuleViolationEvent(productDto, validationResult, nameof(ProductCreatedEvent), userContext);
                return;
            }

            ProductCreatedEvent productCreatedEvent = new ProductCreatedEvent()
            {
                Title = productDto.Title,
                Description = productDto.Description,
                ProductId = this.Id
            };

            this.SetUserContextOnEvent(productCreatedEvent, userContext);

            this.AddAndApplyEvent<ProductAggregate>(productCreatedEvent);
        }

        private void AddBusinessRuleViolationEvent(ProductDto dto, ValidationResult validationResult, string actionName, UserContext userContext)
        {
            EventMessage[] errors = validationResult.Errors.Select(e => e.CustomState as EventMessage).ToArray();

            ProductBusinessRuleViolationEvent ruleViolationEvent = new ProductBusinessRuleViolationEvent(
                actionName,
                errors,
                validationResult: validationResult);

            this.SetUserContextOnEvent(ruleViolationEvent, userContext);

            this.AddAndApplyEvent<ProductAggregate>(ruleViolationEvent);
        }

        private void SetUserContextOnEvent(DomainEvent domainEvent, UserContext userContext)
        {
            domainEvent.SetUserContext(userContext);
        }

        private void Apply(ProductCreatedEvent @event)
        {

        }
    }
}
