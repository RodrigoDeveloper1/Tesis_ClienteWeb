using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class NotificacionesModel : MaestraModel
    {
        #region Variables declaradas
        public List<Notification> listaNotificacionesAutomaticas { get; set; }
        public List<Notification> listaNotificacionesPersonalizadas { get; set; }
        public List<Representative> listaRepresentantes { get; set; }
        public List<string> listaTiposAlertas { get; set; }
        public List<string> listaAtribuciones { get; set; }

        public Notification notificacion { get; set; }
        #endregion

        #region Constructor
        public NotificacionesModel()
        {
            this.listaNotificacionesAutomaticas = new List<Notification>();
            this.listaNotificacionesPersonalizadas = new List<Notification>();
            this.listaRepresentantes = new List<Representative>();
            this.listaTiposAlertas = new List<string>();
            this.listaAtribuciones = new List<string>();
            this.notificacion = new Notification();
        }
        #endregion
    }

    public class NotificacionesPersonalizadasModel : MaestraListaColegiosModel
    {        
        public int idAnoEscolar { get; set; }
        [Display(Name = "Año escolar activo respectivo:")]
        public string labelAnoEscolar { get; set; }


        public List<Notification> listaNotificacionesAutomaticas { get; set; }
        public List<Notification> listaNotificacionesPersonalizadas { get; set; }

        public List<Student> listaEstudiantes { get; set; }
        public List<Notification> listaNotificaciones { get; set; }

        [Display(Name = "Tipo de sujeto:")]
        public string idSujeto { get; set; }
        public SelectList selectListSujetos { get; set; }

        #region Propiedades para el diálogo 'Nueva notificación'
        [Display(Name = "Seleccione el sujeto:")]
        public string idElSujeto { get; set; }
        public SelectList selectListElSujeto { get; set; }

        [Display(Name = "Seleccione un curso:")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        [Display(Name = "Seleccione el tipo de notificación:")]
        public int idTipoNotificacion { get; set; }
        public SelectList selectListTiposNotificacion { get; set; }

        [Display(Name = "Seleccione la atribución (opcional):")]
        public int idAtribucion { get; set; }
        public SelectList selectListAtribucion { get; set; }

        [Display(Name = "El mensaje:")]
        public string mensaje { get; set; }
        #endregion

        public NotificacionesPersonalizadasModel()
        {
            this.listaEstudiantes = new List<Student>();
            this.listaNotificaciones = new List<Notification>();
            this.selectListSujetos = new SelectList(new Dictionary<string, string>());
            
            #region Propiedades para el diálogo 'Nueva notificación'
            this.selectListElSujeto = new SelectList(new Dictionary<string, string>());
            this.selectListCursos = new SelectList(new Dictionary<string, string>());
            this.selectListTiposNotificacion = new SelectList(new Dictionary<string, string>());
            this.selectListAtribucion = new SelectList(new Dictionary<string, string>());
            #endregion
        }
    }

    public class BuzonNotificacionesModel : MaestraListaColegiosModel
    {
        public List<object> listaNotificacionesObject { get; set; }

        public int idAnoEscolar { get; set; }
        [Display(Name = "Año escolar activo respectivo:")]
        public string labelAnoEscolar { get; set; }


        public List<Notification> listaNotificacionesAutomaticas { get; set; }
        public List<Notification> listaNotificacionesPersonalizadas { get; set; }

        public List<Student> listaEstudiantes { get; set; }
        public List<Notification> listaNotificaciones { get; set; }

        [Display(Name = "Tipo de sujeto:")]
        public string idSujeto { get; set; }
        public SelectList selectListSujetos { get; set; }

        #region Propiedades para el diálogo 'Nueva notificación'
        [Display(Name = "Seleccione el sujeto:")]
        public string idElSujeto { get; set; }
        public SelectList selectListElSujeto { get; set; }

        [Display(Name = "Seleccione un curso:")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        [Display(Name = "Seleccione el tipo de notificación:")]
        public int idTipoNotificacion { get; set; }
        public SelectList selectListTiposNotificacion { get; set; }

        [Display(Name = "Seleccione la atribución (opcional):")]
        public int idAtribucion { get; set; }
        public SelectList selectListAtribucion { get; set; }

        [Display(Name = "El mensaje:")]
        public string mensaje { get; set; }
        #endregion

        public BuzonNotificacionesModel()
        {
            this.listaNotificacionesObject = new List<object>();
            this.listaEstudiantes = new List<Student>();
            this.listaNotificaciones = new List<Notification>();
            this.selectListSujetos = new SelectList(new Dictionary<string, string>());
            
            #region Propiedades para el diálogo 'Nueva notificación'
            this.selectListElSujeto = new SelectList(new Dictionary<string, string>());
            this.selectListCursos = new SelectList(new Dictionary<string, string>());
            this.selectListTiposNotificacion = new SelectList(new Dictionary<string, string>());
            this.selectListAtribucion = new SelectList(new Dictionary<string, string>());
            #endregion
        }
    }
}