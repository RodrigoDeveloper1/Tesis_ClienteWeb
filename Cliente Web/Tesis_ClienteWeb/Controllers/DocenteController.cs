using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class DocenteController : MaestraController
    {
        private string _controlador = "Docente";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult AgregarDocente()
        {
            ConfiguracionInicial(_controlador, "AsociarDocente");
            AgregarDocenteModel model = new AgregarDocenteModel();

            #region Sección TempData
            if (TempData["AgregadoCorrecto"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion =  TempData["AgregadoCorrecto"].ToString();
            }
            else if (TempData["Error"] != null)
            {
                ModelState.AddModelError("", TempData["Error"].ToString());
                model.MostrarErrores = "block";
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public JsonResult AgregarDocente(int idCurso, int idMateria, string idDocente)
        {
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            List<CASU> listaCasus = new List<CASU>();

            CASUService casuService = new CASUService();
            #endregion

            #region Obteniendo la lista de CASUS
            listaCasus = casuService.ObtenerListaCASUPor_Curso_Materia(idCurso, idMateria);
            #endregion
            #region Modificando la lista de CASUS
            foreach(CASU casu in listaCasus)
            {
                casu.TeacherId = idDocente;
                try
                {
                    casuService.ModificarCASU(casu);
                    TempData["AgregadoCorrecto"] = "Se a agregado correctamente el usuario a la materia respectiva";
                    jsonResult.Add(new { success = true });
                }
                catch (Exception e)
                {
                    TempData["Error"] = e.Message;
                    jsonResult.Add(new { success = false });
                }
            }
            #endregion

            return Json(jsonResult);
        }
    }
}