using BlazorBootstrap;

namespace OpenGitSync.Client.Services.BreadCrumbs
{
    public class BreadCrumbService
    {
        private readonly BreadcrumbService _breadcrumbService;
        private List<BreadcrumbItem> _breadCrumbs;

        public List<BreadcrumbItem> BreadCrumbs 
        { 
            set { 
                _breadCrumbs = value; 
                _breadcrumbService.Notify(_breadCrumbs);
            } 
            get { return _breadCrumbs; } 
        }

        public BreadCrumbService(BreadcrumbService breadcrumbService) 
        {
            _breadCrumbs = new List<BreadcrumbItem>();
            _breadcrumbService = breadcrumbService;
        }

        public void Clear()
        {
            _breadCrumbs.Clear();
            _breadcrumbService.Notify(_breadCrumbs);
        }

        public void Add(BreadcrumbItem breadcrumbItem)
        {
            _breadCrumbs.Add(breadcrumbItem);
            _breadcrumbService.Notify(_breadCrumbs);
        }

        public void SetLastAsCurrent()
        {
            if (_breadCrumbs.Count > 0)
            {
                _breadCrumbs[_breadCrumbs.Count - 1].IsCurrentPage = true;
                _breadcrumbService.Notify(_breadCrumbs);
            }
        }

    }

    public abstract class BaseBreadCrumbService
    {
        private readonly BreadCrumbService _service; 
        
        protected List<BreadcrumbItem> entityBreadCrumbs;
        
        public List<BreadcrumbItem> BreadCrumbs { get { return _service.BreadCrumbs; } }

        public BaseBreadCrumbService(BreadCrumbService service)
        {
            _service = service;
        }

        protected void Add(string name, string url)
        {
            _service.Add(new BreadcrumbItem { Text = name, Href = url });
        }

        protected void Add(string name)
        {
            _service.Add(new BreadcrumbItem { Text = name, IsCurrentPage = true });
        }

        public void Set()
        {
            SetBase();
            _service.SetLastAsCurrent();
        }

        public void SetBase()
        {
            _service.BreadCrumbs.Clear();
            for (int i = 0; i <= entityBreadCrumbs.Count - 1; i++)
            {
                Add(entityBreadCrumbs[i].Text, entityBreadCrumbs[i].Href);
            }
        }

    }
}
