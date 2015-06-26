using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Tesis_ClienteWeb_Data;
using Tesis_ClienteWeb_Models.POCO;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb.App_Start;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Tesis_ClienteWeb_Data.Repositories;

namespace Tesis_ClienteWeb.Controllers
{
    [Authorize]
    public class LoginController : MaestraController
    {
        #region Variables Identity
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        #endregion

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (ValidandoSesionActiva(true)) //Validando que no haya una sesión previamente iniciada
                return RedirectToAction("Inicio", "Index");
            else
            {
                LoginModel model = new LoginModel();
                return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginModel model)
        {
            #region Inicialización de variables
            UserService userService = new UserService(true); 
            RoleService roleService = new RoleService(true);

            SignInStatus conexion;
            int nroIntentos;
            User usuario;
            #endregion

            /* Validación que se utilizará para definir si se validará el usuario a insertar o no. 
             *      1. ConstantService.PRODUCTION_ENVIRONMENT = true. Se valida.
             *      2. ConstantService.PRODUCTION_ENVIRONMENT = false. No se valida.
             */
            #region Ambiente de producción
            if (ConstanteProduccion())
            {
                #region Validando el modelo
                if (!ModelState.IsValid)
                {
                    model.MostrarErrores = "block";
                    return View(model);
                }
                #endregion
                #region Verificando nombre de usuario
                usuario = userService.ObtenerUsuarioPorUsername(model.Username);
                if (usuario == null)
                {
                    ModelState.AddModelError("", "Nombre de usuario incorrecto.");
                    model.MostrarErrores = "block";

                    return View(model);
                }
                #endregion
                #region Verificando usuario bloqueado
                if(usuario.LockoutEnabled)
                {
                    ModelState.AddModelError("", "Este usuario está bloqueado, por favor contacte al" + 
                        " administrador del sistema.");
                    model.MostrarErrores = "block";

                    return View(model);
                }
                #endregion
                #region Verificando # de intentos
                nroIntentos = ConstantRepository.ACCESS_FAILED_COUNT - usuario.AccessFailedCount;
                if (nroIntentos == 0)
                {
                    await UserManager.SetLockoutEnabledAsync(usuario.Id, true);
                    ModelState.AddModelError("", "El usuario con el que intenta acceder está bloqueado." +
                        " Por favor contacte al administrador del sistema.");
                    model.MostrarErrores = "block";

                    return View(model);
                }
                #endregion
                #region Validando contraseña e iniciando sesión
                /* Notas (04-01-15)
                 * isPersistent: es para definir la implementación del 'Remember Me'.
                 * shouldLockout: define si se le suma un contador al intento de conexión si falla la contraseña.
                 */
                conexion = await SignInManager.PasswordSignInAsync(model.Username, model.Password, 
                    isPersistent: false, shouldLockout: true);

                switch (conexion)
                {
                    case SignInStatus.Success:
                        {
                            await UserManager.ResetAccessFailedCountAsync(usuario.Id);
                            InicializarSesion(usuario);

                            if (Session["RoleName"].ToString().Equals("Administrador"))
                                return RedirectToAction("Menu", "Administrador");
                            else
                                return RedirectToAction("Inicio", "Index");
                        }
                    case SignInStatus.Failure:
                    default:
                        {
                            ModelState.AddModelError("", "Contraseña incorrecta. Posee " + nroIntentos + " intentos más.");
                            model.MostrarErrores = "block";

                            return View(model);
                        }
                }
                #endregion
            }
            #endregion
            #region Ambiente de desarrollo
            else
            {                
                #region Obteniendo rol administrador desarrollo
                Role rol = roleService.ObtenerRolPorNombre(ConstantRepository.DEVELOPMENT_ROLE_NAME);
                #endregion
                #region Validación: Rol administrador desarrollo no existe
                if (rol == null)
                {
                    #region Creando rol administrador desarrollo
                    rol = new Role()
                    {
                        Name = ConstantRepository.DEVELOPMENT_ROLE_NAME,
                        Description = ConstantRepository.DEVELOPMENT_ROLE_DESCRIPTION,
                    };

                    roleService.GuardarRol(rol);
                    #endregion
                    #region Creando usuario 'Desarrollo'
                    usuario = new User()
                    {
                        Email = ConstantRepository.DEVELOPMENT_USER_EMAIL,
                        UserName = ConstantRepository.DEVELOPMENT_USER_USERNAME
                    };

                    await UserManager.CreateAsync(usuario, ConstantRepository.DEVELOPMENT_USER_PASSWORD);
                    UserManager.AddToRole(usuario.Id, rol.Name);
                    UserManager.SetLockoutEnabled(usuario.Id, false);
                    #endregion
                }
                #endregion
                #region Validación: Rol administrador desarrollo si existe
                else
                {
                    #region Obteniendo el usuario 'Desarrollo'
                    usuario = await UserManager.FindByNameAsync(ConstantRepository.DEVELOPMENT_USER_USERNAME);
                    if(usuario == null)
                    {
                        usuario = new User()
                        {
                            Email = ConstantRepository.DEVELOPMENT_USER_EMAIL,
                            UserName = ConstantRepository.DEVELOPMENT_USER_USERNAME
                        };

                        await UserManager.CreateAsync(usuario, ConstantRepository.DEVELOPMENT_USER_PASSWORD);
                        UserManager.AddToRole(usuario.Id, rol.Name);
                        UserManager.SetLockoutEnabled(usuario.Id, false);
                    }
                    #endregion
                }
                #endregion
                #region Inicializando sesión
                await SignInManager.PasswordSignInAsync(usuario.UserName, 
                    ConstantRepository.DEVELOPMENT_USER_PASSWORD, isPersistent: false, shouldLockout: true);

                InicializarSesion(usuario);
                #endregion

                return RedirectToAction("Inicio", "Index");
            }
            #endregion
        }

        private void InicializarSesion(User usuario)
        {            
            SchoolYearService _schoolYearService = new SchoolYearService(true);
            RoleService _roleService = new RoleService(true);

            SchoolYear schoolYear;
            Role rol = _roleService.ObtenerRolPorId(usuario.Roles.First().RoleId);

            Session["UserId"] = usuario.Id;
            Session["Username"] = usuario.UserName;
            Session["RoleId"] = rol.Id;
            Session["RoleName"] = rol.Name;
            Session["Administrador"] = (rol.Name.Equals("Administrador") ||
                                        rol.Name.Equals("Administrador Desarrollo") ? true : false); ;
            Session["Coordinador"] = (rol.Name.Equals("Coordinador") ? true : false);
            
            if ((bool)Session["Administrador"])
            {
                    schoolYear = _schoolYearService
                        .ObtenerAnoEscolarActivoPorColegio(ConstantRepository.ADMINISTRATOR_FIRST_SCHOOL);

                    Session["SchoolId"] = ConstantRepository.ADMINISTRATOR_FIRST_SCHOOL;
                    Session["SchoolYearId"] = schoolYear.SchoolYearId;
                    Session["StartDate"] = schoolYear.StartDate;
                    Session["DateOfCompletion"] = schoolYear.EndDate;
            }
            else
            {
                schoolYear = _schoolYearService.ObtenerAnoEscolarActivoPorColegio(usuario.School.SchoolId);

                Session["SchoolId"] = usuario.School.SchoolId;
                Session["SchoolYearId"] = schoolYear.SchoolYearId;
                Session["StartDate"] = schoolYear.StartDate;
                Session["DateOfCompletion"] = schoolYear.EndDate;
            }                        
        }
    }
}
