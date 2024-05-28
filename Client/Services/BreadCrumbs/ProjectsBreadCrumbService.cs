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

        public void SetNewProject()
        {
            SetBase();
            Add("New project");
        }

        public void SetProject(string projectName)
        {
            SetBase();
            Add(projectName);
        }

    }
}
