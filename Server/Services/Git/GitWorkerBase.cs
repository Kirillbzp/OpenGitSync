using DB.Helpers;

namespace OpenGitSync.Server.Services.Git
{
    
    public class GitWorkerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GitWorkerBase(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        protected (string, string) GetOwnerAndRepo(string url)
        {
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Split('/');
            var owner = segments[1];
            var repo = segments[2];

            if (repo.EndsWith(".git"))
            {
                repo = repo.Substring(0, repo.Length - 4);
            }

            return (owner, repo);
        }

        protected async Task RemoveDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.FoldersToDeleteRepository.SetFolderToDeleteAsync(path);
                    Console.WriteLine($"Error deleting directory {path}: {ex.Message}");
                }
            }
        }
    }
}
