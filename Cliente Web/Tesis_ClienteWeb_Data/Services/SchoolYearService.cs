using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class SchoolYearService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;

        public SchoolYearService()
        {
            this._unidad = new UnitOfWork();
            _InicializadorVariablesSesion();                        
        }
        public SchoolYearService(UnitOfWork unidad)
        {
            this._unidad = unidad;
            _InicializadorVariablesSesion();
        }
        /// <summary>
        /// Constructor que se utiliza para identificar cuando se está haciendo una llamada a este servicio
        /// desde la inicialización de la sesión de usuario. Se pasa como parámetro un booleano que identifica
        /// que si se está incializando la sesión.
        /// </summary>
        /// <param name="inicializacionSesion">El booleano que indica que si se está inicializando la sesión
        /// de usuario.</param>
        public SchoolYearService(bool inicializacionSesion)
        {
            this._unidad = new UnitOfWork();

            if(!inicializacionSesion)
                _InicializadorVariablesSesion();
        }

        /// <summary>
        /// Método interno que inicializa todas las variables de la sesión activa
        /// </summary>
        private void _InicializadorVariablesSesion()
        {
            _session = new SessionVariablesRepository();
        }
        #endregion

        #region CRUD
        public bool ModificarColegio(SchoolYear schoolYear)
        {
            try
            {
                _unidad.RepositorioSchoolYear.Modify(schoolYear);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region Obtener años escolares
        public SchoolYear ObtenerAnoEscolar(int idAnoEscolar)
        {
            SchoolYear anoEscolar = (
                from SchoolYear sy in _unidad.RepositorioSchoolYear._dbset
                    .Include("Periods")
                where sy.SchoolYearId == idAnoEscolar
                select sy)
                    .First<SchoolYear>();

            return anoEscolar;
        }
        
        public SchoolYear ObtenerAnoEscolarActivoPorColegio(int idColegio)
        {
            SchoolYear anoEscolar = new SchoolYear();
            try
            {
                anoEscolar = (from SchoolYear sy in _unidad.RepositorioSchoolYear._dbset
                                  .Include("School")
                                  .Include("Periods")
                                  .Include("Events")
                              where sy.School.SchoolId == idColegio &&
                                    sy.Status == true
                              select sy).First<SchoolYear>();
            }
            catch (InvalidOperationException e) 
            {
                /* Si no existe el colegio entra en esta excepción.
                 * 
                 * Rodrigo Uzcátegui - 04-03-15
                 */
                
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }            
            return anoEscolar;
        }
        public List<SchoolYear> ObtenerAnosEscolaresPorColegio(int idColegio)
        {
            List<SchoolYear> listaAnosEscolares =
                (from SchoolYear sy in _unidad.RepositorioSchoolYear._dbset.Include("School")
                 where sy.School.SchoolId == idColegio
                 select sy).OrderBy(m => m.StartDate).ToList<SchoolYear>();

            return listaAnosEscolares;
        }
        public List<SchoolYear> ObtenerAnosEscolaresSinPeriodosPorColegio(int idColegio)
        {
            List<SchoolYear> listaAnosEscolares =
                (from SchoolYear sy in _unidad.RepositorioSchoolYear._dbset.Include("School")
                 where sy.School.SchoolId == idColegio &&
                       sy.Periods.Count == 0
                 select sy).OrderBy(m => m.StartDate).ToList<SchoolYear>();

            return listaAnosEscolares;
        }
        #endregion
        #region Otros métodos
        #endregion
    }
}