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

        public void SetNewRepositoryBreadCrumb(string projectName, string projectId)
        {
            SetGeneralBreadCrumb();
            AddBreadCrumb(projectName, string.Format("/view-project/{0}", projectId));
            AddBreadCrumb("New repository");
        }

        public void SetRepositoryBreadCrumb(string projectName, string projectId, string repositoryName)
        {
            SetGeneralBreadCrumb();
            AddBreadCrumb(projectName, string.Format("/view-project/{0}", projectId));
            AddBreadCrumb(repositoryName);
        }

    }
}
