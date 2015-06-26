<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EventosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear evento avanzado
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Datos del nuevo evento-->
    <% using (Html.BeginForm("CrearEventoAvanzado", "Eventos", FormMethod.Post, new { @class = "form",
        @role = "form" })) { %>

        <%: Html.AntiForgeryToken() %>
    
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

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <div class="row">
        <div class="col-xs-12">
            <h4>Datos del evento: </h4>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Nombre -->
        <div class="form-group col-xs-6">
            <%: Html.LabelFor(m => m.Name) %>
            <%: Html.TextBoxFor(m => m.Name, new { @PlaceHolder = "Nombre del evento",
                @class = "form-control", @id = "nombre-evento"}) %>
        </div>
         <!-- Fecha de Inicio -->
        <div class="form-group col-xs-3">
            <%: Html.LabelFor(m => m.StartDate) %>
            <%: Html.TextBoxFor(m => m.StartDate, new { @class="datepicker form-control",  
                    @id="fecha-inicio"})%>
        </div>
         <!-- Fecha de Finalización -->
        <div class="form-group col-xs-3">
            <%: Html.LabelFor(m => m.FinishDate) %>
            <%: Html.TextBoxFor(m => m.FinishDate, new { @class="datepicker form-control", 
                    @id="fecha-finalizacion"})%>
        </div>
        <!-- Descripción del evento -->
        <div class="form-group col-xs-6">
            <%: Html.LabelFor(m => m.Description) %>

            <%: Html.TextAreaFor(m => m.Description, new { @class = "form-control", @rows = "4", 
                    @style = "resize: none", @id = "descripcion-evento" })%>
        </div>
        <!-- Hora de Inicio -->
        <div class="form-group col-xs-3">
            <%: Html.LabelFor(m => m.StartHour) %>
            <%: Html.TextBoxFor(m => m.StartHour, new { @class="add-on bootstrap-timepicker form-control",  
                    @id="hora-inicio"})%>
        </div>
        <!-- Hora de Finalización -->
        <div class="form-group col-xs-3">
            <%: Html.LabelFor(m => m.EndHour) %>
            <%: Html.TextBoxFor(m => m.EndHour, new { @class="add-on bootstrap-timepicker form-control", 
                    @id="hora-finalizacion"})%>
        </div>
        <!-- Lista de tipos -->
        <div class="form-group col-xs-6">
            <%: Html.LabelFor(m => m.TipoEvento) %>
            <%: Html.DropDownListFor(m => m.TipoEvento, Model.selectListTiposEventos, "Seleccione el tipo...", 
                new { @class = "form-control selectpicker class-colegios",  @id = "select-tipo" })%>
        </div>
        <!--Separador -->
        <div class="form-group col-xs-12"></div>

    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <!-- Botones -->
    <div class="row">
        <!-- Botón: Agregar -->
        <div class="col-xs-6 text-right">
            <button class="btn btn-lg btn-default" type="submit" id="btn-agregar">
                Agregar
            </button>
        </div>
        <% } %>

        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
            <% using (Html.BeginForm("CrearEvento", "Eventos", FormMethod.Get, new
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
        <!--Separador -->
        <div class="form-group col-xs-12"></div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Eventos/Eventos.css" type="text/css" />     
    <link rel="Stylesheet" href="../../Content/Css/Eventos/CrearEventoAvanzado.css" type="text/css" />     
<link href="../../Content/Css/Eventos/bootstrap-timepicker.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Eventos/CrearEventoAvanzado.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/InicializarDatePickers.js" type="text/javascript"></script>    
    <script src="../../Scripts/Views/Eventos/bootstrap-timepicker.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Crear evento avanzado
</asp:Content>