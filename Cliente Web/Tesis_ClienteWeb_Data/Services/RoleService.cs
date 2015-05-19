using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class RoleService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;

        public RoleService()
        {
            this._unidad = new UnitOfWork();
            _InicializadorVariablesSesion();
        }
        public RoleService(UnitOfWork unidad)
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
        public RoleService(bool inicializacionSesion)
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
        public bool GuardarRol(Role rol)
        {
            if (RolDuplicado(rol.Name))
                return false;

            try
            {
                foreach (Profile perfil in rol.Profiles)
                {
                    _unidad.RepositorioProfile.Modify(perfil);
                }

                _unidad.RepositorioRole.Add(rol);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Método CRUD. Modifica un rol
        /// </summary>
        /// <param name="rol">El rol a modificar</param>
        /// <returns>True = modificado correcto</returns>
        public bool ModificarRol(Role rol)
        {
            try
            {
                _unidad.RepositorioRole.Modify(rol);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool EliminarRolPorId(string idRol)
        {
            try
            {
                Role rol = this.ObtenerRolPorId(idRol);
                _unidad.RepositorioRole.Delete(rol);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region Obtener Roles
        /// <summary>
        /// Método que obtiene el rol según el id.
        /// </summary>
        /// <param name="id">El id del rol</param>
        /// <returns>El rol en cuestión</returns>
        public Role ObtenerRolPorId(string id)
        {
            Role role = (from Role r in _unidad.RepositorioRole._dbset
                             .Include("Profiles")
                          where r.Id == id
                          select r).FirstOrDefault<Role>();

            return role;
        }

        public Role ObtenerRolPorNombre(string nombre)
        {
            Role role = (from Role r in _unidad.RepositorioRole._dbset.Include("Profiles")
                         where r.Name == nombre
                         select r).FirstOrDefault<Role>();

            return role;
        }
        public IQueryable<Role> ObtenerListaRoles()
        {
            IQueryable<Role> listaRoles = _unidad.RepositorioRole.GetAll();

            foreach (Role rol in listaRoles)
            {
                var query = (from Rol in _unidad.RepositorioRole.GetAll()
                             where Rol.Id == rol.Id
                             select new { Perfiles = Rol.Profiles }).FirstOrDefault();

                rol.Profiles = query.Perfiles;
            }

            return listaRoles.OrderBy(m => m.Name);
        }
        public ICollection<Role> ObtenerListaRolesPorPerfil(Profile perfil)
        {
            var query = (from Perfil in _unidad.RepositorioProfile.GetAll()
                         where Perfil.ProfileId == perfil.ProfileId
                         select new { Roles = Perfil.Roles }).FirstOrDefault();

            ICollection<Role> listaRoles = query.Roles;

            return listaRoles;
        }
        #endregion
        #region Otros métodos
        public bool RolDuplicado(string nombre)
        {
            int contador = 0;
            contador = _unidad.RepositorioRole.GetAll().Where(m => m.Name == nombre).Count();

            if (contador == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Método que evalúa, según el nombre del rol pasado por parámetro, las acciones que están permitidas
        /// que realice, en el menú de acciones maestras, representado en un diccionario que asocia la constante
        /// anexada a la acción en particular, con un booleano que indica si tiene permiso o no de realizar esa
        /// acción.
        /// Rodrigo Uzcátegui - 11-05-15
        /// </summary>
        /// <param name="roleName">El nombre del rol.</param>
        /// <returns>El diccionario que asocia la lista de acciones con el booleano de permisología.</returns>
        public ReadOnlyDictionary<int, bool> ObtenerAccesoAccionesMaestras(string roleName)
        {
            ReadOnlyDictionary<int, bool> diccionario = new ReadOnlyDictionary<int, bool>(new Dictionary<int, bool>());

            if (roleName.Equals(ConstantRepository.ADMINISTRATOR_ROLE))
                diccionario = ConstantRepository.ROLE_ACCIONES_MAESTRAS_ADMINISTRADOR;
            else if (roleName.Equals(ConstantRepository.COORDINATOR_ROLE))
                diccionario = ConstantRepository.ROLE_ACCIONES_MAESTRAS_COORDINADOR;

            return diccionario;
        }
        #endregion
    }
}