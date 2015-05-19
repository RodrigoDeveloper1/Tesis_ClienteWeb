<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.AddRoleViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Editar rol
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Formulario Registrar Rol -->
        <div class="col-xs-5">
            <% using (Html.BeginForm("EditarRol", "Administrador", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form",
               }))
               { %>
            <%: Html.AntiForgeryToken() %>
                                          
            <%: Html.HiddenFor(m => m.RolId) %>
                                                                                
            <!-- Nombre del rol -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.RolName) %>

                <%: Html.TextBoxFor(m => m.RolName, new { @PlaceHolder = "Nombre del rol",
                        @class = "form-control", @id = "text-box-rol", @disabled = "disabled"}) %>

                <%: Html.HiddenFor(m => m.RolName) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Descripción del rol -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.Description) %>

                <%: Html.TextAreaFor(m => m.Description, new { @class = "form-control", @rows = "3", 
                        @style = "resize: none", @id = "text-area-descripcion" })%>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>

            <!-- Botón: Agregar -->
            <div class="col-xs-12 text-center">
                <button class="btn btn-lg btn-default" type="submit" id="btn-agregar">
                    Editar
                </button>
            </div>
        </div>
                                                                                                     
        <!-- Panel de Lista de Perfiles -->
        <div class="col-xs-7">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de perfiles</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de perfiles -->
                    <div class="col-xs-12" id="div-tabla-lista-perfiles">
                        <table class="table" id="table-lista-perfiles">
                            <thead>
                                <tr>
                                    <th>Nombre</th>
                                    <th>Controlador</th>
                                    <th>Acción/Método</th>
                                    <th>Incluir</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% for (int i = 0; i < Model.PersonalProfileList.Count(); i++)%>
                                <% { %>

                                <% int idProfileAux = Model.PersonalProfileList[i].profile.ProfileId; %>

                                <tr>
                                    <td class="td-name">
                                        <%: Model.PersonalProfileList[i].profile.Name %>
                                    </td>
                                    <td class="td-controller">
                                        <%: Model.PersonalProfileList[i].profile.ControllerName %>
                                    </td>
                                    <td class="td-action"><%: Model.PersonalProfileList[i].profile.Action %></td>
                                    <td class="td-incluir">
                                        <%: Html.CheckBoxFor(m => m.PersonalProfileList[i].isSelected, 
                                        new 
                                        { 
                                            @value = idProfileAux,
                                            @id = idProfileAux,
                                            @Name = "checkboxList"
                                        })%>                                        
                                    </td>
                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <% } %>
        </div>

        <!-- Separador -->
        <div class="row">
            <div class="col-xs-12">
                <div class="separador"></div>
            </div>
        </div>

        <!-- Botón Cancelar -->
        <div class="col-xs-12 text-center">
            <% using (Html.BeginForm("ListarRoles", "Administrador", FormMethod.Get, new
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Rol.css" type="text/css" />    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Administrador.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Administrador/EditarRol.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Editar rol
</asp:Content>