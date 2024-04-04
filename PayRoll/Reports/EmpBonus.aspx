<%@ Page Title="Employee Bonus Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmpBonus.aspx.cs" Inherits="PayRoll.Reports.EmpBonus" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function onlyAlphabets(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32))
                    return true;
                else
                    return false;
            }
            catch (err) {
                alert(err.Description);
            }
        }
    </script>
    <script type="text/javascript">
        function onlyNumbers(txt) {
            if (txt.which != 8 && txt.which != 0 && txt.which != 46 && (txt.which < 48 || txt.which > 57)) {
                $("#errmsg").html("only number allowed").show().fadeOut("slow");
                return false;
            }
        }
    </script>
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
                                    <span>Employee Bonus </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Employee Bonus Info. Report
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
                                Employee Bonus Info. Report
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <label class="col-sm-1 control-label">From </label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlMnth" runat="server" class="form-control" name="account" CausesValidation="True" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="ddlMnth_SelectedIndexChanged">
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
                                                <asp:RequiredFieldValidator ID="RFVddlMon" InitialValue="00" runat="server" ControlToValidate="ddlMnth" ErrorMessage="Select Month" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" CausesValidation="True" AutoPostBack="True" TabIndex="2" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVddlYear" InitialValue="0000" runat="server" ControlToValidate="ddlYear" ErrorMessage="Select Year" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                            </div>

                                            <label class="col-sm-1 control-label">To </label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlToMnth" runat="server" class="form-control" name="account" CausesValidation="True" AutoPostBack="True" TabIndex="3">
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
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="00" runat="server" ControlToValidate="ddlMnth" ErrorMessage="Select Month" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:DropDownList ID="ddlToYear" runat="server" class="form-control" name="account" CausesValidation="True" AutoPostBack="True" TabIndex="4">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" InitialValue="0000" runat="server" ControlToValidate="ddlToYear" ErrorMessage="Select Year" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-1 control-label">Type</label>
                                            <div class="col-sm-3">
                                                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" class="form-control i-checks" TabIndex="1">
                                                    <asp:ListItem Text="Company" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Contract" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <label class="col-sm-1 control-label">Emp.Code </label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtEmpCode" runat="server" type="text" placeholder="" class="form-control" name="name" TabIndex="5" onkeypress="return onlyNumbers(event,this);" MaxLength="6"></asp:TextBox>
                                            </div>

                                            <label class="col-sm-1 control-label">Bank Name</label>
                                            <div class="col-sm-3">
                                                <asp:DropDownList ID="ddlBank" runat="server" class="form-control m-b" name="account" CausesValidation="True" TabIndex="6"></asp:DropDownList>
                                            </div>
                                             <div class="col-sm-1">
                                                 </div>

                                            <div class="col-sm-3">
                                                <asp:Button ID="btnShow" runat="server" Text="Show" class="btn btn-outline btn-success" ValidationGroup="OK" TabIndex="15" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" TabIndex="16" OnClick="btnCancel_Click" />
                                            </div>

                                            <div class="col-sm-7">
                                                 </div>
                                            <div class="form-group col-lg-2" style="padding-top: 10px">
                                                <asp:CheckBox ID="lftEmp" runat="server" Checked="False" type="checkbox" class="i-checks" TabIndex="17" />
                                                <label for="lftEmp">Left Employee</label>
                                            </div>

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
