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
        <div class="row div-interior-dialog">

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-10">
                    <label id="" class="pull-left" for="input-titulo-evento">Título del Evento:</label>
                    <input type="text" class="form-control pull-right" id="tituloevento" />
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-10">
                    <label id="" class="pull-left" for="descripcionevento">Descripción del Evento:</label>
                    <textarea rows="3" cols="50" type="text" class="form-control pull-right" id="descripcionevento">
            </textarea>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-10">
                    <label for="border-color">Color:</label>
                    <input type="text" id="colorevento" value="0000ff" name="border-color" class="pick-a-color form-control">
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-10 ">
                    <label for="select-seleccionartipoevento">Tipo:</label>
                    <select class="selectpicker form-control" id="select-seleccionartipoevento" data-style="btn-primary"
                        data-live-search="true">
                        <option selected disabled>Seleccione un tipo de evento...</option>

                        <% foreach (var eventType in Model.listaTiposEventos)
                           { %>

                        <option value="<%: eventType %>"><%: eventType%></option>
                        <% } %>
                    </select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
            </div>

            <div class="row">
                <!-- Fecha de Inicio -->
                <div class=" col-xs-10">
                    <label for="fecha-inicio">Fecha inicio:</label>
                    <input type="text" class="datepicker form-control" id="fecha-inicio" />
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
            </div>

            <div class="row">
                <!-- Hora inicio -->
                <div class="col-xs-5 input-append bootstrap-timepicker">
                    <label for="horainicioevento">Hora Inicio:</label>
                    <input type="text" id="horainicioevento" class="add-on form-control ">
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
            </div>

            <div class="row">
                <!-- Fecha de Finalización -->
                <div class=" col-xs-10">
                    <label for="fecha-finalizacion">Fecha finalización:</label>
                    <input type="text" class="datepicker form-control" id="fecha-finalizacion" />
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
            </div>

            <div class=" row">
                <!-- Hora fin -->
                <div class="col-xs-5 input-append bootstrap-timepicker">
                    <label for="horaeventofin">Hora Fin:</label>
                    <input type="text" id="horaeventofin" class="add-on form-control">
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12">
                    <div class="separador"></div>
                </div>
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
    <link href="../../Content/Css/Eventos/Eventos.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/pick-a-color-master/build/1.2.3/css/pick-a-color-1.2.3.min.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Content/Plug-ins/bootstrap-timepicker-gh-pages/js/bootstrap-timepicker.min.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/lib/moment.min.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/fullcalendar.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Eventos/Eventos.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/fullcalendar-2.1.1/lang/es.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/pick-a-color-master/src/js/pick-a-color.js" type="text/javascript"></script>
    <script src="../../Content/Plug-ins/pick-a-color-master/build/dependencies/tinycolor-0.9.15.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Calendario de eventos
</asp:Content>
