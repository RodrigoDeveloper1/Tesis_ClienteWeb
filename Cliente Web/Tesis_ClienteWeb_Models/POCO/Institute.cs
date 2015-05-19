using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Institute
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)] //El id se agrega manualmente
        public int InstituteId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Dom('Pública', 'Privada')
        /// </summary>
        public string Profile { get; set; }

        public List<Core> Cores { get; set; }

        public Institute()
        {
            this.Cores = new List<Core>();
        }
    }
}
