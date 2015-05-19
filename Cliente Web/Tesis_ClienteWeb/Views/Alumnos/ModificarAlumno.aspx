<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.AlumnosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar Alumno
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



  <div class="row">
        

        <% using (Html.BeginForm("ModificarAlumno", "Alumnos", FormMethod.Post, new
           {
               @class = "form",
               @role = "form"
           }))
           { %>
        <%: Html.AntiForgeryToken() %>

        <%: Html.HiddenFor(m => m.Student.StudentId) %>

        <div class="form-group col-xs-4">
            <%: Html.LabelFor(m => m.Student.FirstLastName) %>

            <%: Html.TextBoxFor(m => m.Student.FirstLastName, new {
                        @class = "form-control", @id = "text-box-materianombre"}) %>
        </div>

        <!--Separador --> 
        <div class="form-group col-xs-12 separador-formularios"></div>

                
        <div class="form-group col-xs-4">
            <%: Html.LabelFor(m => m.Student.SecondLastName) %>

            <%: Html.TextBoxFor(m => m.Student.SecondLastName, new {
                        @class = "form-control", @id = "text-box-materianombre"}) %>
        </div>

        <!--Separador --> 
        <div class="form-group col-xs-12 separador-formularios"></div>

                
        <div class="form-group col-xs-4">
            <%: Html.LabelFor(m => m.Student.FirstName) %>

            <%: Html.TextBoxFor(m => m.Student.FirstName, new {
                        @class = "form-control", @id = "text-box-materianombre"}) %>
        </div>

        <!--Separador --> 
        <div class="form-group col-xs-12 separador-formularios"></div>

                
        <div class="form-group col-xs-4">
            <%: Html.LabelFor(m => m.Student.SecondName) %>

            <%: Html.TextBoxFor(m => m.Student.SecondName, new {
                        @class = "form-control", @id = "text-box-materianombre"}) %>
        </div>

        <!--Separador --> 
        <div class="form-group col-xs-12 separador-formularios"></div>

      

        <!-- Botón: Registrar -->
        <div class="col-xs-12">
            <button  class="btn btn-primary" type="submit" id="btn-modificar-alumno">
                        Modificar alumno
            </button>
        </div>
        <% } %>

       
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
<link href="../../Content/Css/Alumnos/ListaAlumnos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Alumnos/Alumnos.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Modificar Alumno
</asp:Content>
