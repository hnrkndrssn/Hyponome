using Microsoft.AspNetCore.Mvc;

namespace Hyponome.Web.Controllers;

[Route("")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "PullRequests");
    }
}
