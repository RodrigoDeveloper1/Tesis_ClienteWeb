using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb_Data.Repositories;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb.Models
{
    public class EventosModel : MaestraListaColegiosModel
    {
        public List<Event> ListaEventos { get; set; }
        public List<String> listaTiposEventos { get; set; }

        public int EventId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string StartHour { get; set; }

        public string EndHour { get; set; }

        public string Color { get; set; }

        [Display(Name = "Lista de tipos de evento:")]
        public string TipoEvento { get; set; }
        public SelectList selectListTiposEventos { get; set; }

        public int idAnoEscolar { get; set; }
        [Display(Name = "Año escolar activo respectivo:")]
        public string labelAnoEscolar { get; set; }

        public EventosModel()
        {
            this.listaTiposEventos = ConstantRepository.EVENT_TYPE_LIST.ToList();
        }
    }
}