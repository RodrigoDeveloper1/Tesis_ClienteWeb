using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Content
    {
        [Key]
        public int ContentId { get; set; }

        //Required - Fluent API
        public int SubjectId { get; set; }
        public int ContentBlockId { get; set; }
        public ContentBlock ContentBlock { get; set; }

        /// <summary>
        /// Para determinar el grupo de tipos de contenidos relacionados entre sí
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Dom(ContentType): Conceptuales, Procedimentales, Aptitudinales
        /// </summary>
        public string ContentType { get; set; } 

        public string Description { get; set; }
    }
}
