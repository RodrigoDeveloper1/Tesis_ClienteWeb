<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EventosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de Eventos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Botón: Crear Evento -->
    <div class="row">
        <div class="row">
            <!-- Lista de colegios -->
            <div class="col-xs-8">
                <%: Html.LabelFor(m => m.idColegio) %>
                <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-colegio-crear" })%>
            </div>

            <!-- Año escolar -->
            <div class="col-xs-4">
                <%: Html.LabelFor(m => m.labelAnoEscolar) %>
                <%: Html.TextBoxFor(m => m.labelAnoEscolar, new { @class="form-control", @id="ano-escolar", 
                @disabled = "disabled"})%>
                <% Html.HiddenFor(m => m.idAnoEscolar); %>
            </div>
        </div>
    </div>

    <!-- Separador -->
    <div class="separador-dialogos"></div>
    <div class="row">
        <div class="col-xs-12">
            <div class="separador"></div>
        </div>
    </div>
    <div class="separador-dialogos"></div>

    <!-- Calendario -->
    <div class="row">
        <div class="col-xs-12">
            <div id='calendar'></div>

            <div style='clear: both'></div>
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
    <!-- Fin Diálogo mensaje del evento -->

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Plug-ins/fullcalendar-2.1.1/fullcalendar.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/bootstrap-timepicker-gh-pages/css/bootstrap-timepicker.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/fullcalendar-2.1.1/fullcalendar.print.css" rel="stylesheet" type="text/css" media="print" />
    <link href="../../Content/Css/Eventos/Eventos.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/pick-a-color-master/build/1.2.3/css/pick-a-color-1.2.3.min.css" rel="stylesheet" type="text/css" />   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Content/Plug-ins/bootstrap-timepicker-gh-pages/js/bootstrap-timepicker.min.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/lib/moment.min.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/fullcalendar.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Eventos/GestionEventos.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/lang/es.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/pick-a-color-master/src/js/pick-a-color.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/pick-a-color-master/build/dependencies/tinycolor-0.9.15.min.js" type="text/javascript"></script>        
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión de eventos
</asp:Content>
