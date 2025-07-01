using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MiniCommerce.Api.App.Conventions;

public class SnakeCaseControllerNameConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var name = controller.ControllerName;

        // CamelCase -> snake_case
        var snake = Regex.Replace(name, "([a-z])([A-Z])", "$1_$2").ToLower();

        string pluralSuffix;

        if (Regex.IsMatch(snake, "(s|x|z|ch|sh)$"))
        {
            pluralSuffix = "es";
        }
        else if (Regex.IsMatch(snake, "[^aeiou]y$"))
        {
            // make plural e.g. company -> companies
            snake = Regex.Replace(snake, "y$", "ie");
            pluralSuffix = "s";
        }
        else
        {
            pluralSuffix = "s";
        }

        var final = snake + pluralSuffix;

        controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel
        {
            Template = final
        };
    }
}