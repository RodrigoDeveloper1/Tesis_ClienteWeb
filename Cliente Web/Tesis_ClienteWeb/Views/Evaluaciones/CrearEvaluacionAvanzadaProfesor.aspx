<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EvaluacionModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear evaluación avanzada
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Datos de la nueva evaluación-->
    <% using (Html.BeginForm("CrearEvaluacionAvanzadaProfesor", "Evaluaciones", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
       { %>
    <%: Html.AntiForgeryToken() %>

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
        <div class="col-xs-12">
            <h4>Datos de la evaluación: </h4>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Nombre -->
        <div class="form-group col-xs-4">
            <%: Html.LabelFor(m => m.Name) %>
            <%: Html.TextBoxFor(m => m.Name, new { @PlaceHolder = "Nombre de la evaluación",
                @class = "form-control", @id = "nombre-evaluacion"}) %>
        </div>

        <!-- Procentaje -->
        <div class="form-group col-xs-2" id="input-porcentaje">
            <div class="input-group">
                <span class="input-group-addon" id="sizing-addon2">%</span>
                <%: Html.TextBoxFor(m => m.Percentage, new { @PlaceHolder = "Porcentaje de la evaluación",
                    @class = "form-control",@onkeypress="return isNumberKey(event)",@maxlength="2", 
                    @id = "porcentaje-evaluacion"}) %>
            </div>
        </div>

        <!-- Lista de actividades -->
        <div class="form-group col-xs-6">
            <%: Html.LabelFor(m => m.TipoEvaluacion) %>
            <%: Html.DropDownListFor(m => m.TipoEvaluacion, Model.selectListTipos, "Seleccione el tipo...", 
                new { @class = "form-control selectpicker class-colegios",  @id = "select-tipo" })%>
        </div>
        <!-- Fecha de Inicio -->
        <div class="form-group col-xs-3">
            <%: Html.LabelFor(m => m.StartDate) %>
            <%: Html.TextBoxFor(m => m.StartDate, new { @class="datepicker form-control",  
                    @id="fecha-inicio"})%>
        </div>
        <!-- Fecha de Finalización -->
        <div class="form-group col-xs-3">
            <%: Html.LabelFor(m => m.FinishDate) %>
            <%: Html.TextBoxFor(m => m.FinishDate, new { @class="datepicker form-control", 
                    @id="fecha-finalizacion"})%>
        </div>
        <!-- Lista de técnicas -->
        <div class="form-group col-xs-6">
            <%: Html.LabelFor(m => m.TecnicaEvaluacion) %>
            <%: Html.DropDownListFor(m => m.TecnicaEvaluacion, Model.selectListTecnicas, "Seleccione la técnica...", 
                new { @class = "form-control selectpicker class-colegios",  @id = "select-tecnica" })%>
        </div>
        <!-- Hora de Inicio -->
        <div class="form-group col-xs-3">
            <%: Html.LabelFor(m => m.StartHour) %>
            <%: Html.TextBoxFor(m => m.StartHour, new { @class="add-on bootstrap-timepicker form-control",  
                    @id="hora-inicio"})%>
        </div>
        <!-- Hora de Finalización -->
        <div class="form-group col-xs-3">
            <%: Html.LabelFor(m => m.EndHour) %>
            <%: Html.TextBoxFor(m => m.EndHour, new { @class="add-on bootstrap-timepicker form-control", 
                    @id="hora-finalizacion"})%>
        </div>       
        <!-- Lista de instrumentos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.InstrumentoEvaluacion) %>
            <%: Html.DropDownListFor(m => m.InstrumentoEvaluacion, Model.selectListInstrumentos, "Seleccione el instrumento...", 
                new { @class = "form-control selectpicker class-colegios",  @id = "select-instrumento" })%>
        </div>
        <!--Separador -->
        <div class="form-group col-xs-12"></div>

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
            <button class="btn btn-lg btn-default" type="submit" id="btn-agregar">
                Agregar
            </button>
        </div>
        <% } %>

        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
            <% using (Html.BeginForm("CrearEvaluacionProfesor", "Evaluaciones", FormMethod.Get, new
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
    <link rel="Stylesheet" href="../../Content/Css/Evaluaciones/CrearEvaluacionAvanzada.css" type="text/css" />     
<link href="../../Content/Css/Eventos/bootstrap-timepicker.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
     <script src="../../Scripts/Views/Evaluaciones/CrearEvaluacionAvanzadaProfesor.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/InicializarDatePickers.js" type="text/javascript"></script>    
    <script src="../../Scripts/Views/Eventos/bootstrap-timepicker.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Crear evento avanzado
</asp:Content>