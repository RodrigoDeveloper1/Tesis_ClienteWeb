<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CalificacionesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar Calificacion Individual
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Lista de cursos, lapsos & materias -->
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
    </div>

    <!-- Separador -->
    <div class="row">
        <div class="col-xs-12 form-group"></div>
        <div class="col-xs-12 form-group"></div>
    </div>

    <!-- Lista de evaluaciones, estudiantes & la nota -->
    <div class="row">
        <!-- Lista de evaluaciones -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idEvaluacion) %>
            <%: Html.DropDownListFor(m => m.idEvaluacion, Model.selectListEvaluaciones, "Seleccione la evaluación...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-evaluacion" })%>
        </div>

        <!-- Lista de estudiantes -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idEstudiante) %>
            <%: Html.DropDownListFor(m => m.idEstudiante, Model.selectListEstudiantes, "Seleccione el estudiante...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-alumno" })%>
        </div>

        <!-- Nota -->
        <div class="col-xs-2" id="nota-input-div">
            <label for="nota-input">Nota del alumno:</label>
            <input class="form-control" id="nota-input" placeholder="Nota del alumno">
        </div>
    </div>        

    <!-- Separador -->
    <div class="row">
        <div class="col-xs-12 form-group"></div>
        <div class="col-xs-12 form-group"></div>
    </div>

    <!-- Separador -->
    <div class="row">
        <div class="col-lg-12"><div class="separador"></div></div>
    </div>

    <!-- Sección de botones -->
    <div class="row">
        <!-- Botón: Modificar nota -->
        <div class="col-xs-6 text-right">
            <button class="btn btn-lg btn-default" id="ModificarNotasButton">
                Modificar
            </button>
        </div>
        
        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
        <% using (Html.BeginForm("GestionCalificaciones", "Calificaciones", FormMethod.Get, new
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
    <link href="../../Content/Css/Calificaciones/ModificarCalificaciones.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Calificaciones/handsontable.full.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Calificaciones/ModificarCalificaciones.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Calificaciones/handsontable.full.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Modificar calificación individual
</asp:Content>