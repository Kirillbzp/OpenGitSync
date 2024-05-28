using BlazorBootstrap;

namespace OpenGitSync.Client.Services.BreadCrumbs
{
    public class RepositoriesBreadCrumbService : BaseBreadCrumbService
    {
        public RepositoriesBreadCrumbService(BreadCrumbService _service) : base(_service)
        {
            entityBreadCrumbs = new List<BreadcrumbItem>();

            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Home", Href = "/" });
            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Projects", Href = "/projects" });
            
        }

        public void SetNewRepository(string projectName, string projectId)
        {
            SetBase();
            Add(projectName, string.Format("/view-project/{0}", projectId));
            Add("New repository");
        }

        public void SetRepository(string projectName, string projectId, string repositoryName)
        {
            SetBase();
            Add(projectName, string.Format("/view-project/{0}", projectId));
            Add(repositoryName);
        }

    }
}
