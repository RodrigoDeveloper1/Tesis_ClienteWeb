<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EvaluacionModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Evaluaciones
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

        <!-- Panel de Lista de Evaluaciones -->
        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Lista de evaluaciones</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de Alumnos -->
                    <div class="col-xs-12" id="div-tabla-lista-evaluaciones">
                        <table class="table" id="table-lista-evaluaciones">
                            <thead>
                                <tr>
                                    <th class="th-evaluacion-prof">Evaluación</th>
                                    <th class="th-tecnica-prof">Técnica</th>
                                    <th class="th-tipo-prof">Tipo</th>
                                    <th class="th-instrumento-prof">Instrumento</th>
                                    <th class="th-porcentaje-prof">%</th>
                                    <th class="th-opcion-prof">Fecha Inicio</th>
                                    <th class="th-opcion-prof">Fecha Fin</th>
                                </tr>
                            </thead>
                            <tbody>
                              
                                <tr> 
                                    <td class="th-evaluacion-prof"></td>
                                    <td class="th-tecnica-prof"></td>
                                    <td class="th-tipo-prof"></td>
                                    <td class="th-instrumento-prof"></td>
                                    <td class="th-porcentaje-prof"></td>
                                    <td class="th-opcion-prof"></td>
                                    <td class="th-opcion-prof"></td>                         
                                </tr>                               
                        </table>
                    </div>
                </div>
            </div>
        </div>
    
    
     <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
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

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
   </div>
    <!----------------------------------- Fin de Fila de Tabla de evaluaciones ------------------------------------>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">

    <link href="../../Content/Css/Evaluaciones/Evaluaciones.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">

<script src="../../Scripts/Views/Evaluaciones/Evaluaciones.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Evaluaciones
</asp:Content>

