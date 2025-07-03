using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MiniCommerce.Api.App.Conventions;

public class KebabCaseControllerNameConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var name = controller.ControllerName;
        
        var kebab = Regex.Replace(name, "([a-z])([A-Z])", "$1-$2").ToLower();

        string pluralSuffix;

        if (Regex.IsMatch(kebab, "(s|x|z|ch|sh)$"))
        {
            pluralSuffix = "es";
        }
        else if (Regex.IsMatch(kebab, "[^aeiou]y$"))
        {
            // make plural e.g. company -> companies
            kebab = Regex.Replace(kebab, "y$", "ie");
            pluralSuffix = "s";
        }
        else
        {
            pluralSuffix = "s";
        }

        var final = kebab + pluralSuffix;

        controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel
        {
            Template = final
        };
    }
}