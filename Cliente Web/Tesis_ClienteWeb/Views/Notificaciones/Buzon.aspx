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
                        <button class="btn btn-default btn-sm checkbox-toggle">
                            <i class="fa fa-square-o"></i>
                        </button>

                        <!-- Botones basura, atrás y adelante -->
                        <div class="btn-group">
                            <button class="btn btn-default btn-sm"><i class="fa fa-trash-o"></i></button>
                            <button class="btn btn-default btn-sm" disabled="disabled"><i class="fa fa-reply"></i></button>
                            <button class="btn btn-default btn-sm" disabled="disabled"><i class="fa fa-share"></i></button>
                        </div>

                        <!-- Botón actualizador-->
                        <button class="btn btn-default btn-sm"><i class="fa fa-refresh"></i></button>


                        <div class="pull-right">
                            1-50/200
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
                    <div class="table-responsive mailbox-messages">
                        <table class="table table-hover table-striped">
                            <tbody>
                                <% foreach (Object NotificationObject in Model.listaNotificacionesObject) %>
                                <% { %>
                                    <% bool Success = Convert.ToBoolean(NotificationObject.GetType().GetProperty("Success").GetValue(NotificationObject, null).ToString()); %>
                                    <% int NotificationId = Convert.ToInt32(NotificationObject.GetType().GetProperty("NotificationId").GetValue(NotificationObject, null).ToString()); %>
                                    <% int SentNotificationId = Convert.ToInt32(NotificationObject.GetType().GetProperty("SentNotificationId").GetValue(NotificationObject, null).ToString()); %>
                                    <% string Notification = NotificationObject.GetType().GetProperty("Notification").GetValue(NotificationObject, null).ToString(); %>
                                    <% int NotificationLength = Notification.Length; %>
                                    <% string NotificationCutted = Notification.Substring(0, 41) + "...";%>

                                    <tr id="<%: NotificationId %>">
                                        <td><input type="checkbox" /></td>
                                        <!--td class="mailbox-star">
                                            <a href="#"><i class="fa fa-star text-yellow"></i></a>
                                        </!td-->
                                        <td class="mailbox-name">
                                            <a href="read-mail.html">Prueba</a>
                                        </td>
                                        <td class="mailbox-subject">
                                            <b>AdminLTE 2.0 Issue</b> - 
                                            <%: (NotificationLength > 41 ? NotificationCutted : Notification) %>
                                        </td>
                                        <td class="mailbox-attachment"></td>
                                        <td class="mailbox-date">5 mins ago</td>
                                    </tr>
                                <% } %>
                                </tbody>
                        </table>
                    </div>
                </div>

                <!-- /.box-body -->
                <div class="box-footer no-padding">
                    <div class="mailbox-controls">
                        <!-- Check all button -->
                        <button class="btn btn-default btn-sm checkbox-toggle"><i class="fa fa-square-o"></i></button>
                        <div class="btn-group">
                            <button class="btn btn-default btn-sm"><i class="fa fa-trash-o"></i></button>
                            <button class="btn btn-default btn-sm"><i class="fa fa-reply"></i></button>
                            <button class="btn btn-default btn-sm"><i class="fa fa-share"></i></button>
                        </div>
                        <!-- /.btn-group -->
                        <button class="btn btn-default btn-sm"><i class="fa fa-refresh"></i></button>
                        <div class="pull-right">
                            1-50/200
                     
                                    <div class="btn-group">
                                        <button class="btn btn-default btn-sm"><i class="fa fa-chevron-left"></i></button>
                                        <button class="btn btn-default btn-sm"><i class="fa fa-chevron-right"></i></button>
                                    </div>
                            <!-- /.btn-group -->
                        </div>
                        <!-- /.pull-right -->
                    </div>
                </div>
            </div>
            <!-- /. box -->
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Notificaciones/Notificaciones.css" rel="Stylesheet" type="text/css"/>
    <link href="../../Content/Css/Notificaciones/NuevaNotificacion.css" rel="Stylesheet" type="text/css"/>
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
    Buzón de notificaciones <small>2 mensajes nuevos</small>
</asp:Content>