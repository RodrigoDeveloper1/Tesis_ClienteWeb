using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class RepresentantesModel : MaestraModel
    {
        public int RepresentativeId { get; set; }

        public string Name { get; set; }

        public Representative Representante { get; set; }

        public List<Representative> listaRepresentantes { get; set; }
    }
}