namespace Hyponome.Web.Models;

public class GitHubOptions
{
    public string WebUri { get; set; } = "https://github.com/";
    public string ApiUri { get; set; } = "https://api.github.com/";
    public string OrganizationName { get; set; } = "";
    public string RepositoryName { get; set; } = "";
    public string AuthToken {get;set;} = "";
}