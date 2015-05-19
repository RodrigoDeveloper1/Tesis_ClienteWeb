using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class LoginModel
    {
        #region Declaración de variables
        [Display(Name = "Nombre de usuario:")]
        [Required(ErrorMessage = "Por favor inserte un nombre de usuario.", AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Display(Name = "Contraseña:")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Por favor inserte una contraseña.", AllowEmptyStrings = false)]
        public string Password { get; set; }

        /// <summary>
        /// Variable que guarda la propiedad de estilo css 'display' para representar si se va a mostrar o no
        /// los mensajes de error asociados al modelo.
        /// </summary>
        public string MostrarErrores { get; set; }
        #endregion

        #region Constructor
        public LoginModel()
        {
            this.MostrarErrores = "none";
        }
        #endregion
    }
}