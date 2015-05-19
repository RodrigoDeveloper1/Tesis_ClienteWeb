using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Core
    {
        //[key] Required - Fluent API
        public int InstituteId { get; set; }
        public Institute Institute { get; set; }

        //[key] Required - Fluent API
        public int CoreId { get; set; }

        [Required]
        public string CoreName { get; set; }

        [Required]
        public string Address { get; set; }

        public List<Opportunity> Offers { get; set; }
        
        public List<Core> Extensions { get; set; }

        public Core()
        {
            this.Offers = new List<Opportunity>();
            this.Extensions = new List<Core>();
        }
    }
}
