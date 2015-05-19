<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CrearAnoEscolarModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear año escolar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Formulario Año y períodos escolares -->
        <div class="col-xs-6">
            <% using (Html.BeginForm("CrearAnoEscolar", "Colegios", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
            <%: Html.AntiForgeryToken() %>

            <!-- Título Año escolar -->
            <div class="col-xs-12">
                <h4 class="subtitulos-ano-escolar">Datos - Año escolar:</h4>
            </div>

            <!-- Datos año escolar -->
            <!-- Fecha de Inicio -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.anoEscolar.StartDate) %>
                <%: Html.TextBoxFor(m => m.anoEscolar.StartDate, new { @class="datepicker form-control",  
                    @id="fecha-inicio"})%>
            </div>

            <!-- Fecha de Finalización -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.anoEscolar.EndDate) %>
                <%: Html.TextBoxFor(m => m.anoEscolar.EndDate, new { @class="datepicker form-control", 
                    @id="fecha-finalizacion"})%>
            </div>

            <!-- Estatus del año escolar -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.estatusPeriodoEscolar) %>
                <%: Html.DropDownListFor(m => m.estatusPeriodoEscolar, Model.listaEstatusPeriodoEscolar, 
                "Seleccione un estatus", new { @class = "form-control selectpicker", 
                    @id = "select-estatus-periodo"})%>
            </div>

            <!-- Separador formularios -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Separador con márgen -->
            <div class="col-xs-12">
                <div class="separador"></div>
            </div>

            <!-- Título Períodos escolares -->
            <div class="col-xs-12">
                <h4 class="subtitulos-ano-escolar">Datos - Períodos escolares:</h4>
            </div>

            <!-- Datos de los períodos escolares -->
            <div class="form col-xs-12">
                <!-- Textos de cabecera de lapsos -->
                <div class="row">
                    <div class="col-xs-2 centrar">
                        <label>Lapsos:</label></div>
                    <div class="col-xs-4 centrar">
                        <label>Fecha de inicio:</label></div>
                    <div class="col-xs-4 centrar">
                        <label>Fecha de finalización:</label></div>
                    <div class="col-xs-2 centrar">
                        <label># Días</label></div>
                </div>

                <% Model.lapsoI.Name = "1er Lapso"; %>
                <% Model.lapsoII.Name = "2do Lapso"; %>
                <% Model.lapsoIII.Name = "3er Lapso"; %>
                <%: Html.HiddenFor(m => m.lapsoI.Name) %>
                <%: Html.HiddenFor(m => m.lapsoII.Name) %>
                <%: Html.HiddenFor(m => m.lapsoIII.Name) %>

                <!-- 1er Lapso -->
                <div class="row">
                    <div class="col-xs-2 centrar">
                        <%: Html.LabelFor(m => m.lapsoI.Name, "1er Lapso") %>                        
                    </div>
                    <div class="col-xs-4 centrar">
                        <%: Html.TextBoxFor(m => m.lapsoI.StartDate, new { @class="datepicker form-control", 
                        @id="fec-ini-1", @disabled = "disabled"})%>
                    </div>
                    <div class="col-xs-4 centrar">
                        <%: Html.TextBoxFor(m => m.lapsoI.FinishDate, new { @class="datepicker form-control", 
                        @id="fec-fin-1", @disabled = "disabled"})%>
                    </div>
                    <div class="col-xs-2 centrar">
                        <%: Html.TextBoxFor(m => m.contadorDiasI, new { @class="form-control centrar", 
                        @id="nro-dias-lapso-1", @disabled="disabled"})%>
                    </div>
                </div>

                <!-- 2do Lapso -->
                <div class="row">
                    <div class="col-xs-2 centrar">
                        <%: Html.LabelFor(m => m.lapsoII.Name, "2do Lapso") %>
                    </div>
                    <div class="col-xs-4 centrar">
                        <%: Html.TextBoxFor(m => m.lapsoII.StartDate, new { @class="datepicker form-control", 
                        @id="fec-ini-2", @disabled = "disabled"})%>
                    </div>
                    <div class="col-xs-4 centrar">
                        <%: Html.TextBoxFor(m => m.lapsoII.FinishDate, new { @class="datepicker form-control", 
                        @id="fec-fin-2", @disabled = "disabled"})%>
                    </div>
                    <div class="col-xs-2 centrar">
                        <%: Html.TextBoxFor(m => m.contadorDiasII, new { @class="form-control centrar", 
                        @id="nro-dias-lapso-2", @disabled="disabled"})%>
                    </div>
                </div>

                <!-- 3er Lapso -->
                <div class="row">
                    <div class="col-xs-2 centrar">
                        <%: Html.LabelFor(m => m.lapsoIII.Name, "3er Lapso") %>
                    </div>
                    <div class="col-xs-4 centrar">
                        <%: Html.TextBoxFor(m => m.lapsoIII.StartDate, new { @class="datepicker form-control", 
                        @id="fec-ini-3", @disabled = "disabled"})%>
                    </div>
                    <div class="col-xs-4 centrar">
                        <%: Html.TextBoxFor(m => m.lapsoIII.FinishDate, new { @class="datepicker form-control", 
                        @id="fec-fin-3", @disabled = "disabled"})%>
                    </div>
                    <div class="col-xs-2 centrar">
                        <%: Html.TextBoxFor(m => m.contadorDiasIII, new { @class="form-control centrar", 
                        @id="nro-dias-lapso-3", @disabled="disabled"})%>
                    </div>
                </div>
            </div>
        </div>
                                                                                                     
        <!-- Panel de lista de colegios -->
        <div class="col-xs-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de colegios</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de colegios -->
                    <div class="col-xs-12" id="div-tabla-lista-colegios">
                        <table class="table" id="table-lista-colegios">
                            <thead>
                                <tr>
                                    <th>Colegio</th>
                                    <th class="centrar">Estatus</th>
                                    <th class="centrar">Año escolar</th>
                                    <th class="centrar">Incluir</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% for (int i = 0; i < Model.listaColegiosPersonales.Count(); i++)%>
                                <% { %>

                                <% int idColegio = Model.listaColegiosPersonales[i].colegio.SchoolId; %>
                                <% string status = Model.listaColegiosPersonales[i].colegio.Status ? 
                                       "fa fa-check" : "fa fa-ban"; %>
                                <% string anoEscolarActivo = Model.listaColegiosPersonales[i].anoEscolarActivo 
                                       ? "fa fa-check" : "fa fa-ban";  %>
                                <% bool incluir = (!Model.listaColegiosPersonales[i].anoEscolarActivo &&
                                       Model.listaColegiosPersonales[i].colegio.Status ? true : false); %>

                                <tr>
                                    <td class="td-name">
                                        <%: Model.listaColegiosPersonales[i].colegio.Name %>
                                    </td>
                                    <td class="td-estatus centrar">
                                        <i class="<%: status %>"></i>
                                    </td>
                                    <td class="td-ano-escolar-activo centrar">
                                        <i class="<%: anoEscolarActivo %>"></i>
                                    </td>
                                    <td class="td-incluir centrar">
                                        <% if(incluir) { %>
                                            <%: Html.CheckBoxFor(m => m.listaColegiosPersonales[i].isSelected, 
                                            new { @value= idColegio, @id= idColegio, @Name= "checkboxList" })%>
                                        <% }
                                           else { %>
                                                <i class="fa fa-ban"></i>
                                        <% } %>
                                    </td>
                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <!-- Separador con márgen -->
        <div class="row">
            <div class="col-xs-12">
                <div class="separador"></div>
            </div>
        </div>

        <!-- Botón: Agregar -->
        <div class="col-xs-6 text-right">
            <button class="btn btn-lg btn-default" id="btn-crear-ano-escolar" type="submit">
                Agregar
            </button>
        </div>
        <% } %>

        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Colegios/AnoEscolar.css" type="text/css" />
    <!-- icheck-1.x -->
    <link rel="Stylesheet" href="../../Content/Plug-ins/icheck-1.x/skins/flat/blue.css" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Colegio/InicializarDatePickers.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/CreateSchoolYear.js" type="text/javascript"></script>
    <!-- icheck-1.x -->
    <script src="../../Content/Plug-ins/icheck-1.x/icheck.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Crear un nuevo año escolar
</asp:Content>