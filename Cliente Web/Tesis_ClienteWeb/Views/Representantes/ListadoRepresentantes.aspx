<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.GestionRepresentantesModel>"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Listado de Representantes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Lista de cursos -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>
        <!-- Separador -->
        <div class="row">
            <div class="col-xs-12">
                <div class="separador"></div>
            </div>
        </div>    	

        </div>
    <div class="row">
        <!-- Tabla de alumnos -->
        <div class="col-xs-6">
            <div class="panel panel-primary"  >
                <div class="panel-heading">
                    <strong>Lista de alumnos</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de alumnos -->
                    <div class="col-xs-12" id="div-tabla-lista-alumnos">
                        <table class="table" id="table-lista-alumnos">
                            <thead>
                                <tr>
                                    <th class="th-primerapellido-alumno">1er Apellido</th>
                                    <th class="th-segundoapellido-alumno">2do Apellido</th>
                                    <th class="th-primernombre-alumno">1er Nombre</th>
                                    <th class="th-segundonombre-alumno">2do Nombre</th>                                                                    
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>                                    
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

         <!-- Tabla de representantes -->
        <div class="col-xs-6">
            <div class="panel panel-primary" >
                <div class="panel-heading">
                    <strong>Lista de representantes</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de representantes -->
                    <div class="col-xs-12" id="div-tabla-lista-representantes">
                        <table class="table" id="table-lista-representantes">
                            <thead>
                                <tr>
                                    <th class="th-apellidos">Nombre</th>
                                    <th class="th-nombres">Primer Apellido</th>
                                    <th class="th-representante-1">Segundo Apellido</th>                                 
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>                            
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

     </div>     
        <!-- Separador -->
        <div class="row">
            <div class="col-xs-12">
                <div class="separador"></div>
            </div>
        </div>    	

        <!-- Botón Cancelar -->
        <div class="col-xs-12 text-center">            
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">  
    <link href="../../Content/Css/Alumnos/Representantes.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Representantes/ListadoRepresentantes.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión de alumnos
</asp:Content>
