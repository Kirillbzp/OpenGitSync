using BlazorBootstrap;

namespace OpenGitSync.Client.Services.BreadCrumbs
{
    public class DashboardBreadCrumbService : BaseBreadCrumbService
    {
        public DashboardBreadCrumbService(BreadCrumbService _service) : base(_service)
        {
            entityBreadCrumbs = new List<BreadcrumbItem>();

            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Home", Href = "/" });
            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Dashboard", Href = "/dashboard" });
        }

    }
}
