<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.NotificacionesPersonalizadasModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Notificaciones personalizadas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Botón Nueva Notificación -->
        <div class="col-xs-12">
            <button type="button" class="btn btn-primary" id="btn-nueva-notificacion">
                Enviar nueva notificaci&oacute;n
            </button>
        </div>

        <!--Separador normal & con línea -->
        <div class="form-group col-xs-12"></div>
        <div class="col-xs-12"><div class="separador"></div></div>

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
                <div class="panel-heading"><strong>Buzón de notificaciones personalizadas:</strong></div>
                <div class="panel-body">                    
                    <!-- Separador -->
                    <div class="col-xs-12">
                        <div class="separador"></div>
                    </div>

                    <!-- Tabla de notificaciones personalizadas -->
                    <div class="col-xs-12" id="div-table-lista-notificaciones-personalizadas">
                        <table class="table table-striped" id="table-lista-notificaciones-personalizadas">
                            <thead>
                                <tr>
                                    <th class="th-tipo">Tipo de alerta</th>
                                    <th class="th-atribucion">Atribuci&oacute;n</th>
                                    <th class="th-sujeto">Sujeto</th>
                                    <th class="th-enviadopor">Enviado por:</th>
                                    <th class="th-curso">Curso</th>
                                    <th class="th-fechaenvio">Fecha de env&iacute;o</th>
                                    <th class="th-mensaje centrar">Mensaje</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="td-tipo"></td>
                                    <td class="td-atribucion"></td>
                                    <td class="td-sujeto"></td> 
                                    <td class="td-enviadopor"></td>
                                    <td class="td-curso"></td>
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

        <!-- Diálogo nueva Notificación -->
        <div id="dialog-nueva-notificacion">
            <!-- Select opciones de sujetos -->
            <div class="col-xs-6">
                <%: Html.LabelFor(m => m.idSujeto)%>
                <%: Html.DropDownListFor(m => m.idSujeto, Model.selectListSujetos, "Tipo de sujeto...", 
                    new { @class = "form-control selectpicker", @id = "select-sujeto"})%>
            </div>
            
            <!-- Select del curso -->
            <div class="col-xs-6">
                <%: Html.LabelFor(m => m.idCurso)%>
                <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione un curso", 
                    new { @class = "form-control selectpicker", @id = "select-cursos", @disabled = "disabled"})%>
            </div>

            <!--Separador normal -->
            <div class="form-group col-xs-12"></div>

            <!-- Select sujeto -->
            <div class="col-xs-12" id="select-sujeto-2">
                <%: Html.LabelFor(m => m.idElSujeto)%>
                <%: Html.DropDownListFor(m => m.idElSujeto, Model.selectListElSujeto, "Seleccione el sujeto...", 
                    new { @class = "form-control selectpicker", @id = "select-el-sujeto"})%>
            </div>

            <!--Separador normal & con línea -->
            <div class="form-group col-xs-12"></div>
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Select tipos de alerta -->
            <div class="col-xs-12" id="select-tipos-notificacion">
                <%: Html.LabelFor(m => m.idTipoNotificacion)%>
                <%: Html.DropDownListFor(m => m.idTipoNotificacion, Model.selectListTiposNotificacion, 
                    "Seleccione el tipo de notificación...", new { @class = "form-control selectpicker", 
                    @id = "select-tipo-notificacion", @disabled = "disabled"})%>
            </div>

            <!--Separador normal & con línea -->
            <div class="form-group col-xs-12"></div>
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Select tipo de atribución -->
            <div class="col-xs-12" id="div-select-atribucion">
                <div class="form-inline">
                    <%: Html.LabelFor(m => m.idAtribucion, new { @class="form-group cabecera-tips"})%>
                    <div class="form-group tip-informacion">
                        *
                        <div class="label label-info tip-mensaje" id="tip-lista-atribucion">
                            Categoría que se le adjudica a la notificación. El campo es opcional.
                        </div> 
                    </div>
                </div>

                <%: Html.DropDownListFor(m => m.idAtribucion, Model.selectListAtribucion, "Seleccione la atribución...", 
                    new { @class = "form-control selectpicker", @id = "select-atribucion" })%>
            </div>

            <!--Separador normal & con línea -->
            <div class="form-group col-xs-12"></div>
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Mensaje de la notificación -->
            <div class="row" id="fila-mensaje-nueva-notificacion">
                <div class="form-group col-xs-12">
                    <%: Html.LabelFor(m => m.mensaje, new { @class="pull-left" })%>
                
                    <%: Html.TextAreaFor(m => m.mensaje, new { @class = "form-control", @rows = "4", 
                            @style = "resize: none", @id = "text-area-notificacion" })%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Notificaciones/Notificaciones.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Notificaciones/NuevaNotificacion.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Notificaciones/TablaNotificaciones.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Notificaciones/NotificacionesPersonalizadas.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Notificaciones/NuevaNotificacion.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Notificaciones personalizadas
</asp:Content>