<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CalificacionesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de calificaciones
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Combobox de cursos  y materias -->
    <div class="row">
        <!-- Lista de cursos -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!-- Lista de lapsos -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idLapso) %>
            <%: Html.DropDownListFor(m => m.idLapso, Model.selectListLapsos, "Seleccione el lapso...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-lapso" })%>
        </div>

        <!-- Lista de materias -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idMateria) %>
            <%: Html.DropDownListFor(m => m.idMateria, Model.selectListMaterias, "Seleccione la materia...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-materia" })%>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-------------------------------- Inicio tabla alumnos -------------------------------------->

    <!--div class="row" id="tablaCargarCalificaciones-div">
        <div class="col-lg-12 handsontable" id="tablaCargarCalificaciones"></div>       
    </div-->

    <div class="handsontable" id="tablaCargarCalificaciones-div" style="width: 67em; height: 60em; overflow: scroll">
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    <!-- Botón: Modificar nota personalizada -->
    <div class="row">
        <div class="col-xs-4 text-right">
            <button class="btn btn-lg btn-default " id="ModificarNotasButton">
                <strong>Modif. Nota Individual</strong>
            </button>
        </div>
        <!-- Botón: Crear nota -->
        <div class="col-xs-4 text-center">
            <button class="btn btn-lg btn-default " id="CargarNotasButton">
                <strong>Cargar</strong>
            </button>
        </div>
        <!-- Botón Cancelar -->
        <div class="col-xs-4 text-left">
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
    </div>

    <div class="col-lg-12">
        <div class="separador"></div>
    </div>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Calificaciones/GestionCalificaciones.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Calificaciones/handsontable.full.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Calificaciones/GestionCalificaciones.js" type="text/javascript" ></script>
    <script src="../../Scripts/Views/Calificaciones/handsontable.full.js" type="text/javascript" ></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Gestión de calificaciones
</asp:Content>

