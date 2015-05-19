using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        
        [Display(Name="Apellido:")]
        [Required(ErrorMessage = "Por favor insertar el primer apellido del alumno", AllowEmptyStrings = false)]
        [StringLength(20,ErrorMessage="El primer apellido debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string FirstLastName { get; set; }

        [Display(Name = "Segundo apellido:")]
        [Required(ErrorMessage = "Por favor insertar el segundo apellido del alumno", AllowEmptyStrings = false)]
        [StringLength(20, ErrorMessage = "El segundo apellido debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string SecondLastName { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "Por favor insertar el primer nombre del alumno", AllowEmptyStrings = false)]
        [StringLength(20, ErrorMessage = "El primer nombre debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Display(Name = "Segundo nombre:")]
        [StringLength(20, ErrorMessage = "El segundo nombre debe tener un máximo de 20 caracteres")]
        [MaxLength(20)]
        public string SecondName { get; set; }

        [Display(Name = "# Lista:")]
        public int NumberList { get; set; }

        [Display(Name = "# Registro:")]
        public int RegistrationNumber { get; set; }

        /// <summary>
        /// Solo tiene un curso activo, los demás son inactivos
        /// </summary>
        public List<Course> Courses { get; set; }
        
        public List<Representative> Representatives { get; set; }

        public List<Score> Scores { get; set; }

        public List<SentNotification> ReceivedNotifications { get; set; }

        public Student()
        {
            this.Courses = new List<Course>();
            this.Representatives = new List<Representative>();
            this.Scores = new List<Score>();
            this.ReceivedNotifications = new List<SentNotification>(); //Lista de notificaciones recibidas
        }
   }
}
