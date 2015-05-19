<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CalificacionesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Definitivas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


 
<!-------------------------------- Combobox de cursos  y materias -------------------------------------->
    <div class="row">
    <!-- Lista de cursos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
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
   

    <!-------------------------------- Inicio tabla alumnos -------------------------------------->
    <!-- Panel de Lista de notas -->
        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Notas Definitivas</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de notas definitivas -->
                    <div class="col-xs-12" id="div-tabla-lista-notas">
                        <table class="table" id="table-lista-notas">
                            <thead>
                                <tr>
                                    <th class="th-num-lista ">#</th>
                                    <th class="th-nombre-alumno">Alumno</th>
                                    <th class="th-1er-lapso">1er Lapso</th>
                                    <th class="th-2do-lapso">2do Lapso</th>
                                    <th class="th-3er-lapso">3er Lapso</th>
                                    <th class="th-definitiva">Definitiva</th>
                                </tr>
                            </thead>
                            <tbody>                              
                                <tr> 
                                    <td class="td-num-lista "></td>
                                    <td class="td-nombre-alumno "></td>   
                                    <td class="td-1er-lapso "></td> 
                                    <td class="td-2do-lapso "></td> 
                                    <td class="td-3er-lapso "></td> 
                                    <td class="td-definitiva"></td>                
                                </tr>                               
                        </table>
                    </div>
                </div>
            </div>
        </div>
     </div>
    <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    
    <div class="row">
    <!-- Botón Cancelar -->
    <div class="col-xs-12 text-center">
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
    <!----------------------------------- Fin de Fila de Tabla de evaluaciones ------------------------------------>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Calificaciones/Definitivas.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Calificaciones/handsontable.full.css" rel="stylesheet" type="text/css" />
    </asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">

<script src="../../Scripts/Views/Calificaciones/Definitivas.js" type="text/javascript" language="javascript"></script>
<script src="../../Scripts/Views/Calificaciones/handsontable.full.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Definitivas
</asp:Content>

