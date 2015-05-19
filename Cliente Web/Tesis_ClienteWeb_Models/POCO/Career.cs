using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Career
    {
        [Key]
        public int CareerId { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// Dom('Carrera Corta', 'Carrera Larga', 'Programa Nacional de Formación')
        /// </summary>
        public string Type { get; set; }

        public string Description { get; set; }

        public string OccupationalArea { get; set; }

        public List<Opportunity> Oportunities { get; set; }

        public List<RelatedCareer> RelatedCareers { get; set; }
        public List<RelatedCareer> PrincipalCareers { get; set; }

        public Career()
        {
            this.Oportunities = new List<Opportunity>();
            this.RelatedCareers = new List<RelatedCareer>();
            this.PrincipalCareers = new List<RelatedCareer>();
        }
    }
}
