<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EventosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Calendario de eventos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Botón: Crear Evento -->
        <div class="col-xs-12">
            <button class="btn btn-primary" id="btn-crear-evento">
                <strong>Crear Evento</strong>
            </button>
        </div>

        <!-- Separador -->
        <div class="col-xs-12">
            <div class="separador-dialogos"></div>
            <div class="separador"></div>
            <div class="separador-dialogos"></div>
        </div>

        <!-- Panel de Calendario -->
        <div class="col-xs-12">
            <div id='calendar'></div>
        </div>
    </div>

    <!-- Diálogo Nuevo Evento -->
    <div id="dialog-nuevo-evento">
        <div class="row">
            <!-- Título del evento -->
            <div class="col-xs-7">
                <label class="pull-left" for="tituloevento">Título del Evento:</label>
                <input type="text" class="form-control" id="tituloevento"/>
            </div>

            <!-- Color del evento -->
            <div class="col-xs-5" id="div-color">
                <label for="border-color">Color:</label>
                <input type="text" id="colorevento" value="0000ff" name="border-color" class="pick-a-color form-control">
            </div>

            <!-- Separador con línea -->
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Descripción del evento -->
            <div class="col-xs-12">
                <label id="" for="descripcionevento">Descripción del Evento:</label>
                <textarea rows="3" cols="50" class="form-control" id="descripcionevento"></textarea>
            </div>

            <!-- Separador con línea -->
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Separador con línea -->
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Lista de tipos de eventos -->
            <div class="col-xs-12">
                <label for="select-seleccionartipoevento">Tipo:</label>
                <select class="selectpicker form-control" id="select-seleccionartipoevento" 
                data-style="btn-primary" data-live-search="true">
                    <option selected disabled>Seleccione un tipo de evento...</option>
                    
                    <% foreach (var eventType in Model.listaTiposEventos) { %>
                    <option value="<%: eventType %>"><%: eventType%></option>
                    <% } %>
                </select>
            </div>

            <!-- Separador con línea -->
            <div class="col-xs-12"><div class="separador"></div></div>
            
            <!-- Fecha de inicio -->
            <div class="col-xs-5">
                <label for="fecha-inicio">Fecha inicio:</label>
                <input type="text" class="datepicker form-control" id="fecha-inicio" />
            </div>

            <!-- Hora de inicio -->
            <div class="col-xs-4 input-append input-group bootstrap-timepicker" id="div-hora-inicio">
                <span class="input-group-addon" id="span-hora-inicio"><i class="fa fa-clock-o"></i></span>
                <input type="text" id="horainicioevento" class="add-on form-control" aria-describedby="span-hora-inicio">
            </div>
            

            <!-- Separador con línea -->
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Fecha de Finalización -->
            <div class=" col-xs-5">
                <label for="fecha-finalizacion">Fecha fin:</label>
                <input type="text" class="datepicker form-control" id="fecha-finalizacion" />
            </div>

            <!-- Hora fin -->
            <div class="col-xs-4 input-append input-group bootstrap-timepicker" id="div-hora-fin">
                <span class="input-group-addon" id="span-hora-fin"><i class="fa fa-clock-o"></i></span>
                <input type="text" id="horaeventofin" class="add-on form-control" aria-describedby="span-hora-fin">
            </div>
        </div>
    </div>

    <!-- Diálogo mensaje del evento -->
    <div id="dialog-mensaje">
        <div class="row">
            <div class="col-xs-12" id="div-cuerpo-mensaje-evento">
                <p id="p-mensaje-evento"></p>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Plug-ins/fullcalendar-2.1.1/fullcalendar.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/bootstrap-timepicker-gh-pages/css/bootstrap-timepicker.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/fullcalendar-2.1.1/fullcalendar.print.css" rel="stylesheet" type="text/css" media="print"/>
    <link href="../../Content/Plug-ins/pick-a-color-master/build/1.2.3/css/pick-a-color-1.2.3.min.css" rel="stylesheet" type="text/css" />

    <link href="../../Content/Css/Eventos/Eventos.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Eventos/DialogoNuevoEvento.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Content/Plug-ins/bootstrap-timepicker-gh-pages/js/bootstrap-timepicker.min.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/lib/moment.min.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/fullcalendar.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Eventos/Eventos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Eventos/DialogoNuevoEvento.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/lang/es.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/pick-a-color-master/src/js/pick-a-color.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/pick-a-color-master/build/dependencies/tinycolor-0.9.15.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Calendario de eventos
</asp:Content>