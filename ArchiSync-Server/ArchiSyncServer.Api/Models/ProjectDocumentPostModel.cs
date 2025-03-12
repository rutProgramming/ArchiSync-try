using System.ComponentModel.DataAnnotations;

namespace ArchiSyncServer.API.Models
{
    public class ProjectDocumentPostModel
    {
        public string FilePath { get; set; }
        public int ProjectId { get; set; }
    }
}
