﻿using AutoMapper;
using DB.Helpers;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface IRepositoryService
    {
        IEnumerable<RepositoryDto> GetRepositories();
        RepositoryDto GetRepositoryById(long id);
        RepositoryDto CreateRepository(RepositoryDto repositoryDto);
        RepositoryDto UpdateRepository(long id, RepositoryDto repositoryDto);
        RepositoryDto DeleteRepository(long id);
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

        public IEnumerable<RepositoryDto> GetRepositories()
        {
            var repositories = _unitOfWork.RepositoryRepository.GetRepositories();
            var repositoryDtos = _mapper.Map<IEnumerable<RepositoryDto>>(repositories);
            return repositoryDtos;
        }

        public RepositoryDto GetRepositoryById(long id)
        {
            var repository = _unitOfWork.RepositoryRepository.GetRepositoryById(id);
            var repositoryDto = _mapper.Map<RepositoryDto>(repository);
            return repositoryDto;
        }

        public RepositoryDto CreateRepository(RepositoryDto repositoryDto)
        {
            var createdRepository = _unitOfWork.RepositoryRepository.CreateRepository(repositoryDto.Name, repositoryDto.Url);
            _unitOfWork.SaveChanges();
            var createdRepositoryDto = _mapper.Map<RepositoryDto>(createdRepository);
            return createdRepositoryDto;
        }

        public RepositoryDto UpdateRepository(long id, RepositoryDto repositoryDto)
        {
            var existingRepository = _unitOfWork.RepositoryRepository.GetRepositoryById(id);

            if (existingRepository == null)
                return null;

            _mapper.Map(repositoryDto, existingRepository);
            var updatedRepository = _unitOfWork.RepositoryRepository.UpdateRepository(existingRepository);
            _unitOfWork.SaveChanges();
            var updatedRepositoryDto = _mapper.Map<RepositoryDto>(updatedRepository);
            return updatedRepositoryDto;
        }

        public RepositoryDto DeleteRepository(long id)
        {
            var existingRepository = _unitOfWork.RepositoryRepository.GetRepositoryById(id);

            if (existingRepository == null)
                return null;

            var deletedRepositoryDto = _mapper.Map<RepositoryDto>(existingRepository);

            _unitOfWork.RepositoryRepository.DeleteRepository(existingRepository);
            _unitOfWork.SaveChanges();
            
            return deletedRepositoryDto;
        }
    }

}
