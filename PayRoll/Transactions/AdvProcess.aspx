<%@ Page Title="Advance Processing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdvProcess.aspx.cs" Inherits="PayRoll.Transactions.AdvProcess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
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
                                    <span>Advance Processing </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Employee Advance Processing
                        </h2>

                    </div>
                </div>
            </div>

            <div class="content">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                Advance Processing
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">Month Name</label>
                                        <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account" TabIndex="1">
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
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" TabIndex="2"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Employee Code</label>
                                        <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Enter Emp. Code" class="form-control" name="name" ValidationGroup="OK" TabIndex="3" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Employee Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <label for="Canteen">Employee Name</label>
                                        <asp:DropDownList ID="ddlEmpName" runat="server" class="form-control js-source-states" name="account" AutoPostBack="True" TabIndex="4" OnSelectedIndexChanged="ddlEmpName_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                    <div class="form-group col-lg-1" style="margin-top: 22px">
                                        <asp:Button ID="btnGetData" runat="server" Text="Get Data" class="btn btn-outline btn-success" OnClick="btnGetData_Click" TabIndex="999" ValidationGroup="OK" />
                                    </div>

                                    <div class="col-lg-12">
                                        <div class="hpanel">
                                            <div class="form-group">
                                                <div class="col-sm-8 col-sm-offset-0">
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="37" />
                                                    <asp:Button ID="btnSave" runat="server" Text="Advance Process" class="btn btn-outline btn-success" OnClick="btnSave_Click" TabIndex="36" />
                                                    <asp:HiddenField ID="HFEmployee" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
