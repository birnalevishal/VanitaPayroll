﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="PayRoll.Reports.Log" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--Page Layout -->
            <div class="small-header">
                <div class="hpanel">
                    <div class="panel-body">
                        <div id="hbreadcrumb" class="pull-right">
                            <ol class="hbreadcrumb breadcrumb">
                                <li><a runat="server" href="~/Default.aspx">Dashboard</a></li>
                                <li>
                                    <span>Reports</span>
                                </li>
                                <li class="active">
                                    <span>Log </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Log
                        </h2>

                    </div>
                </div>
            </div>

            <div class="content">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Log
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <%--<label class="col-sm-1 control-label">Emp.Code </label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtEmpCode" runat="server" type="text" placeholder="" class="form-control" name="name" TabIndex="5"></asp:TextBox>
                                            </div>--%>

                                            <div class="form-group col-lg-2">
                                                <label for="username">Menu</label>
                                                <asp:DropDownList ID="ddlMenu" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlMenu_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                            </div>
                                             <div class="form-group col-lg-3" style="padding-left:25px">
                                                <label for="username">Sub Menu</label>
                                                <asp:DropDownList ID="ddlSubMenu" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="2" >
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group col-lg-2" style="padding-left:25px">
                                                <label for="fromdate">From</label>
                                                <asp:TextBox ID="txtFromDate" class="form-control" runat="server" Text='<%# Eval("IssueDate") %>' TabIndex="3"></asp:TextBox>
                                                <Ajax:CalendarExtender ID="CEtxtFromDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFromDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                            </div>
                                             <div class="form-group col-lg-2" style="padding-left:25px">
                                                <label for="todate">To</label>
                                                <asp:TextBox ID="txtToDate" class="form-control" runat="server" Text='<%# Eval("IssueDate") %>' TabIndex="4"></asp:TextBox>
                                                <Ajax:CalendarExtender ID="CEtxtToDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtToDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToDate" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="col-lg-1">
                                                 <label for="tt"></label>
                                                <asp:Button ID="btnShow" runat="server" Text="Show" class="btn btn-outline btn-success" ValidationGroup="OK" TabIndex="15" OnClick="btnShow_Click" />
                                            </div>
                                            <div class="col-lg-1">
                                                 <label for="tm"></label>
                                                 <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" TabIndex="16" OnClick="btnCancel_Click" />
                                            </div>
                                        </div>

                                        <div class="col-lg-12">
                                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" class="table table-bordered table-striped" Height="600px">
                                            </rsweb:ReportViewer>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row" style="text-align: center;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <div id="progressBackgroundFilter">
                                </div>
                                <div id="processMessage">
                                    Please Wait...<br />
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/ajax-loader.gif" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>

                </div>

                <!--Page Layout -->
        </ContentTemplate>
    </asp:UpdatePanel>


    <script type="text/javascript">
        var updateProgress = null;
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
</asp:Content>
