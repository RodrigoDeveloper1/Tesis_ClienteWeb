using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Controllers;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Data.UserExceptions;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{    
    public class MaestraModel
    {
        /// <summary>
        /// Variable que determina si el usuario tiene permisología para acceder a las acciones maestras
        /// </summary>
        public bool ACCESO_MAESTRAS { get; set; }
        /// <summary>
        /// Variable que determina si el usuario tiene privilegios para ver la lista de colegios de administrador
        /// </summary>
        public bool ADMINISTRADOR { get; set; }
        /// <summary>
        /// Variable que determina el nombre de usuario de la sesión activa.
        /// </summary>
        public string _USERNAME { get; set; }

        /// <summary>
        /// Variable que guarda la propiedad de estilo css 'display' para representar si se va a mostrar o no
        /// los mensajes de error asociados al modelo.
        /// </summary>
        public string MostrarErrores { get; set; }
        /// <summary>
        /// Variable que guarda la propiedad de estilo css 'display' para representar si se va a mostrar o no
        /// los mensajes de aclamaciones asociados al modelo.
        /// </summary>
        public string MostrarAclamaciones { get; set; }        
        /// <summary>
        /// Variable que guarda la propiedad de estilo css 'display' para representar si se va a mostrar o no
        /// el número de notificaciones no leídas asociadas al usuario.
        /// </summary>
        public string MostrarNroNotificaciones { get; set; }
        /// <summary>
        /// Variable que guarda el mensaje asociado a la aclamación ocurrida en las acciones de la página.
        /// </summary>
        public string MensajeAclamacion { get; set; }

        /// <summary>
        /// Variable que indica el nro de notificaciones no leídas de el usuario de una sesión.
        /// </summary>
        public int nroNotificacionesNoLeidas { get; set; }

        [Display(Name = "Lista de colegios:")]
        public int idColegio { get; set; }
        public SelectList selectListColegios { get; set; }

        public List<Event> ListaEventosHeader { get; set; }
        public List<float> ListaPorcentajesPeriodosHeader { get; set; }

        public MaestraModel()
        {
            this.MostrarErrores = "none";
            this.MostrarAclamaciones = "none";
            this.MensajeAclamacion = "";
            this.MostrarNroNotificaciones = "none";
            this.nroNotificacionesNoLeidas = 0;
                        
            this.ListaEventosHeader = new List<Event>();
            this.ListaPorcentajesPeriodosHeader = new List<float>();
            
            this.selectListColegios = new SelectList(new Dictionary<string, string>());
        }
    }

    public class MaestraListaColegiosModel : MaestraModel
    {
        [Required(ErrorMessage = "Por favor seleccione un colegio.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de colegios:")]
        new public int idColegio { get; set; }
        new public SelectList selectListColegios { get; set; }

        public MaestraListaColegiosModel()
        {
            List<School> _listaColegios = new SchoolService().ObtenerListaColegiosActivos().ToList<School>();
            this.selectListColegios = new SelectList(_listaColegios, "SchoolId", "Name");
        }
    }
}