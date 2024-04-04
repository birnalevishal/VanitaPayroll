﻿<%@ Page Title="Bonus Import" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BonusImport.aspx.cs" Inherits="PayRoll.Transactions.BonusImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="small-header">
                <div class="hpanel">
                    <div class="panel-body">
                        <div id="hbreadcrumb" class="pull-right">
                            <ol class="hbreadcrumb breadcrumb">
                                <li><a runat="server" href="~/Default.aspx">Dashboard</a></li>
                                <li>
                                    <span>Transaction</span>
                                </li>
                                <li class="active">
                                    <span>Bonus Import</span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Import Excel For Bonus
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
                                Import Excel For Bonus
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">From Month</label>
                                        <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="1">
                                            <asp:ListItem Text="select" Value="00" Selected="True"></asp:ListItem>
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
                                    <div class="form-group col-lg-2">
                                        <label for="username">Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="2"></asp:DropDownList>
                                    </div>
                                     <div class="form-group col-lg-2">
                                        <label for="username">To Month</label>
                                        <asp:DropDownList ID="ddlMonTo" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="1">
                                            <asp:ListItem Text="select" Value="00" Selected="True"></asp:ListItem>
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
                                    <div class="form-group col-lg-2">
                                        <label for="username">Year</label>
                                        <asp:DropDownList ID="ddlYearTo" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="2"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">On Date</label>
                                        <asp:TextBox ID="txtDt" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="8"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtDt" Format="dd/MM/yyyy"/>
                                        <asp:RequiredFieldValidator ID="RFVtxtdate" runat="server" ControlToValidate="txtDt" ErrorMessage="Date Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>

                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">Select File</label>
                                        <asp:FileUpload ID="FUExcel" runat="server" TabIndex="3" />
                                    </div>
                                             
                                    <div class="form-group col-lg-3" style="padding-top:10px; float:right">
                                        <a href="Download.ashx?file=~/ImportFormatFiles/BonusImportFormat.xlsx" class="btn btn-outline btn-warning" TabIndex="6">Download Import Format</a>
                                    </div>
                                </div>

                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>
                                
                                <div class="form-group">
                                    <div class="col-sm-8 col-sm-offset-0">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="5" />
                                        <asp:Button ID="btnImport" runat="server" Text="Import" class="btn btn-outline btn-success"  ValidationGroup="OK" TabIndex="4" OnClick="btnImport_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
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


        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImport" />
        </Triggers>
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
