<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.RepresentantesModel>"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Modificar Representantes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



 <!-------------------------------- Fila de Tabla de Representantes -------------------------------------->
  <% using (Html.BeginForm("ModificarRepresentantes", "Representantes", FormMethod.Post, new
           {
               @class = "form",
               @role = "form"
           }))
           { %>
        <%: Html.AntiForgeryToken() %>

        <%: Html.HiddenFor(m => m.Representante.RepresentativeId) %>

        <!-- Username -->
        <div class="form-group col-xs-4">
            <%: Html.LabelFor(m => m.Representante.Name) %>

            <%: Html.TextBoxFor(m => m.Representante.Name, new {
                        @class = "form-control", @id = "text-box-representantenombre"}) %>
        </div>
                
        <!--Separador --> 
        <div class="form-group col-xs-12 separador-formularios"></div>

      

        <!-- Botón: Registrar -->
        <div class="col-xs-12">
            <button  class="btn btn-primary" type="submit" id="btn-modificar-representante">
                        Modificar representante
            </button>
        </div>
        <% } %>

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
<link href="../../Content/Css/Representantes/Representantes.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
<script src="../../Scripts/Views/Representantes/Representantes.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Modificar Representantes
</asp:Content>
