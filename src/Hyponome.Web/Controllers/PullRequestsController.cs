using Hyponome.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace Hyponome.Web.Controllers;

[Route("pulls")]
public class PullRequestsController : Controller
{
    readonly IGitHubClientService githubClientService;

    public PullRequestsController(IGitHubClientService githubClientService)
    {
        this.githubClientService = githubClientService;
    }

    [Route("")]
    public async Task<ActionResult> Index()
    {
        var pullRequests = await githubClientService.GetPullRequests();
        Console.WriteLine("[Pulls] Found {0} open pull requests", pullRequests.Count);

        return View(pullRequests);
    }

    [Route("{number:int}")]
    public async Task<ActionResult> PullRequest(int number)
    {
        var pullRequest = await githubClientService.GetPullRequest(number);
        if (pullRequest == null)
        {
            return NotFound();
        }
        return await BuildView(pullRequest);
    }
    private async Task<ActionResult> BuildView(PullRequest pullRequest)
    {
        var pullRequestFiles = await githubClientService.GetPullRequestFiles(pullRequest.Number);
        ViewBag.Files = pullRequestFiles;

        return View("PullRequest", pullRequest);
    }
}