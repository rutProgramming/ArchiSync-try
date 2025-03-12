using ArchiSyncServer.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArchiSyncServer.Core.IServices
{
    public interface IProjectDocumentService
    {
        Task<ProjectDocumentDTO> GetProjectDocumentAsync(int id);
        Task<IEnumerable<ProjectDocumentDTO>> GetAllProjectDocumentsAsync();
        Task<ProjectDocumentDTO> CreateProjectDocumentAsync(ProjectDocumentDTO projectDocumentDto);
        Task UpdateProjectDocumentAsync(ProjectDocumentDTO projectDocumentDto);
        Task DeleteProjectDocumentAsync(int id);
    }

}
