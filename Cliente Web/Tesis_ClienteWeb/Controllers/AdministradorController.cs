using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.App_Start;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class AdministradorController : MaestraController
    {
        private string _controlador = "Administrador";
        private BridgeController _puente = new BridgeController();
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

        #region Menú de Maestras
        [HttpGet]
        public ActionResult Menu()
        {
            ConfiguracionInicial(_controlador, "Menu");
            MenuViewModel model = new MenuViewModel();
            model.diccionarioAcceso = _puente.ObtenerAccesoAccionesMaestras(_session.ROLENAME);

            return View(model);
        }
        #endregion
        #region Gestión de Perfiles
        [HttpGet]
        public ActionResult GestionPerfiles()
        {
            ConfiguracionInicial(_controlador, "GestionPerfiles");
            ProfileViewModel model = new ProfileViewModel();
            #region Sección mensajes TempData
            if (TempData["Message"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["Message"].ToString();
            }
            else if (TempData["Error"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Error"].ToString());
            }
            else if (TempData["ErrorEliminar"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorEliminar"].ToString());
            }
            else if (TempData["AclamacionEliminar"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["AclamacionEliminar"].ToString();
            }
            else if (TempData["ErrorEliminarPorRol"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorEliminarPorRol"].ToString());
            }
            else if (TempData["Ok"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["Ok"].ToString();
            }
            else if (TempData["ModelStateInvalid"] != null)
            {
                model.MostrarErrores = "block";

                for (int contador = 1; contador <= (int)TempData["ContadorModelStateErrors"]; contador++)
                {
                    if (TempData["Error_" + contador].ToString() != "")
                        ModelState.AddModelError("", TempData["Error_" + contador].ToString());
                }
            }
            #endregion
            model.ProfileList = _puente.InicializadorListaPerfiles();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarPerfil (ProfileViewModel model)
        {
            ConfiguracionInicial(_controlador, "AgregarPerfil");
            #region Validación del modelo
            if (!ModelState.IsValid)
            {
                TempData["ModelStateInvalid"] = true;
                TempData["ContadorModelStateErrors"] = ModelState.Count;
                int contador = 1;

                foreach (ModelState error in ModelState.Values)
                {
                    TempData["Error_" + contador] = (error.Errors.Count != 0 ? error.Errors[0].ErrorMessage : "");
                    contador++;
                }

                return RedirectToAction("GestionPerfiles");
            }
            #endregion

            try
            {
                ProfileService _profileService = new ProfileService();
                if(!_profileService.GuardarPerfil(model.profile))
                {
                    TempData["Error"] = "Ya existe un perfil con ese nombre";
                    return RedirectToAction("GestionPerfiles");
                }

                #region Retornando vista con modelo inicializado
                TempData["Ok"] = "Se agregó correctamente el perfil";
                return RedirectToAction("GestionPerfiles");
                #endregion
            }
            catch (Exception)
            {
                TempData["Error"] = "Ya existe un perfil con ese nombre";
                return RedirectToAction("GestionPerfiles");
            }
        }

        [HttpGet]
        public ActionResult EditarPerfil(int id)
        {
            ConfiguracionInicial(_controlador, "EditarPerfil");
            #region Inicializando variables
            ProfileViewModel model = new ProfileViewModel();
            ProfileService _profileService = new ProfileService();
            #endregion
            #region Sección TempData
            if (TempData["ModelError"] != null)
            {
                model.MostrarErrores = "block";
                foreach (ModelState error in (List<ModelState>)TempData["ModelError"])
                {
                    if (error.Errors.Count != 0)
                        ModelState.AddModelError("", error.Errors[0].ErrorMessage);
                }
            }
            #endregion
            model.profile = _profileService.ObtenerPerfilPorId(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPerfil(ProfileViewModel model)
        {            
            #region Validando el modelo
            if (!ModelState.IsValid)
            {
                TempData["ModelError"] = ModelState.Select(m => m.Value).ToList();
                return RedirectToAction("EditarPerfil", _controlador, model.profile.ProfileId);
            }
            #endregion
            #region Declaración de variables
            ProfileService _profileService = new ProfileService();
            #endregion
            #region Modificando el perfil
            try
            {
                if (!_profileService.ModificarPerfil(model.profile))
                {
                    TempData["Error"] = "Ya existe un perfil con ese nombre";
                    return RedirectToAction("EditarPerfil", _controlador, model.profile.ProfileId);
                }
                TempData["Message"] = "Se editó correctamente el perfil '" + model.profile.Name + "'";
                return RedirectToAction("GestionPerfiles");
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("EditarPerfil", _controlador, model.profile.ProfileId);
            }
            #endregion
        }

        [HttpGet]
        public void EliminarPerfil(int id)
        {
            ConfiguracionInicial(_controlador, "EliminarPerfil");
            #region Inicializando variables
            ProfileViewModel model = new ProfileViewModel();
            ProfileService _profileService = new ProfileService();
            Profile perfil = new Profile();
            #endregion
            #region Realizando la acción
            try
            {
                perfil = _profileService.ObtenerPerfilPorIdConRoles(id);

                if (perfil.Roles.Count != 0)
                    TempData["ErrorEliminarPorRol"] = "Este perfil tiene roles asociados, no se puede eliminar";
                else
                {
                    _profileService.EliminarPerfil(perfil);
                    TempData["AclamacionEliminar"] = "Se eliminó correctamente el perfil";
                }
            }
            catch (Exception e)
            {
                TempData["ErrorEliminar"] = e.Message;
            }
            #endregion
        }
        #endregion
        #region Gestión de Roles
        [HttpGet]
        public ActionResult AgregarRol()
        {
            ConfiguracionInicial(_controlador, "AgregarRol");
            AddRoleViewModel model = new AddRoleViewModel();
            #region Sección TempData
            if(TempData["Error"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Error"].ToString());
            }
            else if (TempData["Ok"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["Ok"].ToString();
            }
            else if (TempData["ModelStateInvalid"] != null)
            {
                model.MostrarErrores = "block";

                for (int contador = 1; contador <= (int)TempData["ContadorModelStateErrors"]; contador++)
                {
                    if (TempData["Error_" + contador].ToString() != "")
                        ModelState.AddModelError("", TempData["Error_" + contador].ToString());
                }
            }
            #endregion
            model.PersonalProfileList = _puente.InicializadorListaPerfilesParaRol();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarRol(AddRoleViewModel model, List<string> checkboxList)
        {
            #region Validando el modelo
            if (!ModelState.IsValid)
            {
                TempData["ModelStateInvalid"] = true;
                TempData["ContadorModelStateErrors"] = ModelState.Count;
                int contador = 1;

                foreach (ModelState error in ModelState.Values)
                {
                    TempData["Error_" + contador] = (error.Errors.Count != 0 ? error.Errors[0].ErrorMessage : "");
                    contador++;
                }

                return RedirectToAction("AgregarRol");
            }
            #endregion
            #region Inicializando variables
            ProfileService _profileService = new ProfileService();
            RoleService _roleService = new RoleService();
            List<Profile> listaPerfiles = new List<Profile>();            
            #endregion
            #region Validando número de perfiles
            if(checkboxList == null)
            {
                TempData["Error"] = "Se debe seleccionar al menos un perfil";
                return RedirectToAction("AgregarRol");
            }
            #endregion
            #region Obteniendo perfiles seleccionados
            foreach (string idPerfil in checkboxList)
            {
                try 
                {
                    listaPerfiles.Add(_profileService.ObtenerPerfilPorId(Convert.ToInt32(idPerfil)));
                }
                catch(Exception e)
                {
                    TempData["Error"] = e.Message;
                    return RedirectToAction("AgregarRol");
                }
            }
            #endregion
            #region Inicializando nuevo rol
            Role role = new Role 
            { 
                Description = model.Description, 
                Name = model.RolName,
                Profiles = listaPerfiles
            };
            #endregion
            #region Guardando el nuevo rol
            try
            {
                if (!_roleService.GuardarRol(role))
                {
                    TempData["Error"] = "Ya existe un rol con ese nombre";
                    return RedirectToAction("AgregarRol");
                }

                TempData["Ok"] = "Se agregó correctamente el rol '" + role.Name + "'";
                return RedirectToAction("AgregarRol");
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("AgregarRol");
            }
            #endregion
        }

        [HttpGet]
        public ActionResult ListarRoles()
        {
            ConfiguracionInicial(_controlador, "ListarRoles");
            RoleListViewModel model = new RoleListViewModel();

            #region Sección mensajes TempData
            if (TempData["Message"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["Message"].ToString();
            }
            else if (TempData["ErrorEliminar"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorEliminar"].ToString());
            }
            else if (TempData["AclamacionEliminar"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["AclamacionEliminar"].ToString();
            }
            #endregion

            try
            {
                model.roleList = _puente.InicializadorListaRoles();
            }
            catch (Exception e)
            {
                model.roleList = new List<Role>();
                model.MostrarErrores = "block";
                ModelState.AddModelError("", "Ocurrió un error al intentar generar la lista de roles");
                ModelState.AddModelError("", e.Message);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditarRol(string id)
        {
            ConfiguracionInicial(_controlador, "EditarRol");
            #region Inicializando variables
            AddRoleViewModel model = new AddRoleViewModel();
            RoleService _roleService = new RoleService();
            Role role = new Role();
            #endregion
            #region Sección TempData
            if (TempData["Error"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Error"].ToString());
            }
            #endregion
            #region Asignando el rol a editar al modelo
            role = _roleService.ObtenerRolPorId(id);
            model.Description = role.Description;
            model.RolName = role.Name;
            model.RolId = role.Id;
            #endregion
            #region Obteniendo lista perfiles para modelo
            model.PersonalProfileList = _puente.InicializadorListaPerfilesParaRol();

            foreach (AddRoleViewModel.PersonalProfile perfilGenerico in model.PersonalProfileList)
            {
                if (role.Profiles.Where(m => m.ProfileId == perfilGenerico.profile.ProfileId).Count() != 0)
                    perfilGenerico.isSelected = true;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRol(AddRoleViewModel model, List<string> checkboxList)
        {
            //Si entra en [HttpGet] EditarRol(string id), no necesita de ConfiguracionInicial();
            #region Inicializando variables
            UnitOfWork unidad = new UnitOfWork();
            RoleService _roleService = new RoleService(unidad);
            ProfileService _profileService = new ProfileService(unidad);
            Role nuevoRol = _roleService.ObtenerRolPorId(model.RolId);
            #endregion
            #region Validando # perfiles seleccionados
            if (checkboxList == null)
            {
                TempData["Error"] = "Se debe seleccionar al menos un perfil";
                return RedirectToAction("EditarRol", _controlador, model.RolId);
            }
            #endregion
            #region Asociando los valores al nuevo rol
            nuevoRol.Name = model.RolName;
            nuevoRol.Description = model.Description;
            #endregion
            #region Borrando los anteriores perfiles
            nuevoRol.Profiles = new List<Profile>();
            #endregion
            #region Obteniendo perfiles seleccionados
            foreach (string idPerfil in checkboxList)
            {
                nuevoRol.Profiles.Add(_profileService.ObtenerPerfilPorId(Convert.ToInt32(idPerfil)));
            }
            #endregion
            #region Guardando el nuevo rol
            _roleService.ModificarRol(nuevoRol);
            TempData["Message"] = "Se editó correctamente el rol '" + nuevoRol.Name + "'";
            #endregion

            return RedirectToAction("ListarRoles", "Administrador");
        }

        [HttpPost]
        public JsonResult EliminarRol(string id)
        {
            ConfiguracionInicial(_controlador, "EliminarRol");
            #region Declaración de variables
            AddRoleViewModel model = new AddRoleViewModel();
            RoleService _roleService = new RoleService();
            Role rol = _roleService.ObtenerRolPorId(id);
            List<object> jsonResult = new List<object>();
            #endregion
            try
            {
                if(rol.Users.Count == 0)
                {
                    _roleService.EliminarRolPorId(id);
                    TempData["AclamacionEliminar"] = "Se eliminó correctamente el rol";
                    jsonResult.Add(new { success = true });
                }
                else
                {
                    TempData["ErrorEliminar"] = "Este rol tiene usuarios asociados, no se puede eliminar.";
                    jsonResult.Add(new { success = false });
                }
            }
            catch (Exception e)
            {
                TempData["ErrorEliminar"] = e.Message;
                jsonResult.Add(new { success = false });
            }

            return Json(jsonResult);
        }
        #endregion
        #region Gestión de Usuarios
        [HttpGet]
        public ActionResult AgregarUsuario()
        {
            ConfiguracionInicial(_controlador, "AgregarUsuario");
            #region Declaración de variables
            RegisterUserViewModel model = new RegisterUserViewModel();
            RoleService _roleService = new RoleService();
            UserService _userService = new UserService();
            SchoolService _schoolService = new SchoolService();
            List<Role> listaRoles = new List<Role>();
            List<User> listaUsuarios = new List<User>();
            List<School> listaColegios = new List<School>();
            School colegioMaster = new School();
            Role rolAux = new Role();
            #endregion
            #region Mensajes TempData
            if (TempData["UsuarioAgregado"] != null)
            {
                model.MensajeAclamacion = TempData["UsuarioAgregado"].ToString();
                model.MostrarAclamaciones = "block";
            }
            else if (TempData["Exception"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Exception"].ToString());
            }
            else if (TempData["ModelStateInvalid"] != null)
            {
                model.MostrarErrores = "block";

                for (int contador = 1; contador <= (int)TempData["ContadorModelStateErrors"]; contador++)
                {
                    if (TempData["Error_" + contador].ToString() != "")
                        ModelState.AddModelError("", TempData["Error_" + contador].ToString());
                }
            }
            else if (TempData["IdentityErrors"] != null)
            {
                model.MostrarErrores = "block";

                IdentityResult result = (IdentityResult)TempData["IdentityErrors"];

                string _errorAux = "";

                foreach (var error in result.Errors)
                {
                    #region Transcribiendo los mensajes de errores
                    if (error.StartsWith("Name"))
                    {
                        _errorAux = "Ya existe ese nombre de usuario, por favor seleccionar otro.";
                        ModelState.AddModelError("", _errorAux);
                    }
                    else if (error.StartsWith("Email"))
                    {
                        _errorAux = "Ya existe este correo, por favor insertar otro.";
                        ModelState.AddModelError("", _errorAux);
                    }
                    else if (error.StartsWith("Passwords must have"))
                    {
                        _errorAux = "La contraseña debe tener al menos una letra minúscula ('a'-'z'), " +
                                    "una mayúscula ('A'-'Z') y un caracter que no sea una letra.";
                        ModelState.AddModelError("", _errorAux);
                    }
                    #endregion

                    else
                        ModelState.AddModelError("", error);
                }
            }
            #endregion
            #region Inicializando SelectList de roles
            listaRoles = _roleService.ObtenerListaRoles().ToList<Role>();
            listaRoles = (listaRoles.Count == 0) ? new List<Role>() : listaRoles;
            model.listaRoles = new SelectList(listaRoles, "Id", "Name");
            #endregion
            #region Inicializando SelectList de colegios
            colegioMaster.SchoolId = -1;
            colegioMaster.Name = "No aplica";

            listaColegios = _schoolService.ObtenerListaColegiosActivos().ToList<School>();
            listaColegios.Insert(0, colegioMaster);
            listaColegios = (listaColegios.Count == 0) ? new List<School>() : listaColegios;
            model.selectListColegios = new SelectList(listaColegios, "SchoolId", "Name");
            #endregion
            #region Inicializando lista de usuarios
            listaUsuarios = _userService.ObtenerListaUsuarios().ToList();
            listaUsuarios = (listaUsuarios.Count == 0) ? new List<User>() : listaUsuarios;
            foreach(User usuario in listaUsuarios)
            {
                RegisterUserViewModel.UsuarioPersonal usuarioPersonal = new RegisterUserViewModel.UsuarioPersonal();
                rolAux = _roleService.ObtenerRolPorId(usuario.Roles.First().RoleId);

                usuarioPersonal.usuario = usuario;
                usuarioPersonal.rol = rolAux;

                model.listaUsuariosPersonales.Add(usuarioPersonal);
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarUsuario(RegisterUserViewModel model)
        {
            //Si entra en [HttpGet] AgregarUsuario(), no necesita de ConfiguracionInicial();
            #region Validando el modelo
            if (!ModelState.IsValid)
            {
                TempData["ModelStateInvalid"] = true;
                TempData["ContadorModelStateErrors"] = ModelState.Count;
                int contador = 1;

                foreach (ModelState error in ModelState.Values)
                {
                    TempData["Error_" + contador] = (error.Errors.Count != 0 ? error.Errors[0].ErrorMessage : "");
                    contador++;
                }

                return RedirectToAction("AgregarUsuario");
            }
            #endregion
            #region Inicializando variables
            UnitOfWork unidad = new UnitOfWork();
            RoleService _roleService = new RoleService(unidad);
            UserService userService = new UserService(unidad);
            SchoolService schoolService = new SchoolService(unidad);

            List<Role> listaRoles = new List<Role>();
            List<User> listaUsuarios = new List<User>();
            List<School> listaColegios = new List<School>();

            Role rolAux = new Role();            
            User user;
            IdentityResult result;
            #endregion
                        
            #region Creando el nuevo usuario
            user = new User();
            user.UserName = model.Username.Replace(" ", string.Empty);
            user.Email = model.Email;
            user.Name = model.nombre;
            user.LastName = model.apellido;
            
            result = await UserManager.CreateAsync(user, model.Password);
            #endregion
            #region Configurando el nuevo usuario
            if (result.Succeeded)
            {
                #region Quitándole su bloqueo
                await UserManager.SetLockoutEnabledAsync(user.Id, false);
                #endregion
                #region Asociando el rol al usuario
                rolAux = _roleService.ObtenerRolPorId(model.rolId);
                UserManager.AddToRole(user.Id, rolAux.Name);
                #endregion
                #region Asignando el colegio al usuario
                if(model.idColegio != -1) //Si la selección no fue "No aplica"
                {
                    try
                    {
                        user = userService.ObtenerUsuarioPorId(user.Id);
                        School colegio = schoolService.ObtenerColegioPorId(model.idColegio);
                        user.School = colegio;

                        userService.ModificarUser(user);
                    }
                    catch (Exception e)
                    {
                        TempData["Exception"] = e.Message;

                        return RedirectToAction("AgregarUsuario");
                    }
                }                
                #endregion
                #region Retornando la vista
                TempData["UsuarioAgregado"] = "Se agregó correctamente el usuario '" + user.UserName + "'";
                return RedirectToAction("AgregarUsuario");
                #endregion
            }
            #endregion            
            #region Sección: creación de usuario fallido
            TempData["IdentityErrors"] = result;
            return RedirectToAction("AgregarUsuario");
            #endregion
        }

        [HttpGet]
        public ActionResult DesbloquearUsuario(UserListViewModel model)
        {
            ConfiguracionInicial(_controlador, "DesbloquearUsuario");
            #region Declaración de variables
            List<User> listaUsuariosBloqueados = new List<User>();
            List<User> listaUsuariosHabilitados = new List<User>();
            UserService _userService = new UserService();
            #endregion
            #region Mensajes TempData
            if(TempData["BloqueadoCorrecto"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["BloqueadoCorrecto"].ToString();
            }
            else if (TempData["DesbloqueadoCorrecto"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["DesbloqueadoCorrecto"].ToString();
            }
            else if (TempData["Error"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Error"].ToString());
            }

            #endregion
            #region Inicializando listas de usuarios
            model.listaUsuariosBloqueados = _userService.ObtenerListaUsuariosBloqueados().ToList();

            foreach (User usuario in model.listaUsuariosBloqueados)
            {
                usuario.LockoutEnabled = false;
            }

            model.listaUsuariosHabilitados = _userService.ObtenerListaUsuariosHabilitados().ToList();
            #endregion

            return View(model);
        }

        [HttpPost]
        public bool BloquearUsuario(List<string> values)
        {
            ConfiguracionInicial(_controlador, "BloquearUsuario");
            #region Validando el modelo
            if(values == null)
            {
                TempData["Error"] = "No ha seleccionado ningún usuario para bloquear.";
            }
            #endregion
            #region Bloqueando usuarios
            else
            {
                foreach (string idUsuario in values)
                {
                    UserManager.SetLockoutEnabled(idUsuario, true);
                }

                TempData["BloqueadoCorrecto"] = "Se ha bloqueado correctamente el o los usuarios.";
            }
            #endregion

            return true;
        }

        [HttpPost]
        public async Task<bool> DesbloquearUsuario(List<string> values)
        {
            ConfiguracionInicial(_controlador, "DesbloquearUsuario_POST");
            #region Validando el modelo
            if (values == null)
            {
                TempData["Error"] = "No ha seleccionado ningún usuario para desbloquear.";
            }
            #endregion
            #region Desbloqueando usuarios
            else
            {
                try
                {
                    foreach (string idUsuario in values)
                    {
                        UserManager.SetLockoutEnabled(idUsuario, false);
                        await UserManager.ResetAccessFailedCountAsync(idUsuario);
                    }
                    TempData["DesbloqueadoCorrecto"] = "Se ha desbloqueado correctamente el o los usuarios.";
                }
                catch (NotSupportedException e)
                {
                    //Excepción encapsulada por Rodrigo Uzcátegui - 15-05-15
                    /* Ocurre cuando no estaba implementado el control de las llamadas asíncronas, con el await
                     * y el 'async Task<bool>', no debería volver a entrar en esta excepción.
                     */

                    TempData["Error"] = e.Message;
                    throw e;
                }
            }
            #endregion

            return true;
        }
        
        [HttpGet]
        public ActionResult ListarUsuarios()
        {
            ConfiguracionInicial(_controlador, "ListarUsuarios");
            #region Declaración de variables
            UserListViewModel model = new UserListViewModel();
            List<User> listaUsuarios = new List<User>();
            Role rolAux = new Role();
            UserService _userService = new UserService();
            RoleService _roleService = new RoleService();
            #endregion
            #region Inicializando lista de usuarios
            listaUsuarios = _userService.ObtenerListaUsuarios().ToList();
            listaUsuarios = (listaUsuarios.Count == 0) ? new List<User>() : listaUsuarios;
            foreach (User usuario in listaUsuarios)
            {
                UserListViewModel.UsuarioPersonal usuarioPersonal = new UserListViewModel.UsuarioPersonal();
                rolAux = _roleService.ObtenerRolPorId(usuario.Roles.First().RoleId);

                usuarioPersonal.usuario = usuario;
                usuarioPersonal.rol = rolAux;

                model.listaUsuariosPersonales.Add(usuarioPersonal);
            }
            #endregion
            #region Sección mensajes TempData
            if (TempData["UsuarioEditado"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["UsuarioEditado"].ToString();
            }
            else if (TempData["ErrorEliminar"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorEliminar"].ToString());
            }
            else if (TempData["AclamacionEliminar"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["AclamacionEliminar"].ToString();
            }
            #endregion

            return View(model);
        }

        [HttpGet]
        public ActionResult EditarUsuario(string id)
        {
            ConfiguracionInicial(_controlador, "EditarUsuario");
            #region Inicializando variables
            EditUserViewModel model = new EditUserViewModel();            
            UserService _userService = new UserService();
            RoleService _roleService = new RoleService();
            User usuario = _userService.ObtenerUsuarioPorId(id);
            List<Role> listaRoles = new List<Role>();
            #endregion
            #region Sección TempData
            if (TempData["ModelError"] != null)
            {
                model.MostrarErrores = "block";
                foreach (ModelState error in (List<ModelState>)TempData["ModelError"])
                {
                    if (error.Errors.Count != 0)
                        ModelState.AddModelError("", error.Errors[0].ErrorMessage);
                }
            }
            #endregion
            #region Inicializando SelectList de roles
            listaRoles = _roleService.ObtenerListaRoles().ToList();
            listaRoles = (listaRoles.Count == 0) ? new List<Role>() : listaRoles;
            model.listaRoles = new SelectList(listaRoles, "Id", "Name", usuario.Roles.First().RoleId);
            #endregion
            #region Asignando usuario al modelo
            model.idUsuario = usuario.Id;
            model.Username = usuario.UserName;
            model.OldEmail = usuario.Email;
            #endregion

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(EditUserViewModel model)
        {
            //Si entra en [HttpGet] EditarUsuario(string id), no necesita de ConfiguracionInicial();
            #region Inicializando variables
            List<Role> listaRoles = new List<Role>();
            RoleService _roleService = new RoleService();
            UserService _userService = new UserService();
            User usuario = _userService.ObtenerUsuarioPorId(model.idUsuario);
            Role rol = _roleService.ObtenerRolPorId(model.rolId);
            Role rolViejo;
            #endregion
            #region Validando el modelo
            if (!ModelState.IsValid)
            {
                TempData["ModelError"] = ModelState.Select(m => m.Value).ToList();
                return RedirectToAction("EditarUsuario", _controlador, model.idUsuario);
            }
            #endregion
            #region Validando la antigua contraseña
            if (!UserManager.CheckPassword(usuario, model.OldPassword))
            {
                ModelState.AddModelError("", "Contraseña inválida. Por favor vuelva a intentarlo.");
                TempData["ModelError"] = ModelState.Select(m => m.Value).ToList();
                return RedirectToAction("EditarUsuario", _controlador, model.idUsuario);
            }

            #endregion
            #region Cambiando el correo
            if(model.Email != null)
                UserManager.SetEmail(model.idUsuario, model.Email);
            #endregion
            #region Cambiando la contraseña
            if (model.Password != null)
                UserManager.ChangePassword(model.idUsuario, model.OldPassword, model.Password);
            #endregion
            #region Cambiando el rol
            if (!model.rolId.Equals(usuario.Roles.First().RoleId))
            {
                rolViejo = _roleService.ObtenerRolPorId(usuario.Roles.First().RoleId);

                UserManager.RemoveFromRole(model.idUsuario, rolViejo.Name);
                UserManager.AddToRole(model.idUsuario, rol.Name);
            }
            #endregion
            TempData["UsuarioEditado"] = "Se editó correctamente el usuario '" + model.Username + "'";

            return RedirectToAction("ListarUsuarios");
        }

        [HttpPost]
        public JsonResult EliminarUsuario(string id)
        {
            ConfiguracionInicial(_controlador, "EliminarUsuario");

            User usuario = UserManager.FindById(id);
            List<object> jsonResult = new List<object>();

            try
            {
                UserManager.Delete(usuario);
                TempData["AclamacionEliminar"] = "Se eliminó correctamente el usuario";
                jsonResult.Add(new { success = true });
            }
            catch (Exception e)
            {
                TempData["ErrorEliminar"] = e.Message;
                jsonResult.Add(new { success = false });
            }

            return Json(jsonResult);
        }
        #endregion
    }
}