using System.Text;
using Microsoft.Extensions.Options;
using Octokit;

namespace Hyponome.Web.Models;

public interface IGitHubClientService
{
    Task<List<Issue>> GetPullRequests();
    Task<PullRequest> GetPullRequest(int number);
    Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(int number);
    Task<string> GetFilePatch(int number, string fileName);
}

public class GitHubClientService : IGitHubClientService
{
    readonly ILogger<GitHubClientService> logger;
    readonly GitHubClient githubClient = new(new ProductHeaderValue("Hyponome", "2.0"));
    readonly GitHubOptions Options;

    public GitHubClientService(ILogger<GitHubClientService> logger, IOptions<GitHubOptions> optionsAccessor)
    {
        this.logger = logger;
        Options = optionsAccessor.Value;
        SetCredentials();
    }

    void SetCredentials()
    {
        var authToken = Options.AuthToken;
        if (!string.IsNullOrWhiteSpace(authToken)) 
        {
            githubClient.Credentials = new Credentials(authToken);
            return;
        }

        authToken = Environment.GetEnvironmentVariable("GITHUB_PAT");
        if (!string.IsNullOrWhiteSpace(authToken))
        {
            githubClient.Credentials = new Credentials(authToken);
            return;
        }
    }

    public async Task<List<Issue>> GetPullRequests()
    {
        var issues = await githubClient.Issue.GetAllForRepository(Options.OrganizationName, Options.RepositoryName);
        var pulls = issues.Where(i => i.PullRequest != null);
        return pulls.ToList();
    }

    public async Task<PullRequest> GetPullRequest(int number)
    {
        var pullRequest = await githubClient.PullRequest.Get(Options.OrganizationName, Options.RepositoryName, number);
        return pullRequest;
    }

    public async Task<IReadOnlyList<PullRequestFile>> GetPullRequestFiles(int number)
    {
        return await githubClient.PullRequest.Files(Options.OrganizationName, Options.RepositoryName, number);
    }

    public async Task<string> GetFilePatch(int number, string fileName)
    {
        var response = await githubClient.Connection.Get<string>(
            new Uri($"repos/{Options.OrganizationName}/{Options.RepositoryName}/pulls/{number}", UriKind.Relative), null, "application/vnd.github.diff");


        return GetFilePatch(response.Body, fileName);
    }

    string GetFilePatch(string prDiff, string fileName)
    {
        var split = prDiff.Split("diff --git ", StringSplitOptions.RemoveEmptyEntries);
        var filePatch = split.FirstOrDefault(s => s.StartsWith($"a/{fileName}"));
        if(string.IsNullOrEmpty(filePatch)) return string.Empty;

        var patchLines = filePatch.Split("\n").Skip(4);
        if (patchLines.Count() > 1)
        {
            return string.Join("\n", patchLines);
        }

        return prDiff;
    }
}
