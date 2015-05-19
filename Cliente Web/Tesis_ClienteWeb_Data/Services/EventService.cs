using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class EventService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad ;

        public EventService()
        {
            this._unidad = new UnitOfWork();
            _InicializadorVariablesSesion();
        }
        public EventService(UnitOfWork unidad)
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
        /// <summary>
        /// Método CRUD - Elimina eventos.
        /// Nota (13-05-15): También elimina las notificaciones asociadas a ese evento. Rodrigo Uzcátegui
        /// </summary>
        /// <param name="eventID">El id del evento</param>
        /// <returns>True = Eliminado correcto.</returns>
        public bool EliminarEvento(int eventID)
        {
            Event evento = this.ObtenerEventoPorId(eventID);
            NotificationService notificationService = new NotificationService(this._unidad);

            try
            {
                #region Sección de eliminado de las notificaciones asociadas
                while(evento.Notifications.Count() != 0)
                {
                    notificationService.EliminarNotification(evento.Notifications[0].NotificationId);
                }
                #endregion

                _unidad.RepositorioEvent.Delete(u => u.EventId == eventID);
                _unidad.Save();

                return true;
            }
            catch (DbUpdateException e)
            {
                //Excepción encapsulada por: Rodrigo Uzcátegui - 13-05-15
                /* Entró cuando el evento tenía notificaciones asociadas a él, el cual no habían sido 
                 * eliminadas previamente*/
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Método que crea aquellos eventos que el sistema debe crear de forma automática, según el tipo de
        /// acción que ocurra. Dentro del método se especifia la acción a tomar según el caso.
        /// </summary>
        /// <param name="categoria">La categoría del evento a ser creado</param>
        /// <returns>True: Se creó el evento. False: No se creó el evento</returns>
        public bool CrearEventoGlobal(int categoria, SchoolYear schoolYear)
        {
            #region Declaracion de variables
            UserService userService = new UserService(this._unidad);
            NotificationService notificationService = new NotificationService(this._unidad);

            bool DeleteEvent = false; //Evento global

            List<Event> listaEventos = new List<Event>();
            Event evento1 = new Event(DeleteEvent);
            Event evento2 = new Event(DeleteEvent);
            Event evento3 = new Event(DeleteEvent);
            Event evento4 = new Event(DeleteEvent);
            #endregion

            switch (categoria)
            {
                #region Case: Nuevo año escolar
                case ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_SCHOOL_YEAR:
                    #region Evento #1
                    evento1.Color = 
                        ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar];
                    evento1.EventType = ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar;
                    evento1.Name = "Año escolar";
                    evento1.Description = "Año escolar. " + schoolYear.StartDate.ToShortDateString() + 
                                          " - " + schoolYear.EndDate.ToShortDateString();
                    evento1.StartDate = schoolYear.StartDate;
                    evento1.StartHour = "";
                    evento1.FinishDate = schoolYear.EndDate;
                    evento1.EndHour = "";

                    evento1.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCHOOL_YEAR, schoolYear);
                    evento1.SchoolYear = schoolYear;
                    #endregion

                    #region Añadiendo a la lista de eventos
                    listaEventos.Add(evento1);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo año escolar con períodos
                case ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_SCHOOL_YEAR_WITH_PERIODS:
                    #region Evento #1
                    evento1.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository
                        .EVENT_EVENT_TYPE_PeriodoEscolar];
                    evento1.EventType = ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar;
                    evento1.Name = "Año escolar";
                    evento1.Description = "Año escolar. " + schoolYear.StartDate.ToShortDateString() +
                                          " - " + schoolYear.EndDate.ToShortDateString();
                    evento1.StartDate = schoolYear.StartDate;
                    evento1.StartHour = "";
                    evento1.FinishDate = schoolYear.EndDate;
                    evento1.EndHour = "";

                    evento1.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCHOOL_YEAR, schoolYear);
                    evento1.SchoolYear = schoolYear;
                    #endregion
                    #region Evento #2
                    evento2.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository
                        .EVENT_EVENT_TYPE_PeriodoEscolar];
                    evento2.EventType = ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar;
                    evento2.Name = "Primer Lapso";
                    evento2.Description = "Primer Lapso. " + schoolYear.Periods[0].StartDate.ToShortDateString() +
                                          " - " + schoolYear.Periods[0].FinishDate.ToShortDateString();
                    evento2.StartDate = schoolYear.Periods[0].StartDate;
                    evento2.StartHour = "";
                    evento2.FinishDate = schoolYear.Periods[0].FinishDate;
                    evento2.EndHour = "";

                    evento2.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_I, schoolYear);
                    evento2.SchoolYear = schoolYear;
                    #endregion
                    #region Evento #3
                    evento3.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository
                        .EVENT_EVENT_TYPE_PeriodoEscolar];
                    evento3.EventType = ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar;
                    evento3.Name = "Segundo Lapso";
                    evento3.Description = "Segundo Lapso. " + schoolYear.Periods[1].StartDate.ToShortDateString() +
                                          " - " + schoolYear.Periods[1].FinishDate.ToShortDateString();
                    evento3.StartDate = schoolYear.Periods[1].StartDate;
                    evento3.StartHour = "";
                    evento3.FinishDate = schoolYear.Periods[1].FinishDate;
                    evento3.EndHour = "";

                    evento3.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_II, schoolYear);
                    evento3.SchoolYear = schoolYear;
                    #endregion
                    #region Evento #4
                    evento4.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository
                        .EVENT_EVENT_TYPE_PeriodoEscolar];
                    evento4.EventType = ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar;
                    evento4.Name = "Tercer Lapso";
                    evento4.Description = "Tercer Lapso. " + schoolYear.Periods[2].StartDate.ToShortDateString() +
                                          " - " + schoolYear.Periods[2].FinishDate.ToShortDateString();
                    evento4.StartDate = schoolYear.Periods[2].StartDate;
                    evento4.StartHour = "";
                    evento4.FinishDate = schoolYear.Periods[2].FinishDate;
                    evento4.EndHour = "";

                    evento4.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_III, schoolYear);
                    evento4.SchoolYear = schoolYear;
                    #endregion

                    #region Añadiendo a la lista de eventos
                    listaEventos.Add(evento1);
                    listaEventos.Add(evento2);
                    listaEventos.Add(evento3);
                    listaEventos.Add(evento4);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo evento 1 día
                case ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_EVENT_1_DAY:
                    #region Evento #1
                    evento1.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_EventoUnDia];
                    evento1.EventType = ConstantRepository.EVENT_EVENT_TYPE_EventoUnDia;
                    evento1.Name = "Año escolar";
                    evento1.Description = "Año escolar. " + schoolYear.StartDate.ToShortDateString() +
                                          " - " + schoolYear.EndDate.ToShortDateString();
                    evento1.StartDate = schoolYear.StartDate;
                    evento1.StartHour = "";
                    evento1.FinishDate = schoolYear.EndDate;
                    evento1.EndHour = "";

                    evento1.Notifications = notificationService.CrearNotificacionAutomaticaSinSalvado(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCHOOL_YEAR,
                        schoolYear.StartDate, schoolYear.EndDate, null, null);
                    evento1.SchoolYear = schoolYear;
                    #endregion

                    #region Añadiendo a la lista de eventos
                    listaEventos.Add(evento1);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevos 3 lapsos
                case ConstantRepository.GLOBAL_EVENT_CATEGORY_3_PERIODS:
                    #region Evento #2
                    evento2.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository
                        .EVENT_EVENT_TYPE_PeriodoEscolar];
                    evento2.EventType = ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar;
                    evento2.Name = "Primer Lapso";
                    evento2.Description = "Primer Lapso. " + schoolYear.Periods[0].StartDate.ToShortDateString() +
                                          " - " + schoolYear.Periods[0].FinishDate.ToShortDateString();
                    evento2.StartDate = schoolYear.Periods[0].StartDate;
                    evento2.StartHour = "";
                    evento2.FinishDate = schoolYear.Periods[0].FinishDate;
                    evento2.EndHour = "";

                    evento2.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_I, schoolYear);
                    evento2.SchoolYear = schoolYear;
                    #endregion
                    #region Evento #3
                    evento3.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository
                        .EVENT_EVENT_TYPE_PeriodoEscolar];
                    evento3.EventType = ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar;
                    evento3.Name = "Segundo Lapso";
                    evento3.Description = "Segundo Lapso. " + schoolYear.Periods[1].StartDate.ToShortDateString() +
                                          " - " + schoolYear.Periods[1].FinishDate.ToShortDateString();
                    evento3.StartDate = schoolYear.Periods[1].StartDate;
                    evento3.StartHour = "";
                    evento3.FinishDate = schoolYear.Periods[1].FinishDate;
                    evento3.EndHour = "";

                    evento3.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_II, schoolYear);
                    evento3.SchoolYear = schoolYear;
                    #endregion
                    #region Evento #4
                    evento4.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository
                        .EVENT_EVENT_TYPE_PeriodoEscolar];
                    evento4.EventType = ConstantRepository.EVENT_EVENT_TYPE_PeriodoEscolar;
                    evento4.Name = "Tercer Lapso";
                    evento4.Description = "Tercer Lapso. " + schoolYear.Periods[2].StartDate.ToShortDateString() +
                                          " - " + schoolYear.Periods[2].FinishDate.ToShortDateString();
                    evento4.StartDate = schoolYear.Periods[2].StartDate;
                    evento4.StartHour = "";
                    evento4.FinishDate = schoolYear.Periods[2].FinishDate;
                    evento4.EndHour = "";

                    evento4.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_III, schoolYear);
                    evento4.SchoolYear = schoolYear;
                    #endregion

                    #region Añadiendo a la lista de eventos
                    listaEventos.Add(evento2);
                    listaEventos.Add(evento3);
                    listaEventos.Add(evento4);
                    #endregion
                    break;
                #endregion
            };

            #region Salvando los eventos globales
            try
            {
                foreach (Event evento in listaEventos)
                {
                    _unidad.RepositorioEvent.Add(evento);
                }
                _unidad.Save();

                return true;
            }
            #endregion
            #region Catch de errores
            catch (Exception e)
            {
                throw e;
            }
            #endregion
        }
        /// <summary>
        /// Método que crea eventos personalizados del sistema, según el tipo de acción que ocurra. Dentro del 
        /// método se especifia la acción a tomar según el caso.
        /// </summary>
        /// <param name="categoria">La categoría del evento a ser creado</param>
        /// <returns>True: Se creó el evento. False: No se creó el evento</returns>
        public bool CrearEventoPersonal(int categoria, Assessment assessment)
        {
            #region Declaracion de variables
            UserService userService = new UserService(this._unidad);
            NotificationService notificationService = new NotificationService(this._unidad);

            bool DeleteEvent = true; //Evento personal

            Event evento = new Event(DeleteEvent);
            #endregion

            switch (categoria)
            {
                #region Case: Nueva evaluación
                case ConstantRepository.PERSONAL_EVENT_CATEGORY_NEW_ASSESSMENT:
                    #region Evento #1
                    #region Definiendo el tipo de alerta & color
                    if (assessment.Technique == "Prueba" || assessment.Technique == "Exámen")
                    {
                        evento.Color =
                            ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Exámen];
                        evento.EventType = ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Exámen;
                    }
                    else
                    {
                        evento.Color =
                            ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Neutral];
                        evento.EventType = ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Neutral;
                    }
                    #endregion
                    evento.Name = assessment.Name;
                    evento.Description = assessment.Name + " " + assessment.CASU.Subject.Name + " " + 
                        assessment.CASU.Course.Name;
                    evento.StartDate = assessment.StartDate;
                    evento.StartHour = assessment.StartHour;
                    evento.FinishDate = assessment.FinishDate;
                    evento.EndHour = assessment.EndHour;
                    evento.SchoolYear = assessment.CASU.Period.SchoolYear;
                    evento.Users.Add(assessment.CASU.User); //Docente respectivo
                    
                    evento.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENTS, assessment);
                    #endregion
                    break;
                #endregion
            };

            #region Salvando el evento personalizado
            try
            {
                _unidad.RepositorioEvent.Add(evento);
                _unidad.Save();

                return true;
            }
            #endregion
            #region Catch de errores
            catch (Exception e)
            {
                throw e;
            }
            #endregion
        }
        public Event CrearEventoPersonal_SinGuardar(int categoria, Assessment assessment)
        {
            #region Declaracion de variables
            UserService userService = new UserService(this._unidad);
            NotificationService notificationService = new NotificationService(this._unidad);

            bool DeleteEvent = true; //Evento personal

            Event evento = new Event(DeleteEvent);
            #endregion

            switch (categoria)
            {
                #region Case: Nueva evaluación
                case ConstantRepository.PERSONAL_EVENT_CATEGORY_NEW_ASSESSMENT:
                    #region Evento #1
                    #region Definiendo el tipo de alerta & color
                    if (assessment.Technique == "Prueba" || assessment.Technique == "Exámen")
                    {
                        evento.Color =
                            ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Exámen];
                        evento.EventType = ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Exámen;
                    }
                    else
                    {
                        evento.Color =
                            ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Neutral];
                        evento.EventType = ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Neutral;
                    }
                    #endregion
                    evento.Name = assessment.Name;
                    evento.Description = assessment.Name + " " + assessment.CASU.Subject.Name + " " +
                        assessment.CASU.Course.Name;
                    evento.StartDate = assessment.StartDate;
                    evento.StartHour = assessment.StartHour;
                    evento.FinishDate = assessment.FinishDate;
                    evento.EndHour = assessment.EndHour;
                    evento.SchoolYear = assessment.CASU.Period.SchoolYear;
                    evento.Users.Add(assessment.CASU.User); //Docente respectivo

                    evento.Notifications = notificationService.CrearNotificacionAutomatica(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENTS, assessment);
                    #endregion
                    break;
                #endregion
            };

            return evento;
        }










        public bool CrearEventoGlobalPantallaEventos(int categoria, Event evento)
        {
            #region Declaracion de variables
            UserService userService = new UserService(this._unidad);
            NotificationService notificationService = new NotificationService(this._unidad);

            bool DeleteEvent = false; //Evento global

            List<Event> listaEventos = new List<Event>();
            Event evento1 = new Event(DeleteEvent);
            #endregion

            switch (categoria)
            {
                #region Case: Nuevo evento 1 día
                case ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_EVENT_1_DAY:
                    #region Evento #1
                    evento1.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_EventoUnDia];
                    evento1.EventType = evento.EventType;
                    evento1.Name = evento.Name;
                    evento1.Description = evento.Description;
                    evento1.StartDate = evento.StartDate;
                    evento1.StartHour = evento.StartHour;
                    evento1.FinishDate = evento.FinishDate;
                    evento1.EndHour = evento.EndHour;
;

    evento1.Notifications = notificationService.CrearNotificacionAutomaticaSinSalvadoPantallaEventos(
    ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_EVENT_1_DAY, evento, null);
                    evento1.SchoolYear = evento.SchoolYear;
                    #endregion

                    #region Añadiendo a la lista de eventos
                    listaEventos.Add(evento1);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo evento de varios días
                case ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_EVENT_VARIOUS_DAYS:
                    #region Evento #1
                    evento1.Color = ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias];
                    evento1.EventType = evento.EventType;
                    evento1.Name = evento.Name;
                    evento1.Description = evento.Description;
                    evento1.StartDate = evento.StartDate;
                    evento1.StartHour = evento.StartHour;
                    evento1.FinishDate = evento.FinishDate;
                    evento1.EndHour = evento.EndHour;
                    ;

                    evento1.Notifications = notificationService.CrearNotificacionAutomaticaSinSalvadoPantallaEventos(
                    ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_EVENT_VARIOUS_DAYS, evento, null);
                    evento1.SchoolYear = evento.SchoolYear;
                    #endregion

                    #region Añadiendo a la lista de eventos
                    listaEventos.Add(evento1);
                    #endregion
                    break;
                #endregion
            };

            #region Salvando los eventos globales
            try
            {
                foreach (Event eventox in listaEventos)
                {
                    _unidad.RepositorioEvent.Add(eventox);
                }
                _unidad.Save();

                return true;
            }
            #endregion
            #region Catch de errores
            catch (Exception e)
            {
                throw e;
            }
            #endregion
        }
        /// <summary>
        /// Método que crea un evento personal sin guardarlo en base de datos, según el tipo de acción que 
        /// ocurra.
        /// <returns></returns>
        public Event CrearEventoPersonalSinGuardar(int categoria, CASU salon, Assessment assessment)
        {
            #region Declaracion de variables
            UserService userService = new UserService(this._unidad);
            NotificationService notificationService = new NotificationService(this._unidad);

            bool DeleteEvent = true; //Evento personal

            Event evento = new Event(DeleteEvent);
            #endregion

            switch (categoria)
            {
                #region Case: Nueva evaluación
                case ConstantRepository.PERSONAL_EVENT_CATEGORY_NEW_ASSESSMENT:
                    #region Evento #1
                    #region Definiendo el tipo de alerta & color
                    if (assessment.Technique == "Prueba" || assessment.Technique == "Exámen")
                    {
                        evento.Color = 
                            ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Exámen];
                        evento.EventType = ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Exámen;
                    }
                    else
                    {
                        evento.Color = 
                            ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Neutral];
                        evento.EventType = ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Neutral;
                    }
                    #endregion
                    evento.Name = assessment.Name;
                    evento.Description = assessment.Name + " " + salon.Subject.Name + " " + salon.Course.Name;
                    evento.StartDate = assessment.StartDate;
                    evento.StartHour = assessment.StartHour;
                    evento.FinishDate = assessment.FinishDate;
                    evento.EndHour = assessment.EndHour;

                    evento.Notifications = notificationService.CrearNotificacionAutomaticaSinSalvado(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENTS, assessment.StartDate,
                        assessment.FinishDate, assessment, salon);
                    evento.SchoolYear = salon.Period.SchoolYear;

                    evento.Users.Add(salon.User); //Docente respectivo
                    #endregion
                    break;
                #endregion
            };

            return evento;
        }

        public Event CrearEventoPersonalSinGuardar_S(int categoria, CASU salon, Assessment assessment)
        {
            #region Declaracion de variables
            UserService userService = new UserService(this._unidad);
            NotificationService notificationService = new NotificationService(this._unidad);

            bool DeleteEvent = true; //Evento personal

            Event evento = new Event(DeleteEvent);
            #endregion

            switch (categoria)
            {
                #region Case: Nueva evaluación
                case ConstantRepository.PERSONAL_EVENT_CATEGORY_NEW_ASSESSMENT:
                    #region Evento #1
                    #region Definiendo el tipo de alerta & color
                    if (assessment.Technique == "Prueba" || assessment.Technique == "Exámen")
                    {
                        evento.Color =
                            ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Exámen];
                        evento.EventType = ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Exámen;
                    }
                    else
                    {
                        evento.Color =
                            ConstantRepository.EVENT_COLOR_LIST[ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Neutral];
                        evento.EventType = ConstantRepository.EVENT_EVENT_TYPE_Evaluacion_Neutral;
                    }
                    #endregion
                    evento.Name = assessment.Name;
                    evento.Description = assessment.Name + " " + salon.Subject.Name + " " + salon.Course.Name;
                    evento.StartDate = assessment.StartDate;
                    evento.StartHour = assessment.StartHour;
                    evento.FinishDate = assessment.FinishDate;
                    evento.EndHour = assessment.EndHour;

                    evento.Notifications = notificationService.CrearNotificacionAutomaticaSinSalvado_S(
                        ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENTS, assessment.StartDate,
                        assessment.FinishDate, assessment, salon);
                    evento.SchoolYear = salon.Period.SchoolYear;

                    evento.Users.Add(salon.User); //Docente respectivo
                    #endregion
                    break;
                #endregion
            };

            return evento;
        }

        public bool CrearEventoPersonalizado(int categoria, Event evento, User profesor)
        {
            #region Declaracion de variables
            UserService userService = new UserService(_unidad);
            NotificationService notificationService = new NotificationService(_unidad);

            #endregion

            switch (categoria)
            {
                #region Case: Nuevo evento personal 1 dia
                case ConstantRepository.PERSONAL_EVENT_CATEGORY_1_DAY:
                    #region Evento 
                    evento.Users.Add(profesor);
                    evento.Notifications = notificationService.CrearNotificacionAutomaticaSinSalvadoPantallaEventos(
                    ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERSONAL_EVENT_1_DAY, evento, profesor);
                    
                    #endregion

                    break;
                #endregion
                #region Case: Nuevo evento personal varios dias
                case ConstantRepository.PERSONAL_EVENT_CATEGORY_VARIOUS_DAYS:
                    #region Evento
                    evento.Users.Add(profesor);
                    evento.Notifications = notificationService.CrearNotificacionAutomaticaSinSalvadoPantallaEventos(
                    ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERSONAL_EVENT_VARIOUS_DAYS, evento, profesor);

                    #endregion

                    break;
                #endregion
            };

            #region Salvando el evento perzonalizado
            try
            {

                _unidad.RepositorioEvent.Add(evento);

                _unidad.Save();

                return true;
            }
            #endregion
            #region Catch de errores
            catch (Exception e)
            {
                throw e;
            }
            #endregion
        }
        public bool GuardarEvento(Event evento)
        {
            
            try
            {
                _unidad.RepositorioEvent.Add(evento);
                _unidad.Save();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
        public bool ModificarEvento(Event evento)
        {
            try
            {
                _unidad.RepositorioEvent.Modify(evento);
                _unidad.Save();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        
        #endregion
        #region Obtener Eventos
        /// <summary>
        /// Método que devuelve el evento asociado a una evaluación en particular.
        /// Rodrigo Uzcátegui - 13-05-15
        /// </summary>
        /// <param name="idEvaluacion">Id de la evaluación</param>
        /// <returns>El evento respectivo</returns>
        public Event ObtenerEventoPor_Evaluacion(int idEvaluacion)
        {
            AssessmentService assessmentService = new AssessmentService(this._unidad);
            Assessment assessment = assessmentService.ObtenerEvaluacionPor_Id(idEvaluacion);

            return assessment.Event;
        }
        /// <summary>
        /// Método que obtiene la lista de los próximos # eventos a realizarse.
        /// Rodrigo Uzcátegui - 15-05-15
        /// </summary>
        /// <param name="idUsuario">El id del usuario para los eventos asociados</param>
        /// <param name="nroEventos">El número de eventos a obtener. Si el número es cero (0) o un número 
        /// negativo, obtiene todos los eventos asociados</param>
        /// <returns>La lista de eventos respectiva.</returns>
        public List<Event> ObtenerProximosEventosPor_Usuario(string idUsuario, int nroEventos)
        {
            User usuario = new UserService().ObtenerUsuarioPorId(idUsuario);

            List<Event> listaEventos =
                usuario.Events.Where(m => m.StartDate > DateTime.Now)
                    .OrderBy(o => o.StartDate)
                    .ThenBy(t => t.StartHour)
                    .ToList<Event>();

            if (listaEventos.Count == 0) //No posee eventos asociados
                listaEventos = new List<Event>();
            else
                if (nroEventos > 0)
                    listaEventos = listaEventos.Take(nroEventos).ToList();

            return listaEventos;
        }











        /// <summary>
        /// Método que obtiene la lista de solo los nombres de los eventos del año escolar de la sesión
        /// 
        /// Rodrigo Uzcátegui - 27-02-15
        /// </summary>
        /// <returns>La lista de nombres de los eventos del año escolar</returns>
        public List<string> ObtenerListaNombreEventosPorAñoEscolar()
        {
            List<string> lista = (
                from Event evento in _unidad.RepositorioEvent._dbset
                where evento.StartDate >= _session.STARTDATE &&
                      evento.FinishDate <= _session.DATEOFCOMPLETION
                orderby evento.StartDate
                orderby evento.StartHour
                select evento.Name)
                    .ToHashSet<string>()
                    .ToList<string>();

            return lista;
        }
        /// <summary>
        /// Método que obtiene la lista de los próximos 5 eventos que ocurrirán
        /// </summary>
        /// <param name="idUsuario">el id del usuario</param>
        /// <returns>La lista de eventos</returns>
        public List<Event> ObtenerListaEventosPróximos(string idUsuario)
        {
            User usuario = new UserService().ObtenerUsuarioPorId(idUsuario);

            List<Event> listaEventos = 
                usuario.Events.Where(m => m.StartDate > DateTime.Now)
                    .OrderBy(o => o.StartDate)
                    .ThenBy(t => t.StartHour).Take(5).ToList();

            return listaEventos;
        }
        /// <summary>
        /// Método que obtiene la lista de eventos globales asociados a un año escolar en particular.
        /// </summary>
        /// <returns>La lista de eventos respectiva</returns>
        public List<Event> ObtenerListaEventosGlobalesPor_SAnoEscolar()
        {
            List<Event> lista = (from Event evento in _unidad.RepositorioEvent._dbset
                                 where evento.DeleteEvent == false 
                                       //&& evento.schoolYear_SchoolYearId = _session.SCHOOLYEARID
                                 select evento)
                                    .ToList<Event>();

            return lista;
        }
        /// <summary>
        /// Método que obtiene la lista de eventos por usuario
        /// 
        /// Rodrigo Uzcátegui - 28-03-15
        /// </summary>
        /// <returns>La lista de eventos respectiva</returns>
        public List<Event> ObtenerListaEventosPor_SUsuario()
        {
            UserService service = new UserService(this._unidad);
            User usuario = service.ObtenerUsuarioPorId(_session.USERID);
            List<Event> listaEventos = usuario.Events;

            return listaEventos;
        }
        public List<Event> ObtenerListaEventosPor_SUsuario(DateTime fecha)
        {
            UserService service = new UserService(this._unidad);
            User usuario = service.ObtenerUsuarioPorId(_session.USERID);
            List<Event> listaEventos = new List<Event>();

            foreach (Event evento in usuario.Events)
            {
                if ((evento.StartDate.Month == fecha.Month && 
                     evento.StartDate.Year == fecha.Year) || 
                    (evento.FinishDate.Month == fecha.Month &&
                     evento.FinishDate.Year == fecha.Year))
                    listaEventos.Add(evento);
            }

            return listaEventos;
        }










     //   public Event ObtenerIdEventoPorAtributos(Assessment evaluacion, string nombreevento)
       // {
         //   Event eventoparamodificar = (
           //     from Event evento in _unidad.RepositorioEvent._dbset
             //   where evento.Name == evaluacion.Name &&evento.Description == nombreevento && 
               // evento.StartDate == evaluacion.StartDate && evento.FinishDate == evaluacion.FinishDate &&
          //      evento.StartHour == evaluacion.StartHour
          //            && evento.EndHour == evaluacion.EndHour
          //      select evento).FirstOrDefault<Event>();

//            return eventoparamodificar;
  //      }





        public Event ObtenerEventoPorId(int id)
        {

            Event Eventseleccionado = (from Event evento in _unidad.RepositorioEvent._dbset
                                           .Include("Notifications")
                                       where evento.EventId == id
                                       select evento).FirstOrDefault<Event>();
            return Eventseleccionado;

        }
        public List<Event> ObtenerListaEventos()
        {
            _unidad = new UnitOfWork();
            return _unidad.RepositorioEvent.GetAll().ToList<Event>();
        }

        public List<string> ObtenerListaTiposDeEvento()
        {
            /* Tipo de eventos es la categoría a la que va asociada ese evento, los tipos de eventos 
             * son:
             *    1. Evaluación.
             *    2. Reunión.
             *    3. Día Del Colegio.
             *    4. Feriado.
             *    5. Corte De Notas.             
             */

            List<string> listaTiposEvento = new List<string>();

            listaTiposEvento.Add("Evaluación");
            listaTiposEvento.Add("Reunión");
            listaTiposEvento.Add("Día Del Colegio");
            listaTiposEvento.Add("Feriado");
            listaTiposEvento.Add("Corte De Notas");

            return listaTiposEvento;
        }

        

        
        public List<Event> ObtenerListaEventosPorUsuario(string idUsuario)
        {
            #region Lista eventos del usuario
            User usuario = new UserService().ObtenerUsuarioPorId(idUsuario);
            List<Event> listaEventos = usuario.Events;
            #endregion
            #region Lista eventos globales
            #endregion

            return listaEventos;
        }
        public List<Event> ObtenerListaEventosPróximosHeader(string idUsuario)
        {
            User usuario = new UserService().ObtenerUsuarioPorId(idUsuario);
            List<Event> listaEventos = null;

            if (usuario != null) //Revisar esta validación - Rodrigo Uzcátegui (05-12-15)
                listaEventos = usuario.Events.Where(m => m.StartDate > DateTime.Now).OrderBy(o => o.StartDate).ThenBy(t => t.StartHour).Take(3).ToList();

            return listaEventos;

        }

        /// <summary>
        /// Método que obtiene la lista de los próximos eventos a realizarse para el header del usuario
        /// conectado. Utiliza la variable de sesión del id del usuario.
        /// 
        /// Rodrigo Uzcátegui - 01-03-15
        /// </summary>
        /// <returns></returns>
        public List<Event> ObtenerListaEventosPróximosHeader()
        {
            User usuario = new UserService().ObtenerUsuarioPorId(_session.USERID);
            List<Event> listaEventos = null;

            if (usuario != null) 
                listaEventos = 
                    usuario.Events.Where(m => m.StartDate > DateTime.Now)
                        .OrderBy(o => o.StartDate)
                        .ThenBy(t => t.StartHour).Take(3).ToList();

            return listaEventos;
        }
        #endregion
        #region Otros métodos
   
        #endregion
    }
}