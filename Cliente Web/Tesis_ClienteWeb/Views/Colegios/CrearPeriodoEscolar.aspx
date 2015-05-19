<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.PeriodoEscolarModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear períodos escolares
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Formulario Periodo Escolar -->
        <div class="col-xs-12">
            <div class="form">
                <%: Html.AntiForgeryToken() %>

                <!-- Lista de colegios -->
                <div class="col-xs-8">
                    <%: Html.LabelFor(m => m.idColegio) %>
                    <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
                    new { @class = "form-control selectpicker class-colegios",  @id = "select-colegio" })%>
                </div>

                <!--Separador normal -->
                <div class="form-group col-xs-12"></div>

                <!-- Lista de años escolares -->            
                <div class="col-xs-8">
                    <%: Html.LabelFor(m => m.idAnoEscolar, "Lista de años escolares (solo aquellos sin períodos " + 
                        " escolares)") %>
                    <%: Html.DropDownListFor(m => m.idAnoEscolar, Model.selectListAnosEscolares, 
                    "Seleccione el año escolar...", new { @class = "form-control selectpicker",  
                        @id = "select-ano-escolar" })%>
                </div>

                <!--Separador normal -->
                <div class="form-group col-xs-12"></div>

                <!-- Título Períodos escolares -->
                <div class="col-xs-12">
                    <h5 id="subtitulo-periodo-escolar">Datos - Períodos escolares:</h5>
                </div>

                <!-- Panel de datos de los períodos escolares -->
                <div class="form col-xs-8">
                    <div class="panel panel-default">
                        <div class="panel-body">
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
                                        <%: Html.TextBoxFor(m => m.lapsoI.StartDate, new { 
                                            @class="datepicker form-control", @id="fec-ini-1"})%>
                                    </div>
                                    <div class="col-xs-4 centrar">
                                        <%: Html.TextBoxFor(m => m.lapsoI.FinishDate, new { 
                                            @class="datepicker form-control", @id="fec-fin-1"})%>
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
                                        <%: Html.TextBoxFor(m => m.lapsoII.StartDate, new { 
                                            @class="datepicker form-control", @id="fec-ini-2"})%>
                                    </div>
                                    <div class="col-xs-4 centrar">
                                        <%: Html.TextBoxFor(m => m.lapsoII.FinishDate, new { 
                                            @class="datepicker form-control", @id="fec-fin-2"})%>
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
                                        <%: Html.TextBoxFor(m => m.lapsoIII.StartDate, new { 
                                            @class="datepicker form-control", @id="fec-ini-3"})%>
                                    </div>
                                    <div class="col-xs-4 centrar">
                                        <%: Html.TextBoxFor(m => m.lapsoIII.FinishDate, new { 
                                            @class="datepicker form-control", @id="fec-fin-3"})%>
                                    </div>
                                    <div class="col-xs-2 centrar">
                                        <%: Html.TextBoxFor(m => m.contadorDiasIII, new { @class="form-control centrar", 
                                        @id="nro-dias-lapso-3", @disabled="disabled"})%>
                                    </div>
                                </div>
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
                                                                                                     
                <!-- Botón: Asociar -->
                <div class="col-xs-6 text-right">
                    <button class="btn btn-lg btn-default" type="submit" id="btn-agregar">
                        Asociar
                    </button>
                </div>
            </div>

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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Colegios/PeriodoEscolar.css" type="text/css" />    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Colegio/PeriodoEscolar.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/InicializarDatePickers.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Definir nuevos períodos escolares.
</asp:Content>