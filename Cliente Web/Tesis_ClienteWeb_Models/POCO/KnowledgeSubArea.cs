using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class KnowledgeSubArea
    {
        [Key]
        public int KnowledgeSubAreaId { get; set; }

        public string Title { get; set; }

        public List<Career> Careers { get; set; }

        public KnowledgeSubArea()
        {
            this.Careers = new List<Career>();
        }
    }
}
