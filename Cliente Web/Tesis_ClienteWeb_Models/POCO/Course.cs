using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
                
       // public User User { get; set; }

        [Display(Name = "Nombre del curso (autogenerable):")]
        [Required(ErrorMessage = "Por favor insertar el nombre del curso", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "El nombre debe tener un máximo de {1} caracteres")]
        [MaxLength(100)]
        public string Name { get; set; } /* El nombre debe ser autogenerable.
                                          * Ejemplo: {Grado} + ' - Sección ' + {Sección}
                                          *          1er Grado - Sección A
                                          *          3er Grado - Sección Única (Solo cuando sección es 'U').
                                          *          
                                          * Cuando el curso esté inactivo, el nombre deberá indicar que está 
                                          * inactivo.
                                          * 
                                          * Ejemplo: 2do Grado - Sección B (Inactivo).
                                          */

        [Display(Name = "Grado del curso:")]
        [Required(ErrorMessage = "Por favor especifique un grado.", AllowEmptyStrings = false)]
        public int Grade { get; set; } /* Los grados son:
                                        *       1ero (1)
                                        *       2do  (2)
                                        *       3ero (3)
                                        *       4to  (4)
                                        *       5to  (5)
                                        *       6to  (6)
                                        *       7mo  (7)
                                        *       8vo  (8)
                                        *       9no  (9)
                                        *       10mo (10)
                                        *       11mo (11)
                                        */

        [Display(Name = "Sección:")]
        [Required(ErrorMessage = "Por favor especifique una sección.", AllowEmptyStrings = false)]
        public string Section { get; set; } //Constante: COURSE_SECTION_LIST

        public School School { get; set; }

        public List<Student> Students { get; set; }

        public List<CASU> CASUs { get; set; }

        public Course()
        {
            this.Students = new List<Student>();
            this.CASUs = new List<CASU>();
        }
    }
}
