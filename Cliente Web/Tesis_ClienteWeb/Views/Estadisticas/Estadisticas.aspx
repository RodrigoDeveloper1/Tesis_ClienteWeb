<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EstadisticasModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Estadísticas
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
    </div>
<!-------------------------------- Fin de Combobox de cursos y materias -------------------------------------->

     <!---------------------------------------- Gráfico de barras ---------------------------------------->
        <!--div class="col-lg-4"-->
       <div class="row">
        <div class="col-xs-8">
            <div class="panel panel-green" id="panel-rankingalumnos">
                <div class="panel-heading">
                    <i class="fa fa-bar-chart-o fa-fw"></i> Ranking de alumnos

                    <div class="pull-right">                        
                        <i class="fa fa-file-pdf-o fa-fw"></i>
                    </div>
                    
                </div>
                
                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <div id="morris-bar-rankingalumnos"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-------------------------------------- Fin de Gráfico de barras ----------------------------------->
        
        <!----------------------------------- Descripción Gráfico de barras --------------------------------->
       <div class="col-xs-4">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i> Descripción
                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body">
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
                 labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
                  laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in 
                  voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat 
                  cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                </div>
            </div>
       </div>
        <!--------------------------------- Fin Descripción Gráfico de barras ------------------------------->
       
        </div>
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>

        <!-------------------------------------- Gráfico de Apro vs Repro ----------------------------------->
        <!--div class="col-lg-4"-->
        <div class="row">
        <div class="col-xs-8">
            <div class="panel panel-yellow">
               <div class="panel-heading">
                    <i class="fa fa-pie-chart fa-fw"></i> Alumnos aprobados vs reprobados

                    <div class="pull-right">                        
                        <i class="fa fa-file-pdf-o fa-fw"></i>
                    </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <div id="morris-donut-aprovsrepro"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
      
       <!----------------------------- Fin Descripción Gráfico de Apro vs Repro ----------------------------->
       <!------------------------------ Descripción Gráfico de Apro vs Repro -------------------------------->
       <div class="col-xs-4">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i> Descripción
                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body">
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
                 labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
                  laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in 
                  voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat 
                  cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                </div>
            </div>
       </div>
        <!--------------------------- Fin Descripción Gráfico de Apro vs Repro ------------------------------>
       
        </div>
      
       <div class="col-lg-12">
            <div class="separador"></div>
        </div>
        <!----------------------------------- Gráfico de Rendimiento Académico ------------------------------>
        <!--div class="col-lg-4"-->
        <div class="row">
        <div class="col-xs-8">        
            <div class="panel panel-red">
                <div class="panel-heading">

                    <i class="fa fa-line-chart fa-fw"></i> Rendimiento académico por materias
                    <div class="pull-right">                        
                        <i class="fa fa-file-pdf-o fa-fw"></i>
                    </div>

                </div>
                <div class="panel-body">
                    <div id="morris-area-chart"></div>
                </div>
            </div>
        </div>
        
        <!------------------------------ Fin de Gráfico de Rendimiento Académico ---------------------------->
        <!--------------------------- Descripción Gráfico de Rendimiento Académico -------------------------->
       <div class="col-xs-4">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i> Descripción
                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body">
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
                 labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
                  laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in 
                  voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat 
                  cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                </div>
            </div>
       </div>
        <!----------------------- Fin Descripción Gráfico de Rendimiento Académico --------------------------->
       
        </div>
       
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
        <!----------------------------------- Gráfico de Materia dificultad --------------------------------->
        <div class="row">
        <div class="col-xs-8">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <i class="fa fa-pie-chart fa-fw"></i> Materias según su grado de dificultad

                   <div class="pull-right">                        
                        <i class="fa fa-file-pdf-o fa-fw"></i>
                    </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <div id="morris-donut-difmaterias"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
       
        <!------------------------------- Fin de Gráfico de Materia Dificultas ------------------------------>
        <!---------------------------- Descripción Gráfico de Materia Dificultad ---------------------------->
       <div class="col-xs-4">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <i class="fa fa-list-alt fa-fw"></i> Descripción
                    <div class="separador-dialogos"></div>
                </div>
                <div class="panel-body">
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
                 labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
                  laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in 
                  voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat 
                  cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                </div>
            </div>
       </div>
        <!----------------------- Fin Descripción Gráfico de Materia Dificultad ----------------------------->
       
        </div>

        <!-------------------------------------- Fin de Fila de Gráficos ------------------------------------>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">

<link href="../../Content/Css/Estadisticas/Estadisticas.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Maestra/morris.min.js" type="text/javascript" language="javascript"></script>
    <script src="../../Scripts/Views/Maestra/morris-data.js" type="text/javascript" language="javascript"></script>
    <script src="../../Scripts/Views/Estadisticas/Estadisticas.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Estadísticas
</asp:Content>
