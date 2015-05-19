using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Tesis_ClienteWeb_Data.Repositories
{
    public static class ConstantRepository
    {
        #region Constantes de seguridad
        /// <summary>
        /// Constante que determina el número de intentos fallidos que tiene un usuario cuando intenta
        /// conectarse a su cuenta. Si este usuario se ha equivocado 4 veces en insertar su contraseña, el
        /// usuario será bloqueado.
        /// </summary>
        public const int ACCESS_FAILED_COUNT = 4;

        /// <summary>
        /// Constante que determina si el proyecto está corriéndose en ambiente de producción o en ambiente de
        /// desarrollo, de ser un ambiente de producción se validará el usuario en la pantalla de login. De ser 
        /// un ambiente en desarrollo, no se validará el usuario y se guardará en la variable de sesión como si
        /// de un administrador se tratase.
        /// </summary>
        public const bool PRODUCTION_ENVIRONMENT = true;

        /// <summary>
        /// Constante que determina si se validarán los perfiles asociados a un usuario cuando esté intente
        /// realizar una acción o dirigirse a una ventana dentro del sistema. De ser true, se validarán los
        /// perfiles asociados y se determinará si tiene privilegios para acceder a cierto privilegio en la 
        /// página. De ser false no se validan los perfiles.
        /// </summary>
        public const bool PROFILE_VALIDATION = true;
        #endregion
        #region Constantes de credenciales del usuario 'Desarrollo'
        public const string DEVELOPMENT_USER_PASSWORD = "Qwe123@";
        public const string DEVELOPMENT_USER_USERNAME = "Desarrollo";
        public const string DEVELOPMENT_USER_EMAIL = "desarrollo@faroatenas.com";
        public const string DEVELOPMENT_ROLE_NAME = "Administrador Desarrollo";
        public const string DEVELOPMENT_ROLE_DESCRIPTION = "Rol administrador para ambiente de desarrollo.";
        #endregion
        #region Constantes de formatos de horas
        public const string DATE_FORMAT = "dd/mm/yyyy";
        public const string HOUR_FORMAT = "hh:mm:ss";
        #endregion
        #region Constantes de notificaciones
        #region Tipos de alerta
        public const string NOTIFICATION_ALERT_TYPE_Aviso = "Aviso";
        public const string NOTIFICATION_ALERT_TYPE_Citacion = "Citación";
        public const string NOTIFICATION_ALERT_TYPE_Convocatoria = "Convocatoria";
        public const string NOTIFICATION_ALERT_TYPE_Emergencia = "Emergencia";
        public const string NOTIFICATION_ALERT_TYPE_Inasistencia = "Inasistencia";
        public const string NOTIFICATION_ALERT_TYPE_MensajeNeutral = "Mensaje neutral";
        public const string NOTIFICATION_ALERT_TYPE_PeticionNotas = "Petición de notas";
        public const string NOTIFICATION_ALERT_TYPE_ResultadoNotas = "Resultado de notas";
        public const string NOTIFICATION_ALERT_TYPE_Evento = "Evento";
        #endregion
        #region Atribuciones
        public const string NOTIFICATION_ATTRIBUTION_NA = "N/A";
        public const string NOTIFICATION_ATTRIBUTION_PeriodoEscolar = "Período escolar";
        public const string NOTIFICATION_ATTRIBUTION_PlanEvaluacion = "Plan de evaluación";
        public const string NOTIFICATION_ATTRIBUTION_CorteNotas = "Corte de notas";
        public const string NOTIFICATION_ATTRIBUTION_Boleta = "Boleta";
        public const string NOTIFICATION_ATTRIBUTION_EventoUnDia = "Evento de un día";
        public const string NOTIFICATION_ATTRIBUTION_EventoVariosDias = "Evento de varios días";
        public const string NOTIFICATION_ATTRIBUTION_EventoFeriadoUnDia = "Evento feriado de un día";
        public const string NOTIFICATION_ATTRIBUTION_EventoFeriadoVariosDias = "Evento feriado de varios días";
        public const string NOTIFICATION_ATTRIBUTION_ReunionConsejo = "Reunión/Consejo de profesores";
        //La lista de materias
        #endregion
        #region Listas de tipos de alerta - Notificaciones personalizadas
        #region Constant: NOTIFICATION_ALERT_TYPE_COORDINATOR_COURSE
        private static readonly ReadOnlyCollection<string> _notification_alert_type_coordinator_course =
            new ReadOnlyCollection<string>(new[] {
                NOTIFICATION_ALERT_TYPE_Aviso,
                NOTIFICATION_ALERT_TYPE_Citacion
            });
        public static ReadOnlyCollection<string> NOTIFICATION_ALERT_TYPE_COORDINATOR_COURSE
        {
            get { return _notification_alert_type_coordinator_course; }
        }
        #endregion
        #region Constant: NOTIFICATION_ALERT_TYPE_COORDINATOR_TEACHER
        private static readonly ReadOnlyCollection<string> _notification_alert_type_coordinator_teacher =
            new ReadOnlyCollection<string>(new[] {
                NOTIFICATION_ALERT_TYPE_Aviso,
                NOTIFICATION_ALERT_TYPE_Convocatoria,
                NOTIFICATION_ALERT_TYPE_MensajeNeutral,
                NOTIFICATION_ALERT_TYPE_PeticionNotas,
            });
        public static ReadOnlyCollection<string> NOTIFICATION_ALERT_TYPE_COORDINATOR_TEACHER
        {
            get { return _notification_alert_type_coordinator_teacher; }
        }
        #endregion
        #region Constant: NOTIFICATION_ALERT_TYPE_COORDINATOR_REPRESENTATIVE
        private static readonly ReadOnlyCollection<string> _notification_alert_type_coordinator_representative =
            new ReadOnlyCollection<string>(new[] {
                NOTIFICATION_ALERT_TYPE_Aviso,
                NOTIFICATION_ALERT_TYPE_Citacion,
                NOTIFICATION_ALERT_TYPE_Emergencia,
                NOTIFICATION_ALERT_TYPE_Inasistencia,
                NOTIFICATION_ALERT_TYPE_MensajeNeutral,                
            });
        public static ReadOnlyCollection<string> NOTIFICATION_ALERT_TYPE_COORDINATOR_REPRESENTATIVE
        {
            get { return _notification_alert_type_coordinator_representative; }
        }
        #endregion
        #endregion
        #region Constantes de categorías - Notificaciones automáticas
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCHOOL_YEAR = 1;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_I = 2;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_II = 3;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_III = 4;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENTS = 5;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENT_SCORE = 6;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_EVENT = 7;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_EVENT_1_DAY = 8;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_EVENT_VARIOUS_DAYS = 9;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERSONAL_EVENT_1_DAY = 10;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERSONAL_EVENT_VARIOUS_DAYS = 11;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCORE = 12;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_SCORE = 13;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_START_DATE = 14;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_FINISH_DATE = 15;
        public const int AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_BOTH_DATES = 16;
        #endregion
        #endregion
        #region Constantes de eventos
        #region Tipos de eventos
        public const string EVENT_EVENT_TYPE_PeriodoEscolar = NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
        public const string EVENT_EVENT_TYPE_PlanEvaluacion = NOTIFICATION_ATTRIBUTION_PlanEvaluacion;
        public const string EVENT_EVENT_TYPE_Evaluacion_Neutral = "Evaluación";
        public const string EVENT_EVENT_TYPE_Evaluacion_Exámen = "Exámen";
        public const string EVENT_EVENT_TYPE_CorteNotas = NOTIFICATION_ATTRIBUTION_CorteNotas;
        public const string EVENT_EVENT_TYPE_Boleta = NOTIFICATION_ATTRIBUTION_Boleta;
        public const string EVENT_EVENT_TYPE_EventoUnDia = NOTIFICATION_ATTRIBUTION_EventoUnDia;
        public const string EVENT_EVENT_TYPE_EventoVariosDias = NOTIFICATION_ATTRIBUTION_EventoVariosDias;
        public const string EVENT_EVENT_TYPE_EventoFeriadoUnDia = NOTIFICATION_ATTRIBUTION_EventoFeriadoUnDia;
        public const string EVENT_EVENT_TYPE_EventoFeriadoVariosDias = NOTIFICATION_ATTRIBUTION_EventoFeriadoVariosDias;
        public const string EVENT_EVENT_TYPE_ReunionConsejo = NOTIFICATION_ATTRIBUTION_ReunionConsejo;

        #endregion
        #region Constantes de categorías - Notificaciones automáticas
        public const int GLOBAL_EVENT_CATEGORY_NEW_SCHOOL_YEAR = 1;
        public const int GLOBAL_EVENT_CATEGORY_NEW_SCHOOL_YEAR_WITH_PERIODS = 2;
        public const int PERSONAL_EVENT_CATEGORY_NEW_ASSESSMENT = 3;
        public const int GLOBAL_EVENT_CATEGORY_NEW_EVENT_1_DAY = 4;
        public const int GLOBAL_EVENT_CATEGORY_NEW_EVENT_VARIOUS_DAYS = 5;
        public const int PERSONAL_EVENT_CATEGORY_1_DAY = 6;
        public const int PERSONAL_EVENT_CATEGORY_VARIOUS_DAYS = 7;
        public const int GLOBAL_EVENT_CATEGORY_3_PERIODS = 8;
        #endregion

        #region Constant: EVENT_COLOR_LIST
        private static readonly ReadOnlyDictionary<string, string> _event_color_list =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>()
            {
                {EVENT_EVENT_TYPE_PeriodoEscolar, "00FF80"},
                {EVENT_EVENT_TYPE_PlanEvaluacion, "0080FF"},
                {EVENT_EVENT_TYPE_Evaluacion_Neutral, "8000FF"},
                {EVENT_EVENT_TYPE_Evaluacion_Exámen, "F00C0C"},
                {EVENT_EVENT_TYPE_CorteNotas, "FF3C00"},
                {EVENT_EVENT_TYPE_Boleta, "00FFBC"},
                {EVENT_EVENT_TYPE_EventoUnDia, "FF00C4"},
                {EVENT_EVENT_TYPE_EventoVariosDias, "CA97D4"},
                {EVENT_EVENT_TYPE_EventoFeriadoUnDia, "F9F932"},
                {EVENT_EVENT_TYPE_EventoFeriadoVariosDias, "E2E245"},
                {EVENT_EVENT_TYPE_ReunionConsejo, "C1C1BE"}
            });
        public static ReadOnlyDictionary<string, string> EVENT_COLOR_LIST
        {
            get { return _event_color_list; }
        }
        #endregion
        #region Constant: EVENT_TYPE_LIST
        private static readonly ReadOnlyCollection<string> _eventType_list = new ReadOnlyCollection<string>(new[] 
        {            
            EVENT_EVENT_TYPE_PeriodoEscolar,
            EVENT_EVENT_TYPE_PlanEvaluacion,
            EVENT_EVENT_TYPE_Evaluacion_Neutral,
            EVENT_EVENT_TYPE_Evaluacion_Exámen,
            EVENT_EVENT_TYPE_CorteNotas,
            EVENT_EVENT_TYPE_Boleta,
            EVENT_EVENT_TYPE_EventoUnDia,
            EVENT_EVENT_TYPE_EventoVariosDias,
            EVENT_EVENT_TYPE_EventoFeriadoUnDia,
            EVENT_EVENT_TYPE_EventoFeriadoVariosDias,
            EVENT_EVENT_TYPE_ReunionConsejo
        });
        public static ReadOnlyCollection<string> EVENT_TYPE_LIST
        {
            get { return _eventType_list; }
        }
        #endregion
        #endregion
        #region Constantes de roles
        public const string ADMINISTRATOR_ROLE = "Administrador";
        public const string COORDINATOR_ROLE = "Coordinador";
        public const string TEACHER_ROLE = "Docente";
        #endregion
        #region Constantes de lapsos
        public const string PERIOD_ONE = "1er Lapso";
        public const string PERIOD_TWO = "2do Lapso";
        public const string PERIOD_THREE = "3er Lapso";
        #endregion
        #region Constantes de reportes
        /// <summary>
        /// Constante que indica el directorio de los archivos utilitarios para el diseño de los reportes.
        /// </summary>
        public const string REPORT_UTILITIES_DIRECTORY = "~/Content/Images/Reports";
        /// <summary>
        /// Constante que indica el directorio donde se guardaran, del lado del servidor, los reportes emitidos.
        /// </summary>
        public const string REPORT_SERVER_DOWNLOAD_DIRECTORY = "~/App_Uploads/Reports_ServerSide";
        public const string REPORT_SERVER_REMAINS_DIRECTORY = "~/App_Uploads/Reports_Remains";
        public const string REPORT_HEADER_BACKGROUND = "Header.png";
        public const string REPORT_SUBHEADER_BACKGROUND = "SubHeader.png";
        public const string REPORT_LOGO = "Logo.png";
        public const string REPORT_NODATA = "No-data.png";
        public const string REPORT_NODATA_150 = "No-data_150.png";
        public const string REPORT_NODATA_130 = "No-data_130.png";
        public const string REPORT_NODATA_110 = "No-data_110.png";
        public const string REPORT_NODATA_100 = "No-data_100.png";
        public const string REPORT_NODATA_90 = "No-data_90.png";
        #endregion
        #region Constantes de acciones de menú de Maestras
        public const int MASTER_ACTION_ALUMNOS_AGREGAR_ALUMNOS = 1;
        public const int MASTER_ACTION_ALUMNOS_GESTION_ALUMNOS = 2;
        public const int MASTER_ACTION_ALUMNOS_ASOCIAR_REPRESENTANTES = 3;
        public const int MASTER_ACTION_COLEGIOS_CREAR_COLEGIO = 4;
        public const int MASTER_ACTION_COLEGIOS_LISTADO_COLEGIOS = 5;
        public const int MASTER_ACTION_COLEGIOS_NUEVO_ANO_ESCOLAR = 6;
        public const int MASTER_ACTION_COLEGIOS_ASIGNACION_PERIODOS_ESCOLARES = 7;
        public const int MASTER_ACTION_CURSOS_CREAR_CURSO = 8;
        public const int MASTER_ACTION_CURSOS_GESTION_CURSO = 9;
        public const int MASTER_ACTION_DOCENTE_ASOCIAR_DOCENTE = 10;
        public const int MASTER_ACTION_EVALUACION_CREAR_EVALUACION = 11;
        public const int MASTER_ACTION_EVALUACION_MODIFICAR_EVALUACION = 12;
        public const int MASTER_ACTION_EVENTOS_CREAR_EVENTOS_GENERALES = 13;        
        public const int MASTER_ACTION_MATERIAS_GESTION_MATERIAS = 14;
        public const int MASTER_ACTION_MATERIAS_MODIFICAR_MATERIAS = 15;
        public const int MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_AUTOMATICAS = 16;
        public const int MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_PERSONALIZADAS = 17;
        public const int MASTER_ACTION_SEGURIDAD_AGREGAR_USUARIO = 18;
        public const int MASTER_ACTION_SEGURIDAD_LISTAR_USUARIO = 19;
        public const int MASTER_ACTION_SEGURIDAD_BLOQUEAR_DESBLOQUEAR_USUARIO = 20;
        public const int MASTER_ACTION_SEGURIDAD_AGREGAR_ROL = 21;
        public const int MASTER_ACTION_SEGURIDAD_LISTAR_ROLES = 22;
        public const int MASTER_ACTION_SEGURIDAD_GESTION_PERFILES = 23;
        public const int MASTER_ACTION_EVENTOS_GESTION_EVENTOS_GENERALES = 24;

        #region Constant: ROLE_ACCIONES_MAESTRAS_ADMINISTRADOR
        private static readonly ReadOnlyDictionary<int, bool> _role_acciones_maestras_administrador =
            new ReadOnlyDictionary<int, bool>(new Dictionary<int, bool>() { 
                { MASTER_ACTION_ALUMNOS_AGREGAR_ALUMNOS, true },    
                { MASTER_ACTION_ALUMNOS_GESTION_ALUMNOS, true },
                { MASTER_ACTION_ALUMNOS_ASOCIAR_REPRESENTANTES, true },
                { MASTER_ACTION_COLEGIOS_CREAR_COLEGIO, true },
                { MASTER_ACTION_COLEGIOS_LISTADO_COLEGIOS, true },
                { MASTER_ACTION_COLEGIOS_NUEVO_ANO_ESCOLAR, true },
                { MASTER_ACTION_COLEGIOS_ASIGNACION_PERIODOS_ESCOLARES, true },
                { MASTER_ACTION_CURSOS_CREAR_CURSO, true },
                { MASTER_ACTION_CURSOS_GESTION_CURSO, true },
                { MASTER_ACTION_DOCENTE_ASOCIAR_DOCENTE, true },
                { MASTER_ACTION_EVALUACION_CREAR_EVALUACION, true },
                { MASTER_ACTION_EVALUACION_MODIFICAR_EVALUACION, true },
                { MASTER_ACTION_EVENTOS_CREAR_EVENTOS_GENERALES, true },                
                { MASTER_ACTION_MATERIAS_GESTION_MATERIAS, true },
                { MASTER_ACTION_MATERIAS_MODIFICAR_MATERIAS, true },
                { MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_AUTOMATICAS, true },
                { MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_PERSONALIZADAS, false },
                { MASTER_ACTION_SEGURIDAD_AGREGAR_USUARIO, true },
                { MASTER_ACTION_SEGURIDAD_LISTAR_USUARIO, true },
                { MASTER_ACTION_SEGURIDAD_BLOQUEAR_DESBLOQUEAR_USUARIO, true },
                { MASTER_ACTION_SEGURIDAD_AGREGAR_ROL, true },
                { MASTER_ACTION_SEGURIDAD_LISTAR_ROLES, true },
                { MASTER_ACTION_SEGURIDAD_GESTION_PERFILES, true },
                { MASTER_ACTION_EVENTOS_GESTION_EVENTOS_GENERALES, true },
            });
        public static ReadOnlyDictionary<int, bool> ROLE_ACCIONES_MAESTRAS_ADMINISTRADOR
        {
            get { return _role_acciones_maestras_administrador; }
        }
        #endregion
        #region Constant: ROLE_ACCIONES_MAESTRAS_COORDINADOR
        private static readonly ReadOnlyDictionary<int, bool> _role_acciones_maestras_coordinador =
            new ReadOnlyDictionary<int, bool>(new Dictionary<int, bool>() { 
                { MASTER_ACTION_ALUMNOS_AGREGAR_ALUMNOS, true },    
                { MASTER_ACTION_ALUMNOS_GESTION_ALUMNOS, true },
                { MASTER_ACTION_ALUMNOS_ASOCIAR_REPRESENTANTES, true },
                { MASTER_ACTION_COLEGIOS_CREAR_COLEGIO, false },
                { MASTER_ACTION_COLEGIOS_LISTADO_COLEGIOS, false },
                { MASTER_ACTION_COLEGIOS_NUEVO_ANO_ESCOLAR, false },
                { MASTER_ACTION_COLEGIOS_ASIGNACION_PERIODOS_ESCOLARES, false },
                { MASTER_ACTION_CURSOS_CREAR_CURSO, true },
                { MASTER_ACTION_CURSOS_GESTION_CURSO, true },
                { MASTER_ACTION_DOCENTE_ASOCIAR_DOCENTE, true },
                { MASTER_ACTION_EVALUACION_CREAR_EVALUACION, true },
                { MASTER_ACTION_EVALUACION_MODIFICAR_EVALUACION, true },
                { MASTER_ACTION_EVENTOS_CREAR_EVENTOS_GENERALES, true },                
                { MASTER_ACTION_MATERIAS_GESTION_MATERIAS, true },
                { MASTER_ACTION_MATERIAS_MODIFICAR_MATERIAS, true },
                { MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_AUTOMATICAS, false },
                { MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_PERSONALIZADAS, true },
                { MASTER_ACTION_SEGURIDAD_AGREGAR_USUARIO, false },
                { MASTER_ACTION_SEGURIDAD_LISTAR_USUARIO, false },
                { MASTER_ACTION_SEGURIDAD_BLOQUEAR_DESBLOQUEAR_USUARIO, false },
                { MASTER_ACTION_SEGURIDAD_AGREGAR_ROL, false },
                { MASTER_ACTION_SEGURIDAD_LISTAR_ROLES, false },
                { MASTER_ACTION_SEGURIDAD_GESTION_PERFILES, false },
                { MASTER_ACTION_EVENTOS_GESTION_EVENTOS_GENERALES, true },
            });
        public static ReadOnlyDictionary<int, bool> ROLE_ACCIONES_MAESTRAS_COORDINADOR
        {
            get { return _role_acciones_maestras_coordinador; }
        }
        #endregion
        #endregion

        #region Otras constantes
        /// <summary>
        /// Constante que guarda el id del primer colegio que se mostrará para los usuarios administradores
        /// </summary>
        public const int ADMINISTRATOR_FIRST_SCHOOL = 1;

        /// <summary>
        /// Constante que indica el número de meses que extenderá el año escolar (tanto en el límite de inicio,
        /// como para el límite de finaliazción) para realizar diferentes acciones como:
        /// 
        /// 1. Encontrar notificaciones automáticas que no estén dentro del período de fechas de un año escolar.
        /// 2. Mostrar en el calendario de fechas días previos y posteriores a las del año escolar.
        /// </summary>
        public const int MONTH_NUMBER_EXTENSION_LIMIT = 1;

        #region Constant: SEX_LIST_SHORT
        private static readonly ReadOnlyCollection<string> _sex_list_short =
            new ReadOnlyCollection<string>(new[] 
        {
            "F", "M",
        });
        public static ReadOnlyCollection<string> SEX_LIST_SHORT
        {
            get { return _sex_list_short; }
        }
        #endregion

        #region Constant: SEX_LIST_LARGE
        private static readonly ReadOnlyCollection<string> _sex_list_large =
            new ReadOnlyCollection<string>(new[] 
        {
            "Femenino", "Masculino",
        });
        public static ReadOnlyCollection<string> SEX_LIST_LARGE
        {
            get { return _sex_list_large; }
        }
        #endregion

        #region Constant: IDENTITY_NUMBER_TYPE_LIST
        private static readonly ReadOnlyCollection<string> _identity_number_type_list =
            new ReadOnlyCollection<string>(new[] 
        {
            "V-", "E-",
        });
        public static ReadOnlyCollection<string> IDENTITY_NUMBER_TYPE_LIST
        {
            get { return _identity_number_type_list; }
        }
        #endregion
        #endregion

        #region Estándares
        #region Reportes

        #endregion
        #endregion



        #region Constantes de evaluación
        #region Constant: ACTIVITY_LIST
        private static readonly ReadOnlyCollection<string> _activity_list = new ReadOnlyCollection<string>(new[] 
        {
            "Aplicación de prueba",
            "Charla participativa",
            "Coloquio",            
	        "Demostración",
            "Diálogo o debate público",
	        "Discusión en pequeños grupos", 
            "Discusión socializada",
            "Dramatización",
	        "Entrevista colectiva",
            "Entrevista o consulta pública",
	        "Estudio de casos",
	        "Estudio Dirigido",
            "Exposición",
	        "Ficha", 
	        "Foro",
	        "Guía",
	        "Interacción Constructiva",
	        "Intercambio de experiencias",
	        "Lectura",
	        "Mesa redonda",
            "Motríz",
	        "Oral",
            "Otro",
	        "Panel",
	        "Phillips 22",
	        "Phillips 66", 
	        "Reproducción",
	        "Resolución de problemas",
	        "Seminario",
	        "Simposio",  
	        "Simulación",	        
	        "Sociodrama",  
            "Taller",
            
        });
        public static ReadOnlyCollection<string> ACTIVITY_LIST
        {
            get { return _activity_list; }
        }

        #endregion
        #region Constant: TECHNIQUE_LIST
        private static readonly ReadOnlyCollection<string> _technique_list = new ReadOnlyCollection<string>(new[] 
        {          
            "Análisis de las producciones",
            "Autoevaluación",
            "Coevaluación",
            "Encuesta",
	        "Entrevista en profundidad",
	        "Entrevista focalizada",
            "Etnografía de la clase",
            "Mapa Conceptual",
	        "Observación directa",
	        "Observación indirecta",
	        "Observación no participante",
	        "Observación participante",
            "Otro",
            "Portafolio",
            "Prueba",
        });
        public static ReadOnlyCollection<string> TECHNIQUE_LIST
        {
            get { return _technique_list; }
        }

        #endregion
        #region Constant: INSTRUMENT_LIST
        private static readonly ReadOnlyCollection<string> _instrument_list = new ReadOnlyCollection<string>(new[] 
        {            
            
	        "Análisis de problemas",
	        "Cuestionario",
	        "Encuesta",
            "Escala de estimación",
            "Escala de estimación descriptiva",
            "Escala de estimación de caracterización",
	        "Escala de estimación de categoría",
            "Escala de estimación de frecuencia",
            "Escala de estimación numéricas",
	        "Guión de Análisis",
	        "Guía de Entrevistas",
	        "Guión de Encuestas",
	        "Guión de Entrevistas",
	        "Hoja de Análisis",
	        "Hoja de Autoevaluación",
            "Hoja de Coevaluación", 
	        "Hoja de Evaluación",
            "Hoja de Registros",
	        "Lista de cotejo",
            "Otro",
	        "Prueba de Ensayo",
            "Prueba de respuesta libre",
	        "Prueba escrita estructurada",
	        "Prueba escrita no estructurada",
	        "Prueba escrita semi-estructurada",
	        "Prueba Práctica",
	        "Prueba Oral",            
	        "Registro",
	        "Registro anecdótico",
	        "Registro descriptivo",
	        "Registro diario",	
	        "Sociograma",
	        "Test",

        });
        public static ReadOnlyCollection<string> INSTRUMENT_LIST
        {
            get { return _instrument_list; }
        }
        #endregion
        #endregion
        #region Constant: GRADE_LIST
        private static readonly ReadOnlyCollection<string> _grade_list = new ReadOnlyCollection<string>(new[] 
        {
            "1er Grado", "2do Grado", "3er Grado", "4to Grado", "5to Grado", "6to Grado", "7mo Grado",
            "8vo Grado", "9no Grado", "10mo Grado", "11mo Grado"
        });
        public static ReadOnlyCollection<string> GRADE_LIST
        {
            get { return _grade_list; }
        }
        #endregion
        #region Constant: COURSE_SECTION_LIST
        private static readonly ReadOnlyCollection<string> _course_section_list =
            new ReadOnlyCollection<string>(new[] {
            "A", "B", "C", "D", "E", "Única"
        });
        public static ReadOnlyCollection<string> COURSE_SECTION_LIST
        {
            get { return _course_section_list; }
        }
        #endregion

        /// <summary>
        /// Creado por Fabio, método que convierte las linq en hashset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            try
            {
                return new HashSet<T>(source);
            }
            catch (ArgumentNullException)
            {
                // Fabio Puchetti
                // 05/04/2015
                return new HashSet<T>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
