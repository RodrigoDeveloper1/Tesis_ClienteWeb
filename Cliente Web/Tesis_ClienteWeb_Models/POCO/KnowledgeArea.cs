using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class KnowledgeArea
    {
        [Key]
        public int KnowledgeAreaId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<KnowledgeSubArea> KnowledgeSubAreas { get; set; }

        public KnowledgeArea()
        {
            this.KnowledgeSubAreas = new List<KnowledgeSubArea>();
        }
    }
}
