﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CalificacionesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Cargar Calificaciones
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


 
<!-------------------------------- Combobox de cursos  y materias -------------------------------------->
    <div class="row">
<!-- Lista de cursos -->
        <div class="col-xs-12">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>
             <!--Separador -->
    <div class="form-group col-xs-12"></div>   
         <!-- Lista de lapsos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idLapso) %>
            <%: Html.DropDownListFor(m => m.idLapso, Model.selectListLapsos, "Seleccione el lapso...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-lapso" })%>
        </div>
        <!-- Lista de materias -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idMateria) %>
            <%: Html.DropDownListFor(m => m.idMateria, Model.selectListMaterias, "Seleccione la materia...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-materia" })%>
        </div>
        
         <!--Separador -->
    <div class="form-group col-xs-12"></div>   
    </div>
<!-------------------------------- Fin de Combobox de cursos y materias -------------------------------------->

    <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-------------------------------- Inicio tabla alumnos -------------------------------------->
    <div class="row">
        <div class="col-lg-12 handsontable" id="tablaCargarCalificaciones"></div>
       
    </div>    
    
    <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    <!-- Botón: Crear curso -->
    <div class="row">
        <div class="col-xs-6 text-right">
            <button class="btn btn-lg btn-default " id="CargarNotasButton">
                <strong>Cargar</strong>
            </button>
        </div>
    <!-- Botón Cancelar -->
    <div class="col-xs-6 text-left">
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

    <div class="col-lg-12">
        <div class="separador"></div>
    </div>
    <!----------------------------------- Fin de Fila de Tabla de evaluaciones ------------------------------------>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Calificaciones/CargarCalificaciones.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Calificaciones/handsontable.full.css" rel="stylesheet" type="text/css" />
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">

<script src="../../Scripts/Views/Calificaciones/CargarCalificaciones.js" type="text/javascript" language="javascript"></script>
<script src="../../Scripts/Views/Calificaciones/handsontable.full.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Cargar Calificaciones
</asp:Content>

