using ArchiSyncServer.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiSyncServer.Core.IRepositories
{

    public interface IDocumentRepository : IGenericRepository<Document>
    {
        // הוספת מתודולוגיות ספציפיות למסמכים אם יש צורך
    }


}
