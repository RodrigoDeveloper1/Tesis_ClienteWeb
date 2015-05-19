<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Editar Perfil
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


                    <div class="panel panel-primary ">
					<div class="panel-heading">
							
								<h2 class="panel-title">Opciones de perfil</h2>
														
                         </div> 
                           
                          <div class="panel-body">
								<div class="row">
									<div class="col-xs-6">
										<form class="form-horizontal" role="form">
											<div class="form-group">
												<label class="col-xs-3 control-label">Nombre de usuario</label>
												<div class="col-xs-5">
													<p class="form-control-static">Pablo Perez</p>
												</div>
											</div>
											<div class="form-group">
												<label class="col-xs-3 control-label">Imagen de Usuario</label>
												<div class="col-xs-9">
													
                                                    
                                                    
                                                    <div class="">
														<div class="col-xs-3" id="foto-profile"><img id="img-profile" class="thumbnail" src="../../Content/Images/About/fabio.png"></div>
														<div class="col-xs-6">
															
                                                                
                                                                    <span class="btn btn-primary btn-file" id="span-btn-files">
                                                                        <strong>Examinar...</strong><input type="file" id="input-load-file" />
                                                                    </span>
                                                                     
                                                        </div>
															
													</div>



													</div>
											</div>
											<div class="form-group">
												<label class="col-xs-3 control-label">Nombre completo</label>
												<div class="col-xs-9">
													<input class="form-control" type="text" placeholder="Pablo Perez">
												</div>
											</div>
											<div class="form-group">
												<label class="col-xs-3 control-label">E-mail</label>
												<div class="col-xs-9">
													<input class="form-control" type="text" placeholder="pabloperez@domain.com">
												</div>
											</div>
											<div class="form-group">
												<div class="col-xs-offset-3 col-xs-10">
													<div class="checkbox styled-checkbox">
														<label class="has-pretty-child">
															<div class="clearfix prettycheckbox labelright  blue "><input type="checkbox" name="legal" value="true" style="display: none;"><a href="#" class=""></a>
                                                      
                                                        </div>
														</label>
													</div>
												</div>
											</div>
											<div class="form-group">
												<div class="col-xs-offset-3 col-xs-10">
													<button class="btn btn-lg btn-primary" type="submit">Salvar cambios</button>
												</div>
											</div>
										</form>
									</div>
									<div class="col-xs-6">
										<h4>Cambiar password</h4>
										<p>Suspendisse sed turpis sem. Morbi tempus sapien accumsan metus ultricies tristique. Vestibulum lacus orci, consequat dignissim ullamcorper vel, viverra eleifend enim. Nullam et turpis urna, eu gravida ligula.</p>
										<form class="form-horizontal" action="" role="form">
											<div class="form-group">
												<label class="col-xs-3 control-label">Nuevo Password</label>
												<div class="col-xs-5">
													<input class="form-control" type="password">
												</div>
											</div>
											<div class="form-group">
												<label class="col-xs-3 control-label">Confirmar Password</label>
												<div class="col-xs-5">
													<input class="form-control" type="password">
												</div>
											</div>
											<div class="form-group">
												<div class="col-xs-offset-3 col-xs-10">
													<button class="btn btn-lg btn-primary" type="submit">Cambiar password</button>
												</div>
											</div>
										</form>
									</div>
								</div>
                            </div>
						</div> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">

<link href="../../Content/Css/Profile/Profile.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Perfil
</asp:Content>
