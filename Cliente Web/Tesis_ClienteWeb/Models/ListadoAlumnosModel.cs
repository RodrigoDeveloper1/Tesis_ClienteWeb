using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class ListadoAlumnosModel
    {
        public List<Course> listaCursos { get; set; }

        public int ListNumber { get; set; }

        public string FirstLastName { get; set; }

        public string SecondLastName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }
    }
}