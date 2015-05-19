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
    public class ColegiosModel : MaestraModel
    {
        #region Variables declaradas
        public School colegio { get; set; }

        [Display(Name = "Estatus del colegio:")]
        [Required(ErrorMessage = "Por favor seleccione el estatus del colegio.")]
        public string estatusColegio { get; set; }

        [Display(Name = "Estatus del período escolar:")]
        [Required(ErrorMessage = "Por favor seleccione el estatus del período escolar.")]
        public string estatusPeriodoEscolar { get; set; }

        [Required(ErrorMessage = "Por favor seleccione una fecha de inicio del período escolar", 
            AllowEmptyStrings = false)]
        [Display(Name = "Fecha de inicio:")]
        [DataType(DataType.DateTime)]
        public DateTime FechaInicioPeriodo { get; set; }

        [Required(ErrorMessage = "Por favor seleccione una fecha de finalización del período escolar",
            AllowEmptyStrings = false)]
        [Display(Name = "Fecha de finalización:")]
        [DataType(DataType.DateTime)]
        public DateTime FechaFinalizacionPeriodo { get; set; }

        public SelectList listaEstatusColegio { get; set; }

        public SelectList listaEstatusPeriodoEscolar { get; set; }
        #endregion

        #region Constructor
        public ColegiosModel()
        {
            this.colegio = new School();
            this.listaEstatusColegio = new SelectList(new Dictionary<string, string>());
            this.listaEstatusPeriodoEscolar = new SelectList(new Dictionary<string, string>());
        }
        #endregion
    }

    public class ListarColegiosModel : MaestraModel
    {
        public List<School> listaColegios { get; set; }

        public ListarColegiosModel()
        {
            this.listaColegios = new List<School>();
        }
    }

    public class EditarColegioModel : MaestraModel
    {
        public School colegio { get; set; }

        public string fechaCreacion { get; set; }

        [Display(Name = "Estatus del colegio:")]
        public string estatus { get; set; }

        public EditarColegioModel()
        {
            this.colegio = new School();
        }
    }

    public class CrearAnoEscolarModel : MaestraModel
    {
        [Display(Name="Estatus del año escolar:")]
        [Required(ErrorMessage = "Por favor seleccione el estatus del colegio.")]
        public string estatusPeriodoEscolar { get; set; }

        public SelectList listaEstatusPeriodoEscolar { get; set; }

        public SchoolYear anoEscolar { get; set; }

        public Period lapsoI { get; set; }
        public Period lapsoII { get; set; }
        public Period lapsoIII { get; set; }

        public int contadorDiasI { get; set; }
        public int contadorDiasII { get; set; }
        public int contadorDiasIII { get; set; }

        public List<PersonalSchool> listaColegiosPersonales { get; set; }
        
        public CrearAnoEscolarModel() : base()
        {
            this.lapsoI = new Period();
            this.lapsoII = new Period();
            this.lapsoIII = new Period();
            
            this.anoEscolar = new SchoolYear();            
            this.listaEstatusPeriodoEscolar = new SelectList(new Dictionary<string, string>());
            this.listaColegiosPersonales = new List<PersonalSchool>();
        }
        
        #region Clase PersonalSchool
        public class PersonalSchool
        {
            public School colegio { get; set; }
            public bool isSelected { get; set; }
            public bool anoEscolarActivo { get; set; }

            public PersonalSchool()
            {
                this.colegio = new School();
                this.isSelected = false;
                this.anoEscolarActivo = false;
            }
        }
        #endregion
    }

    public class PeriodoEscolarModel : MaestraListaColegiosModel
    {
        [Display(Name = "Fecha inicio del año escolar:")]
        public string fechaInicioAnoEscolar { get; set; }

        [Display(Name = "Fecha finalización del año escolar:")]
        public string fechaFinAnoEscolar { get; set; }

        [Required(ErrorMessage = "Por favor seleccione un año escolar.", AllowEmptyStrings = false)]
        [Display(Name = "Lista de años escolares:")]
        public int idAnoEscolar { get; set; }

        public SelectList selectListAnosEscolares { get; set; }

        public Period lapsoI { get; set; }
        public Period lapsoII { get; set; }
        public Period lapsoIII { get; set; }

        public int contadorDiasI { get; set; }
        public int contadorDiasII { get; set; }
        public int contadorDiasIII { get; set; }

        public PeriodoEscolarModel()
        {
            this.lapsoI = new Period();
            this.lapsoII = new Period();
            this.lapsoIII = new Period();

            this.selectListAnosEscolares = new SelectList(new Dictionary<string, string>());
        }
    }
}