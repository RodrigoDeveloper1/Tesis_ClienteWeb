using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class MateriasModel : MaestraListaColegiosModel
    {
        #region Declaración de variables

        public List<Course> listaCursos { get; set; }
        public List<Subject> listaMaterias { get; set; }
        public List<Period> listaLapsos { get; set; }

        [Display(Name = "Lista de cursos:")]
        [Required(ErrorMessage = "Por favor seleccione el curso.")]
        public int idCurso { get; set; }
        public SelectList selectListCursos { get; set; }

        [Display(Name = "Lista de profesores:")]
        [Required(ErrorMessage = "Por favor seleccione el profesor.")]
        public string idProfesor { get; set; }
        public SelectList selectListProfesores { get; set; }

        [Display(Name = "Lista Materias")]
        [Required(ErrorMessage = "Por favor seleccione la materia.")]
        public int idMateria { get; set; }
        public SelectList selectListMaterias { get; set; }

        [Display(Name = "Lista de lapsos:")]
        [Required(ErrorMessage = "Por favor seleccione el lapso.")]
        public int idLapso { get; set; }
        public SelectList selectListLapsos { get; set; }  
 
        public Subject materia { get; set; }

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "Por favor ingrese el nombre.")]
        public string Name { get; set; }

        [Display(Name = "Código:")]
        [Required(ErrorMessage = "Por favor ingrese el código.")]
        public string SubjectCode { get; set; }

        [Display(Name = "Pensum:")]
        [Required(ErrorMessage = "Por favor ingrese el pensum.")]
        public string Pensum { get; set; }

        [Display(Name = "Grado:")]
        [Required(ErrorMessage = "Por favor ingrese el grado.")]
        public int Grade { get; set; }

        #endregion

        #region Constructor
        public MateriasModel()
        {
            this.materia = new Subject();
            this.listaCursos = new List<Course>();
            this.listaMaterias = new List<Subject>();
            this.selectListCursos = new SelectList(new Dictionary<string, string>());
        }
        #endregion
    }
}