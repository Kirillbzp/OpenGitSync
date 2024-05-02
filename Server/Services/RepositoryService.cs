using AutoMapper;
using DB.Helpers;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface IRepositoryService
    {
        Task<IEnumerable<RepositoryDto>> GetRepositories();
        Task<IEnumerable<RepositoryDto>> GetRepositoriesByProject(long projectId);
        Task<RepositoryDto> GetRepositoryById(long id);
        Task<RepositoryDto> CreateRepository(RepositoryCreateDto repositoryDto);
        Task<RepositoryDto> UpdateRepository(long id, RepositoryDto repositoryDto);
        Task<RepositoryDto> DeleteRepository(long id);
        Task<IEnumerable<RepositoryDto>> RepositoryTypeahead(string query, long projectId);
    }

    public class RepositoryService : IRepositoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RepositoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RepositoryDto>> GetRepositories()
        {
            var repositories = await _unitOfWork.RepositoryRepository.GetRepositories();
            var repositoryDtos = _mapper.Map<IEnumerable<RepositoryDto>>(repositories);
            return repositoryDtos;
        }

        public async Task<RepositoryDto> GetRepositoryById(long id)
        {
            var repository = await _unitOfWork.RepositoryRepository.GetRepositoryById(id);
            var repositoryDto = _mapper.Map<RepositoryDto>(repository);
            return repositoryDto;
        }

        public async Task<IEnumerable<RepositoryDto>> GetRepositoriesByProject(long projectId)
        {
            var repositories = await _unitOfWork.RepositoryRepository.GetRepositoriesByProjectId(projectId);
            var repositoryDtos = _mapper.Map<IEnumerable<RepositoryDto>>(repositories);
            return repositoryDtos;
        }

        public async Task<RepositoryDto> CreateRepository(RepositoryCreateDto repositoryDto)
        {
            var repository = _mapper.Map<Repository>(repositoryDto);
            var createdRepository = await _unitOfWork.RepositoryRepository.CreateRepository(repository);
            await _unitOfWork.SaveChanges();
            var createdRepositoryDto = _mapper.Map<RepositoryDto>(createdRepository);
            return createdRepositoryDto;
        }

        public async Task<RepositoryDto> UpdateRepository(long id, RepositoryDto repositoryDto)
        {
            var existingRepository = await _unitOfWork.RepositoryRepository.GetRepositoryById(id);

            if (existingRepository == null)
                return null;

            _mapper.Map(repositoryDto, existingRepository);
            var updatedRepository = await _unitOfWork.RepositoryRepository.UpdateRepository(existingRepository);
            _unitOfWork.SaveChanges();
            var updatedRepositoryDto = _mapper.Map<RepositoryDto>(updatedRepository);
            return updatedRepositoryDto;
        }

        public async Task<RepositoryDto> DeleteRepository(long id)
        {
            var existingRepository = await _unitOfWork.RepositoryRepository.GetRepositoryById(id);

            if (existingRepository == null)
                return null;

            var deletedRepositoryDto = _mapper.Map<RepositoryDto>(existingRepository);

            await _unitOfWork.RepositoryRepository.DeleteRepository(existingRepository);
            await _unitOfWork.SaveChanges();
            
            return deletedRepositoryDto;
        }

        public async Task<IEnumerable<RepositoryDto>> RepositoryTypeahead(string query, long projectId)
        {
            var repositories = await _unitOfWork.RepositoryRepository.RepositoryTypeahead(query, projectId);
            var repositoryDtos = _mapper.Map<IEnumerable<RepositoryDto>>(repositories);
            return repositoryDtos;
        }
    }

}
