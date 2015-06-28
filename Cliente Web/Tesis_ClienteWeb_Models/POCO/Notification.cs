using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Notification
    {
        [Key]        
        public int NotificationId { get; set; }

        /// <summary>
        /// Atribución es el contexto al que va asociado esa 
        /// notificación, los contextos son: 
        ///    1. Todos los nombres de las materias.
        ///    2. Todos los nombres de los eventos.
        ///    3. Período escolar
        ///    4. N/A
        /// </summary>
        [Required(ErrorMessage = "Por favor seleccione el tipo de atribución", AllowEmptyStrings = false)]
        public string Attribution { get; set; } 

        [Required(ErrorMessage = "Por favor seleccione el tipo de alerta.", AllowEmptyStrings = false)]
        public string AlertType { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.DateTime)]
        public DateTime DateOfCreation { get; set; }

        [Required(AllowEmptyStrings=false)]
        [DataType(DataType.DateTime)]
        public DateTime SendDate { get; set; }

        [Display(Name = "Mensaje de la notificación:")]
        [Required(ErrorMessage = "Por favor insertar el mensaje de la notificación", AllowEmptyStrings = false)]
        [StringLength(140, ErrorMessage = "El mensaje debe tener un máximo de 140 caracteres")]
        [MaxLength(140)]
        public string Message { get; set; }
                
        public bool Automatic { get; set; }
                
        public User User { get; set; }

        public List<SentNotification> SentNotifications { get; set; }

        public Event Event { get; set; }

        [Required]
        public SchoolYear SchoolYear { get; set; }

        #region Constructores
        public Notification()
        {
            this.SentNotifications = new List<SentNotification>(); //Lista de notificaciones enviadas
            this.DateOfCreation = DateTime.Now;
        }

        public Notification(bool Automatic)
        {
            this.SentNotifications = new List<SentNotification>(); //Lista de notificaciones enviadas
            this.DateOfCreation = DateTime.Now;
            this.Automatic = Automatic;
        }
        #endregion
    }
}
