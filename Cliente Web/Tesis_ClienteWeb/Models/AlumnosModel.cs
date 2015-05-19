using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class AlumnosModel : MaestraModel
    {
        [Required(ErrorMessage = "Por favor seleccione un curso.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de cursos activos:")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        public List<Course> listaCursos { get; set; }

        public List<Student> listaAlumnos { get; set; }

        public Student Student { get; set; }
        public int ListNumber { get; set; }

        public string FirstLastName { get; set; }

        public string SecondLastName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }
    }

    public class AgregarAlumnosModel : MaestraListaColegiosModel
    {
        [Required(ErrorMessage = "Por favor seleccione un curso.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de cursos:")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        public int idAnoEscolar { get; set; }
        [Display(Name = "Año escolar activo respectivo:")]
        public string labelAnoEscolar { get; set; }

        public List<Student> listaEstudiantes { get; set; }

        public AgregarAlumnosModel()
        {
            this.listaEstudiantes = new List<Student>();
            this.selectListCursos = new SelectList(new Dictionary<string, string>());
        }
    }

    public class AsociarRepresentantesModel : MaestraListaColegiosModel
    {
        [Required(ErrorMessage = "Por favor seleccione un curso.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de cursos:")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        public int idAnoEscolar { get; set; }

        [Display(Name = "Año escolar activo respectivo:")]
        public string labelAnoEscolar { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un estudiante.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de estudiantes:")]
        public int idEstudiante { get; set; }
        public SelectList selectListEstudiantes { get; set; }

        public Representative representante1 { get; set; }
        public Representative representante2 { get; set; }

        [Display(Name = "Cédula de identidad:")]
        public string tipoCedula { get; set; }
        public SelectList selectListTiposCedula { get; set; }

        // Se enlaza directamente la selección con el representante.
        public SelectList selectListSexos { get; set; }

        public AsociarRepresentantesModel()
        {
            this.selectListEstudiantes = new SelectList(new Dictionary<string, string>());
            this.selectListCursos = new SelectList(new Dictionary<string, string>());
            this.selectListTiposCedula = new SelectList(new Dictionary<string, string>());
            this.selectListSexos = new SelectList(new Dictionary<string, string>());
            this.representante1 = new Representative();
            this.representante2 = new Representative();
        }
    }

    public class EditarAlumnoModel : MaestraModel
    {
        public Student Student { get; set; }

        [Display(Name = "Cédula de identidad:")]
        public string tipoCedula { get; set; }
        public SelectList selectListTiposCedula { get; set; }

        // Se enlaza directamente la selección con el representante.
        public SelectList selectListSexos { get; set; }

        public EditarAlumnoModel()
        {

        }

        public EditarAlumnoModel(int id)
        {
            StudentService studentService = new StudentService();

            this.selectListTiposCedula = new SelectList(new Dictionary<string, string>());
            this.selectListSexos = new SelectList(new Dictionary<string, string>());

            this.Student = studentService.ObtenerAlumnoPorId(id);
        }
    }
}