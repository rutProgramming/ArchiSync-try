using ArchiSyncServer.core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArchiSyncServer.core.Iservices

{
    public interface IProjectService
    {
        Task<ProjectDTO> GetProjectAsync(int id);
        Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync();
        Task<ProjectDTO> CreateProjectAsync(ProjectDTO projectDto);
        Task UpdateProjectAsync(ProjectDTO projectDto);
        Task DeleteProjectAsync(int id);
    }


}
