using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class CASU
    {
        //Required - Fluent API
        public int CourseId { get; set; }
        public Course Course { get; set; }

        //Required - Fluent API
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        //Required - Fluent API
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        //Not Required - Fluent API
        public List<Assessment> Assessments { get; set; }

        //Not Required - Fluent API
        public string TeacherId { get; set; }
        public User Teacher { get; set; }

        public CASU()
        {
            this.Assessments = new List<Assessment>();
        }
    }
}
