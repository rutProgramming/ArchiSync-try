using ArchiSyncServer.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiSyncServer.core.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }

}
