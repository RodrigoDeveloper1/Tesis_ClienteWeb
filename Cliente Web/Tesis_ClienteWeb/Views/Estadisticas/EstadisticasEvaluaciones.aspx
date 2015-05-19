<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EstadisticasModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Estadísticas por evaluación
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Combobox de cursos y materias -->
    <div class="row">
        <!-- Lista de cursos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!-- Lista de lapsos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idLapso) %>
            <%: Html.DropDownListFor(m => m.idLapso, Model.selectListLapsos, "Seleccione el lapso...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-lapso" })%>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Lista de materias -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idMateria) %>
            <%: Html.DropDownListFor(m => m.idMateria, Model.selectListMaterias, "Seleccione la materia...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-materia" })%>
        </div>

        <!-- Lista de evaluaciones -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idEvaluacion) %>
            <%: Html.DropDownListFor(m => m.idEvaluacion, Model.selectListEvaluaciones, "Seleccione la evaluación...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-evaluacion" })%>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Separador con línea -->
        <div class="col-xs-12">
            <div class="separador"></div>
        </div>
    </div>  

    <!-- Sección - Gráfico de torta. % Aprobados vs Reprobados -->
    <div class="row">
        <!-- Gráfico de torta -->
        <div class="col-xs-8">
            <div class="panel panel-yellow" id="panel-aprovvsrepro">
                <div class="panel-heading" id="panel-aprovvsrepro-heading">
                    <i class="fa fa-pie-chart fa-fw"></i>
                    <strong>Estadística - % alumnos aprobados vs reprobados.</strong>

                    <div class="pull-right">
                        <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-estadisticas-aprobados-vs-reprobados"></a>
                    </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12" id="aprovvsrepro-div">
                            <div class="centrar" id="morris-donut-aprovsrepro">
                                <i class='fa fa-ban fa-6 iconogris' style='font-size: 27em;'></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Descripción -->
        <div class="col-xs-4">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i>
                    <strong>Descripción</strong>

                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body descripcion-estadistica" id="3DescripciónEvaluacion"></div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    
    <!-- Sección - Gráfico de barras. Top 10 mejores resultados -->
    <div class="row">
        <!-- Gráfico de barras -->
        <div class="col-xs-8">
            <div class="panel panel-green" id="panel-rankingalumnos">
                <div class="panel-heading">
                    <i class="fa fa-bar-chart-o fa-fw"></i>
                    <strong>Estadística - Top 10 resultados destacados.</strong>

                    <div class="pull-right">
                        <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-estadisticas-top10"></a>
                    </div>

                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12" id="ranking-alumnos-div">
                            <div class="centrar" id="morris-bar-rankingalumnos">
                                <i class='fa fa-ban fa-6 iconogris' style='font-size: 27em;'></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Descripción -->
        <div class="col-xs-4">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i>
                    <strong>Descripción</strong>

                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body descripcion-estadistica" id="1DescripciónEvaluacion"></div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Sección - Gráfico de barras. Top 10 peores resultados -->
    <div class="row">
        <!-- Gráfico de barras -->
        <div class="col-xs-8">
            <div class="panel panel-primary" id="panel-ranking-peores">
                <div class="panel-heading">
                    <i class="fa fa-bar-chart fa-fw"></i>
                    <strong>Estadística - Top 10 resultados deficientes.</strong>

                   <div class="pull-right">
                       <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-estadisticas-top10peores"></a>
                   </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12" id="ranking-peores-div">
                            <div class="centrar" id="morris-ranking-peores">
                                <i class='fa fa-ban fa-6 iconogris' style='font-size: 27em;'></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Descripción -->
        <div class="col-xs-4">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i>
                    <strong>Descripción</strong>

                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body descripcion-estadistica" id="2DescripciónEvaluacion">
                </div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    
    <!-- Sección - Gráfico de torta. Proporción de notas alcanzadas -->
    <div class="row">
        <!-- Gráfico de torta -->
        <div class="col-xs-8">
            <div class="panel panel-red" id="panel-proporcion-notas-evaluacion">
                <div class="panel-heading">

                    <i class="fa fa-pie-chart fa-fw"></i>
                    <strong>Estadística - Proporción de notas en la evaluación.</strong>

                    <div class="pull-right">
                        <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-estadisticas-proporcion-notas"></a>
                    </div>

                </div>
                <div class="panel-body" id="proporcion-notas-evaluacion-div">
                    <div class="centrar" id="proporcion-notas-evaluacion-chart">
                        <i class='fa fa-ban fa-6 iconogris' style='font-size: 27em;'></i>
                    </div>
                </div>
            </div>
        </div>

        <!-- Descripción -->
        <div class="col-xs-4">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i>
                    <strong>Descripción</strong>

                    <div class="separador-dialogos"></div>
                </div>

                <div class="panel-body descripcion-estadistica" id="4DescripciónEvaluacion"></div>
            </div>
        </div>
                
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Botón Cancelar -->
    <div class="row">
        <!-- Botón Cancelar -->
        <div class="col-xs-12 text-center">
            <% using (Html.BeginForm("Inicio", "Index", FormMethod.Get, new
            {
                @class = "form",
                @role = "form"
            }))
               { %>
            <button class="btn btn-lg btn-default " type="submit">
                Cancelar
            </button>
            <% } %>
        </div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Estadisticas/EstadisticasEvaluaciones.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Maestra/morris.min.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Maestra/morris-data.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Estadisticas/EstadisticasEvaluaciones.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Estadisticas/Reportes.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Estadísticas por evaluación
</asp:Content>