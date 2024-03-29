﻿using Aggregates.Validators;
using Dtos;
using Events;
using FluentValidation.Results;
using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core.Domain;
using Platform.Infrastructure.Core.Events;

namespace Aggregates
{
    public class ProductAggregate : AggregateRoot
    {
        public string Title { get; private set; }
        public string Description { get; private set; }


        public void CreateProduct(ProductDto productDto, UserContext userContext)
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
                ProductId = productDto.ProductId
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
            this.Title = @event.Title;
            this.Description = @event.Description;
            this.Id = @event.ProductId;
            this.SetDefaultValue(@event.UserContext);
        }
    }
}
