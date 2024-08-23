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
        var filesWithPatch = pullRequestFiles.Where(f => !string.IsNullOrEmpty(f.Patch)).ToList();
        foreach(var file in pullRequestFiles.Except(filesWithPatch))
        {
            var fileWithPatch = new PullRequestFile(
                file.Sha,
                file.FileName,
                file.Status,
                file.Additions,
                file.Deletions,
                file.Changes,
                file.BlobUrl,
                file.RawUrl,
                file.ContentsUrl, 
                await GetFilePatch(pullRequest.Number, file.FileName),
                file.PreviousFileName
            );

            filesWithPatch.Add(fileWithPatch);
        }
        ViewBag.Files = filesWithPatch;

        return View("PullRequest", pullRequest);
    }

    async Task<string> GetFilePatch(int number, string fileName)
    {
        return (await githubClientService.GetFilePatch(number, fileName)) ?? "";

    }
}