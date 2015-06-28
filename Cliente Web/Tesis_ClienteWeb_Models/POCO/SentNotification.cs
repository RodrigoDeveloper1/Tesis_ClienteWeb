using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class SentNotification
    {
        //Required - Fluent API
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SentNotificationId { get; set; }
        
        /// <summary>
        /// Variable que indica si esta notificación fue leída por el destinatario.
        /// </summary>
        [Required]
        public bool Read { get; set; }

        /// <summary>
        /// Variable que indica si está notificación fue enviada o no.
        /// </summary>
        [Required]
        public bool Sent { get; set; }

        /// <summary>
        /// Campo definido para establecer qué notificación es nueva y cuál no para los representantes del 
        /// cliente móvil.
        /// </summary>
        [Required]
        public bool New { get; set; }

        //Required - Fluent API
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        //Not Required - Fluent API
        public Student Student { get; set; }
        
        //Not Required - Fluent API
        public User User { get; set; }        

        //Not Required - Fluent API
        public Course Course { get; set; }

        public SentNotification()
        {
            this.Read = false;
            this.Sent = false;
            this.New = true;
        }
    }
}
