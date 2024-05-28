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

        public void SetNewSyncSetting(string projectName, string projectId)
        {
            SetBase();
            Add(projectName, string.Format("/view-project/{0}", projectId));
            Add("New Sync setting");
        }

        public void SetSyncSetting(string projectName, string projectId, string syncSettingName)
        {
            SetBase();
            Add(projectName, string.Format("/view-project/{0}", projectId));
            Add(syncSettingName);
        }

    }
}
