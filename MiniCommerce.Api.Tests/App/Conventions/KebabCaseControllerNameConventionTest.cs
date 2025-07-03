using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MiniCommerce.Api.App.Conventions;
using NUnit.Framework;

namespace MiniCommerce.Api.Tests.App.Conventions;

[TestFixture]
[TestOf(typeof(KebabCaseControllerNameConvention))]
public class KebabCaseControllerNameConventionTest
{
    [TestCase("Product", "products")]
    [TestCase("Box", "boxes")]
    [TestCase("Company", "companies")]
    [TestCase("UserProfile", "user-profiles")]
    [TestCase("Tax", "taxes")]
    [TestCase("Branch", "branches")]
    [TestCase("Category", "categories")]
    [TestCase("Dish", "dishes")]
    public void Apply_ShouldSetKebabPluralRoute(string controllerName, string expectedRoute)
    {
        // Arrange
        var selector = new SelectorModel
        {
            AttributeRouteModel = new AttributeRouteModel()
        };

        var controllerModel = new ControllerModel(typeof(object).GetTypeInfo(), new List<object>())
        {
            ControllerName = controllerName,
            Selectors = { selector }
        };

        var convention = new KebabCaseControllerNameConvention();

        // Act
        convention.Apply(controllerModel);

        // Assert
        Assert.That(controllerModel.Selectors[0].AttributeRouteModel.Template, Is.EqualTo(expectedRoute));
    }
}