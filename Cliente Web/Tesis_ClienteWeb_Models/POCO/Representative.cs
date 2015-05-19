using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Representative
    {
        [Key]
        public int RepresentativeId { get; set; }

        [Display(Name= "Sexo:")]
        [Required(ErrorMessage = "Por favor seleccionar el género del representante", AllowEmptyStrings = false)]
        public bool Gender { get; set; } /* Género: 1 = Másculino. 
                                          *         0 = Femenino.
                                          */

        [Required(ErrorMessage = "Por favor especifique el número de cédula", AllowEmptyStrings= false)]
        public string IdentityNumber { get; set; } //Cédula de identidad
        
        [Display(Name= "Nombre(s):")]
        [Required(ErrorMessage = "Por favor insertar el nombre del representante", AllowEmptyStrings = false)]
        [StringLength(80, ErrorMessage = "El nombre debe tener un máximo de 80 caracteres")]
        [MaxLength(80)]
        public string Name { get; set; }

        [Display(Name = "Apellido #1:")]
        [Required(ErrorMessage = "Por favor insertar el apellido del representante", AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [Display(Name = "Apellido #2:")]
        [Required(ErrorMessage = "Por favor insertar el segundo apellido del representante", AllowEmptyStrings = false)]
        public string SecondLastName { get; set; }

        [Index("Representative_EmailIndex", IsUnique = true)]
        [Display(Name= "Correo electrónico:")]
        [Required(ErrorMessage = "Por favor insertar un correo.", AllowEmptyStrings = false)]
        [StringLength(45, ErrorMessage = "El nombre debe tener un máximo de 45 caracteres")]
        [MaxLength(45)]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public List<Student> Students { get; set; }

        public Representative()
        {
            this.Students = new List<Student>();            
        }
    }
}
