using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class PeriodService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;

        public PeriodService()
        {
            this._unidad = new UnitOfWork();
        }
        public PeriodService(UnitOfWork unidad)
        {
            this._unidad = unidad;
        }

        /// <summary>
        /// Método interno que inicializa todas las variables de la sesión activa
        /// </summary>
        private void _InicializadorVariablesSesion()
        {
            _session = new SessionVariablesRepository();
        }
        #endregion

        public bool GuardarPeriod(Period period)
        {

            _unidad.RepositorioPeriod.Add(period);

            try
            {
                _unidad.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ModificarPeriod(Period period)
        {
            try
            {
                _unidad.RepositorioPeriod.Modify(period);
                _unidad.Save();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool EliminarPeriod(int periodID)
        {
            var aux = _unidad.RepositorioPeriod.GetAll().Where(u => u.PeriodId == periodID).Count();
            return true;
        }

        public Period ObtenerPeriodPorId(int id)
          {
              Period lapso = (from Period s in _unidad.RepositorioPeriod.GetAll()
                                 where s.PeriodId == id
                                 select s).FirstOrDefault<Period>();
              return lapso;
          }

        /// <summary>
        /// Método que obtiene el período presente en el transcurso de un año escolar en específico. Se compara
        /// con base a la fecha del día de hoy (DateTime.Now). Si el año escolar es inactivo, no resultará ningún 
        /// período escolar.
        /// </summary>
        /// <param name="SchoolYearId">El id del año escolar</param>
        /// <returns>El período actual en el transcurso del año</returns>
        public Period ObtenerPeriodoActivoPor_AnoEscolar(int SchoolYearId)
        {
            Period periodo = (
                from Period period in _unidad.RepositorioPeriod._dbset
                where period.SchoolYear.SchoolYearId == SchoolYearId &&
                      period.StartDate <= DateTime.Now &&
                      period.FinishDate >= DateTime.Now
                select period)
                    .FirstOrDefault<Period>();

            return periodo;
        }
        public Period ObtenerPeriodoActivoPor_SAnoEscolar()
        {
            Period periodo = (
                from Period period in _unidad.RepositorioPeriod._dbset
                where period.SchoolYear.SchoolYearId == _session.SCHOOLYEARID &&
                      period.StartDate <= DateTime.Now &&
                      period.FinishDate >= DateTime.Now
                select period)
                    .FirstOrDefault<Period>();

            return periodo;
        }
    }
}
