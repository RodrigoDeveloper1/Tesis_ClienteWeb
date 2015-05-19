using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class State
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)] //El id se agrega manualmente
        public int StateId { get; set; }

        public string Name { get; set; }

        public List<Core> Cores { get; set; }

        public State()
        {
            this.Cores = new List<Core>();
        }
    }
}
