using System;
using System.Collections.Generic;
using System.Linq;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class UserService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;
        
        public UserService()
        {
            this._unidad = new UnitOfWork();
            _InicializadorVariablesSesion();
        }
        public UserService(UnitOfWork unidad)
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
        public UserService(bool inicializacionSesion)
        {
            this._unidad = new UnitOfWork();

            if (!inicializacionSesion)
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
        /// <summary>
        /// Método CRUD. Modifica el usuario.
        /// 11-04-15
        /// </summary>
        /// <param name="user">El usuario a modificar</param>
        /// <returns>True = Se modificó correctamente.</returns>
        public bool ModificarUser(User user)
        {
            try
            {
                _unidad.RepositorioUser.Modify(user);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion
        #region Obtener usuarios
        /// <summary>
        /// Método que obtiene el usuario que está conectado en la sesión.
        /// </summary>
        /// <returns>El usuario de la sesión</returns>
        public User ObtenerUsuarioDeSesion()
        {
            User user = this.ObtenerUsuarioPorId(_session.USERID);

            return user;
        }
        
        
        
        /// <summary>
        /// Método que obtiene la lista de docentes asociado al coordinador.
        /// </summary>
        /// <returns>La lista de docentes</returns>
        public List<User> ObtenerListaDocentesAsociadosAlCoordinador()
        {
            List<User> listaDocentes = null;

            if (_session.COORDINADOR)
            {
                listaDocentes = (from CASU casu in _unidad.RepositorioCASU._dbset
                                 where casu.Period.SchoolYear.School.SchoolId == _session.SCHOOLID &&
                                       casu.Period.SchoolYear.SchoolYearId == _session.SCHOOLYEARID &&
                                       casu.Teacher.Id != _session.USERID //Para que no traiga al usuario conectado
                                 select casu.Teacher)
                                    .OrderBy(m => m.UserName)
                                    .ToHashSet<User>()
                                    .ToList<User>();
            }

            return listaDocentes;
        }        
        /// <summary>
        /// Método que obtiene la lista de docentes (coordinadores y profesores) del colegio.
        /// Rodrigo Uzcátegui - 03-04-15
        /// </summary>
        /// <param name="idColegio">El id del colegio</param>
        /// <returns>La lista de docentes respectiva</returns>
        public List<User> ObtenerListaDocentesPor_Colegio(int idColegio)
        {
            #region Declaración de variables
            Role rol;
            RoleService roleService = new RoleService(this._unidad);
            List<User> listaUsuarios = new List<User>();
            #endregion

            List<User> lista = (
                from User docente in _unidad.RepositorioUser._dbset
                    .Include("School")
                where docente.School.SchoolId == idColegio
                select docente)
                    .OrderBy(m => m.LastName)
                    .ToList<User>();

            foreach(User docente in lista)
            {
                rol = roleService.ObtenerRolPorId(docente.Roles.First().RoleId);
                if (rol.Name.Equals(ConstantRepository.TEACHER_ROLE) ||
                   rol.Name.Equals(ConstantRepository.COORDINATOR_ROLE))
                    listaUsuarios.Add(docente);
            }

            return listaUsuarios;
        }        
        /// <summary>
        /// Método que obtiene la lista de docentes del salón (casu) según los datos pasados por parámetro
        /// Rodrigo Uzcátegui - 01-04-15
        /// </summary>
        /// <param name="idMateria">Id de la materia</param>
        /// <param name="idCurso">Id del curso</param>
        /// <param name="idPeriodo">Id del período</param>
        /// <returns>La lista de docentes</returns>
        public List<User> ObtenerListaDocentesPor_Materia_Curso_Periodo(int idMateria, int idCurso, int idPeriodo)
        {
            List<User> lista = (
                from CASU classroom in _unidad.RepositorioCASU._dbset
                    .Include("Teacher")
                where classroom.CourseId == idCurso &&
                      classroom.SubjectId == idMateria &&
                      classroom.PeriodId == idPeriodo
                select classroom.Teacher)
                    .OrderBy(m => m.LastName)
                    .ToList<User>();

            return lista;
        }
        /// <summary>
        /// Método que obtiene la lista de usuarios según el id del colegio.
        /// Rodrigo Uzcátegui - 15-05-15
        /// </summary>
        /// <param name="idColegio">El id del colegio</param>
        /// <returns>La lista de usuarios respectiva</returns>
        public List<User> ObtenerListaUsuariosPor_Colegio(int idColegio)
        {
            List<User> listaUsuarios = (
                from School colegio in _unidad.RepositorioSchool._dbset
                    .Include("Users")
                where colegio.SchoolId == idColegio
                select colegio.Users)
                    .FirstOrDefault()
                    .OrderBy(m => m.UserName)
                    .ToList<User>();

            return listaUsuarios;
        }
        


        public User ObtenerUsuarioPorId(string id)
        {
            User usuario = (from User u in _unidad.RepositorioUser._dbset
                                .Include("School")
                                .Include("Events")
                                .Include("CASUs.Course")
                                .Include("CASUs.Subject")
                            where u.Id == id
                            select u).FirstOrDefault<User>();

            return usuario;
        }
        public User ObtenerUsuarioPorUsername(string username)
        {
            try
            {
                User usuario = (from User s in _unidad.RepositorioUser._dbset.Include("School")
                                where s.UserName == username
                                select s).FirstOrDefault<User>();

                return usuario;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Equals("Sequence contains no elements"))
                    return null; //Para cargas iniciales.
                else
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IQueryable<User> ObtenerListaUsuarios()
        {
            IQueryable<User> listaUsuarios = (from u in _unidad.RepositorioUser._dbset.Include("Roles")
                                              select u);

            return listaUsuarios.OrderBy(m => m.UserName);
        }
        public IQueryable<User> ObtenerListaUsuariosHabilitados()
        {
            IQueryable<User> listaUsuarios = _unidad.RepositorioUser
                .GetAll()
                .Where(m => m.LockoutEnabled == false)
                .OrderBy(m => m.UserName); ;

            return listaUsuarios;
        }
        public IQueryable<User> ObtenerListaUsuariosBloqueados()
        {
            IQueryable<User> listaUsuarios = _unidad.RepositorioUser
                .GetAll()
                .Where(m => m.LockoutEnabled == true)
                .OrderBy(m => m.UserName);

            return listaUsuarios;
        }
        #endregion
        #region Otros métodos                
        public void AsociarUsuarioAlColegio(string idUsuario, int idColegio)
        {
            try
            {
                SchoolService servicioColegio = new SchoolService();
                //School colegio = servicioColegio.ObtenerColegioPorId(idColegio);
                School colegio = servicioColegio.ObtenerColegioEntidadUnica(idColegio);
                _unidad.RepositorioSchool.Modify(colegio);

                User usuario = this.ObtenerUsuarioPorId(idUsuario);

                usuario.School = colegio;
                _unidad.RepositorioUser.Modify(usuario);
                _unidad.Save();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool AsociarEventoAUser(User usuario, string iduser)
        {


            try
            {
                foreach (Event evento in usuario.Events)
                {
                    _unidad.RepositorioEvent.Modify(evento);
                }

                _unidad.RepositorioUser.Update(usuario, iduser);
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