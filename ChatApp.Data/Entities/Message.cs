using ChatApp.Shared.Models.Commons;
using ChatApp.Shared.Models.Commons.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ChatApp.Data.Entities
{
    [Index(nameof(CreatedAt), Name = "Messages_CreatedAt_idx")]
    public class Message : BaseEntity<long>
    {
        public string Content { get; set; }
        public long UserId { get; set; }
        public long? GroupId { get; set; }
        public Group Group { get; set; }
    }
}
