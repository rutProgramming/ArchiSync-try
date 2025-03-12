using AutoMapper;
using ArchiSyncServer.core.Entities;
using ArchiSyncServer.Core.IServices;
using ArchiSyncServer.core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchiSyncServer.core.DTOs;
using ArchiSyncServer.Core.IRepositories;


namespace ArchiSyncServer.Service.Services
{
    public class ProjectDocumentService : IProjectDocumentService
    {
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ProjectDocumentService(IProjectDocumentRepository projectDocumentRepository, IMapper mapper, IRepositoryManager repositoryManager)
        {
            _projectDocumentRepository = projectDocumentRepository;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }

        public async Task<ProjectDocumentDTO> GetProjectDocumentAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }
            var projectDocument = await _projectDocumentRepository.GetByIdAsync(id);
            return _mapper.Map<ProjectDocumentDTO>(projectDocument);
        }

        public async Task<IEnumerable<ProjectDocumentDTO>> GetAllProjectDocumentsAsync()
        {
            var projectDocuments = await _projectDocumentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDocumentDTO>>(projectDocuments);
        }

        public async Task<ProjectDocumentDTO> CreateProjectDocumentAsync(ProjectDocumentDTO projectDocumentDto)
        {
            if (projectDocumentDto == null)
            {
                throw new ArgumentNullException(nameof(projectDocumentDto), "Project document data cannot be null.");
            }

            var existingProjectDocument = await _projectDocumentRepository.GetByIdAsync(projectDocumentDto.ProjectDocumentId);
            if (existingProjectDocument != null)
            {
                throw new ArgumentException("Project document  already exists.");
            }
            var projectDocument = _mapper.Map<ProjectDocument>(projectDocumentDto);
            var createdProjectDocument = await _projectDocumentRepository.CreateAsync(projectDocument);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<ProjectDocumentDTO>(createdProjectDocument);
        }
        

        public async Task UpdateProjectDocumentAsync(ProjectDocumentDTO projectDocumentDto)
        {
            var projectDocument = _mapper.Map<ProjectDocument>(projectDocumentDto);
            await _projectDocumentRepository.UpdateAsync(projectDocument);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteProjectDocumentAsync(int id)
        {
            var projectDocument = await _projectDocumentRepository.GetByIdAsync(id);
            await _projectDocumentRepository.DeleteAsync(id);
        }
    }

}
