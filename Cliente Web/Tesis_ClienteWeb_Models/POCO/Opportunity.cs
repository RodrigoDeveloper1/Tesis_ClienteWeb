using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Opportunity
    {
        //Required - Fluent API
        public int CareerId { get; set; }
        public Career Career { get; set; }

        //Required - Fluent API
        public int CoreId { get; set; }
        public int InstituteId { get; set; }
        public Core Core { get; set; }

        public string Degree { get; set; }

        public int Term { get; set; }

        public List<Subject> Subjects { get; set; }

        public Opportunity()
        {
            this.Subjects = new List<Subject>();
        }
     }
}