using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class IndicatorService
    {
        private UnitOfWork _unidad;

        //Constructores
        public IndicatorService()
        {
            this._unidad = new UnitOfWork();
        }
        public IndicatorService(UnitOfWork _unidad)
        {
            this._unidad = _unidad;
        }

        #region CRUD
        /// <summary>
        /// Método CRUD - Modificar IndicatorAssignation
        /// </summary>
        /// <param name="IA">El IndicatorAssignation a modificar</param>
        /// <returns>True = Modificado exitoso</returns>
        public bool ModificarIndicatorAssignation(IndicatorAssignation IA)
        {
            try
            {
                _unidad.RepositorioIndicatorAssignation.Modify(IA);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        
        #region Obteniendo Indicadores
        /// <summary>
        /// Método que obtiene el indicador por el id asociado. Se debe recordar que la clase Indicator tiene
        /// dos atributos como clave primaria.
        /// Rodrigo Uzcátegui - 24-05-15
        /// </summary>
        /// <param name="idIndicador">El id del indicador</param>
        /// <returns>El indicador respectivo.</returns>
        public Indicator ObtenerIndicadorPor_Id(int idIndicador)
        {
            Indicator indicador = (
                from Indicator aux in _unidad.RepositorioIndicator._dbset
                    .Include("Competency.Subject")
                    .Include("Competency.Indicators")
                    .Include("IndicatorAssessments")
                where aux.IndicatorId == idIndicador
                select aux)
                    .FirstOrDefault();

            return indicador;
        }
        /// <summary>
        /// Método que obtiene la lista de indicadores de una competencia en particular.
        /// Rodrigo Uzcátegui - 24-05-15
        /// </summary>
        /// <param name="idCompetencia">El id de la competencia</param>
        /// <returns>La lista de indicadores respectiva.</returns>
        public List<Indicator> ObteniendoListaIndicadoresPor_Competencia(int idCompetencia)
        {
            List<Indicator> listaIndicadores = (
                from Indicator indicator in _unidad.RepositorioIndicator._dbset
                    .Include("Competency")
                    .Include("IndicatorAssessments")
                where indicator.CompetencyId == idCompetencia
                select indicator)
                    .ToList<Indicator>();

            return listaIndicadores;
        }
        #endregion

        #region Obteniendo Competencias
        public List<Competency> ObteniendoListaCompetenciasPor_Evaluacion(int idEvaluacion)
        {
            List<Competency> listaCompetencias = new List<Competency>();

            #region Obteniendo la evaluación respectiva
            AssessmentService assessmentService = new AssessmentService(this._unidad);
            Assessment assessment = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);
            #endregion
            #region Ciclo por cada IndicatorAssessment
            foreach (IndicatorAssessment IA in assessment.IndicatorAssessments)
            {
                IndicatorAssessment auxIA = this.ObtenerIndicatorAssessmentPor_Id(IA.IndicatorId, 
                    IA.CompetencyId, IA.AssessmentId);

                listaCompetencias.Add(auxIA.Indicator.Competency);
            }
            #endregion

            return listaCompetencias.ToHashSet<Competency>().ToList<Competency>();
        }
        #endregion

        #region Obteniendo IndicatorAssessment
        /// <summary>
        /// Método que obtiene el IndicatorAssessment correspondiente según los id de su clave primaria.
        /// Rodrigo Uzcátegui - 24-05-15
        /// </summary>
        /// <param name="IndicatorId">El id del indicador</param>
        /// <param name="CompetencyId">El id de la competencia</param>
        /// <param name="AssessmentId">El id de la evaluación</param>
        /// <returns>El IndicatorAssessment respectivo</returns>
        public IndicatorAssessment ObtenerIndicatorAssessmentPor_Id(int IndicatorId, 
            int CompetencyId, int AssessmentId)
        {
            IndicatorAssessment indicatorAssessment = (
                from IndicatorAssessment IA in _unidad.RepositorioIndicatorAssessment._dbset
                    .Include("Indicator.Competency")
                    .Include("Assessment")
                    .Include("IndicatorAsignations")
                where IA.IndicatorId == IndicatorId &&
                      IA.CompetencyId == CompetencyId &&
                      IA.AssessmentId == AssessmentId
                select IA)
                    .FirstOrDefault<IndicatorAssessment>();

            return indicatorAssessment;
        }
        #endregion

        #region Obteniendo IndicatorAssignations
        /// <summary>
        /// Método que obtiene el IndicatorAssignation respectivo según el id principal.
        /// Rodrigo Uzcátegui - 25-05-15
        /// </summary>
        /// <param name="IndicatorAssignationId">El id del IndicatorAssignation</param>
        /// <returns>El IndicatorAssignation respectivo.</returns>
        public IndicatorAssignation ObtenerIndicatorAssignationPor_Id(int IndicatorAssignationId)
        {
            IndicatorAssignation IA = (
                from IndicatorAssignation auxIA in _unidad.RepositorioIndicatorAssignation._dbset
                    .Include("IndicatorAssessment.Assessment")
                where auxIA.IndicatorAssignationId == IndicatorAssignationId
                select auxIA)
                    .FirstOrDefault<IndicatorAssignation>();

            return IA;
        }
        #endregion

        #region Otros métodos
        #endregion
    }
}