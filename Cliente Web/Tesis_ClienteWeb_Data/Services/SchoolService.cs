using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class SchoolService
    {
        private UnitOfWork _unidad;

        //Constructores
        public SchoolService()
        {
            this._unidad = new UnitOfWork();
        }
        public SchoolService(UnitOfWork unidad)
        {
            this._unidad = unidad;
        }

        #region CRUD
        /// <summary>
        /// Método CRUD. Guardar Colegio.
        /// </summary>
        /// <param name="school">El objeto colegio</param>
        /// <returns>True = Guardado correcto</returns>
        public bool GuardarColegio(School school)
        {
            try
            {
                _unidad.RepositorioSchool.Add(school);
                _unidad.Save();

                return true;
            }
            catch (DbUpdateException e)
            {
                //Cuando el nombre de un colegio está repetido.
                //Rodrigo Uzcátegui - 14/04/15
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Método CRUD. Modificar colegio
        /// </summary>
        /// <param name="school">El Colegio a editar</param>
        /// <returns>True = Editado correcto.</returns>
        public bool ModificarColegio(School school)
        {
            try
            {
                _unidad.RepositorioSchool.Modify(school);
                _unidad.Save();

                return true;
            }
            catch (DbUpdateException e)
            {
                //Excepción encapsulada por: Rodrigo Uzcátegui - 11-05-15
                //Entra cuando se duplica el nombre de un colegio.
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region Obtener colegios
        /// <summary>
        /// Método que obtiene la lista global de colegios activos de la base de datos
        /// </summary>
        /// <returns>La lista de colegios respectiva</returns>
        public List<School> ObtenerListaColegiosActivos()
        {
            List<School> lista = (
                    from School school in _unidad.RepositorioSchool._dbset
                        .Include("SchoolYears")
                        .Include("Users")
                        .Include("Subjects")
                    where school.Status == true
                    select school)
                        .OrderBy(m => m.Name)
                        .ToList<School>();

            return lista;
        }

        public School ObtenerColegioPorId(int id)
        {
            School colegio = (
                from School s in _unidad.RepositorioSchool._dbset
                    .Include("Users")
                    .Include("Subjects")
                    .Include("SchoolYears")
                    .Include("SchoolYears.Periods")
                    .Include("SchoolYears.Periods.CASUs")
                where s.SchoolId == id
                select s)
                    .FirstOrDefault<School>();

            return colegio;
        }
        public School ObtenerColegioEntidadUnica(int id)
        {
            return _unidad.RepositorioSchool.GetById(id);
        }
        
        public IQueryable<School> ObtenerListaColegios()
        {
            IQueryable<School> listaColegios = _unidad.RepositorioSchool.GetAll();

            return listaColegios.OrderBy(m => m.Name);
        }

        
        #endregion
        #region Otros métodos
        public bool ColegioPoseeAnoEscolarActivo(int idColegio)
        {
            School colegio = (from School s in _unidad.RepositorioSchool._dbset.Include("SchoolYears")
                              where s.SchoolId == idColegio
                              select s).First<School>();

            foreach (SchoolYear schoolYear in colegio.SchoolYears)
            {
                if (schoolYear.Status)
                    return true;
            }

            return false;
        }
        public bool HabilitarColegio(int idColegio)
        {
            School colegio = this.ObtenerColegioPorId(idColegio);
            colegio.Status = true;

            try
            {
                _unidad.RepositorioSchool.Modify(colegio);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool HabilitarColegio(School colegio)
        {
            colegio.Status = true;

            try
            {
                _unidad.RepositorioSchool.Modify(colegio);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool SuspenderColegio(int idColegio)
        {
            School colegio = this.ObtenerColegioPorId(idColegio);
            colegio.Status = false;

            try
            {
                _unidad.RepositorioSchool.Modify(colegio);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool SuspenderColegio(School colegio)
        {
            colegio.Status = false;

            try
            {
                _unidad.RepositorioSchool.Modify(colegio);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}