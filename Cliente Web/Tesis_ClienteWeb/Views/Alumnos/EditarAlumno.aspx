<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EditarAlumnoModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Editar alumno
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <% using (Html.BeginForm("EditarAlumno", "Alumnos", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
        <%: Html.AntiForgeryToken() %>

        <%: Html.HiddenFor(m => m.Student.StudentId) %>

        <!-- Datos básicos -->
        <div class="col-xs-6">
            <div class="col-xs-12">
                <h4>Datos básicos:</h4>
            </div>

            <!-- Primer nombre -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.FirstName) %>
                <%: Html.TextBoxFor(m => m.Student.FirstName, new { @class = "form-control"}) %>
            </div>

            <!-- Segundo nombre -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.SecondName) %>
                <%: Html.TextBoxFor(m => m.Student.SecondName, new { @class = "form-control"}) %>
            </div>

            <!-- Primer apellido -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.FirstLastName) %>
                <%: Html.TextBoxFor(m => m.Student.FirstLastName, new { @class = "form-control"}) %>
            </div>

            <!-- Segundo apellido -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.SecondLastName) %>
                <%: Html.TextBoxFor(m => m.Student.SecondLastName, new { @class = "form-control"}) %>
            </div>
        </div>

        <!-- Datos como estudiante -->
        <div class="col-xs-6">
            <div class="col-xs-12">
                <h4>Datos como estudiante:</h4>
            </div>

            <!-- # de Lista -->
            <div class ="form-group col-xs-3">
                <%: Html.LabelFor(m => m.Student.NumberList) %>

                <div class="input-group col-xs-12">
                    <span class="input-group-addon" id="basic-addon1">#</span>
                    <%: Html.TextBoxFor(m => m.Student.NumberList, new { @class = "form-control" })%>                                        
                </div>
            </div>

            <div class="col-xs-12"></div>

            <!-- Número de registro -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.RegistrationNumber) %>
                <%: Html.TextBoxFor(m => m.Student.RegistrationNumber, new { @class = "form-control"}) %>
            </div>
        </div>
        
        <!-- Separador -->
        <div class="form-group col-xs-12"></div>
        <div class="row">
	        <div class="col-xs-12">
		        <div class="separador"></div>
	        </div>
        </div>

        <!-- Datos - Representante #1 -->
        <div class="col-xs-6">
            <!-- Subtítulo -->
            <div class="col-xs-12">
                <h4>Datos - Representante #1 (Opcional):</h4>
            </div>

            <!-- Cédula -->
            <div class="col-xs-6 form-group">
                <%: Html.LabelFor(m => m.tipoCedula)%> 

                <div class="input-group">
                    <%: Html.DropDownListFor(m => m.tipoCedula, Model.selectListTiposCedula, new {
                        @class="input-group-btn selectpicker combo-tipos-cedula", @type="button", 
                        @id="select-cedula-1" }) %>

                    <%: Html.TextBoxFor(m => m.Student.Representatives[0].IdentityNumber, 
                    new { @class="form-control", @id= "cedula-representante-1"})%>
                </div>
            </div>

            <!-- Sexo -->
            <div class="col-xs-6 form-group">
                <% string sexo = (Model.Student.Representatives[0].Gender ? "1" : "0"); %>
                <%: Html.LabelFor(m => m.Student.Representatives[0].Gender)%> 
                <%: Html.DropDownListFor(m => sexo, Model.selectListSexos, 
                new { @class = "form-control selectpicker",  @id = "select-sexo-1" })%>
            </div>

            <!-- Nombre -->
            <div class ="form-group col-xs-8">
                <%: Html.LabelFor(m => m.Student.Representatives[0].Name) %>

                <%: Html.TextBoxFor(m => m.Student.Representatives[0].Name, new { @PlaceHolder = "Nombre(s) del" + 
                " representante", @class = "form-control", @id = "nombre-representante-1"}) %>
            </div>

            <!-- Apellido #1 -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.Representatives[0].LastName) %>

                <%: Html.TextBoxFor(m => m.Student.Representatives[0].LastName, new { @PlaceHolder = "Primer apellido", 
                @class = "form-control", @id = "apellido-1-representante-1"}) %>
            </div>

            <!-- Apellido #2 -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.Representatives[0].SecondLastName) %>

                <%: Html.TextBoxFor(m => m.Student.Representatives[0].SecondLastName, new { @PlaceHolder = "Segundo apellido", 
                @class = "form-control", @id = "apellido-2-representante-1"}) %>
            </div>

            <!-- Correo electrónico -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.Student.Representatives[0].Email) %>

                <%: Html.TextBoxFor(m => m.Student.Representatives[0].Email, new { @class = "form-control", 
                @PlaceHolder = "Correo electrónico", @id = "correo-1", @type = "email" }) %>
            </div>
        </div>

        <!-- Datos - Representante #2 -->
        <div class="col-xs-6">
            <!-- Subtítulo -->
            <div class="col-xs-12">
                <h4>Datos - Representante #2 (Opcional):</h4>
            </div>

            <!-- Cédula -->
            <div class="col-xs-6 form-group">
                <%: Html.LabelFor(m => m.tipoCedula)%> 

                <div class="input-group">
                    <%: Html.DropDownListFor(m => m.tipoCedula, Model.selectListTiposCedula, new {
                        @class="input-group-btn selectpicker combo-tipos-cedula", @type="button", 
                        @id="select-cedula-2" }) %>

                    <%: Html.TextBoxFor(m => m.Student.Representatives[1].IdentityNumber, new { 
                        @class="form-control", @id= "cedula-representante-2"})%>
                </div>
            </div>

            <!-- Sexo -->
            <div class="col-xs-6 form-group">
                <% sexo = (Model.Student.Representatives[1].Gender ? "1" : "0"); %>
                <%: Html.LabelFor(m => m.Student.Representatives[1].Gender)%> 
                <%: Html.DropDownListFor(m => sexo, Model.selectListSexos, new { @class = "form-control selectpicker", 
                    @id = "select-sexo-2" })%>
            </div>

            <!-- Nombre -->
            <div class ="form-group col-xs-8">
                <%: Html.LabelFor(m => m.Student.Representatives[1].Name) %>

                <%: Html.TextBoxFor(m => m.Student.Representatives[1].Name, new { @PlaceHolder = "Nombre(s) del" + 
                " representante", @class = "form-control", @id = "nombre-representante-2"}) %>
            </div>

            <!-- Apellido #1 -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.Representatives[1].LastName) %>

                <%: Html.TextBoxFor(m => m.Student.Representatives[1].LastName, new { @PlaceHolder = "Primer apellido", 
                @class = "form-control", @id = "apellido-1-representante-2"}) %>
            </div>

            <!-- Apellido #2 -->
            <div class ="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Student.Representatives[1].SecondLastName) %>

                <%: Html.TextBoxFor(m => m.Student.Representatives[1].SecondLastName, new { @PlaceHolder = "Segundo apellido", 
                @class = "form-control", @id = "apellido-2-representante-2"}) %>
            </div>

            <!-- Correo electrónico -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.Student.Representatives[1].Email) %>

                <%: Html.TextBoxFor(m => m.Student.Representatives[1].Email, new { @PlaceHolder = "Correo electrónico",
                @class = "form-control", @id = "correo-2", @type = "email" }) %>
            </div>
        </div>

        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
            <div class="row">
	            <div class="col-xs-12">
		            <div class="separador"></div>
	            </div>
            </div>
        <div class="form-group col-xs-12"></div>
        
        <!-- Botón: Editar -->
        <div class="col-xs-6 text-right">
            <button class="btn btn-lg btn-default" type="submit" id="btn-agregar">
                Editar
            </button>
        </div>
        <% } %>

        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
            <% using (Html.BeginForm("GestionAlumnos", "Alumnos", FormMethod.Get, new
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

        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
            <div class="row">
	            <div class="col-xs-12">
		            <div class="separador"></div>
	            </div>
            </div>
        <div class="form-group col-xs-12"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Alumnos/EditarAlumno.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Alumnos/GestionTablasAlumnos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Alumnos/AgregarAlumnos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Alumnos/ManejadorArchivosAlumnos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Alumnos/EditarAlumno.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Editar alumno
</asp:Content>