﻿<%@ Page Title="Paid Leaves Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaidLeavesReport.aspx.cs" Inherits="PayRoll.Reports.PaidLeavesReport" %>

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
                                    <span>Paid Leaves Report </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Paid Leaves Report
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
                                Paid Leaves Report
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <label class="col-sm-1 control-label">From </label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlMnth" runat="server" class="form-control" CausesValidation="false" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="ddlMnth_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value="00" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="January" Value="01"></asp:ListItem>
                                                    <asp:ListItem Text="February" Value="02"></asp:ListItem>
                                                    <asp:ListItem Text="March" Value="03"></asp:ListItem>
                                                    <asp:ListItem Text="April" Value="04"></asp:ListItem>
                                                    <asp:ListItem Text="May" Value="05"></asp:ListItem>
                                                    <asp:ListItem Text="June" Value="06"></asp:ListItem>
                                                    <asp:ListItem Text="July" Value="07"></asp:ListItem>
                                                    <asp:ListItem Text="August" Value="08"></asp:ListItem>
                                                    <asp:ListItem Text="September" Value="09"></asp:ListItem>
                                                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" CausesValidation="false" AutoPostBack="True" TabIndex="2" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>

                                            <label class="col-sm-1 control-label">To </label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlToMnth" runat="server" class="form-control" name="account" CausesValidation="false" TabIndex="3">
                                                    <asp:ListItem Text="Select" Value="00" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="January" Value="01"></asp:ListItem>
                                                    <asp:ListItem Text="February" Value="02"></asp:ListItem>
                                                    <asp:ListItem Text="March" Value="03"></asp:ListItem>
                                                    <asp:ListItem Text="April" Value="04"></asp:ListItem>
                                                    <asp:ListItem Text="May" Value="05"></asp:ListItem>
                                                    <asp:ListItem Text="June" Value="06"></asp:ListItem>
                                                    <asp:ListItem Text="July" Value="07"></asp:ListItem>
                                                    <asp:ListItem Text="August" Value="08"></asp:ListItem>
                                                    <asp:ListItem Text="September" Value="09"></asp:ListItem>
                                                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlToYear" runat="server" class="form-control" name="account" CausesValidation="false" TabIndex="4">
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                        
                                        <div class="form-group">
                                            <div class="col-sm-4">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" TabIndex="16" OnClick="btnCancel_Click" CausesValidation="False" />
                                                <asp:Button ID="btnSearch" runat="server" Text="Show" class="btn btn-outline btn-success" ValidationGroup="OK" TabIndex="15" OnClick="btnSearch_Click" CausesValidation="False" />

                                            </div>
                                        </div>

                                    </div>

                                    <div class="col-lg-12">
                                        <div class="hr-line-dashed"></div>
                                    </div>
                                    <div class="col-lg-12">
                                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" class="table table-bordered table-striped" Height="800px"></rsweb:ReportViewer>
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
