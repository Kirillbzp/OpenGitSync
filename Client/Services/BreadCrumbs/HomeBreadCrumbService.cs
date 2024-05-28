using BlazorBootstrap;

namespace OpenGitSync.Client.Services.BreadCrumbs
{
    public class HomeBreadCrumbService : BaseBreadCrumbService
    {
        public HomeBreadCrumbService(BreadCrumbService _service) : base(_service)
        {
            entityBreadCrumbs = new List<BreadcrumbItem>();

            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Home", Href = "/" });
        }

    }
}
