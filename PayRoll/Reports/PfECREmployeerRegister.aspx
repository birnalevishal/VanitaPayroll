<%@ Page Title="PF ECR Employeer Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PfECREmployeerRegister.aspx.cs" Inherits="PayRoll.Reports.PfECREmployeerRegister" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                            <span>PF ECR Employeer Register </span>
                        </li>
                    </ol>
                </div>
                <h2 class="font-light m-b-xs">PF ECR Employeer Register
                </h2>
            </div>
        </div>
    </div>

    <div class="content">
        <div class="row">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    <div class="panel-tools">
                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                        <a class="closebox"><i class="fa fa-times"></i></a>
                    </div>
                    PF ECR Employeer Register                   
                </div>

                <div class="panel-body">
                    <div class="col-lg-12">
                        <div class="form-group col-lg-2">
                            <label for="username">Month Name</label>
                            <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account" CausesValidation="True" AutoPostBack="True" TabIndex="1">
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
                            <asp:RequiredFieldValidator ID="RFVddlMon" InitialValue="00" runat="server" ControlToValidate="ddlMon" ErrorMessage="Select Month" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-lg-2">
                            <label for="username">Year</label>
                            <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" CausesValidation="True" AutoPostBack="True" TabIndex="2">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RFVddlYear" InitialValue="0000" runat="server" ControlToValidate="ddlYear" ErrorMessage="Select Year" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                        </div>

                    </div>
                    <div class="col-lg-12">
                        <div class="form-group">
                            <div class="col-sm-8 col-sm-offset-0">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" TabIndex="6" OnClick="btnCancel_Click"  />
                                <asp:Button ID="btnSave" runat="server" Text="Show" class="btn btn-outline btn-success" ValidationGroup="OK" TabIndex="5" OnClick="btnSave_Click"  />
                                <asp:Button ID="btnCSV" runat="server" Text="CSV Format" class="btn btn-outline btn-success" ValidationGroup="OK" TabIndex="5" OnClick="btnCSV_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <div class="hr-line-dashed"></div>
                    </div>
                    <div class="col-lg-12">
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" class="table table-bordered table-striped" Height="400px">
                        </rsweb:ReportViewer>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
