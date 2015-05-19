<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.MateriasModel>"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Materias
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-------------------------------- Combobox de cursos -------------------------------------->
    <div class="row">
        <!-- Lista de cursos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!-- Panel de Lista de Materias -->
    <div class="row">
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de materias</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de Materias -->
                    <div class="col-xs-12" id="div-tabla-lista-materias">
                        <table class="table" id="table-lista-materias">
                            <thead>
                                <tr>
                                    <th class="th-nombre">Nombre de materia</th>
                                    <th class="th-codigo">Código</th>
                                    <th class="th-pensum">Pensum</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="th-nombre"></td>
                                    <td class="th-codigo"></td>
                                    <td class="th-pensum"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="separador"></div>
            </div>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">

    <link href="../../Content/Css/Materias/Materias.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
<script src="../../Scripts/Views/Materias/Materias.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Materias
</asp:Content>
