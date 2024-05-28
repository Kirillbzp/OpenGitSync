using BlazorBootstrap;

namespace OpenGitSync.Client.Services.BreadCrumbs
{
    public class UserSettingsBreadCrumbService : BaseBreadCrumbService
    {
        public UserSettingsBreadCrumbService(BreadCrumbService _service) : base(_service)
        {
            entityBreadCrumbs = new List<BreadcrumbItem>();

            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Home", Href = "/" });
            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "User Settings", Href = "/profile" });
        }

    }
}
