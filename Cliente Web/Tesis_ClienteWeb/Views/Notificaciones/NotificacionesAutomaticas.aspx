<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.NotificacionesPersonalizadasModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Notificaciones automáticas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-5">
            <div class="form-inline">
                <%: Html.LabelFor(m => m.idColegio, new { @class="form-group cabecera-tips"})%>
                <div class="form-group tip-informacion">
                    *
                    <div class="label label-info tip-mensaje" id="tip-lista-colegios-4">
                        Se muestran solo aquellos colegios activos. 
                    </div> 
                </div>
            </div>
            
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
            new { @class = "form-control selectpicker class-colegios",  @id = "select-colegio" })%>
        </div>

        <!-- Año escolar -->
        <div class="col-xs-3">
            <%: Html.LabelFor(m => m.labelAnoEscolar) %>
            <%: Html.TextBoxFor(m => m.labelAnoEscolar, new { @class="form-control", @id="ano-escolar", 
                @disabled = "disabled"})%>
            <% Html.HiddenFor(m => m.idAnoEscolar); %>
        </div>
                
        <!--Separador normal & con línea -->
        <div class="form-group col-xs-12"></div>
        <div class="col-xs-12"><div class="separador"></div></div>

        <!-- Buzón de lista de notificaciones -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading"><strong>Buzón de notificaciones automáticas:</strong></div>
                <div class="panel-body">                    
                    <!-- Separador -->
                    <div class="col-xs-12">
                        <div class="separador"></div>
                    </div>

                    <!-- Tabla de notificaciones automáticas -->
                    <div class="col-xs-12" id="div-table-lista-notificaciones-automaticas">
                        <table class="table table-striped" id="table-lista-notificaciones-automaticas">
                            <thead>
                                <tr>
                                    <th class="th-tipo">Tipo de alerta</th>
                                    <th class="th-atribucion">Atribuci&oacute;n</th>
                                    <th class="th-fechaenvio">Fecha de env&iacute;o</th>
                                    <th class="th-mensaje centrar">Mensaje</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="td-tipo"></td>
                                    <td class="td-atribucion"></td>
                                    <td class="td-fechaenvio"></td>
                                    <td class="td-mensaje centrar"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
                                
        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
        <div class="row">
            <div class="col-xs-12">
                <div class="separador"></div>
            </div>
        </div>
        <div class="form-group col-xs-12"></div>
        
        <!-- Botón Cancelar -->
        <div class="col-xs-12 text-center">
            <% using (Html.BeginForm("Menu", "Administrador", FormMethod.Get, new
            {
                @class = "form",
                @role = "form"
            }))
               { %>
            <button class="btn btn-lg btn-default" type="submit">
                Cancelar
            </button>
            <% } %>
        </div>

        <!--Separador normal x2 -->
        <div class="form-group col-xs-12"></div>
        <div class="form-group col-xs-12"></div>

        <!-- Diálogo mensaje de la notificación -->
        <div id="dialog-mensaje">
            <div class="row">
                <div class="col-xs-12" id="div-cuerpo-mensaje-notificacion">
                    <p id="p-mensaje-notificacion"></p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Notificaciones/Notificaciones.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Notificaciones/TablaNotificacionesAutomaticas.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Notificaciones/NotificacionesAutomaticas.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Notificaciones automáticas
</asp:Content>