using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Country
    {        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)] //El id se agrega manualmente
        public int CountryId { get; set; }

        public string CountryName { get; set; }
        
        public string OfficialName { get; set; }

        public List<Region> Regions { get; set; }

        public List<State> States { get; set; }

        public Country()
        {
            this.Regions = new List<Region>();
            this.States = new List<State>();
        }
    }
}
