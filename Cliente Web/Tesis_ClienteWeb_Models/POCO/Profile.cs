using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }

        /* MaxLenght vs StringLenght:
         * 
         *      MaxLength is used for the Entity Framework to decide how large to make a string value field 
         * when it creates the database.
         * 
         *      StringLength is a data annotation that will be used for validation of user input.
         * 
         * Fuente: http://stackoverflow.com/questions/5717033/stringlength-vs-maxlength-attributes-asp-net-mvc-with-entity-framework-ef-code-f
         */

        [Index("Profile_NameIndex", IsUnique = true)]
        [Display(Name = "Nombre del perfil:")]
        [Required(ErrorMessage = "Por favor inserte el nombre del perfil.", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "El nombre del perfil debe tener un máximo de 100 caracteres")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Nombre del controlador:")]
        [Required(ErrorMessage = "Por favor inserte un controlador.", AllowEmptyStrings = false)]
        public string ControllerName { get; set; }

        [Index("Profile_ActionIndex", IsUnique = true)]
        [Display(Name = "Nombre de la acción:")]
        [Required(ErrorMessage = "Por favor inserte una acción.", AllowEmptyStrings = false)]
        [StringLength(100, ErrorMessage = "La acción del perfil debe tener un máximo de 100 caracteres")]
        [MaxLength(100)]
        public string Action { get; set; }

        public ICollection<Role> Roles { get; set; }

        public Profile()
        {
            Roles = new HashSet<Role>();
        }
    }
}
