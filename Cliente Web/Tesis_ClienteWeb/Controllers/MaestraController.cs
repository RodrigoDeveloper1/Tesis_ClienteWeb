using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Data.UserExceptions;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class MaestraController : Controller
    {
        /// <summary>
        /// Variable que establece las variables de sesión definidas.
        /// </summary>
        protected SessionVariablesRepository _session;

        /// <summary>
        /// Método que encapsula los métodos de ValidandoSesionActiva y ValidandoPrivilegios en una sola
        /// llamada para ser más fácil su mantenimiento.
        /// </summary>
        /// <param name="controlador">Nombre del controlador asociado a la vista actual.</param>
        /// <param name="accion">Nombre del método al que se está ingresando dentro del controlador de la vista
        /// actual.</param>
        public void ConfiguracionInicial(string controlador, string accion)
        {
            ObteniendoSesion();
            ValidandoSesionActiva();
            ValidandoPrivilegios(controlador, accion);
        }

        /// <summary>
        /// Método que inicializa la variable de sesión que encapsula todas las variables de sesión inicializadas
        /// en el proyecto.
        /// Rodrigo Uzcátegui - 27-04-15
        /// </summary>
        protected void ObteniendoSesion()
        {
            try
            {
                _session = new SessionVariablesRepository();
            }
            catch (SessionExpiredException)
            {
                Server.ClearError();
                Response.Redirect("/Errores/SessionExpired");
            }
        }
        /// <summary>
        /// Método que valida si la sesión del usuario está activa o no
        /// </summary>
        protected void ValidandoSesionActiva()
        {
            try
            {
                if (HttpContext.Session.IsNewSession)
                    throw new SessionExpiredException();
            }
            catch (SessionExpiredException e)
            {
                Server.ClearError();
                throw e; //Response.Redirect("/Errores/SessionExpired");
            }
        }
        /// <summary>
        /// Método que valida si la sesión del usuario está activa o no. 
        /// Rodrigo Uzcátegui - 15/05/15
        /// </summary>
        /// <param name="login">Booleano que determina si se está realizando la validación desde la vista
        /// de login.</param>
        /// <returns>Booleano True = Si se está desde login y si hay sesión activa, False = No se está desde 
        /// login o no hay sesión activa.</returns>
        protected bool ValidandoSesionActiva(bool login)
        {
            if (!HttpContext.Session.IsNewSession && 
                 HttpContext.Session["UserId"] != null && 
                 login)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Método que valida si el usuario de la sesión tiene los privilegios requeridos para realizar cierta
        /// acción
        /// </summary>
        /// <param name="controlador">El controlador que guarda esa acción</param>
        /// <param name="accion">El nombre de la acción</param>
        protected void ValidandoPrivilegios(string controlador, string accion)
        {
            if (ConstantePrivilegios())
            {
                bool permitido = false;

                //if (!administrador) //Validación comentada por Rodrigo Uzcátegui - 10-05-15
                //{
                Role role = new RoleService().ObtenerRolPorId(_session.ROLEID);
                foreach (Profile profile in role.Profiles)
                {
                    if (profile.ControllerName == controlador && profile.Action == accion)
                    {
                        permitido = true;
                        break;
                    }
                }

                if (!permitido)
                    Response.Redirect("/Errores/SinPrivilegios"); 
                //}
            }
        }               
                
        /// <summary>
        /// Método que obtiene el valor de la constante de la variable de producción. Se utiliza
        /// para evitar warnings de llamdas de valores que no pueden cambiar (por ser una constante).
        /// Rodrigo Uzcátegui - 02-03-15
        /// </summary>
        /// <returns>True: Estamos en ambiente de producción. False: Estamos en ambiente de desarrollo</returns>
        public bool ConstanteProduccion()
        {
            return ConstantRepository.PRODUCTION_ENVIRONMENT;
        }
        /// <summary>
        /// Método que obtiene el valor de la constante de la variable de validación de perfiles. Se utiliza
        /// para evitar warnings de llamdas de valores que no pueden cambiar (por ser una constante).
        /// Rodrigo Uzcátegui - 03-03-15
        /// </summary>
        /// <returns>True: Si se validan los perfiles. False: No se validan los perfiles</returns>
        public bool ConstantePrivilegios()
        {
            return ConstantRepository.PROFILE_VALIDATION;
        }

        public Period ObtenerPeriodoActualDetalle(DateTime fechaactual,List<Period> listadeperiodos)
        {
            TimeSpan diaslapso1 = listadeperiodos[0].FinishDate.Date.Subtract(listadeperiodos[0].StartDate.Date);
            TimeSpan diaslapso2 = listadeperiodos[1].FinishDate.Date.Subtract(listadeperiodos[1].StartDate.Date);
            TimeSpan diaslapso3 = listadeperiodos[2].FinishDate.Date.Subtract(listadeperiodos[2].StartDate.Date);
          
            if (fechaactual.Date >= listadeperiodos[0].StartDate.Date && 
                fechaactual.Date <= listadeperiodos[0].FinishDate.Date)
                return listadeperiodos[0];
            else
            {
                if (fechaactual.Date >= listadeperiodos[1].StartDate.Date && 
                    fechaactual.Date <= listadeperiodos[1].FinishDate.Date)
                    return listadeperiodos[1];
                else
                {
                    if (fechaactual.Date >= listadeperiodos[2].StartDate.Date && 
                        fechaactual.Date <= listadeperiodos[2].FinishDate.Date)
                        return listadeperiodos[2];
                }
            }

            return null;
        }
    }
}