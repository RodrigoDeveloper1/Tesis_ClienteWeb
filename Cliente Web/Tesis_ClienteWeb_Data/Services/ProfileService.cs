using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class ProfileService
    {        
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;

        public ProfileService()
        {
            this._unidad = new UnitOfWork();
        }
        public ProfileService(UnitOfWork unidad)
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

        #region CRUD
        public bool EliminarPerfil(Profile perfil)
        {
            try
            {
                _unidad.RepositorioProfile.Delete(perfil);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void EliminarPerfilPorId(int id)
        {
            try
            {
                _unidad.RepositorioProfile.Delete(m => m.ProfileId == id);
                _unidad.Save();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool GuardarPerfil(Profile perfil)
        {
            if(PerfilDuplicado(perfil.Name))
                return false;

            try
            {
                _unidad.RepositorioProfile.Add(perfil);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool ModificarPerfil(Profile perfil)
        {
            try
            {
                _unidad.RepositorioProfile.Modify(perfil);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #region Obtener Perfiles
        public Profile ObtenerPerfilPorId(int id)
        {
            Profile perfil = _unidad.RepositorioProfile.GetById(id);

            return perfil;
        }
        public Profile ObtenerPerfilPorIdConRoles(int id)
        {
            Profile perfil = (from Profile p in _unidad.RepositorioProfile._dbset.Include("Roles")
                              where p.ProfileId == id
                              select p).FirstOrDefault<Profile>();

            return perfil;
        }
        public ICollection<Profile> ObtenerListaPerfiles()
        {
            try
            {
                return _unidad.RepositorioProfile.GetAll().OrderBy(m => m.Name).ToList<Profile>();
            }
            catch (InvalidOperationException e)
            {                
                throw e;
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IQueryable<Profile> ObtenerListaPerfilesPorRol(Role rol)
        {
            return null;
        }            
        #endregion
        #region Otros métodos
        public bool PerfilDuplicado(string nombre)
        {
            int contador = 0;
            try 
            {
                contador = _unidad.RepositorioProfile.GetAll().Where(m => m.Name == nombre).Count();

                if (contador == 0)
                    return false;
                else
                    return true;
            }
            catch(InvalidOperationException e)
            {
                //Carga inicial del proyecto
                //return false; //Se editó para arreglar warnings del proyecto - Rodrigo Uzcátegui 02-03-15
                throw e;
            }
        }
        public void EliminarRoldePerfil(int idPerfil, string idRol)
        {
            Profile perfil = this.ObtenerPerfilPorIdConRoles(idPerfil);

            Role rol = perfil.Roles.Where(r => r.Id == idRol).FirstOrDefault<Role>();

            perfil.Roles.Remove(rol);

            _unidad.RepositorioProfile.Modify(perfil);

            _unidad.Save();
        }
        #endregion
    }
}