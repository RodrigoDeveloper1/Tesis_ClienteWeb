<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.BuzonNotificacionesModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Buzon
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-primary">
                <div class="box-body no-padding">
                    <!-- Botones de control-->
                    <div class="mailbox-controls">
                        <!-- Botón de check all buttons -->
                        <button class="btn btn-default btn-sm checkbox-toggle" disabled="disabled">
                            <i class="fa fa-square-o"></i>
                        </button>

                        <!-- Botones basura, atrás y adelante -->
                        <div class="btn-group">
                            <button class="btn btn-default btn-sm" disabled="disabled"><i class="fa fa-trash-o"></i></button>
                            <button class="btn btn-default btn-sm" disabled="disabled"><i class="fa fa-reply"></i></button>
                            <button class="btn btn-default btn-sm" disabled="disabled"><i class="fa fa-share"></i></button>
                        </div>

                        <!-- Botón actualizador & mensajes enviados-->
                        <div class="btn-group">
                            <button class="btn btn-default btn-sm btn-refresh"><i class="fa fa-refresh"></i></button>
                            <button class="btn btn-default btn-sm buzon-enviados"><i class="fa fa-share-square-o"></i></button>
                        </div>

                        <!-- Botón nueva notificación -->
                        <button class="btn btn-default btn-sm btn-nueva-notificacion"><i class="fa fa-envelope-o"></i></button>

                        <!-- Flechas derecha e izquierda -->
                        <div class="pull-right">
                            <!--1-50/200-->
                            <div class="btn-group">
                                <button class="btn btn-default btn-sm" disabled="disabled">
                                    <i class="fa fa-chevron-left"></i>
                                </button>
                                <button class="btn btn-default btn-sm" disabled="disabled">
                                    <i class="fa fa-chevron-right"></i>
                                </button>
                            </div>
                        </div>
                    </div>

                    <!-- Tabla de notificaciones -->
                    <div class="table-responsive mailbox-messages" id="div-table-notificaciones">
                        <table class="table table-hover table-striped" id="table-notificaciones">
                            <tbody>
                                <% foreach (Object NotificationObject in Model.listaNotificacionesObject) %>
                                <% { %>
                                    <% bool Success = Convert.ToBoolean(NotificationObject.GetType().GetProperty("Success").GetValue(NotificationObject, null).ToString()); %>
                                    <% int NotificationId = Convert.ToInt32(NotificationObject.GetType().GetProperty("NotificationId").GetValue(NotificationObject, null).ToString()); %>
                                    <% int SentNotificationId = Convert.ToInt32(NotificationObject.GetType().GetProperty("SentNotificationId").GetValue(NotificationObject, null).ToString()); %>
                                    <% string Notification = NotificationObject.GetType().GetProperty("Notification").GetValue(NotificationObject, null).ToString(); %>
                                    <% int NotificationLength = Notification.Length; %>
                                    <% string NotificationCutted = (NotificationLength > 41 ?
                                           Notification.Substring(0, 41) + "..." : Notification);%>
                                    <% string NotficationAttribution = NotificationObject.GetType().GetProperty("Attribution").GetValue(NotificationObject, null).ToString(); %>
                                    <% string From = NotificationObject.GetType().GetProperty("From").GetValue(NotificationObject, null).ToString(); %>
                                    <% string DateOfCreation = NotificationObject.GetType().GetProperty("DateOfCreation").GetValue(NotificationObject, null).ToString(); %>    

                                    <tr id="<%: NotificationId %>">
                                        <td><input type="checkbox" /></td>
                                        <!--td class="mailbox-star">
                                            <a href="#"><i class="fa fa-star text-yellow"></i></a>
                                        </!td-->
                                        <td class="mailbox-name" style="font-style: italic">
                                            <!--a href="read-mail.html"><%: From %></a-->
                                            <%: From %>
                                        </td>
                                        <td class="mailbox-subject">
                                            <b><%: NotficationAttribution %></b> - 
                                            <%: (NotificationLength > 41 ? NotificationCutted : Notification) %>
                                        </td>
                                        <td class="mailbox-attachment"></td>
                                        <td class="mailbox-date"><%: DateOfCreation %></td>
                                    </tr>
                                <% } %>
                                </tbody>
                        </table>
                    </div>
                </div>

                <div class="box-footer no-padding">
                    <div class="mailbox-controls">
                        <!-- Check all button -->
                        <button class="btn btn-default btn-sm checkbox-toggle"><i class="fa fa-square-o"></i></button>
                        <div class="btn-group">
                            <button class="btn btn-default btn-sm" disabled="disabled"><i class="fa fa-trash-o"></i></button>
                            <button class="btn btn-default btn-sm" disabled="disabled"><i class="fa fa-reply"></i></button>
                            <button class="btn btn-default btn-sm" disabled="disabled"><i class="fa fa-share"></i></button>
                        </div>
                        
                        <!-- Botón actualizador & mensajes enviados-->
                        <div class="btn-group">
                            <button class="btn btn-default btn-sm btn-refresh"><i class="fa fa-refresh"></i></button>
                            <button class="btn btn-default btn-sm buzon-enviados"><i class="fa fa-share-square-o"></i></button>
                        </div>
                        
                        <!-- Botón nueva notificación -->
                        <button class="btn btn-default btn-sm btn-nueva-notificacion">
                            <i class="fa fa-envelope-o"></i>
                        </button>

                        <!-- Flechas derecha e izquierda -->
                        <div class="pull-right">
                            <!-- 1-50/200 -->
                     
                            <div class="btn-group">
                                <button class="btn btn-default btn-sm" disabled="disabled">
                                    <i class="fa fa-chevron-left"></i>
                                </button>
                                <button class="btn btn-default btn-sm" disabled="disabled">
                                    <i class="fa fa-chevron-right"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <!-- Diálogo nueva Notificación -->
        <div id="dialog-nueva-notificacion">
            <!-- Select del curso -->
            <div class="col-xs-12">
                <%: Html.LabelFor(m => m.idCurso)%>
                <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione un curso", 
                    new { @class = "form-control selectpicker", @id = "select-cursos"})%>
            </div>

            <!--Separador normal -->
            <div class="form-group col-xs-12"></div>

            <!-- Select sujeto -->
            <div class="col-xs-12" id="select-sujeto-2">
                <%: Html.LabelFor(m => m.idElSujeto)%>
                <%: Html.DropDownListFor(m => m.idElSujeto, Model.selectListElSujeto, "Seleccione el sujeto...", 
                    new { @class = "form-control selectpicker", @id = "select-el-sujeto"})%>
            </div>

            <!--Separador normal & con línea -->
            <div class="form-group col-xs-12"></div>
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Select tipos de alerta -->
            <div class="col-xs-12" id="select-tipos-notificacion">
                <%: Html.LabelFor(m => m.idTipoNotificacion)%>
                <%: Html.DropDownListFor(m => m.idTipoNotificacion, Model.selectListTiposNotificacion, 
                    "Seleccione el tipo de notificación...", new { @class = "form-control selectpicker", 
                    @id = "select-tipo-notificacion", @disabled = "disabled"})%>
            </div>

            <!--Separador normal & con línea -->
            <div class="form-group col-xs-12"></div>
            <div class="col-xs-12"><div class="separador"></div></div>

            <!-- Mensaje de la notificación -->
            <div class="row" id="fila-mensaje-nueva-notificacion">
                <div class="form-group col-xs-12">
                    <%: Html.LabelFor(m => m.mensaje, new { @class="pull-left" })%>
                
                    <%: Html.TextAreaFor(m => m.mensaje, new { @class = "form-control", @rows = "4", 
                            @style = "resize: none", @id = "text-area-notificacion" })%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Notificaciones/Notificaciones.css" rel="Stylesheet" type="text/css"/>
    <link href="../../Content/Css/Notificaciones/NuevaNotificacionDocente.css" rel="Stylesheet" type="text/css"/>
    <!-- Ionicons -->
    <link href="../../Content/Plug-ins/ionicons-2.0.1/css/ionicons.min.css" rel="Stylesheet" type="text/css"/>
    <!-- AdminLTE-2.1.1 -->
    <link href="../../Content/Plug-ins/AdminLTE-2.1.1/dist/css/AdminLTE.min.css" rel="Stylesheet" type="text/css"/>
    <link href="../../Content/Plug-ins/AdminLTE-2.1.1/dist/css/skins/_all-skins.min.css" rel="Stylesheet" type="text/css"/>
    <!-- icheck-1.x -->
    <link href="../../Content/Plug-ins/icheck-1.x/skins/flat/blue.css" rel="Stylesheet" type="text/css"/>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Notificaciones/Notificaciones.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Notificaciones/NotificacionesPersonalizadas.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Notificaciones/Buzon.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Notificaciones/NuevaNotificacionDocente.js" type="text/javascript"></script>
    <!-- AdminLTE-2.1.1 -->
    <script src="../../Content/Plug-ins/AdminLTE-2.1.1/dist/js/app.min.js" type="text/javascript"></script>
    <!-- icheck-1.x -->
    <script src="../../Content/Plug-ins/icheck-1.x/icheck.min.js" type="text/javascript"></script>

     <!-- Page Script -->
    <script>
      $(function () {
        //Enable iCheck plugin for checkboxes
        //iCheck for checkbox and radio inputs
        $('.mailbox-messages input[type="checkbox"]').iCheck({
          checkboxClass: 'icheckbox_flat-blue',
          radioClass: 'iradio_flat-blue'
        });

        //Enable check and uncheck all functionality
        $(".checkbox-toggle").click(function () {
          var clicks = $(this).data('clicks');
          if (clicks) {
            //Uncheck all checkboxes
            $(".mailbox-messages input[type='checkbox']").iCheck("uncheck");
            $(".fa", this).removeClass("fa-check-square-o").addClass('fa-square-o');
          } else {
            //Check all checkboxes
            $(".mailbox-messages input[type='checkbox']").iCheck("check");
            $(".fa", this).removeClass("fa-square-o").addClass('fa-check-square-o');
          }          
          $(this).data("clicks", !clicks);
        });

        //Handle starring for glyphicon and font awesome
        $(".mailbox-star").click(function (e) {
          e.preventDefault();
          //detect type
          var $this = $(this).find("a > i");
          var glyph = $this.hasClass("glyphicon");
          var fa = $this.hasClass("fa");

          //Switch states
          if (glyph) {
            $this.toggleClass("glyphicon-star");
            $this.toggleClass("glyphicon-star-empty");
          }

          if (fa) {
            $this.toggleClass("fa-star");
            $this.toggleClass("fa-star-o");
          }
        });
      }); 
    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Buzón de notificaciones 
    <!--small>2 mensajes nuevos</small-->
</asp:Content>