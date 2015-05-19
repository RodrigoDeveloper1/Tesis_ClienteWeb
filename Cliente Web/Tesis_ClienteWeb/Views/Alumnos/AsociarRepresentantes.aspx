<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.AsociarRepresentantesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Asociar representantes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-6">
            <div class="form-inline">
                <%: Html.LabelFor(m => m.idColegio, new { @class="form-group cabecera-tips" })%>
                <div class="form-group tip-informacion">
                    *
                    <div class="label label-info tip-mensaje" id="tip-lista-colegios-3">
                        Se muestran solo aquellos colegios activos. 
                    </div> 
                </div>
            </div>
            
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
            new { @class = "form-control selectpicker class-colegios",  @id = "select-colegio" })%>
        </div>

        <!-- Año escolar -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.labelAnoEscolar) %>
            <%: Html.TextBoxFor(m => m.labelAnoEscolar, new { @class="form-control", @id="ano-escolar", 
                @disabled = "disabled"})%>
            <% Html.HiddenFor(m => m.idAnoEscolar); %>
        </div>

        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>

        <!-- Lista de cursos -->
        <div class="col-xs-6">
            <div class="form-inline">
                <%: Html.LabelFor(m => m.idCurso, new { @class="form-group cabecera-tips"})%> 
                <div class="form-group tip-informacion">
                    *
                    <div class="label label-info tip-mensaje" id="tip-lista-cursos-3">
                        Los cursos cargados en esta lista son aquellos que pertenecen a períodos escolares 
                        activos, a lo que corresponderán a un año escolar en curso. 
                    </div> 
                </div>
            </div>

            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!-- Lista de estudiantes -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idEstudiante)%> 
            <%: Html.DropDownListFor(m => m.idEstudiante, Model.selectListEstudiantes, "Seleccione un estudiante...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-estudiantes" })%>
        </div>
        
        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>

        <!-- Separador con línea -->
        <div class="col-xs-12">
            <div class="separador"></div>
        </div>

        <!-- Mensaje de error personalizado #1 - No hay info del alumno -->
        <div class="row">
            <div class="col-xs-12">
                <div class="alert alert-warning" id="div-alerta-no-alumno" role="alert">
                    <strong>Hubo un error del sistema. El alumno se indica que no existe.</strong>
                </div>
            </div>
        </div>
        
        <!-- Mensaje de error personalizado #2 - No hay representantes -->
        <div class="row">
            <div class="col-xs-12">
                <div class="alert alert-warning" id="div-alerta-no-representantes" role="alert">
                    <strong>El alumno no posee representantes asociados.</strong>
                </div>
            </div>
        </div>

        <!-- Mensaje de error personalizado #3 - No hay representantes -->
        <div class="row">
            <div class="col-xs-12">
                <div class="alert alert-warning" id="div-alerta-no-representante-2" role="alert">
                    <strong>El alumno solo posee un representante asociado.</strong>
                </div>
            </div>
        </div>
        
        <!-- Datos - Representante #1 -->
        <div class="col-xs-6">
            <!-- Subtítulo -->
            <div class="col-xs-12">
                <h4>Datos - Representante #1:</h4>
            </div>

            <!-- Cédula -->
            <div class="col-xs-6 form-group">
                <%: Html.LabelFor(m => m.tipoCedula)%> 

                <div class="input-group">
                    <%: Html.DropDownListFor(m => m.tipoCedula, Model.selectListTiposCedula, new {
                        @class="input-group-btn selectpicker combo-tipos-cedula", @type="button", 
                        @id="select-cedula-1" }) %>

                    <%: Html.TextBoxFor(m => m.representante1.IdentityNumber, new { @class="form-control", 
                        @id= "cedula-representante-1"})%>
                </div>
            </div>

            <!-- Sexo -->
            <div class="col-xs-6 form-group">
                <%: Html.LabelFor(m => m.representante1.Gender)%> 
                <%: Html.DropDownListFor(m => m.representante1.Gender, Model.selectListSexos, new { 
                    @class = "form-control selectpicker",  @id = "select-sexo-1" })%>
            </div>

            <!-- Nombre -->
            <div class ="form-group col-xs-8">
                <%: Html.LabelFor(m => m.representante1.Name) %>

                <%: Html.TextBoxFor(m => m.representante1.Name, new { @PlaceHolder = "Nombre(s) del" + 
                " representante", @class = "form-control", @id = "nombre-representante-1"}) %>
            </div>

            <!-- Apellido #1 -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.representante1.LastName) %>

                <%: Html.TextBoxFor(m => m.representante1.LastName, new { @PlaceHolder = "Primer apellido", 
                @class = "form-control", @id = "apellido-1-representante-1"}) %>
            </div>

            <!-- Apellido #2 -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.representante1.SecondLastName) %>

                <%: Html.TextBoxFor(m => m.representante1.SecondLastName, new { @PlaceHolder = "Segundo apellido", 
                @class = "form-control", @id = "apellido-2-representante-1"}) %>
            </div>

            <!-- Correo electrónico -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.representante1.Email) %>

                <%: Html.TextBoxFor(m => m.representante1.Email, new { @PlaceHolder = "Correo electrónico",
                @class = "form-control", @id = "correo-1", @type = "email" }) %>
            </div>
        </div>

        <!-- Datos - Representante #2 -->
        <div class="col-xs-6">
            <!-- Subtítulo -->
            <div class="col-xs-12">
                <h4>Datos - Representante #2:</h4>
            </div>

            <!-- Cédula -->
            <div class="col-xs-6 form-group">
                <%: Html.LabelFor(m => m.tipoCedula)%> 

                <div class="input-group">
                    <%: Html.DropDownListFor(m => m.tipoCedula, Model.selectListTiposCedula, new {
                        @class="input-group-btn selectpicker combo-tipos-cedula", @type="button", 
                        @id="select-cedula-2" }) %>

                    <%: Html.TextBoxFor(m => m.representante2.IdentityNumber, new { @class="form-control", 
                        @id= "cedula-representante-2"})%>
                </div>
            </div>

            <!-- Sexo -->
            <div class="col-xs-6 form-group">
                <%: Html.LabelFor(m => m.representante2.Gender)%> 
                <%: Html.DropDownListFor(m => m.representante2.Gender, Model.selectListSexos, new { 
                    @class = "form-control selectpicker",  @id = "select-sexo-2" })%>
            </div>

            <!-- Nombre -->
            <div class ="form-group col-xs-8">
                <%: Html.LabelFor(m => m.representante2.Name) %>

                <%: Html.TextBoxFor(m => m.representante2.Name, new { @PlaceHolder = "Nombre(s) del" + 
                " representante", @class = "form-control", @id = "nombre-representante-2"}) %>
            </div>

            <!-- Apellido #1 -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.representante2.LastName) %>

                <%: Html.TextBoxFor(m => m.representante2.LastName, new { @PlaceHolder = "Primer apellido", 
                @class = "form-control", @id = "apellido-1-representante-2"}) %>
            </div>

            <!-- Apellido #2 -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.representante2.SecondLastName) %>

                <%: Html.TextBoxFor(m => m.representante2.SecondLastName, new { @PlaceHolder = "Segundo apellido", 
                @class = "form-control", @id = "apellido-2-representante-2"}) %>
            </div>

            <!-- Correo electrónico -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.representante2.Email) %>

                <%: Html.TextBoxFor(m => m.representante2.Email, new { @PlaceHolder = "Correo electrónico",
                @class = "form-control", @id = "correo-2", @type = "email" }) %>
            </div>
        </div>
        
        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
        <div class="row"><div class="col-xs-12"><div class="separador"></div></div></div>
        <div class="form-group col-xs-12"></div>
        
        <!-- Botón Asociar -->
        <div class="col-xs-6 text-right btn-tip-mensaje">
            <button type="button" class="btn btn-lg btn-default" id="btn-asociar-representantes">
                Asociar 
                <span class="tip-informacion">
                    *

                    <span class="label label-info tip-mensaje" id="tip-btn-asociar">
                        Al darle click al botón se guardarán los datos de el o los representantes y se asociarán 
                        al alumno seleccionado. De tener el alumno representantes previamente asociados se 
                        actualizarán los datos de dichos representantes. Si todos los datos de uno de los 
                        representantes no se llenaron, se omitirá su guardado.
                    </span> 
                </span>
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

        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>

        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Alumnos/Representantes.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Alumnos/GestionListasRepresentantes.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Alumnos/AsociarRepresentantes.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión representantes por alumno
</asp:Content>