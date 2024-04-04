﻿<%@ Page Title="Bonus Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BonusRegister.aspx.cs" Inherits="PayRoll.Reports.BonusRegister" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
                                    <span>Bonus Register </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Bonus Register Report
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
                                Bonus Register Report
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">Bonus Period </label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" TabIndex="2">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:Button ID="btnSave" runat="server" Text="Show" class="btn btn-outline btn-success" ValidationGroup="OK" TabIndex="15" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" TabIndex="16" OnClick="btnCancel_Click" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-12">
                                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" class="table table-bordered table-striped" Height="700px">
                                        </rsweb:ReportViewer>
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
