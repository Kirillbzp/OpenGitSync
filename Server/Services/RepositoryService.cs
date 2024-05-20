using AutoMapper;
using DB.Helpers;
using DB.Models;
using OpenGitSync.Server.Helpers.Git;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface IRepositoryService
    {
        Task<IEnumerable<RepositoryDto>> GetRepositories(string userId);
        Task<IEnumerable<RepositoryDto>> GetRepositoriesByProject(long projectId, string userId);
        Task<RepositoryDto> GetRepositoryById(long id, string userId);
        Task<RepositoryDto> CreateRepository(RepositoryCreateDto repositoryDto, string userId);
        Task<RepositoryDto?> UpdateRepository(long id, RepositoryDto repositoryDto, string userId);
        Task<RepositoryDto?> DeleteRepository(long id, string userId);
        Task<IEnumerable<RepositoryDto>> RepositoryTypeahead(string query, long projectId, string userId);
        Task<bool> CheckConnection(string url, string token);
    }

    public class RepositoryService : IRepositoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGitHubService _gitService;

        public RepositoryService(IUnitOfWork unitOfWork, IMapper mapper, IGitHubService gitService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _gitService = gitService;
        }

        public async Task<IEnumerable<RepositoryDto>> GetRepositories(string userId)
        {
            var repositories = await _unitOfWork.RepositoryRepository.GetRepositories(userId);
            var repositoryDtos = _mapper.Map<IEnumerable<RepositoryDto>>(repositories);
            return repositoryDtos;
        }

        public async Task<RepositoryDto> GetRepositoryById(long id, string userId)
        {
            var repository = await _unitOfWork.RepositoryRepository.GetRepositoryById(id, userId);
            var repositoryDto = _mapper.Map<RepositoryDto>(repository);
            return repositoryDto;
        }

        public async Task<IEnumerable<RepositoryDto>> GetRepositoriesByProject(long projectId, string userId)
        {
            var repositories = await _unitOfWork.RepositoryRepository.GetRepositoriesByProjectId(projectId, userId);
            var repositoryDtos = _mapper.Map<IEnumerable<RepositoryDto>>(repositories);
            return repositoryDtos;
        }

        public async Task<RepositoryDto> CreateRepository(RepositoryCreateDto repositoryDto, string userId)
        {
            var repository = _mapper.Map<RepositoryModel>(repositoryDto);
            var createdRepository = await _unitOfWork.RepositoryRepository.CreateRepository(repository);
            await _unitOfWork.SaveChanges();
            var createdRepositoryDto = _mapper.Map<RepositoryDto>(createdRepository);
            return createdRepositoryDto;
        }

        public async Task<RepositoryDto?> UpdateRepository(long id, RepositoryDto repositoryDto, string userId)
        {
            var existingRepository = await _unitOfWork.RepositoryRepository.GetRepositoryById(id, userId);

            if (existingRepository == null)
                return null;

            _mapper.Map(repositoryDto, existingRepository);
            var updatedRepository = await _unitOfWork.RepositoryRepository.UpdateRepository(existingRepository);
            await _unitOfWork.SaveChanges();
            var updatedRepositoryDto = _mapper.Map<RepositoryDto>(updatedRepository);
            return updatedRepositoryDto;
        }

        public async Task<RepositoryDto?> DeleteRepository(long id, string userId)
        {
            var existingRepository = await _unitOfWork.RepositoryRepository.GetRepositoryById(id, userId);

            if (existingRepository == null)
                return null;

            var deletedRepositoryDto = _mapper.Map<RepositoryDto>(existingRepository);

            await _unitOfWork.RepositoryRepository.DeleteRepository(existingRepository);
            await _unitOfWork.SaveChanges();
            
            return deletedRepositoryDto;
        }

        public async Task<IEnumerable<RepositoryDto>> RepositoryTypeahead(string query, long projectId, string userId)
        {
            var repositories = await _unitOfWork.RepositoryRepository.RepositoryTypeahead(query, projectId, userId);
            var repositoryDtos = _mapper.Map<IEnumerable<RepositoryDto>>(repositories);
            return repositoryDtos;
        }

        public async Task<bool> CheckConnection(string url, string token)
        {
            return await _gitService.CheckConnection(url, token);
        }
    }

}
