using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class ContentBlock
    {
        [Key]
        public int ContentBlockId { get; set; }

        //Required - Fluent API
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        List<Content> Contents { get; set; }

        public ContentBlock()
        {
            Contents = new List<Content>();
        }
    }
}
