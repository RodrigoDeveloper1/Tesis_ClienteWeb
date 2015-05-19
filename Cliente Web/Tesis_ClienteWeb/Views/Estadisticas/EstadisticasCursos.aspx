<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EstadisticasModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Estadísticas por curso
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Combobox de cursos -->
    <div class="row">
        <!-- Lista de cursos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
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

    <!-- Sección - Gráfico de barras. Rendimiento académico por materia en el curso -->
    <div class="row">
        <!-- Gráfico de barras -->
        <div class="col-xs-8">
            <div class="panel panel-green" id="panel-rankingalumnos">
                <div class="panel-heading">
                    <i class="fa fa-bar-chart-o fa-fw"></i>
                    <strong>Estadística - Rendimiento académico por materia en el curso.</strong>

                    <div class="pull-right">
                        <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-estadisticas-cursos-rendimiento"></a>
                    </div>

                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12" id="ranking-alumnos-div">
                            <div class=" centrar" id="morris-bar-rankingalumnos">
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
                    <i class="fa fa-list-alt fa-fw"></i>Descripción
                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body" id="1DescripciónCurso">
                </div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    
    <!-- Sección - Gráfico de barras. Ranking de promedios por materia en el curso - 1er Lapso -->
    <div class="row">
        <!-- Gráfico de barras -->
        <div class="col-xs-8">
            <div class="panel panel-primary" id="panel-ranking-peores">
                <div class="panel-heading">
                    <i class="fa fa-bar-chart-o fa-fw"></i>
                    <strong>Estadística - Ranking de promedios por materia en el curso - 1er Lapso.</strong>

                   <div class="pull-right">
                       <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-estadisticas-cursos-ranking-1"></a>
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
                    <i class="fa fa-list-alt fa-fw"></i>Descripción
                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body" id="2DescripciónCurso">
                </div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    
    <!-- Sección - Gráfico de barras. Ranking de promedios por materia en el curso - 2do Lapso -->
    <div class="row">
        <!-- Gráfico de barras -->
        <div class="col-xs-8">
            <div class="panel panel-yellow" id="panel-ranking-peores2">
                <div class="panel-heading">
                    <i class="fa fa-bar-chart-o fa-fw"></i>
                    <strong>Estadística - Ranking de promedios por materia en el curso - 2do Lapso.</strong>

                   <div class="pull-right">
                       <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-estadisticas-cursos-ranking-2"></a>
                   </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12" id="ranking-peores-div2">
                            <div class="centrar" id="morris-ranking-peores2">
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
                    <i class="fa fa-list-alt fa-fw"></i>Descripción
                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body" id="3DescripciónCurso">
                </div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    
    <!-- Sección - Gráfico de barras. Ranking de promedios por materia en el curso - 3er Lapso -->
    <div class="row">
        <!-- Gráfico de barras -->
        <div class="col-xs-8">
            <div class="panel panel-red" id="panel-ranking-peores3">
                <div class="panel-heading">
                    <i class="fa fa-bar-chart-o fa-fw"></i>
                    <strong>Estadística - Ranking de promedios por materia en el curso - 3er Lapso.</strong>

                   <div class="pull-right">
                       <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-estadisticas-cursos-ranking-3"></a>
                   </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12" id="ranking-peores-div3">
                            <div class="centrar" id="morris-ranking-peores3">
                                <i class='fa fa-ban fa-6 iconogris' style='font-size: 27em;'></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Descripción -->
        <div class="col-xs-4">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i>Descripción
                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body" id="4DescripciónCurso">
                </div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    
    <!-- Botón Cancelar -->
    <div class="row">
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
    
    <!-- Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">

<link href="../../Content/Css/Estadisticas/EstadisticasCursos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Maestra/morris.min.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Maestra/morris-data.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Estadisticas/EstadisticasCursos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Estadisticas/Reportes.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Estadísticas por curso
</asp:Content>
