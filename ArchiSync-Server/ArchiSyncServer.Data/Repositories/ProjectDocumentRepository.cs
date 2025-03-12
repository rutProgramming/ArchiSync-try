using ArchiSyncServer.core.Entities;
using ArchiSyncServer.Core.IRepositories;
using ArchiSyncServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiSyncServer.Data.Repositories
{
    public class ProjectDocumentRepository : GenericRepository<ProjectDocument>, IProjectDocumentRepository
    {
        public ProjectDocumentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

}

