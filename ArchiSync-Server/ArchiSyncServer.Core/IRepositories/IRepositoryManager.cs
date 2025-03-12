using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiSyncServer.Core.IRepositories
{
    public interface IRepositoryManager
    {
        IProjectDocumentRepository ProjectDocument { get; }
        IProjectRepository Project { get; }
        IDocumentRepository Document { get; }
        ICommentRepository Comment { get; }
        Task SaveAsync();
    }


}
