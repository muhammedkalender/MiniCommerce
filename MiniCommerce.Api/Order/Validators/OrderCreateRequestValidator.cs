using FluentValidation;
using MiniCommerce.Application.Order.DTOs;

namespace MiniCommerce.Api.Order.Validators;

public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
{
    public OrderCreateRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("userId is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("productId is required.");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("quantity is required.")
            .GreaterThan(0).WithMessage("quantity must be more than 0.")
            .LessThanOrEqualTo(1000).WithMessage("quantity must not exceed 1000.");

        RuleFor(x => x.PaymentMethod)
            .NotNull().WithMessage("paymentMethod is required.")
            .IsInEnum().WithMessage("An invalid value was provided for paymentMethod.");
    }
}