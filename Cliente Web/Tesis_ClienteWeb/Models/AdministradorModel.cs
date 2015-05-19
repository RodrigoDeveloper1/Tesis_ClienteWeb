using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using Tesis_ClienteWeb_Models.POCO;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tesis_ClienteWeb_Data.Repositories;

namespace Tesis_ClienteWeb.Models
{
    public class RegisterUserViewModel : MaestraModel
    {
        #region Variables declaradas

        #region Username
        [Display(Name = "Nombre de usuario:")]
        [Required(ErrorMessage = "Por favor insertar un nombre de usuario.", AllowEmptyStrings = false)]
        [StringLength(16,
            MinimumLength = 6,
            ErrorMessage = "El nombre de usuario debe tener un mínimo de {2} caracteres y un máximo de {1}.")]
        public string Username { get; set; }
        #endregion
        #region Correo electrónico
        [Display(Name = "Correo electrónico:")]
        [Required(ErrorMessage = "Por favor insertar un correo electrónico.", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Por favor insertar un correo electrónico válido.")]
        [StringLength(50,
            MinimumLength = 14,
            ErrorMessage = "El correo debe tener un mínimo de {2} caracteres y un máximo de {1}.")]
        public string Email { get; set; }
        #endregion
        #region Confirmar correo electrónico
        [Display(Name = "Confirmar correo electrónico:")]
        [System.ComponentModel.DataAnnotations.Compare("Email",
            ErrorMessage = "El correo y el correo de confirmación no coinciden.")]
        public string ConfirmEmail { get; set; }
        #endregion
        #region Contraseña
        [Display(Name = "Contraseña:")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Por favor insertar una contraseña.", AllowEmptyStrings = false)]
        [StringLength(20,
            MinimumLength = 6,
            ErrorMessage = "La contraseña debe tener un mínimo de {2} caracteres y un máximo de {1}.")]
        public string Password { get; set; }
        #endregion
        #region Confirmar contraseña
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña:")]
        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
        #endregion
        #region Id del rol
        [Display(Name = "Lista de roles:")]
        [Required(ErrorMessage = "Por favor seleccione un rol.", AllowEmptyStrings = false)]
        public string rolId { get; set; }
        #endregion

        [Display(Name = "Nombre:")]
        [Required(ErrorMessage = "Por favor especifique un nombre.", AllowEmptyStrings = false)]
        public string nombre { get; set; }

        [Display(Name = "Apellido:")]
        [Required(ErrorMessage = "Por favor especifique un apellido.", AllowEmptyStrings = false)]
        public string apellido { get; set; }

        #region Variables internas
        public SelectList listaRoles { get; set; }
        public List<UsuarioPersonal> listaUsuariosPersonales { get; set; }

        public class UsuarioPersonal
        {
            public User usuario { get; set; }
            public Role rol { get; set; }

            public UsuarioPersonal()
            {
                this.usuario = new User();
                this.rol = new Role();
            }
        }
        #endregion
        #endregion
        #region Constructor
        public RegisterUserViewModel()
        {
            this.listaUsuariosPersonales = new List<UsuarioPersonal>();
        }
        #endregion
    }

    public class EditUserViewModel : MaestraModel
    {
        #region Variables declaradas
        #region Nombre de usuario
        [Display(Name = "Nombre de usuario:")]
        public string Username { get; set; }
        #endregion
        #region Antiguo correo electrónico
        [Display(Name = "Antiguo correo electrónico:")]
        public string OldEmail { get; set; }
        #endregion
        #region Nuevo correo electrónico
        [Display(Name = "Modificar correo (opcional):")]
        [EmailAddress(ErrorMessage = "Por favor insertar un correo electrónico válido.")]
        [StringLength(50,
            MinimumLength = 14,
            ErrorMessage = "El correo debe tener un mínimo de {2} caracteres y un máximo de {1}.")]
        public string Email { get; set; }
        #endregion
        #region Confirmar correo electrónico
        [Display(Name = "Confirmar nuevo correo:")]
        [System.ComponentModel.DataAnnotations.Compare("Email",
            ErrorMessage = "El correo y el correo de confirmación no coinciden.")]
        public string ConfirmEmail { get; set; }
        #endregion
        #region Antigua contraseña
        [Display(Name = "Antigua contraseña:")]
        [Required(ErrorMessage = "Por favor insertar la contraseña.", AllowEmptyStrings = false)]
        public string OldPassword { get; set; }
        #endregion
        #region Contraseña
        [Display(Name = "Modificar contraseña (opcional):")]
        [DataType(DataType.Password)]
        [StringLength(20,
            MinimumLength = 6,
            ErrorMessage = "La contraseña debe tener un mínimo de {2} caracteres y un máximo de {1}.")]
        public string Password { get; set; }
        #endregion
        #region Confirmar contraseña
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña:")]
        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
        #endregion
        #region Id del rol
        [Display(Name = "Lista de roles:")]
        [Required(ErrorMessage = "Por favor seleccione un rol.", AllowEmptyStrings = false)]
        public string rolId { get; set; }
        #endregion

        #region Variables internas
        public SelectList listaRoles { get; set; }
        public string idUsuario { get; set; }
        #endregion
        #endregion
        #region Constructor
        public EditUserViewModel()
        { 

        }
        #endregion
    }

    public class UserListViewModel : MaestraModel
    {
        #region Variables declaradas
        public List<UsuarioPersonal> listaUsuariosPersonales { get; set; }
        public List<User> listaUsuariosHabilitados { get; set; }
        public List<User> listaUsuariosBloqueados { get; set; }

        public class UsuarioPersonal
        {
            public User usuario { get; set; }
            public Role rol { get; set; }

            public UsuarioPersonal()
            {
                this.usuario = new User();
                this.rol = new Role();
            }
        }
        #endregion
        #region Constructor
        public UserListViewModel()
        {
            this.listaUsuariosPersonales = new List<UsuarioPersonal>();
        }
        #endregion
    }

    public class ProfileViewModel : MaestraModel
    {
        #region Variables declaradas
        public Profile profile { get; set; }

        public List<Profile> ProfileList { get; set; }
        #endregion
    }
    
    public class AddRoleViewModel : MaestraModel
    {
        #region Variables declaradas
        [Display(Name = "Rol:")]
        [Required(ErrorMessage = "Por favor insertar el nombre del rol.", AllowEmptyStrings = false)]
        public string RolName { get; set; }

        [Display(Name = "Descripción:")]
        public string Description { get; set; }

        public List<PersonalProfile> PersonalProfileList { get; set; }

        //Solo para editar Rol
        public string RolId { get; set; }
        #endregion

        public AddRoleViewModel()
        {
            this.PersonalProfileList = new List<PersonalProfile>();
        }

        public class PersonalProfile
        {
            public Profile profile { get; set; }            
            public bool isSelected { get; set; }

            public PersonalProfile()
            {
                profile = new Profile();
                isSelected = false;
            }
        }
    }

    public class RoleListViewModel : MaestraModel
    {
        public List<Role> roleList { get; set; }

        public RoleListViewModel()
        {
            this.roleList = new List<Role>();
        }
    }        

    public class MenuViewModel : MaestraModel
    {
        public ReadOnlyDictionary<int, bool> diccionarioAcceso { get; set; }
        public int MASTER_ACTION_ALUMNOS_AGREGAR_ALUMNOS { get { return ConstantRepository.MASTER_ACTION_ALUMNOS_AGREGAR_ALUMNOS; } }
        public int MASTER_ACTION_ALUMNOS_GESTION_ALUMNOS { get { return ConstantRepository.MASTER_ACTION_ALUMNOS_GESTION_ALUMNOS; } }
        public int MASTER_ACTION_ALUMNOS_ASOCIAR_REPRESENTANTES { get { return ConstantRepository.MASTER_ACTION_ALUMNOS_ASOCIAR_REPRESENTANTES; } }
        public int MASTER_ACTION_COLEGIOS_CREAR_COLEGIO { get { return ConstantRepository.MASTER_ACTION_COLEGIOS_CREAR_COLEGIO; } }
        public int MASTER_ACTION_COLEGIOS_LISTADO_COLEGIOS { get { return ConstantRepository.MASTER_ACTION_COLEGIOS_LISTADO_COLEGIOS; } }
        public int MASTER_ACTION_COLEGIOS_NUEVO_ANO_ESCOLAR { get { return ConstantRepository.MASTER_ACTION_COLEGIOS_NUEVO_ANO_ESCOLAR; } }
        public int MASTER_ACTION_COLEGIOS_ASIGNACION_PERIODOS_ESCOLARES { get { return ConstantRepository.MASTER_ACTION_COLEGIOS_ASIGNACION_PERIODOS_ESCOLARES; } }
        public int MASTER_ACTION_CURSOS_CREAR_CURSO { get { return ConstantRepository.MASTER_ACTION_CURSOS_CREAR_CURSO; } }
        public int MASTER_ACTION_CURSOS_GESTION_CURSO { get { return ConstantRepository.MASTER_ACTION_CURSOS_GESTION_CURSO; } }
        public int MASTER_ACTION_DOCENTE_ASOCIAR_DOCENTE { get { return ConstantRepository.MASTER_ACTION_DOCENTE_ASOCIAR_DOCENTE; } }
        public int MASTER_ACTION_EVALUACION_CREAR_EVALUACION { get { return ConstantRepository.MASTER_ACTION_EVALUACION_CREAR_EVALUACION; } }
        public int MASTER_ACTION_EVALUACION_MODIFICAR_EVALUACION { get { return ConstantRepository.MASTER_ACTION_EVALUACION_MODIFICAR_EVALUACION; } }
        public int MASTER_ACTION_EVENTOS_CREAR_EVENTOS_GENERALES { get { return ConstantRepository.MASTER_ACTION_EVENTOS_CREAR_EVENTOS_GENERALES; } }
        public int MASTER_ACTION_MATERIAS_GESTION_MATERIAS { get { return ConstantRepository.MASTER_ACTION_MATERIAS_GESTION_MATERIAS; } }
        public int MASTER_ACTION_MATERIAS_MODIFICAR_MATERIAS { get { return ConstantRepository.MASTER_ACTION_MATERIAS_MODIFICAR_MATERIAS; } }
        public int MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_AUTOMATICAS { get { return ConstantRepository.MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_AUTOMATICAS; } }
        public int MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_PERSONALIZADAS { get { return ConstantRepository.MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_PERSONALIZADAS; } }
        public int MASTER_ACTION_SEGURIDAD_AGREGAR_USUARIO { get { return ConstantRepository.MASTER_ACTION_SEGURIDAD_AGREGAR_USUARIO; } }
        public int MASTER_ACTION_SEGURIDAD_LISTAR_USUARIO { get { return ConstantRepository.MASTER_ACTION_SEGURIDAD_LISTAR_USUARIO; } }
        public int MASTER_ACTION_SEGURIDAD_BLOQUEAR_DESBLOQUEAR_USUARIO { get { return ConstantRepository.MASTER_ACTION_SEGURIDAD_BLOQUEAR_DESBLOQUEAR_USUARIO; } }
        public int MASTER_ACTION_SEGURIDAD_AGREGAR_ROL { get { return ConstantRepository.MASTER_ACTION_SEGURIDAD_AGREGAR_ROL; } }
        public int MASTER_ACTION_SEGURIDAD_LISTAR_ROLES { get { return ConstantRepository.MASTER_ACTION_SEGURIDAD_LISTAR_ROLES; } }
        public int MASTER_ACTION_SEGURIDAD_GESTION_PERFILES { get { return ConstantRepository.MASTER_ACTION_SEGURIDAD_GESTION_PERFILES; } }
        public int MASTER_ACTION_EVENTOS_GESTION_EVENTOS_GENERALES { get { return ConstantRepository.MASTER_ACTION_EVENTOS_GESTION_EVENTOS_GENERALES; } }

        public MenuViewModel()
        {
            this.diccionarioAcceso = new ReadOnlyDictionary<int, bool>(new Dictionary<int, bool>());
        }
    }
}
