using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hyponome.Web.Extensions;

public static class HtmlHelperExtensions
{
    public static HtmlString GetApplicationVersion() => new(HyponomeWeb.Version);
}