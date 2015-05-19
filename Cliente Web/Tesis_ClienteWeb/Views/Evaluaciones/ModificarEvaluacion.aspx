<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EvaluacionModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar evaluación
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Lista de colegios y lapsos -->
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-5">
            <%: Html.LabelFor(m => m.idColegio) %>
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-colegio-modif" })%>
        </div>
        
        <!-- Lista de lapsos -->
        <div class="col-xs-3">
            <%: Html.LabelFor(m => m.idLapso) %>
            <%: Html.DropDownListFor(m => m.idLapso, Model.selectListLapsos, "Seleccione el lapso...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-lapso-modif" })%>
        </div>

        <!-- Lista de cursos -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-curso-modif" })%>
        </div>
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!-- Lista de cursos y materias -->
    <div class="row">
        <!-- Lista de materias -->
        <div class="col-xs-5">
            <%: Html.LabelFor(m => m.idMateria) %>
            <%: Html.DropDownListFor(m => m.idMateria, Model.selectListMaterias, "Seleccione la materia...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-materia-modif" })%>
        </div>        
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <!-- Tabla de evaluaciones -->
    <div class="row">
        <div class="col-xs-12">
            <h4>Lista de evaluaciones: </h4>
        </div>

        <div class="panel-body">
            <!-- Tabla de Evaluaciones -->
            <div class="col-xs-12" id="div-tabla-lista-evaluaciones">
                <table class="table" id="table-lista-evaluaciones">
                    <thead>
                        <tr>
                            <th class="th-evaluacion">Evaluación</th>
                            <th class="th-tecnica">Técnica</th>
                            <th class="th-tipo">Tipo</th>
                            <th class="th-instrumento">Instrumento</th>
                            <th class="th-porcentaje">%</th>
                            <th class="th-opcion">Fecha Inicio</th>
                            <th class="th-opcion">Fecha Fin</th>
                            <th class="th-opcion">Eliminar</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <!-- Datos de la evaluación -->
    <div class="row">
        <!-- Título: Datos de la evaluación -->
        <div class="col-xs-12">
            <h4>Datos de la evaluación: </h4>
        </div>

        <!-- Formulario: Datos de la evaluación - Parte I -->
        <div class="col-xs-5">
            <!-- Nombre -->
            <div class="form-group col-xs-8" id="nombre-evaluacion-div">
                <label for="nombre-evaluacion">Nombre de la evaluación:</label>
                <input class="form-control" id="nombre-evaluacion" placeholder="Evaluación">
            </div>

            <!-- Procentaje -->
            <div class="form-group col-xs-4">
                <div class="input-group col-xs-12 form-subir" id="porcentaje-evaluacion-div">
                    <span class="input-group-addon" id="basic-addon1">%</span>
                    <input class="form-control" id="porcentaje-evaluacion" aria-describedby="basic-addon1">
                </div>
            </div>

            <!-- Fecha de Inicio -->
            <div class="form-group col-xs-5">
                <label for="fechainicio-evaluacion">Fecha inicio:</label>
                <input class="form-control datepicker" id="fechainicio-evaluacion">
            </div>

            <!-- Hora de Inicio -->
            <div class="form-group col-xs-5 col-xs-offset-2 form-subir">
                <div class="input-group col-xs-12" id="">
                    <span class="input-group-addon" id="clock-ini"><i class="fa fa-clock-o"></i></span>
                    <input class="add-on bootstrap-timepicker form-control" 
                        id="horainicio-evaluacion" aria-describedby="clock-ini">
                </div>
            </div>

            <!-- Fecha de Finalización -->
            <div class="form-group col-xs-5">
                <label for="fechafin-evaluacion">Fecha fin:</label>
                <input class="form-control datepicker" id="fechafin-evaluacion">
            </div>
        
            <!-- Hora de Finalización -->
            <div class="form-group col-xs-5 col-xs-offset-2 form-subir">
                <div class="input-group col-xs-12" id="">
                    <span class="input-group-addon" id="clock-fin"><i class="fa fa-clock-o"></i></span>
                    <input class="add-on bootstrap-timepicker form-control" 
                        id="horafin-evaluacion" aria-describedby="clock-fin">
                </div>
            </div>
        </div>

        <!-- Formulario: Datos de la evaluación - Parte II -->
        <div class="col-xs-5 col-xs-offset-1">
            <!-- Lista de actividades -->
            <div class="form-group col-xs-12">
                <label for="select-tipo">Tipo de evaluación:</label>
                <select class="form-control selectpicker" id="select-tipo" data-live-search="true">
                    <option selected disabled>Seleccione un tipo...</option>
                    <%  foreach (string tipo in Model.listaTiposNormal)
                        { %>
                    <option value="<%: tipo %>"><%: tipo %></option>
                    <% } %>
                </select>
            </div>
        
            <!-- Lista de técnicas -->
            <div class="form-group col-xs-12">
                <label for="tecnica-evaluacion">Técnicas de evaluación:</label>
                <select class="form-control selectpicker" id="tecnica-evaluacion" data-live-search="true">
                    <option selected disabled>Seleccione una técnica...</option>
                    <%  foreach (string tecnica in Model.listaTecnicasNormal)
                        { %>
                    <option value="<%: tecnica %>"><%: tecnica %></option>
                    <% } %>
                </select>
            </div>

            <!-- Lista de instrumentos -->
            <div class="form-group col-xs-12">
                <label for="instrumento-evaluacion">Instrumentos de evaluación:</label>
                <select class="form-control selectpicker" id="instrumento-evaluacion" data-live-search="true">
                    <option selected disabled>Seleccione un instrumento...</option>
                    <%  foreach (string instrumento in Model.listaInstrumentosNormal)
                        { %>
                    <option value="<%: instrumento %>"><%: instrumento %></option>
                    <% } %>
                </select>
            </div>
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
        <div class="col-xs-6 text-right">
            <button class="btn btn-lg btn-default" id="btn-modificar-evaluacion">
                Modificar
            </button>
        </div>

        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
            <% using (Html.BeginForm("Menu", "Administrador", FormMethod.Get, new
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
    <link rel="Stylesheet" href="../../Content/Css/Evaluaciones/ModificarEvaluacion.css" type="text/css" />     
    <link href="../../Content/Css/Eventos/bootstrap-timepicker.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Evaluaciones/ModificarEvaluacion.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/InicializarDatePickers.js" type="text/javascript"></script>    
    <script src="../../Scripts/Views/Eventos/bootstrap-timepicker.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Modificar evaluación
</asp:Content>