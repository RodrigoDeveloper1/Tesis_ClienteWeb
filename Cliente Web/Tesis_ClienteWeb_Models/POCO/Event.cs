using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Por favor insertar el nombre del evento", AllowEmptyStrings = false)]
        [StringLength(40, ErrorMessage = "El nombre debe tener un máximo de 40 caracteres")]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Por favor insertar la descripción del evento", AllowEmptyStrings = false)]
        [StringLength(140, ErrorMessage = "La descripción debe tener un máximo de 140 caracteres")]
        [MaxLength(140)]
        public string Description { get; set; }
        
        #region StartDate & FinishDate
        /* Rodrigo Uzcátegui - 06-03-15
         * 
         * El calendario de eventos debe mostrar dos eventos por cada evento en base de datos, siempre y cuando
         * la fecha de inicio sea diferente a la fecha de fin. Esto se hace para indicar que en tal día se da 
         * inicio a un evento en particular, y en tal día se da fin a ese evento. Estos dos eventos del calendario
         * (el de inicio y el de fin) no significa que hayan dos eventos en base de datos. Significa que ese
         * evento de base de datos se muestra en dos eventos de calendario.
         */
        [Required(ErrorMessage = "Por favor insertar la fecha de inicio del evento", AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Por favor insertar la fecha de finalización del evento", AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime FinishDate { get; set; }
        #endregion

        [StringLength(20, ErrorMessage = "La hora debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string StartHour { get; set; }

        [StringLength(20, ErrorMessage = "La hora debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string EndHour { get; set; }

        public string Color { get; set; }

        public string EventType { get; set; } //Constante

        /// <summary>
        /// Booleano que determina si ese evento se puede eliminar o no. La regla de negocio indica que no se
        /// pueden eliminar los eventos que van dirigidos a todos los docentes del año escolar (eventos 
        /// globales), si un evento es dirigido a unos sujetos en específicos se puede eliminar dicho evento 
        /// (eventos personalizados).
        /// 
        /// True = Evento personalizado. Puede eliminarse.
        /// False = Evento global. No puede eliminarse.
        /// </summary>
        public bool DeleteEvent { get; set; }

        [Required]
        public SchoolYear SchoolYear { get; set; }

        public List<User> Users { get; set; }

        public virtual List<Notification> Notifications { get; set; }

        #region Constructores
        public Event()
        {
            this.Users = new List<User>();
            this.Notifications = new List<Notification>();
        }

        public Event(bool DeleteEvent)
        {
            this.Users = new List<User>();
            this.Notifications = new List<Notification>();
            this.DeleteEvent = DeleteEvent;
        }
        #endregion
    }
}
