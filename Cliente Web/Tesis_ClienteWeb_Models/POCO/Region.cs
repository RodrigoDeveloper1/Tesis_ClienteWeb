using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Region
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)] //El id se agrega manualmente
        public int RegionId { get; set; }

        public string Name { get; set; }

        public List<State> States { get; set; }

        public Region()
        {
            this.States = new List<State>();
        }
    }
}
