using BlazorBootstrap;

namespace OpenGitSync.Client.Services.BreadCrumbs
{
    public class ProjectsBreadCrumbService : BaseBreadCrumbService
    {
        public ProjectsBreadCrumbService(BreadCrumbService _service) : base(_service)
        {
            entityBreadCrumbs = new List<BreadcrumbItem>();

            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Home", Href = "/" });
            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Projects", Href = "/projects"});

        }

        public void SetNewProjectBreadCrumb()
        {
            SetGeneralBreadCrumb();
            AddBreadCrumb("New project");
        }

        public void SetProjectBreadCrumb(string projectName)
        {
            SetGeneralBreadCrumb();
            AddBreadCrumb(projectName);
        }

    }
}
