using BlazorBootstrap;

namespace OpenGitSync.Client.Services.BreadCrumbs
{
    public class SyncSettingsBreadCrumbService : BaseBreadCrumbService
    {
        public SyncSettingsBreadCrumbService(BreadCrumbService _service) : base(_service)
        {
            entityBreadCrumbs = new List<BreadcrumbItem>();

            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Home", Href = "/" });
            entityBreadCrumbs.Add(new BreadcrumbItem { Text = "Projects", Href = "/projects" });

        }

        public void SetNewSyncSettingBreadCrumb(string projectName, string projectId)
        {
            SetGeneralBreadCrumb();
            AddBreadCrumb(projectName, string.Format("/view-project/{0}", projectId));
            AddBreadCrumb("New Sync setting");
        }

        public void SetSyncSettingBreadCrumb(string projectName, string projectId, string syncSettingName)
        {
            SetGeneralBreadCrumb();
            AddBreadCrumb(projectName, string.Format("/view-project/{0}", projectId));
            AddBreadCrumb(syncSettingName);
        }

    }
}
