<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.MateriasModel>"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar Materias
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



     <!-------------------------------------- Fila de Modificar Materia ------------------------------------->
    <div class="row">
        

        <% using (Html.BeginForm("ModificarMaterias", "Materias", FormMethod.Post, new
           {
               @class = "form",
               @role = "form"
           }))
           { %>
        <%: Html.AntiForgeryToken() %>

        <%: Html.HiddenFor(m => m.Materia.SchoolSubjectdId) %>

        <!-- Username -->
        <div class="form-group col-xs-4">
            <%: Html.LabelFor(m => m.Materia.Name) %>

            <%: Html.TextBoxFor(m => m.Materia.Name, new {
                        @class = "form-control", @id = "text-box-materianombre"}) %>
        </div>
                
        <!--Separador --> 
        <div class="form-group col-xs-12 separador-formularios"></div>

      

        <!-- Botón: Registrar -->
        <div class="col-xs-12">
            <button  class="btn btn-primary" type="submit" id="btn-modificar-materia">
                        Modificar materia
            </button>
        </div>
        <% } %>

       
    </div>
    <!------------------------------------ Fin de Fila Modificar Materia -------------------------------->
   

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">

    <link href="../../Content/Css/Materias/Materias.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
<script src="../../Scripts/Views/Materias/Materias.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Modificar Materias
</asp:Content>
