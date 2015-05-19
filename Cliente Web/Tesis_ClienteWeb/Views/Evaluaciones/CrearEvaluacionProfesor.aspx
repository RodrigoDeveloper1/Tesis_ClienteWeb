<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EvaluacionModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear evaluación
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Datos de la nueva evaluación-->
   

    <div class="row">
           <!-- Lista de cursos -->
        <div class="col-xs-8">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-curso-crear" })%>
        </div>     
        
        <!-- Lista de lapsos -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idLapso) %>
            <%: Html.DropDownListFor(m => m.idLapso, Model.selectListLapsos, "Seleccione el lapso...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-lapso-crear" })%>
        </div>
       
    </div>         
    <!--Separador -->
    <div class="form-group col-xs-12"></div>   
    <div class="row">          
        <!-- Lista de materias -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idMateria) %>
            <%: Html.DropDownListFor(m => m.idMateria, Model.selectListMaterias, "Seleccione la materia...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-materia-crear" })%>
        </div>
    </div>
    <!---------------- Fin de Comboboxde colegios, lapsos, cursos, profesores y materias ------------>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <div class="row">       
          <!-- Panel de lista de evaluaciones -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading"><strong>Lista de evaluaciones a agregar:</strong></div>
                <div class="panel-body">

                    <!-- Tabla de evaluaciones editable -->
                    <div class="col-xs-12" id="div-table-lista-evaluaciones-cargar">
                        <table class="table table-striped">
                            <thead>
                                <tr>
                                    <th class="th-nombre centrar">Nombre</th>
                                    <th class="th-porcentaje centrar">%</th>
                                    <th class="th-inicio centrar">Inicio</th>
                                    <th class="th-fin centrar">Fin</th>                                   
                                    <th class="th-tipo centrar">Tipo</th>                                                         
                                    <th class="th-agregar-evaluaciones">
                                        <i class="ui-icon ui-icon-check iconos-comentados" 
                                            id="i-salvar-todas-filas">
                                        </i>
                                    </th>
                                    <th class="th-eliminar-evaluaciones">
                                        <i class="fa fa-minus-circle iconos-comentados" 
                                            id="i-eliminar-todas-filas">
                                        </i>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="td-nro-evaluaciones">
                                        <i class="ui-icon ui-icon-plusthick" id="i-add-fila-evaluaciones"></i>
                                    </td>
                                    <td class="td-nombre"></td>
                                    <td class="td-porcentaje"></td>
                                    <td class="td-inicio"></td>
                                    <td class="td-fin"></td>
                                    <td class="td-tipo"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-xs-12">
          <a class="" href="../Evaluaciones/CrearEvaluacionAvanzadaProfesor">
               <span class="">Crear Evaluación Avanzada</span>
          </a>
        </div>
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
         <!-- Botón Agregar -->
        <div class="col-xs-6 text-right">
            <button type="button" class="btn btn-lg btn-default" id="btn-agregar-evaluaciones">Agregar</button>
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
        <!--Separador -->
        <div class="form-group col-xs-12"></div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
      <link rel="Stylesheet" href="../../Content/Css/Evaluaciones/CrearEvaluacion.css" type="text/css" />        
<link href="../../Content/Css/Eventos/bootstrap-timepicker.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Evaluaciones/Evaluaciones.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Evaluaciones/CrearEvaluacionProfesor.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/InicializarDatePickers.js" type="text/javascript"></script>    
    <script src="../../Scripts/Views/Eventos/bootstrap-timepicker.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
 Crear evaluación 
</asp:Content>