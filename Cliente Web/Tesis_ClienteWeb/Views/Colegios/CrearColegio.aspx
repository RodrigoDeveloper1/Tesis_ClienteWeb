<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.ColegiosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear colegio
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Formulario Crear colegio -->
    <div class="row">
        <!-- Datos del nuevo colegio-->
        <div class="col-xs-6">
            <% using (Html.BeginForm("CrearColegio", "Colegios", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
            <%: Html.AntiForgeryToken() %>

            <!-- Título Año escolar -->
            <div class="col-xs-12">
                <h4 class="subtitulos-ano-escolar">Datos - Nuevo colegio:</h4>
            </div>

            <!-- Nombre del colegio -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.colegio.Name) %>

                <%: Html.TextBoxFor(m => m.colegio.Name, new { @PlaceHolder = "Nombre del colegio",
                        @class = "form-control"}) %>
            </div>

            

            <!-- Lista de estatus -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.estatusColegio) %>
                <%: Html.DropDownListFor(m => m.estatusColegio, Model.listaEstatusColegio, "Seleccione un estatus", 
                new { 
                    @class = "form-control selectpicker", 
                    @id = "select-estatus-colegio" 
                })%>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>
        </div>

        <!-- Datos del nuevo año escolar -->
        <div class="col-xs-6">
            <!-- Título Año escolar -->
            <div class="col-xs-12">
                <h4 class="subtitulos-ano-escolar">Datos - Nuevo año escolar:</h4>
            </div>

            <!-- Fecha de inicio del año escolar -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.FechaInicioPeriodo) %>
                <%: Html.TextBoxFor(m => m.FechaInicioPeriodo, new { @class="datepicker form-control", 
                    @id="fecha-inicio" }) %>
            </div>

            <!-- Fecha de finalización del año escolar -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.FechaFinalizacionPeriodo) %>
                <%: Html.TextBoxFor(m => m.FechaFinalizacionPeriodo, new { @class="datepicker form-control", 
                    @id="fecha-finalizacion" }) %>
            </div>

            <!-- Estatus del año escolar -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.estatusPeriodoEscolar) %>
                <%: Html.DropDownListFor(m => m.estatusPeriodoEscolar, Model.listaEstatusPeriodoEscolar, 
                "Seleccione un estatus", new { @class = "form-control selectpicker", @id = "select-estatus-periodo"})%>
            </div>
        </div>
    </div>

    <!-- Botones -->
    <div class="row">
        <!-- Botón: Agregar -->
        <div class="col-xs-5 text-right">
            <button class="btn btn-lg btn-default" type="submit" id="btn-agregar">
                Agregar
            </button>
        </div>
        <% } %>

        <!-- Botón Cancelar -->
        <div class="col-xs-7 text-left">
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
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Rol.css" type="text/css" />    
    <link rel="Stylesheet" href="../../Content/Css/Administrador/CrearColegio.css" type="text/css" />    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Administrador.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/ManejadorEstatus.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/InicializarDatePickers.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/CrearColegio.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Crear colegio
</asp:Content>