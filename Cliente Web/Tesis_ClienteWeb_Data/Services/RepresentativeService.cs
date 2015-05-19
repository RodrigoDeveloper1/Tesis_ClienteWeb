using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class RepresentativeService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;

        public RepresentativeService()
        {
            this._unidad = new UnitOfWork();
            _InicializadorVariablesSesion();
        }
        public RepresentativeService(UnitOfWork unidad)
        {
            this._unidad = unidad;
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
        public bool GuardarRepresentante(Representative representante)
        {
            _unidad.RepositorioRepresentative.Add(representante);

            try
            {
                _unidad.Save();
                return true;
            }
            catch (Exception e)
            {
                //return false; Se editó para arreglar warnings - Rodrigo Uzcátegui - 03-02-15
                throw e;
            }
        }
        public bool EliminarRepresentante(int representativeID)
        {
            _unidad = new UnitOfWork();
            var aux = _unidad.RepositorioRepresentative.GetAll().Where(u => u.RepresentativeId ==
                                                                       representativeID).Count();
            if (aux != 0)
            {
                _unidad.RepositorioRepresentative.Delete(u => u.RepresentativeId == representativeID);
                _unidad.Save();
                return true;
            }
            return false;
        }
        public bool ModificarRepresentante(Representative representante, int id)
        {
            _unidad = new UnitOfWork();
            try
            {
                _unidad.RepositorioRepresentative.Update(representante, id);
                _unidad.Save();
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// Método CRUD. Modificar representante.
        /// Rodrigo Uzcátegui - 11-04-15
        /// </summary>
        /// <param name="representante">El objeto representante</param>
        /// <returns>True = si se modificó. </returns>
        public bool ModificarRepresentante(Representative representante)
        {
            try
            {
                this._unidad.RepositorioRepresentative.Modify(representante);
                this._unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion
        #region Obtener representantes
        public Representative ObtenerRepresentantePorId(int id)
        {
            _unidad = new UnitOfWork();
            Representative representanteseleccionado = _unidad.RepositorioRepresentative.GetById(id);
            return representanteseleccionado;

        }

        public IQueryable<Representative> ObtenerListaRepresentantes()
        {
            _unidad = new UnitOfWork();
            return _unidad.RepositorioRepresentative.GetAll();
        }
        
        public List<Representative> ObtenerListaRepresentantesPorAlumno(int idEstudiante)
        {
            Student alumno = new StudentService().ObtenerAlumnoPorId(idEstudiante);

            List<Representative> listaRepresentantes = alumno.Representatives;


            return listaRepresentantes;
        }
        public List<Representative> ObtenerListaRepresentantesPorCurso(int idCurso)
        {
            List<Representative> lista = new List<Representative>();
            List<Student> listaEstudiantes = new List<Student>();

            listaEstudiantes = (from CASU casu in _unidad.RepositorioCASU._dbset
                                    .Include("Course.Students.Representatives.Students")
                                where casu.Course.CourseId == idCurso
                                select casu.Course.Students)
                                    .FirstOrDefault()
                                    .OrderBy(m => m.NumberList)
                                    .ToList<Student>();

            foreach(Student student in listaEstudiantes)
            {
                foreach(Representative representative in student.Representatives)
                {
                    lista.Add(representative);
                }
            }

            return lista;
        }
        #endregion
        #region Otros métodos
        #endregion                
    }
}