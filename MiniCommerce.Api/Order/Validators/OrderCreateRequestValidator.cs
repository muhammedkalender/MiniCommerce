using FluentValidation;
using MiniCommerce.Application.Order.DTOs;

namespace MiniCommerce.Api.Order.Validators;

public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
{
    public OrderCreateRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required.")
            .GreaterThan(0).WithMessage("Quantity must be more than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Quantity must not exceed 1000.");

        RuleFor(x => x.PaymentMethod)
            .NotNull().WithMessage("PaymentMethod is required.")
            .IsInEnum().WithMessage("An invalid value was provided for PaymentMethod.");
    }
}