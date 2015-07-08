using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Services
{
    public class NotificationService
    {
        #region Configuración inicial
        private SessionVariablesRepository _session;
        private UnitOfWork _unidad;

        public NotificationService()
        {
            this._unidad = new UnitOfWork();
            _InicializadorVariablesSesion();
        }
        public NotificationService(UnitOfWork unidad)
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
        /// Método que guarda la nueva notificación pasada por parámetro. Consta de 3 fases: La primera cambia 
        /// el estatus en el contexto del registro del usuario que se le asocia como el creador de la
        /// notificación. La segunda fase cambia el estatus de él o los sujetos a quien va dirigida la 
        /// notificación. La tercera fase agrega la nueva notificación al contexto.
        /// 
        /// Rodrigo Uzcátegui - 28-02-15
        /// </summary>
        /// <param name="notification">La notificación a ser guarda</param>
        /// <returns>Booleano que indica si se guardó exitosamente el registro</returns>
        public bool GuardarNotification(Notification notification)
        {
            #region Declaración de variables
            int idSentNotification;
            #endregion

            #region Obteniendo el id de los SentNotifications
            idSentNotification = this.ObtenerIdSentNotification();
            #endregion

            try
            {
                #region Fase #1 - Cambiando el estatus del usuario creador
                _unidad.RepositorioUser.Modify(notification.User);
                #endregion
                #region Fase #2 - Cambiando el estatus del sujeto
                foreach (SentNotification SN in notification.SentNotifications)
                {
                    #region Sujeto -> Representante
                    if (SN.Student != null)
                    {                        
                        _unidad.RepositorioStudent.Modify(SN.Student);

                        SN.SentNotificationId = idSentNotification;
                        idSentNotification++;
                    }
                    #endregion
                    #region Sujeto -> Docente
                    else if (SN.User != null)
                    {
                        _unidad.RepositorioUser.Modify(SN.User);

                        SN.SentNotificationId = idSentNotification;
                        idSentNotification++;
                    }
                    #endregion
                    #region Sujeto -> Curso
                    else if (SN.Course != null)
                    {
                        _unidad.RepositorioCourse.Modify(SN.Course);

                        SN.SentNotificationId = idSentNotification;
                        idSentNotification++;
                    }
                    #endregion
                }
                #endregion
                #region Fase #3 - Agregando la nueva notificación
                _unidad.RepositorioNotification.Add(notification);
                #endregion

                _unidad.Save();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Equals("An entity object cannot be referenced by multiple instances of IEntityChangeTracker."))
                    throw new Exception("Error #04ABZD1. Por favor contacte al equipo de soporte de Faro Atenas.");
                
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Método CRUD - Modificar evaluación
        /// </summary>
        /// <param name="notification">Objeto notificación a modificar</param>
        /// <returns>True = Modificado exitoso.</returns>
        public bool ModificarNotification(Notification notification)
        {
            try
            {
                _unidad.RepositorioNotification.Modify(notification);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Método CRUD - Eliminar notificación.
        /// </summary>
        /// <param name="notificationID">El id de la notificación a eliminar</param>
        /// <returns>True = Eliminado correcto.</returns>
        public bool EliminarNotification(int notificationID)
        {
            try
            {
                _unidad.RepositorioNotification.Delete(u => u.NotificationId == notificationID);
                _unidad.Save();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        /// <summary>
        /// Método que crea el listado de notificaciones según la categoría que se le pase como parámetro. No
        /// guarda en base de datos la lista de notificaciones generadas.
        /// Rodrigo Uzcátegui - 15-05-15
        /// </summary>
        /// <param name="categoria">La categoría de las notificaciones respectivas</param>
        /// <param name="schoolYear">El año escolar utilizado para crear las notificaciones asociadas.</param>
        /// <returns>La lista de notificaciones respectivas</returns>
        public List<Notification> CrearNotificacionAutomatica(int categoria, SchoolYear schoolYear)
        {
            #region Declaración de variables
            bool Automatico = true;

            List<Notification> listaNotificaciones = new List<Notification>();
            Notification notificacion = new Notification(Automatico);
            Notification notificacion2 = new Notification(Automatico);
            Notification notificacion3 = new Notification(Automatico);
            Notification notificacion4 = new Notification(Automatico);
            Notification notificacion5 = new Notification(Automatico);
            Notification notificacion6 = new Notification(Automatico);
            #endregion

            switch (categoria)
            {
                #region Case: Nuevo año escolar
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCHOOL_YEAR:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el año escolar";
                    notificacion.SendDate = schoolYear.StartDate.AddDays(-15);
                    notificacion.SchoolYear = schoolYear;
                    #endregion
                    #region Notificación #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el año escolar";
                    notificacion2.SendDate = schoolYear.StartDate.AddDays(-8);
                    notificacion2.SchoolYear = schoolYear;
                    #endregion
                    #region Notificación #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana comienza el año escolar";
                    notificacion3.SendDate = schoolYear.StartDate.AddDays(-1);
                    notificacion3.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el año escolar";
                    notificacion4.SendDate = schoolYear.EndDate.AddDays(-15);
                    notificacion4.SchoolYear = schoolYear;
                    #endregion
                    #region Notificación #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el año escolar";
                    notificacion5.SendDate = schoolYear.EndDate.AddDays(-8);
                    notificacion5.SchoolYear = schoolYear;
                    #endregion
                    #region Notificación #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el año escolar";
                    notificacion6.SendDate = schoolYear.EndDate.AddDays(-1);
                    notificacion6.SchoolYear = schoolYear;
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso I
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_I:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 1er lapso";
                    notificacion.SendDate = schoolYear.Periods[0].StartDate.AddDays(-14);
                    notificacion.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 1er lapso";
                    notificacion2.SendDate = schoolYear.Periods[0].StartDate.AddDays(-7);
                    notificacion2.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 1er lapso";
                    notificacion3.SendDate = schoolYear.Periods[0].StartDate.AddDays(-1);
                    notificacion3.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 1er lapso";
                    notificacion4.SendDate = schoolYear.Periods[0].StartDate.AddDays(-14);
                    notificacion4.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 1er lapso";
                    notificacion5.SendDate = schoolYear.Periods[0].StartDate.AddDays(-7);
                    notificacion5.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 1er lapso";
                    notificacion6.SendDate = schoolYear.Periods[0].StartDate.AddDays(-1);
                    notificacion6.SchoolYear = schoolYear;
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso II
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_II:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 2do lapso";
                    notificacion.SendDate = schoolYear.Periods[1].StartDate.AddDays(-14);
                    notificacion.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 2do lapso";
                    notificacion2.SendDate = schoolYear.Periods[1].StartDate.AddDays(-7);
                    notificacion2.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 2do lapso";
                    notificacion3.SendDate = schoolYear.Periods[1].StartDate.AddDays(-1);
                    notificacion3.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 2do lapso";
                    notificacion4.SendDate = schoolYear.Periods[1].StartDate.AddDays(-14);
                    notificacion4.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 2do lapso";
                    notificacion5.SendDate = schoolYear.Periods[1].StartDate.AddDays(-7);
                    notificacion5.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 2do lapso";
                    notificacion6.SendDate = schoolYear.Periods[1].StartDate.AddDays(-1);
                    notificacion6.SchoolYear = schoolYear;
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso III
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_III:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 3er lapso";
                    notificacion.SendDate = schoolYear.Periods[2].StartDate.AddDays(-14);
                    notificacion.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 3er lapso";
                    notificacion2.SendDate = schoolYear.Periods[2].StartDate.AddDays(-7);
                    notificacion2.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 3er lapso";
                    notificacion3.SendDate = schoolYear.Periods[2].StartDate.AddDays(-1);
                    notificacion3.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 3er lapso";
                    notificacion4.SendDate = schoolYear.Periods[2].StartDate.AddDays(-14);
                    notificacion4.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 3er lapso";
                    notificacion5.SendDate = schoolYear.Periods[2].StartDate.AddDays(-7);
                    notificacion5.SchoolYear = schoolYear;
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 3er lapso";
                    notificacion6.SendDate = schoolYear.Periods[2].StartDate.AddDays(-1);
                    notificacion6.SchoolYear = schoolYear;
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
            }

            return listaNotificaciones;
        }
        /// <summary>
        /// Método que crea el listado de notificaciones según la categoría que se le pase como parámetro. No
        /// guarda en base de datos la lista de notificaciones generadas.
        /// Rodrigo Uzcátegui - 15-05-15
        /// </summary>
        /// <param name="categoria">La categoría de las notificaciones respectivas</param>
        /// <param name="assessment">La evaluación utilizada para crear las notificaciones asociadas.</param>
        /// <returns>La lista de notificaciones respectivas</returns>
        public List<Notification> CrearNotificacionAutomatica(int categoria, Assessment assessment)
        {
            #region Declaración de variables
            bool Automatico = true;

            List<Notification> listaNotificaciones = new List<Notification>();
            Notification notificacion = new Notification(Automatico);
            Notification notificacion2 = new Notification(Automatico);
            Notification notificacion3 = new Notification(Automatico);
            Notification notificacion4 = new Notification(Automatico);
            Notification notificacion5 = new Notification(Automatico);
            Notification notificacion6 = new Notification(Automatico);
            Notification aux = null;
            #endregion

            switch (categoria)
            {
                #region Case: Nueva evaluación
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENTS:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = assessment.CASU.Subject.Name;
                    notificacion.Message = "Próximo " + assessment.Name + " (" + assessment.Instrument + ") - En 1 semana";
                    notificacion.SendDate = assessment.StartDate.AddDays(-8); //Una semana antes
                    notificacion.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = assessment.CASU.Subject.Name;
                    notificacion2.Message = assessment.Name + " (" + assessment.Instrument + ")" + " - Mañana";
                    notificacion2.SendDate = assessment.StartDate.AddDays(-1);
                    notificacion2.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    #endregion
                    #region Condición -> Evaluación dura más de un día
                    if (assessment.StartDate != assessment.FinishDate)
                    {
                        #region Notificacion #3
                        notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                        notificacion3.Attribution = assessment.CASU.Subject.Name;
                        notificacion3.Message = assessment.Name + " (" + assessment.Instrument + ")" + 
                            ". Finaliza en 1 semana";
                        notificacion3.SendDate = assessment.FinishDate.AddDays(-8);
                        notificacion3.SchoolYear = assessment.CASU.Period.SchoolYear;
                        #endregion
                        #region Notificacion #4
                        notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                        notificacion4.Attribution = assessment.CASU.Subject.Name;
                        notificacion4.Message = assessment.Name + " (" + assessment.Instrument + ")" + 
                            ". Finaliza mañana";
                        notificacion4.SendDate = assessment.FinishDate.AddDays(-1);
                        notificacion4.SchoolYear = assessment.CASU.Period.SchoolYear;
                        #endregion
                        #region Añadiendo a lista de notificaciones
                        listaNotificaciones.Add(notificacion3);
                        listaNotificaciones.Add(notificacion4);
                        #endregion
                    }
                    #endregion
                    break;
                #endregion
                #region Case: Fecha de inicio de evaluación modificada
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_START_DATE:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = assessment.CASU.Subject.Name;
                    notificacion.Message = "Se ha cambiado la fecha de inicio de la evaluación. Ahora " + 
                        "empezará el: " + assessment.StartDate.ToShortDateString();
                    notificacion.SendDate = assessment.StartDate.AddDays(1); //El día siguiente
                    notificacion.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion                    
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = assessment.CASU.Subject.Name;
                    notificacion2.Message = "Próximo " + assessment.Name + " (" + assessment.Instrument + ")" 
                        + " - En 1 semana";
                    notificacion2.SendDate = assessment.StartDate.AddDays(-8); //Una semana antes
                    notificacion2.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = assessment.CASU.Subject.Name;
                    notificacion3.Message = assessment.Name + " (" + assessment.Instrument + ")" + " - Mañana";
                    notificacion3.SendDate = assessment.StartDate.AddDays(-1); //Un día antes
                    notificacion3.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    #endregion

                    #region Eliminando las notificaciones respectivas
                    aux = assessment.Event.Notifications.Where(m => m.Message.Contains("Próximo") || 
                        m.Message.Contains("Mañana")).FirstOrDefault<Notification>();
                    while(true)
                    {
                        if (aux == null)
                            break;
                        else
                        {
                            this.EliminarNotification(aux.NotificationId);
                            aux = assessment.Event.Notifications.Where(m => m.Message.Contains("Próximo") ||
                                m.Message.Contains("Mañana")).FirstOrDefault<Notification>();
                        }
                    }
                    #endregion
                    break;
                #endregion
                #region Case: Fecha de finalización de evaluación modificada
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_FINISH_DATE:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = assessment.CASU.Subject.Name;
                    notificacion.Message = "Se ha cambiado la fecha de finalización de la evaluación. Ahora " +
                        "terminará el: " + assessment.FinishDate.ToShortDateString();
                    notificacion.SendDate = assessment.FinishDate.AddDays(1); //El día siguiente
                    notificacion.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = assessment.CASU.Subject.Name;
                    notificacion2.Message = assessment.Name + " (" + assessment.Instrument + ")" + 
                        ". Finaliza en 1 semana";
                    notificacion2.SendDate = assessment.FinishDate.AddDays(-8);
                    notificacion2.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = assessment.CASU.Subject.Name;
                    notificacion3.Message = assessment.Name + " (" + assessment.Instrument + ")" + 
                        ". Finaliza mañana";
                    notificacion3.SendDate = assessment.FinishDate.AddDays(-1);
                    notificacion3.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    #endregion

                    #region Eliminando las notificaciones respectivas
                    aux = assessment.Event.Notifications.Where(m => m.Message.Contains("Finaliza"))
                            .FirstOrDefault<Notification>();
                    while(true)
                    {
                        if (aux == null)
                            break;
                        else
                        {
                            this.EliminarNotification(aux.NotificationId);
                            aux = assessment.Event.Notifications.Where(m => m.Message.Contains("Finaliza"))
                                .FirstOrDefault<Notification>();
                        }
                    }
                    #endregion
                    break;
                #endregion
                #region Case: Fechas de evaluación modificadas
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_ASSESSMENT_BOTH_DATES:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = assessment.CASU.Subject.Name;
                    notificacion.Message = "Se han cambiado las fechas de inicio y de finalización de " + 
                        "la evaluación. Ahora empezará el: " + assessment.StartDate.ToShortDateString() + 
                        ", y finalizará el: " + assessment.FinishDate.ToShortDateString();
                    notificacion.SendDate = assessment.StartDate.AddDays(1); //El día siguiente
                    notificacion.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = assessment.CASU.Subject.Name;
                    notificacion2.Message = "Próximo " + assessment.Name + " (" + assessment.Instrument + ")" 
                        + " - En 1 semana";
                    notificacion2.SendDate = assessment.StartDate.AddDays(-8); //Una semana antes
                    notificacion2.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = assessment.CASU.Subject.Name;
                    notificacion3.Message = assessment.Name + " (" + assessment.Instrument + ")" + " - Mañana";
                    notificacion3.SendDate = assessment.StartDate.AddDays(-1); //Un día antes
                    notificacion3.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = assessment.CASU.Subject.Name;
                    notificacion4.Message = assessment.Name + " (" + assessment.Instrument + ")" + 
                        ". Finaliza en 1 semana";
                    notificacion4.SendDate = assessment.FinishDate.AddDays(-8);
                    notificacion4.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = assessment.CASU.Subject.Name;
                    notificacion5.Message = assessment.Name + " (" + assessment.Instrument + ")" 
                        + ". Finaliza mañana";
                    notificacion5.SendDate = assessment.FinishDate.AddDays(-1);
                    notificacion5.SchoolYear = assessment.CASU.Period.SchoolYear;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    #endregion

                    #region Eliminando las notificaciones respectivas
                    aux = assessment.Event.Notifications.Where(m => m.Message.Contains("Próximo") ||
                        m.Message.Contains("Mañana") || m.Message.Contains("Finaliza")).FirstOrDefault<Notification>();
                    while(true)
                    {
                        if (aux == null)
                            break;
                        else
                        {
                            this.EliminarNotification(aux.NotificationId);
                            aux = assessment.Event.Notifications.Where(m => m.Message.Contains("Próximo") ||
                                m.Message.Contains("Mañana") || m.Message.Contains("Finaliza")).FirstOrDefault<Notification>();
                        }
                    }
                    #endregion
                    break;
                #endregion
            }

            return listaNotificaciones;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoria"></param>
        /// <param name="evento"></param>
        /// <param name="profesor"></param>
        /// <returns></returns>
        public List<Notification> CrearNotificacionAutomatica(int categoria, Event evento, User profesor)
        {
            #region Declaración de variables
            bool Automatico = true;

            List<Notification> listaNotificaciones = new List<Notification>();
            Notification notificacion = new Notification(Automatico);
            Notification notificacion2 = new Notification(Automatico);
            Notification notificacion3 = new Notification(Automatico);
            Notification notificacion4 = new Notification(Automatico);
            #endregion

            switch (categoria)
            {
                #region Case: Evento de 1 dia
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_EVENT_1_DAY:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_EventoUnDia;
                    notificacion.Message = "En 1 semana inicia: " + evento.Name;
                    notificacion.SendDate = evento.StartDate.AddDays(-8);
                    notificacion.SchoolYear = evento.SchoolYear;
                    #endregion
                    #region Notificación #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_EventoUnDia;
                    notificacion2.Message = "Mañana inicia: " + evento.Name;
                    notificacion2.SendDate = evento.StartDate.AddDays(-1);
                    notificacion2.SchoolYear = evento.SchoolYear;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    #endregion
                    break;
                #endregion
                #region Case: Evento de varios dias
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_EVENT_VARIOUS_DAYS:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion.Attribution = ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias;
                    notificacion.Message = "En 1 semana inicia, " + evento.Name;
                    notificacion.SendDate = evento.StartDate.AddDays(-8);
                    notificacion.SchoolYear = evento.SchoolYear;
                    #endregion
                    #region Notificación #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion2.Attribution = ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias;
                    notificacion2.Message = "Mañana inicia, " + evento.Name;
                    notificacion2.SendDate = evento.StartDate.AddDays(-1);
                    notificacion2.SchoolYear = evento.SchoolYear;
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion3.Attribution = ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias;
                    notificacion3.Message = "En 1 semana finaliza, " + evento.Name;
                    notificacion3.SendDate = evento.FinishDate.AddDays(-8);
                    notificacion3.SchoolYear = evento.SchoolYear;
                    #endregion
                    #region Notificación #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion4.Attribution = ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias;
                    notificacion4.Message = "Mañana finaliza, " + evento.Name;
                    notificacion4.SendDate = evento.FinishDate.AddDays(-1);
                    notificacion4.SchoolYear = evento.SchoolYear;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    #endregion
                    break;
                #endregion

                #region Case: Evento Perzonalizado de 1 dia
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERSONAL_EVENT_1_DAY:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_EventoUnDia;
                    notificacion.Message = "En 1 semana inicia, " + evento.Name;
                    notificacion.SendDate = evento.StartDate.AddDays(-8);
                    notificacion.User = profesor;
                    #endregion
                    #region Notificación #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_EventoUnDia;
                    notificacion2.Message = "Mañana inicia, " + evento.Name;
                    notificacion2.SendDate = evento.StartDate.AddDays(-1);
                    notificacion2.User = profesor;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    #endregion
                    break;
                #endregion
                #region Case: Evento Perzonalizado de varios dias
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERSONAL_EVENT_VARIOUS_DAYS:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion.Attribution = ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias;
                    notificacion.Message = "En 1 semana inicia, " + evento.Name;
                    notificacion.SendDate = evento.StartDate.AddDays(-8);
                    notificacion.User = profesor;
                    #endregion
                    #region Notificación #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion2.Attribution = ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias;
                    notificacion2.Message = "Mañana inicia, " + evento.Name;
                    notificacion2.SendDate = evento.StartDate.AddDays(-1);
                    notificacion2.User = profesor;
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion3.Attribution = ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias;
                    notificacion3.Message = "En 1 semana finaliza, " + evento.Name;
                    notificacion3.SendDate = evento.FinishDate.AddDays(-8);
                    notificacion3.User = profesor;
                    #endregion
                    #region Notificación #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Evento;
                    notificacion4.Attribution = ConstantRepository.EVENT_EVENT_TYPE_EventoVariosDias;
                    notificacion4.Message = "Mañana finaliza, " + evento.Name;
                    notificacion4.SendDate = evento.FinishDate.AddDays(-1);
                    notificacion4.User = profesor;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    #endregion
                    break;
                #endregion
            }

            return listaNotificaciones;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="categoria"></param>
        /// <param name="score"></param>
        /// <param name="docente"></param>
        /// <returns></returns>
        public Notification       CrearNotificacionAutomatica(int categoria, Score score, User docente)
        {
            #region Declaración de variables
            bool Automatico = true;
            Notification notificacion = new Notification(Automatico);
            #endregion

            #region Cálculo de datos
            AssessmentService assessmentService = new AssessmentService(this._unidad);
            SubjectService subjectService = new SubjectService(this._unidad);
            StudentService studentService = new StudentService(this._unidad);

            Assessment assessment = assessmentService.ObtenerEvaluacionPor_Id(score.AssessmentId);
            CASU casu = assessment.CASU;
            int grado = casu.Course.Grade;
            Subject subject = casu.Subject;
            SchoolYear schoolYear = casu.Period.SchoolYear;
            Student student = studentService.ObtenerAlumnoPorId(score.StudentId);
            #endregion

            switch (categoria)
            {
                #region Case: Nueva nota
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCORE:
                    #region Notificación #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_ResultadoNotas;
                    notificacion.Attribution = subject.Name;
                    notificacion.Message = 
                        "Su hijo sacó, " + student.FirstName + " " + student.FirstLastName + ", " +
                        (grado > 6 ? score.NumberScore.ToString() : score.LetterScore) + 
                        " en la evaluación: " + assessment.Name + ".";
                    notificacion.SendDate = DateTime.Now;
                    notificacion.SchoolYear = schoolYear;
                    notificacion.User = docente;
                    #endregion
                    break;
                #endregion
                #region Case: Nota modificada
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_MODIFY_SCORE:
                    #region Notificación #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_ResultadoNotas;
                    notificacion.Attribution = subject.Name;
                    notificacion.Message = 
                        "La nota obtenida por el estudiante " + student.FirstName + " " + student.FirstLastName +
                        " en la evaluación: " + assessment.Name + " (" + assessment.Percentage + "%), ha sido" + 
                        " modificada. El nuevo resultado es: " + (grado > 6 ? score.NumberScore.ToString() + 
                        " puntos." : score.LetterScore);
                    notificacion.SendDate = DateTime.Now;
                    notificacion.SchoolYear = schoolYear;
                    notificacion.User = docente;
                    #endregion
                    break;
                #endregion
            }

            return notificacion;
        }
















        public bool GuardarNotificationNotas(Notification notification)
        {

            try
            {
                
               
                _unidad.RepositorioNotification.Add(notification);
                _unidad.Save();

                return true;
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
      
        }
        
        #endregion
        #region Obtener notificaciones
        /// <summary>
        /// Método que obtiene la notificación según el id por parámetro.
        /// Rodrigo Uzcátegui - 29-06-15
        /// </summary>
        /// <param name="idNotificacion">El id de la notificación</param>
        /// <returns>La notificación respectiva</returns>
        public Notification ObtenerNotificacionPor_Id(int idNotificacion)
        {
            Notification notification = (
                from Notification notifAux in _unidad.RepositorioNotification._dbset
                    .Include("SentNotifications")
                    .Include("Event")
                    .Include("SchoolYear")
                where notifAux.NotificationId == idNotificacion
                select notifAux)
                    .FirstOrDefault<Notification>();

            #region Obteniendo los datos completos de las SentNotifications
            List<SentNotification> listaSN = new List<SentNotification>();
            foreach(SentNotification SN in notification.SentNotifications)
            {
                 listaSN.Add(
                     this.ObtenerSentNotificationPor_Id(notification.NotificationId, SN.SentNotificationId));
            }
            notification.SentNotifications = listaSN;
            #endregion
            
            return notification;
        }
        
        /// <summary>
        /// Método que obtiene la lista de notificaciones personales según el colegio.
        /// </summary>
        /// <returns>La lista de notificaciones personales respectiva.</returns>
        public List<Notification> ObtenerListaNotificacionesPersonalesPor_SColegio()
        {
            List<Notification> listaNotificaciones;

            listaNotificaciones =
                (from Notification notification in _unidad.RepositorioNotification._dbset
                     .Include("User")
                     .Include("SentNotifications.Student.Representatives")
                     .Include("SentNotifications.Course")
                     .Include("SentNotifications.User")
                 where notification.User.School.SchoolId == _session.SCHOOLID &&
                       notification.SendDate >= _session.STARTDATE &&
                       notification.SendDate <= _session.DATEOFCOMPLETION &&
                       notification.Automatic == false
                 select notification).OrderBy(m => m.SendDate).ToList<Notification>();

            return listaNotificaciones;
        }
        public List<Notification> ObtenerListaNotificacionesPersonalesPor_Colegio(int idColegio, 
            DateTime fechaMinima, DateTime fechaMaxima)
        {
            List<Notification> listaNotificaciones;

            listaNotificaciones =
                (from Notification notification in _unidad.RepositorioNotification._dbset
                        .Include("User")
                        .Include("SentNotifications.Student.Representatives")
                        .Include("SentNotifications.Course")
                        .Include("SentNotifications.User")
                 where notification.User.School.SchoolId == idColegio &&
                     notification.SendDate >= fechaMinima &&
                     notification.SendDate <= fechaMaxima &&
                     notification.Automatic == false
                 select notification).OrderBy(m => m.SendDate).ToList<Notification>();

            return listaNotificaciones;
        }

        /// <summary>
        /// Método que obtiene la lista de notificaciones personales según el colegio.
        /// </summary>
        /// <returns>La lista de notificaciones personales respectiva.</returns>
        public List<Notification> ObtenerListaNotificacionesAutomaticasPor_SColegio()
        {
            List<Notification> listaNotificaciones;

            listaNotificaciones =
                (from Notification notification in _unidad.RepositorioNotification._dbset
                     .Include("User")
                     .Include("SentNotifications.Student.Representatives")
                     .Include("SentNotifications.Course")
                     .Include("SentNotifications.User")
                 where notification.User.School.SchoolId == _session.SCHOOLID &&
                       notification.SendDate >= _session.STARTDATE.AddMonths(- ConstantRepository.MONTH_NUMBER_EXTENSION_LIMIT) &&
                       notification.SendDate <= _session.DATEOFCOMPLETION.AddMonths(ConstantRepository.MONTH_NUMBER_EXTENSION_LIMIT) &&
                       notification.Automatic == true
                 select notification).OrderBy(m => m.SendDate).ToList<Notification>();

            return listaNotificaciones;
        }
        public List<Notification> ObtenerListaNotificacionesAutomaticasPor_Colegio(int idColegio,
            DateTime fechaMinima, DateTime fechaMaxima)
        {
            List<Notification> listaNotificaciones;
            DateTime fechaMinimaReal = fechaMinima.AddMonths(-ConstantRepository.MONTH_NUMBER_EXTENSION_LIMIT);
            DateTime fechaMaximaReal = fechaMaxima.AddMonths(ConstantRepository.MONTH_NUMBER_EXTENSION_LIMIT);

            listaNotificaciones =
                (from Notification notification in _unidad.RepositorioNotification._dbset
                 where notification.Automatic == true &&
                       notification.Event.SchoolYear.School.SchoolId == idColegio &&
                       notification.SendDate >= fechaMinimaReal &&
                       notification.SendDate <= fechaMaximaReal
                 select notification).OrderBy(m => m.SendDate).ToList<Notification>();

            return listaNotificaciones;
        }

        public IQueryable<Notification> ObtenerListaNotificacionesAutomaticas()
        {
            IQueryable<Notification> listaNotificaciones;

            try
            {
                listaNotificaciones = _unidad.RepositorioNotification.GetAll()
                    .Where(m => m.Automatic == true).OrderBy(m => m.SendDate);
            }
            catch (Exception e)
            {
                //listaNotificaciones = new List<Notification>();
                throw e;
            }

            return listaNotificaciones;
        }        
        public List<Notification> ObtenerListaNotificacionesPersonales()
        {
            #region Declaración de variables
            List<Notification> listaNotificaciones;
            #endregion
            #region Sección administrador
            #endregion

                listaNotificaciones = _unidad.RepositorioNotification.GetAll()
                    .Where(m => m.Automatic == false).OrderBy(m => m.SendDate).ToList<Notification>();

            return listaNotificaciones;
        }        

        /// <summary>
        /// Método que obtiene la lista de notificaciones respectiva por usuario durante un año escolar en 
        /// cuestión.
        /// Rodrigo Uzcátegui - 10-05-15
        /// </summary>
        /// <param name="idUsuario">Id del usuario</param>
        /// <param name="idAnoEscolar">Id del año escolar</param>
        /// <returns>La lista de notificaciones respectiva</returns>
        public List<Notification> ObtenerListaNotificacionesPor_Docente_AnoEscolar(string idUsuario, 
            int idAnoEscolar)
        {            
            #region Declaración de variables
            List<Notification> listaNotificaciones = new List<Notification>();
            List<Notification> listaAux;
            #endregion
            #region Obteniendo la lista de Cursos asociados al docente
            CourseService courseService = new CourseService();
            List<Course> listaCursos =
                courseService.ObtenerListaCursoPor_Docente_AnoEscolar(idUsuario, idAnoEscolar);
            #endregion
            #region Ciclo por cada curso asociado al docente
            foreach (Course course in listaCursos)
            {
                #region Lista de notificaciones asociadas al curso
                listaAux = (
                from SentNotification n in _unidad.RepositorioSentNotification._dbset
                    .Include("Notification.SentNotifications")
                    .Include("Student")
                    .Include("User")
                    .Include("Course")
                where n.Course.CourseId == course.CourseId
                select n.Notification)
                    .ToList<Notification>();
                #endregion
                #region Llenando la lista de notificaciones
                foreach (Notification notification in listaAux)
                {
                    listaNotificaciones.Add(this.ObtenerNotificacionPor_Id(notification.NotificationId));
                }
                #endregion
            }
            #endregion
            #region Lista de notificaciones dirigidas al docente
            listaAux = (
                from SentNotification n in _unidad.RepositorioSentNotification._dbset
                    .Include("Notification.SentNotifications")
                    .Include("Student")
                    .Include("User")
                    .Include("Course")
                where n.User.Id == idUsuario
                select n.Notification)
                    .ToHashSet<Notification>()
                    .ToList<Notification>();
            #endregion
            #region Llenando la lista de notificaciones
            foreach (Notification notification in listaAux)
            {
                listaNotificaciones.Add(this.ObtenerNotificacionPor_Id(notification.NotificationId));
            }
            #endregion
            #region Reordenando la lista
            listaNotificaciones = listaNotificaciones
                .OrderByDescending(m => m.DateOfCreation)
                .ThenByDescending(m => m.SendDate)
                .ToList<Notification>();
            #endregion

            return listaNotificaciones;
        }
        
        /// <summary>
        /// Método que obtiene la lista de notificaciones enviadas por un usuario (docente) en particular.
        /// Rodrigo Uzcátegui - 07-07-15
        /// </summary>
        /// <param name="idUsuario">El id del usuario</param>
        /// <returns>La lista de notificaciones respectiva</returns>
        public List<Notification> ObtenerListaNotificacionesEnviadasPor_Usuario(string idUsuario)
        {
            List<Notification> listaNotificaciones = (
                from Notification n in _unidad.RepositorioNotification._dbset
                    .Include("SentNotifications")
                    .Include("User")
                    .Include("Event")
                    .Include("SchoolYear")
                where n.User.Id == idUsuario
                select n)
                    .OrderByDescending(m => m.DateOfCreation)
                    .ThenByDescending(m => m.SendDate)
                    .ToList<Notification>();

            return listaNotificaciones;
        }
        #endregion
        #region Obtener SentNotifications
        /// <summary>
        /// Método que obtiene el SentNotifications respectivo según los id por parámetro.
        /// Rodrigo Uzcátegui - 29-06-15
        /// </summary>
        /// <param name="NotificationId">El id de la notificación asociada</param>
        /// <param name="SentNotificationId">El id del SentNotification asociado</param>
        /// <returns>El SentNotification respectivo</returns>
        public SentNotification ObtenerSentNotificationPor_Id(int NotificationId, int SentNotificationId)
        {
            SentNotification SN = (
                from SentNotification SNAux in _unidad.RepositorioSentNotification._dbset
                    .Include("Notification")
                    .Include("Student")
                    .Include("User")
                    .Include("Course")
                where SNAux.NotificationId == NotificationId &&
                      SNAux.SentNotificationId == SentNotificationId
                select SNAux)
                    .FirstOrDefault<SentNotification>();

            return SN;
        }
        /// <summary>
        /// Método que obtiene el SentNotification asociado según el id de la notificación y el usuario 
        /// remitido. 
        /// Rodrigo Uzcátegui - 29-06-15
        /// </summary>
        /// <param name="NotificationId">Id de la notificación respectiva.</param>
        /// <param name="UserId">El id del usuario dirigido.</param>
        /// <returns>El SentNotification respectivo</returns>
        public SentNotification ObtenerSentNotificationPor_Notification_DocenteDirigido(int NotificationId, 
            string UserId)
        {
            SentNotification SN = (
               from SentNotification SNAux in _unidad.RepositorioSentNotification._dbset
                   .Include("Notification")
                   .Include("Student")
                   .Include("User")
                   .Include("Course")
               where SNAux.NotificationId == NotificationId &&
                     SNAux.User.Id == UserId
               select SNAux)
                   .FirstOrDefault<SentNotification>();

            return SN;
        }
        #endregion
        #region Otros métodos
        


        public List<string> ObtenerListaTiposAlerta()
        {
            /* Tipo de alerta es la categoría a la que va asociada esa notificación, los tipos de categorías 
             * son:
             *    1. Aviso de evaluación.
             *    2. Aviso de evento.
             *    3. Aviso de corte de notas.
             *    4. Aviso de citación.
             *    5. Aviso de inasistencia.
             *    6. Amonestación.
             *    7. Resultado de notas.
             */

            List<string> listaTiposAlertas = new List<string>();

            listaTiposAlertas.Add("Aviso de evaluación");
            listaTiposAlertas.Add("Aviso de evento");
            listaTiposAlertas.Add("Aviso de corte de notas");
            listaTiposAlertas.Add("Aviso de citación");
            listaTiposAlertas.Add("Aviso de inasistencia");
            listaTiposAlertas.Add("Amonestación");
            listaTiposAlertas.Add("Resultado de notas");

            return listaTiposAlertas;
        }
        
        /// <summary>
        /// Método que obtiene la lista de atribuciones de los eventos de ese colegio. La lista de atribuciones
        /// se llena de la siguiente forma:
        ///     1. N/A 
        ///     2. Todos los nombres de las materias.
        ///     3. Todos los nombres de los eventos.
        /// 
        /// Rodrigo Uzcátegui - 27/02/15
        /// </summary>
        /// <returns>La lista de atribuciones respectiva</returns>
        public List<string> ObtenerListaAtribuciones()
        {
            #region Declaración de variables
            List<string> lista = new List<string>();
            SubjectService subjectService = new SubjectService();
            EventService eventService = new EventService();
            #endregion

            lista.Add(ConstantRepository.NOTIFICATION_ATTRIBUTION_NA);
            lista = subjectService.ObtenerListaNombreMateriasPor_SAnoEscolar();
            lista = eventService.ObtenerListaNombreEventosPorAñoEscolar();
            lista.Add(ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar);
            lista.Add(ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar);
            lista.Add(ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar);
            lista.Add(ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar);

            return lista;
        }

        /// <summary>
        /// Método que obtiene la lista de atribuciones de los eventos de ese colegio. La lista de atribuciones
        /// se llena de la siguiente forma:
        ///     1. N/A 
        ///     2. Todos los nombres de las materias.
        ///     3. Todos los nombres de los eventos.
        /// 
        /// Rodrigo Uzcátegui - 27/02/15
        /// </summary>
        /// <returns>El diccionario de atribuciones respectiva</returns>
        public Dictionary<string, string> ObtenerDiccionarioAtribuciones()
        {
            #region Declaración de variables
            Dictionary<string, string> lista = new Dictionary<string, string>();
            List<string> listaMaterias = new List<string>();
            List<string> listaEventos = new List<string>();
            SubjectService subjectService = new SubjectService();
            EventService eventService = new EventService();
            #endregion

            #region Obteniendo las listas
            listaMaterias = subjectService.ObtenerListaNombreMateriasPor_SAnoEscolar();
            listaEventos = eventService.ObtenerListaNombreEventosPorAñoEscolar();
            #endregion

            #region Llenando el diccionario
            lista.Add("N/A","N/A");
            foreach (string materia in listaMaterias) { lista.Add(materia, materia); }
            foreach (string evento in listaEventos) { lista.Add(evento, evento); }
            #endregion

            return lista;
        }

        /// <summary>
        /// Método que saca el número de notificaciones en bd y le suma uno a ese valor para tener un nuevo id
        /// a una nueva notificación.
        /// 
        /// Rodrigo Uzcátegui - 28-02-15
        /// </summary>
        /// <returns>El número de notificaciones en bd más 1</returns>
        public int ObtenerIdSentNotification()
        {
            int nroSentNotifications = _unidad.RepositorioSentNotification.GetAll().Count();
            nroSentNotifications++;

            return nroSentNotifications;
        }

        /// <summary>
        /// Método que obtiene por el usuario de la sesión iniciada, el número de notificaciones que no ha 
        /// leído.
        /// 
        /// Rodrigo Uzcátegui - 01-03-15
        /// </summary>
        /// <returns>El número de notificaciones no leídas</returns>
        public int ObtenerNumeroNotificacionesNoLeidas()
        {
            int nroNotificacionesNoLeidas = (
                from SentNotification SN in _unidad.RepositorioSentNotification._dbset
                where SN.User.Id == _session.USERID &&
                      SN.Read == false
                select SN)
                    .ToList<SentNotification>()
                    .Count();

            return nroNotificacionesNoLeidas;
        }

        public List<Notification> CrearNotificacionAutomaticaSinSalvado(int categoria, DateTime fechaInicio,
            DateTime fechaFin, Assessment evaluacion, CASU salon)
        {
            #region Declaración de variables
            bool Automatico = true;

            List<Notification> listaNotificaciones = new List<Notification>();
            Notification notificacion = new Notification(Automatico);
            Notification notificacion2 = new Notification(Automatico);
            Notification notificacion3 = new Notification(Automatico);
            Notification notificacion4 = new Notification(Automatico);
            Notification notificacion5 = new Notification(Automatico);
            Notification notificacion6 = new Notification(Automatico);
            #endregion

            switch (categoria)
            {
                #region Case: Nuevo año escolar
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCHOOL_YEAR:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el año escolar";
                    notificacion.SendDate = fechaInicio.AddDays(-15);
                    #endregion
                    #region Notificación #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el año escolar";
                    notificacion2.SendDate = fechaInicio.AddDays(-8);
                    #endregion
                    #region Notificación #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana comienza el año escolar";
                    notificacion3.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el año escolar";
                    notificacion4.SendDate = fechaFin.AddDays(-15);
                    #endregion
                    #region Notificación #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el año escolar";
                    notificacion5.SendDate = fechaInicio.AddDays(-8);
                    #endregion
                    #region Notificación #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el año escolar";
                    notificacion6.SendDate = fechaInicio.AddDays(-1);
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso I
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_I:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 1er lapso";
                    notificacion.SendDate = fechaInicio.AddDays(-15);
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 1er lapso";
                    notificacion2.SendDate = fechaInicio.AddDays(-8);
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 1er lapso";
                    notificacion3.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 1er lapso";
                    notificacion4.SendDate = fechaFin.AddDays(-15);
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 1er lapso";
                    notificacion5.SendDate = fechaFin.AddDays(-8);
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 1er lapso";
                    notificacion6.SendDate = fechaFin.AddDays(-1);
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso II
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_II:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 2do lapso";
                    notificacion.SendDate = fechaInicio.AddDays(-14);
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 2do lapso";
                    notificacion2.SendDate = fechaInicio.AddDays(-7);
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 2do lapso";
                    notificacion3.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 2do lapso";
                    notificacion4.SendDate = fechaFin.AddDays(-14);
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 2do lapso";
                    notificacion5.SendDate = fechaFin.AddDays(-7);
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 2do lapso";
                    notificacion6.SendDate = fechaFin.AddDays(-1);
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso III
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_III:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 3er lapso";
                    notificacion.SendDate = fechaInicio.AddDays(-14);
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 3er lapso";
                    notificacion2.SendDate = fechaInicio.AddDays(-7);
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 3er lapso";
                    notificacion3.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 3er lapso";
                    notificacion4.SendDate = fechaFin.AddDays(-14);
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 3er lapso";
                    notificacion5.SendDate = fechaFin.AddDays(-7);
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 3er lapso";
                    notificacion6.SendDate = fechaFin.AddDays(-1);
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Evaluación
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENTS:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = salon.Subject.Name;
                    notificacion.Message = "Próximo " + evaluacion.Instrument + " - En 1 semana";
                    notificacion.SendDate = fechaInicio.AddDays(-8);
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = salon.Subject.Name;
                    notificacion2.Message = evaluacion.Instrument + " - Mañana";
                    notificacion2.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    #endregion
                    #region Condición -> Evaluación dura más de un día
                    if(fechaInicio != fechaFin)
                    {
                        #region Notificacion #3
                        notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                        notificacion3.Attribution = salon.Subject.Name;
                        notificacion3.Message = evaluacion.Instrument + ". Finaliza en 1 semana";
                        notificacion3.SendDate = fechaFin.AddDays(-8);
                        #endregion
                        #region Notificacion #4
                        notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                        notificacion4.Attribution = salon.Subject.Name;
                        notificacion4.Message = evaluacion.Instrument + ". Finaliza mañana";
                        notificacion4.SendDate = fechaFin.AddDays(-1);
                        #endregion

                        #region Añadiendo a lista de notificaciones
                        listaNotificaciones.Add(notificacion3);
                        listaNotificaciones.Add(notificacion4);
                        #endregion
                    }
                    #endregion
                    
                    break;
                #endregion
            }

            return listaNotificaciones;
        }

        public List<Notification> CrearNotificacionAutomaticaSinSalvado_S(int categoria, DateTime fechaInicio,
           DateTime fechaFin, Assessment evaluacion, CASU salon)
        {
            #region Declaración de variables
            bool Automatico = true;
            SessionVariablesRepository _session = new SessionVariablesRepository();
            SchoolYearService _schoolYearService = new SchoolYearService();
            SchoolYear anoEscolar = _schoolYearService.ObtenerAnoEscolar(_session.SCHOOLYEARID);
            List<Notification> listaNotificaciones = new List<Notification>();
            Notification notificacion = new Notification(Automatico);
            Notification notificacion2 = new Notification(Automatico);
            Notification notificacion3 = new Notification(Automatico);
            Notification notificacion4 = new Notification(Automatico);
            Notification notificacion5 = new Notification(Automatico);
            Notification notificacion6 = new Notification(Automatico);
            #endregion

            switch (categoria)
            {
                #region Case: Nuevo año escolar
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_SCHOOL_YEAR:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el año escolar";
                    notificacion.SendDate = fechaInicio.AddDays(-15);
                    #endregion
                    #region Notificación #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el año escolar";
                    notificacion2.SendDate = fechaInicio.AddDays(-8);
                    #endregion
                    #region Notificación #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana comienza el año escolar";
                    notificacion3.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el año escolar";
                    notificacion4.SendDate = fechaFin.AddDays(-15);
                    #endregion
                    #region Notificación #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el año escolar";
                    notificacion5.SendDate = fechaInicio.AddDays(-8);
                    #endregion
                    #region Notificación #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el año escolar";
                    notificacion6.SendDate = fechaInicio.AddDays(-1);
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso I
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_I:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 1er lapso";
                    notificacion.SendDate = fechaInicio.AddDays(-15);
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 1er lapso";
                    notificacion2.SendDate = fechaInicio.AddDays(-8);
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 1er lapso";
                    notificacion3.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 1er lapso";
                    notificacion4.SendDate = fechaFin.AddDays(-15);
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 1er lapso";
                    notificacion5.SendDate = fechaFin.AddDays(-8);
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 1er lapso";
                    notificacion6.SendDate = fechaFin.AddDays(-1);
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso II
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_II:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 2do lapso";
                    notificacion.SendDate = fechaInicio.AddDays(-14);
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 2do lapso";
                    notificacion2.SendDate = fechaInicio.AddDays(-7);
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 2do lapso";
                    notificacion3.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 2do lapso";
                    notificacion4.SendDate = fechaFin.AddDays(-14);
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 2do lapso";
                    notificacion5.SendDate = fechaFin.AddDays(-7);
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 2do lapso";
                    notificacion6.SendDate = fechaFin.AddDays(-1);
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Nuevo período escolar - Lapso III
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_NEW_PERIOD_III:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion.Message = "En 2 semanas inicia el 3er lapso";
                    notificacion.SendDate = fechaInicio.AddDays(-14);
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion2.Message = "En 1 semana inicia el 3er lapso";
                    notificacion2.SendDate = fechaInicio.AddDays(-7);
                    #endregion
                    #region Notificacion #3
                    notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion3.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion3.Message = "Mañana inicia el 3er lapso";
                    notificacion3.SendDate = fechaInicio.AddDays(-1);
                    #endregion
                    #region Notificacion #4
                    notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion4.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion4.Message = "En 2 semanas finaliza el 3er lapso";
                    notificacion4.SendDate = fechaFin.AddDays(-14);
                    #endregion
                    #region Notificacion #5
                    notificacion5.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion5.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion5.Message = "En 1 semana finaliza el 3er lapso";
                    notificacion5.SendDate = fechaFin.AddDays(-7);
                    #endregion
                    #region Notificacion #6
                    notificacion6.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion6.Attribution = ConstantRepository.NOTIFICATION_ATTRIBUTION_PeriodoEscolar;
                    notificacion6.Message = "Mañana finaliza el 3er lapso";
                    notificacion6.SendDate = fechaFin.AddDays(-1);
                    #endregion

                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    listaNotificaciones.Add(notificacion3);
                    listaNotificaciones.Add(notificacion4);
                    listaNotificaciones.Add(notificacion5);
                    listaNotificaciones.Add(notificacion6);
                    #endregion
                    break;
                #endregion
                #region Case: Evaluación
                case ConstantRepository.AUTOMATIC_NOTIFICATIONS_CATEGORY_ASSESSMENTS:
                    #region Notificacion #1
                    notificacion.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion.Attribution = salon.Subject.Name;
                    notificacion.Message = "Próximo " + evaluacion.Instrument + " - En 1 semana";
                    notificacion.SendDate = fechaInicio.AddDays(-8);
                    notificacion.SchoolYear = anoEscolar;
                    #endregion
                    #region Notificacion #2
                    notificacion2.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                    notificacion2.Attribution = salon.Subject.Name;
                    notificacion2.Message = evaluacion.Instrument + " - Mañana";
                    notificacion2.SendDate = fechaInicio.AddDays(-1);
                    notificacion2.SchoolYear = anoEscolar;
                    #endregion
                    #region Añadiendo a lista de notificaciones
                    listaNotificaciones.Add(notificacion);
                    listaNotificaciones.Add(notificacion2);
                    #endregion
                    #region Condición -> Evaluación dura más de un día
                    if (fechaInicio != fechaFin)
                    {
                        #region Notificacion #3
                        notificacion3.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                        notificacion3.Attribution = salon.Subject.Name;
                        notificacion3.Message = evaluacion.Instrument + ". Finaliza en 1 semana";
                        notificacion3.SendDate = fechaFin.AddDays(-8);
                        notificacion3.SchoolYear = anoEscolar;
                        #endregion
                        #region Notificacion #4
                        notificacion4.AlertType = ConstantRepository.NOTIFICATION_ALERT_TYPE_Aviso;
                        notificacion4.Attribution = salon.Subject.Name;
                        notificacion4.Message = evaluacion.Instrument + ". Finaliza mañana";
                        notificacion4.SendDate = fechaFin.AddDays(-1);
                        notificacion4.SchoolYear = anoEscolar;
                        #endregion

                        #region Añadiendo a lista de notificaciones
                        listaNotificaciones.Add(notificacion3);
                        listaNotificaciones.Add(notificacion4);
                        #endregion
                    }
                    #endregion

                    break;
                #endregion
            }

            return listaNotificaciones;
        }
        

        
        #endregion
    }
}
