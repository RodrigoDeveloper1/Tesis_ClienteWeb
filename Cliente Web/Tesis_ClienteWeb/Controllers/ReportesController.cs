﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Data.UserExceptions;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Controllers
{
    public class ReportesController : MaestraController
    {
        [HttpGet]
        public ActionResult GenerarReporte(string path)
        {
            string tituloReporte = "";

            if (TempData["Evaluación"] != null) tituloReporte = "Reporte por evaluación - Faro Atenas.pdf";
            else if (TempData["Materia"] != null) tituloReporte = "Reporte por materia - Faro Atenas.pdf";
            else if (TempData["Curso"] != null) tituloReporte = "Reporte por curso - Faro Atenas.pdf";
            else tituloReporte = "Reporte - Faro Atenas.pdf";

            return File(path, "application/pdf", tituloReporte);

            /*#region Descifrando el archivo pdf
            byte[] pdfBytes = Convert.FromBase64String(path);
            MemoryStream ms = new MemoryStream(pdfBytes, 0, pdfBytes.Length);
            ms.Write(pdfBytes, 0, pdfBytes.Length);
            #endregion*/

            /*Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline;filename=data.pdf");
            Response.BufferOutput = true;
            byte[] pdf;
            Response.AddHeader("Content-Length", pdfBytes.Length.ToString());
            Response.BinaryWrite(pdfBytes);
            Response.End();*/

            //return new FileStreamResult(ms, "application/pdf");
        }
    }
}
