using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class RelatedCareer
    {
        public int PrincipalCareerId { get; set; }
        public Career PrincipalCareer { get; set; }

        public int RelatedCareerId { get; set; }
        public Career Career { get; set; }

        public RelatedCareer()
        {
        }
    }
}
