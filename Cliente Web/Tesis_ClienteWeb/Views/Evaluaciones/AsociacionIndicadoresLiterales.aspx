<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.MatrizIndicadoresLiteralesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Matriz de asociación de indicadores/literales
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.HiddenFor(m => m.assessment.AssessmentId, new { @id = "id-evaluacion"}) %>

    <!-- Lista de competencias -->
    <div class="row">
        <!-- Nombre de la evaluación -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.assessment.Name) %>
            <%: Html.TextBoxFor(m => m.assessment.Name, new { @class="form-control", @disabled = "disabled", 
                @style = "font-size: bold"})%>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- SelectList de competencias -->
        <div class="col-xs-12">
            <%: Html.LabelFor(m => m.idCompetencia) %>
            <%: Html.DropDownListFor(m => m.idCompetencia, Model.selectListCompetencies, 
            "Seleccione la competencia...", new { @class = "form-control selectpicker",  
                @id = "select-competencias" }) %>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Separador con línea -->
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Lista de indicadores asociados -->
    <div class="row" id="fila-lista-indicadores"></div>

    <!-- Matriz de asociación de indicadores/literales -->
    <div class="row">
        <!-- Título de la tabla -->
        <div class="col-xs-12">
            <h4>Tabla indicadores/literales:</h4>
        </div>

        <!-- Tabla excel -->
        <div class="col-xs-6">
            <div class="handsontable" id="matriz-indicadores-literales"></div>
        </div>

        <!-- Botón: Testeo % alcanzado -->
        <div class="col-xs-6 form-horizontal">
            <div class="form-group text-center">
                <button class="btn btn-lg btn-primary hidden" id="btn-testing">Calcular % alcanzado</button>
            </div>
            <div class="form-group text-center" id="porcentaje-competencia-alcanzado">                
            </div>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Separador con línea -->
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Botones -->
    <div class="row">
        <!-- Botón: Actualizar -->
        <div class="col-xs-6 text-center">
            <button class="btn btn-lg btn-primary" id="btn-actualizar">Actualizar</button>
        </div>

        <!-- Botón: Cancelar -->
        <div class="col-xs-6 text-center">
            <% using (Html.BeginForm("Inicio", "Index", FormMethod.Get, new
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
    <link href="../../Content/Css/Calificaciones/handsontable.full.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Evaluaciones/AsociacionIndicadoresLiterales.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Calificaciones/handsontable.full.js" type="text/javascript" ></script>
    <script src="../../Scripts/Views/Evaluaciones/Matriz_IndicadoresLiterales.js" type="text/javascript" ></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Matriz de asociación de indicadores/literales
</asp:Content>