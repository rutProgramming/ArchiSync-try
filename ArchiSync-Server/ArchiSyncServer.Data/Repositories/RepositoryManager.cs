using ArchiSyncServer.core.Entities;
using ArchiSyncServer.Core.IRepositories;
using ArchiSyncServer.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiSyncServer.Data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ICommentRepository _commentRepository;

        public RepositoryManager(ApplicationDbContext context)
        {
            _context = context;
            _projectDocumentRepository = new ProjectDocumentRepository(context);
            _projectRepository = new ProjectRepository(context);
            _documentRepository = new DocumentRepository(context);
            _commentRepository = new CommentRepository(context);
        }

        public IProjectDocumentRepository ProjectDocument => _projectDocumentRepository;
        public IProjectRepository Project => _projectRepository;
        public IDocumentRepository Document => _documentRepository;
        public ICommentRepository Comment => _commentRepository;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
