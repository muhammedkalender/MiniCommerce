using System;
using FluentValidation.TestHelper;
using MiniCommerce.Api.Order.Validators;
using MiniCommerce.Application.Order.DTOs;
using MiniCommerce.Domain.Payment.Enums;
using NUnit.Framework;

namespace MiniCommerce.Api.Tests.Order.Validators;

[TestFixture]
[TestOf(typeof(OrderCreateRequestValidator))]
public class OrderCreateRequestValidatorTest
{
    private OrderCreateRequestValidator _validator = null!;

    [SetUp]
    public void Setup()
    {
        _validator = new OrderCreateRequestValidator();
    }

    [Test]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
        var model = new OrderCreateRequest { UserId = Guid.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        var model = new OrderCreateRequest { ProductId = Guid.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Test]
    public void Should_Have_Error_When_Quantity_Is_Zero()
    {
        var model = new OrderCreateRequest { Quantity = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Test]
    public void Should_Have_Error_When_Quantity_Is_More_Than_1000()
    {
        var model = new OrderCreateRequest { Quantity = 1001 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Test]
    public void Should_Not_Have_Errors_For_Valid_Model()
    {
        var model = new OrderCreateRequest
        {
            UserId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 3,
            PaymentMethod = PaymentMethod.BankTransfer
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}