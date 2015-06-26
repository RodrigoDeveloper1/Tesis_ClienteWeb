using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    [Authorize]
    public class ColegiosController : MaestraController
    {
        private string _controlador = "Colegios";
        private BridgeController _puente = new BridgeController();

        #region Gestión de colegios
        [HttpGet]
        public ActionResult CrearColegio()
        {
            ConfiguracionInicial(_controlador, "CrearColegio");
            ColegiosModel model = new ColegiosModel();
            #region Sección TempData
            if(TempData["ColegioGuardado"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["ColegioGuardado"].ToString();
            }
            else if (TempData["ColegioDuplicado"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ColegioDuplicado"].ToString());
            }
            else if (TempData["ErrorCrearColegio"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorCrearColegio"].ToString());
            }
            else if (TempData["ModelError"] != null)
            {
                model.MostrarErrores = "block";
                foreach(ModelState error in (List<ModelState>)TempData["ModelError"])
                {
                    if(error.Errors.Count != 0)
                        ModelState.AddModelError("", error.Errors[0].ErrorMessage);
                }
            }
            #endregion
            #region Inicialización de listas de estatus
            model.listaEstatusColegio = _puente.InicializadorListaEstatus(model.listaEstatusColegio);
            model.listaEstatusPeriodoEscolar = _puente.InicializadorListaEstatus(model.listaEstatusPeriodoEscolar);
            #endregion

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearColegio(ColegiosModel model)
        {
            //Si entra en [HttpGet] CrearColegio(), no necesita de ConfiguracionInicial();
            #region Declaración de variables
            School colegio = new School();
            SchoolYear anoEscolar = new SchoolYear();

            UnitOfWork _unidad = new UnitOfWork();
            SchoolService _schoolService = new SchoolService(_unidad);
            SchoolYearService _schoolYearService = new SchoolYearService(_unidad);
            EventService _eventService = new EventService(_unidad);
            #endregion
            #region Validando el modelo
            if (!ModelState.IsValid && (model.estatusColegio == null || model.colegio.Name == null))
            {
                TempData["ModelError"] = ModelState.Select(m => m.Value).ToList();
                return RedirectToAction("CrearColegio");
            }
            #endregion
            #region Definiendo el nuevo colegio
            colegio.Name = model.colegio.Name;
            colegio.DateOfCreation = new DateTime(model.colegio.DateOfCreation.Year,
                model.colegio.DateOfCreation.Month, model.colegio.DateOfCreation.Day);
            colegio.Status = (model.estatusColegio.Equals("Activo") ? true : false);
            colegio.Phone1 = model.colegio.Phone1;
            colegio.Phone2 = model.colegio.Phone2;
            colegio.Address = model.colegio.Address;
            #endregion
            #region Definiendo el año escolar
            if(colegio.Status && model.estatusPeriodoEscolar != null)
            {
                anoEscolar.StartDate = model.FechaInicioPeriodo;
                anoEscolar.EndDate = model.FechaFinalizacionPeriodo;
                anoEscolar.Status = (model.estatusPeriodoEscolar.Equals("Activo") ? true : false);
                anoEscolar.School = colegio;

                colegio.SchoolYears.Add(anoEscolar);
            }
            else
                colegio.SchoolYears = null;
            #endregion
            #region Creando el nuevo colegio
            try
            {
                #region Guardando el colegio
                if (_schoolService.GuardarColegio(colegio))
                {
                    #region Colegio sin año escolar
                    if (colegio.SchoolYears == null)
                        TempData["ColegioGuardado"] = "Se guardó correctamente el colegio sin año escolar.";
                    #endregion
                    #region Colegio con año escolar
                    else
                    {
                        #region Evento automático
                        _eventService.CrearEventoGlobal(
                            ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_SCHOOL_YEAR,
                            colegio.SchoolYears[0]);
                        #endregion

                        TempData["ColegioGuardado"] = "Se guardó correctamente el colegio con su respectivo" +
                            " año escolar.";
                    }
                    #endregion
                }
                #endregion
                #region Catch del error
                else
                    TempData["ColegioDuplicado"] = "Ya existe un colegio con ese nombre";
                #endregion
            }
            #endregion
            #region Catch de errores
            catch (DbUpdateException e)
            {
                //Excepción del índice Unique en el campo nombre de School
                //Rodrigo Uzcátegui - 14-04-15
                if (e.Message.Contains("School_NameIndex"))
                {
                    TempData["ErrorCrearColegio"] = "El colegio '" + colegio.Name + "', ya existe en la base " +
                        "de datos.";
                }                    
                else
                    TempData["ErrorCrearColegio"] = e.Message;
            }            
            catch (Exception e)
            {
                TempData["ErrorCrearColegio"] = e.Message;
            }
            #endregion

            return RedirectToAction("CrearColegio");
        }

        [HttpGet]
        public ActionResult ListarColegios()
        {
            ConfiguracionInicial(_controlador, "ListarColegios");
            #region Inicialización de variables
            ListarColegiosModel model = new ListarColegiosModel();
            SchoolService _schoolService = new SchoolService();
            #endregion
            #region Mensajes de TempData
            if(TempData["ColegioEditado"] != null)
            {
                model.MensajeAclamacion = TempData["ColegioEditado"].ToString();
                model.MostrarAclamaciones = "block";
            }
            else if (TempData["ColegioSuspendido"] != null)
            {
                model.MensajeAclamacion = TempData["ColegioSuspendido"].ToString();
                model.MostrarAclamaciones = "block";
            }
            else if (TempData["ErrorColegioSuspendido"] != null)
            {
                ModelState.AddModelError("", TempData["ColegioSuspendido"].ToString());
                model.MostrarErrores = "block";
            }
            else if (TempData["ColegioHabilitado"] != null)
            {
                model.MensajeAclamacion = TempData["ColegioHabilitado"].ToString();
                model.MostrarAclamaciones = "block";
            }
            else if (TempData["ErrorColegioHabilitado"] != null)
            {
                ModelState.AddModelError("", TempData["ErrorColegioHabilitado"].ToString());
                model.MostrarErrores = "block";
            }
            #endregion
            #region Obteniendo la lista de colegios
            model.listaColegios = _schoolService.ObtenerListaColegios().ToList<School>();
            #endregion

            return View(model);
        }

        [HttpGet]
        public ActionResult EditarColegio(int id)
        {
            ConfiguracionInicial(_controlador, "EditarColegio");
            #region Inicialización de variables
            EditarColegioModel model = new EditarColegioModel();
            SchoolService _schoolService = new SchoolService();
            #endregion
            #region Sección TempData
            if (TempData["Error"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Error"].ToString());
            }
            #endregion
            #region Asignando valores al colegio
            model.colegio = _schoolService.ObtenerColegioPorId(id);
            model.estatus = (model.colegio.Status == true) ? "Activado" : "Desactivado";
            model.fechaCreacion = model.colegio.DateOfCreation.ToShortDateString();
            #endregion

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarColegio(EditarColegioModel model)
        {
            //Si entra en [HttpGet] EditarColegio(int id), no necesita de ConfiguracionInicial();
            #region Validación del modelo
            if (!ModelState.IsValid)
            {
                #region Asignando valores al modelo
                model.estatus = (model.colegio.Status == true) ? "Activado" : "Desactivado";
                model.fechaCreacion = model.colegio.DateOfCreation.ToShortDateString();
                #endregion
                
                model.MostrarErrores = "block";

                return View(model);
            }
            #endregion
            #region Inicialización de variables
            SchoolService _schoolService = new SchoolService();
            #endregion
            #region Actualizando el colegio
            School colegio = _schoolService.ObtenerColegioPorId(model.colegio.SchoolId);
            colegio.Name = model.colegio.Name;
            #endregion
            #region Intentando editar
            try
            {
                _schoolService.ModificarColegio(colegio);
                TempData["ColegioEditado"] = "Se editó correctamente el colegio '" + colegio.Name + "'";

                return RedirectToAction("ListarColegios");
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("EditarColegio", model.idColegio);
            }
            #endregion
        }
        
        [HttpPost]
        public bool SuspenderColegio(int idColegio)
        {
            ConfiguracionInicial(_controlador, "SuspenderColegio");
            SchoolService _schoolService = new SchoolService();

            try
            {
                School colegio = _schoolService.ObtenerColegioPorId(idColegio);
                _schoolService.SuspenderColegio(colegio);

                TempData["ColegioSuspendido"] = "Se suspendió el colegio '" + colegio.Name + "'";

                return true;
            }
            catch (Exception e)
            {
                TempData["ErrorColegioSuspendido"] = e.Message;

                return false;
            }
        }

        [HttpPost]
        public bool HabilitarColegio(int idColegio)
        {
            ConfiguracionInicial(_controlador, "HabilitarColegio");
            SchoolService _schoolService = new SchoolService();

            try
            {
                School colegio = _schoolService.ObtenerColegioPorId(idColegio);

                _schoolService.HabilitarColegio(colegio);

                TempData["ColegioHabilitado"] = "Se habilitó el colegio '" + colegio.Name + "'";

                return true;
            }
            catch (Exception e)
            {
                TempData["ErrorColegioHabilitado"] = e.Message;

                return false;
            }
        }
        #endregion
        #region Gestión de años escolares
        [HttpGet]
        public ActionResult CrearAnoEscolar()
        {
            ConfiguracionInicial(_controlador, "CrearAnoEscolar");
            CrearAnoEscolarModel model = new CrearAnoEscolarModel();

            #region Sección TempData
            if (TempData["AgregadoExitoso"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["AgregadoExitoso"].ToString();
            }
            else if (TempData["ModelStateInvalid"] != null)
            {
                model.MostrarErrores = "block";

                for (int contador = 1; contador <= (int)TempData["ContadorModelStateErrors"]; contador++ )
                {
                    if (TempData["Error_" + contador].ToString() != "")
                        ModelState.AddModelError("", TempData["Error_" + contador].ToString());
                }

                if (TempData["checkBoxList"] != null)
                    ModelState.AddModelError("", TempData["checkBoxList"].ToString());
            }
            else if (TempData["Exception"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Exception"].ToString());
            }
            #endregion

            #region Inicializando listas
            model.listaEstatusPeriodoEscolar = _puente.InicializadorListaEstatus(model.listaEstatusPeriodoEscolar);
            model.listaColegiosPersonales = _puente.InicializarListaColegiosPersonales(model.listaColegiosPersonales);
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult CrearAnoEscolar(CrearAnoEscolarModel model, List<string> checkboxList)
        {
            //Si entra en [HttpGet] CrearAnoEscolar(), no necesita de ConfiguracionInicial();
            #region Validación del modelo
            if (!ModelState.IsValid || checkboxList == null)
            {
                if (model.estatusPeriodoEscolar != null &&
                    !(model.estatusPeriodoEscolar.Equals("Inactivo") && model.anoEscolar.StartDate != null &&
                    model.anoEscolar.EndDate != null) || model.anoEscolar.StartDate == null ||
                    model.anoEscolar.EndDate == null || model.estatusPeriodoEscolar == null)
                {
                    TempData["ModelStateInvalid"] = true;
                    TempData["ContadorModelStateErrors"] = ModelState.Count;
                    int contador = 1;

                    foreach (ModelState error in ModelState.Values)
                    {
                        TempData["Error_" + contador] = (error.Errors.Count != 0 ?
                            error.Errors[0].ErrorMessage : "");
                        contador++;
                    }

                    #region Validación de nros de colegios
                    if (checkboxList == null)
                        TempData["checkBoxList"] =
                            "No se ha seleccionado ningún colegio para asociar el nuevo año escolar.";
                    #endregion

                    return RedirectToAction("CrearAnoEscolar");
                }
            }
            #endregion
            #region Declaración de variables
            List<School> listaColegios = new List<School>();
            UnitOfWork _unidad = new UnitOfWork();
            SchoolService _schoolService = new SchoolService(_unidad);
            EventService _eventService = new EventService(_unidad);

            Period lapsoI;
            Period lapsoII;
            Period lapsoIII;
            string mensaje = "";
            #endregion
            #region Obteniendo colegios seleccionados
            foreach (string idColegio in checkboxList)
            {
                listaColegios.Add(_schoolService.ObtenerColegioPorId(Convert.ToInt32(idColegio)));
            }
            #endregion
            #region Foreach de colegios
            foreach (School colegio in listaColegios)
            {
                #region Inicializando nuevo año escolar
                SchoolYear anoEscolar = new SchoolYear();
                anoEscolar.StartDate = model.anoEscolar.StartDate;
                anoEscolar.EndDate = model.anoEscolar.EndDate;
                anoEscolar.Status = (model.estatusPeriodoEscolar == "Activo" ? true : false);
                #endregion
                #region Inicializando los períodos escolares
                if (anoEscolar.Status)
                {
                    #region Lapso I
                    lapsoI = new Period()
                    {
                        Name = model.lapsoI.Name,
                        StartDate = model.lapsoI.StartDate,
                        FinishDate = model.lapsoI.FinishDate,
                    };
                    #endregion
                    #region Lapso II
                    lapsoII = new Period()
                    {
                        Name = model.lapsoII.Name,
                        StartDate = model.lapsoII.StartDate,
                        FinishDate = model.lapsoII.FinishDate,
                    };
                    #endregion
                    #region Lapso III
                    lapsoIII = new Period()
                    {
                        Name = model.lapsoIII.Name,
                        StartDate = model.lapsoIII.StartDate,
                        FinishDate = model.lapsoIII.FinishDate,
                    };
                    #endregion
                    #region Asociando los períodos al año escolar
                    anoEscolar.Periods.Add(lapsoI);
                    anoEscolar.Periods.Add(lapsoII);
                    anoEscolar.Periods.Add(lapsoIII);
                    #endregion
                }
                #endregion
                #region Asociando el año escolar al colegio
                colegio.SchoolYears.Add(anoEscolar);
                #endregion
                #region Try: Modificando el colegio
                try
                {
                    if (_schoolService.ModificarColegio(colegio))
                    {
                        #region Evento automático                        
                        if (anoEscolar.Periods.Count == 0)
                            _eventService.CrearEventoGlobal(
                                ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_SCHOOL_YEAR, 
                                anoEscolar);
                        else
                            _eventService.CrearEventoGlobal(
                                ConstantRepository.GLOBAL_EVENT_CATEGORY_NEW_SCHOOL_YEAR_WITH_PERIODS,
                                anoEscolar);
                        #endregion
                    }
                }
                #endregion
                #region Catch del error
                catch (Exception e)
                {
                    TempData["Exception"] = e.Message;

                    return RedirectToAction("CrearAnoEscolar");
                }
                #endregion
                #region Creando el mensaje de éxito
                mensaje += colegio.Name + ", ";
                #endregion
            }
            #endregion
            #region Retornando la vista con el colegio recién modificado
            #region Quitando la última coma ',' en el mensaje
            mensaje = mensaje.TrimEnd(',');
            #endregion
            TempData["AgregadoExitoso"] = "Se ha agregado el año a el/los siguiente(s) colegio(s): " + mensaje;

            return RedirectToAction("CrearAnoEscolar");
            #endregion
        }
        #endregion
        #region Gestión de períodos escolares
        [HttpGet]
        public ActionResult CrearPeriodoEscolar()
        {
            ConfiguracionInicial(_controlador, "CrearPeriodoEscolar");
            PeriodoEscolarModel model = new PeriodoEscolarModel();
            #region Sección mensajes TempData
            if (TempData["ErrorAsignacion"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorAsignacion"].ToString());
            }
            else if (TempData["ErrorAnoEscolar"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorAnoEscolar"].ToString());
            }
            else if (TempData["AgregadoExitoso"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["AgregadoExitoso"].ToString();
            }
            else if (TempData["Exception"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Exception"].ToString());
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public bool AsociarPeriodosEscolares(int idAnoEscolar, string fec_ini_1, string fec_fin_1, 
            string fec_ini_2, string fec_fin_2, string fec_ini_3, string fec_fin_3)
        {
            ConfiguracionInicial(_controlador, "CrearPeriodoEscolar");
            #region Validación del modelo
            if (idAnoEscolar == 0)
                TempData["ErrorAnoEscolar"] = "Por favor, seleccione un año escolar";
            else  if (fec_ini_1 == null || fec_fin_1 == "" || fec_ini_2 == null || fec_fin_2 == "" || 
                      fec_ini_3 == null || fec_fin_3 == "")
            {
                TempData["ErrorAsignacion"] = "Error al tratar de asignar los períodos escolares, ¿Está " + 
                    "seguro de haber llenado todos los campos?";
            }
            #endregion
            else
            {
                #region Declaración de servicios
                UnitOfWork unitOfWork = new UnitOfWork();
                SchoolYearService schoolYearService = new SchoolYearService(unitOfWork);
                EventService eventService = new EventService(unitOfWork);
                #endregion

                #region Obteniendo el año escolar
                SchoolYear schoolYear = schoolYearService.ObtenerAnoEscolar(idAnoEscolar);
                #endregion
                #region Declaración del Lapso I
                Period lapsoI = new Period();
                lapsoI.StartDate = Convert.ToDateTime(fec_ini_1);
                lapsoI.FinishDate = Convert.ToDateTime(fec_fin_1);
                lapsoI.Name = ConstantRepository.PERIOD_ONE;
                #endregion
                #region Declaración del Lapso II
                Period lapsoII = new Period();
                lapsoII.StartDate = Convert.ToDateTime(fec_ini_2);
                lapsoII.FinishDate = Convert.ToDateTime(fec_fin_2);
                lapsoII.Name = ConstantRepository.PERIOD_TWO;
                #endregion
                #region Declaración del Lapso III
                Period lapsoIII = new Period();
                lapsoIII.StartDate = Convert.ToDateTime(fec_ini_3);
                lapsoIII.FinishDate = Convert.ToDateTime(fec_fin_3);
                lapsoIII.Name = ConstantRepository.PERIOD_THREE;
                #endregion
                #region Asignando los lapsos al año escolar
                schoolYear.Periods.Add(lapsoI);
                schoolYear.Periods.Add(lapsoII);
                schoolYear.Periods.Add(lapsoIII);
                #endregion
                #region Modificando el año escolar
                try 
                {
                    if(schoolYearService.ModificarColegio(schoolYear))
                    {
                        eventService.CrearEventoGlobal(ConstantRepository.GLOBAL_EVENT_CATEGORY_3_PERIODS, 
                            schoolYear);

                        TempData["AgregadoExitoso"] = "Se asociaron correctamente los lapsos al año escolar.";
                    }
                }
                #endregion
                #region Catch del error
                catch (Exception e)
                {
                    TempData["Exception"] = e.Message;
                }
                #endregion
            }

            return true;
        }
        #endregion
    }
}