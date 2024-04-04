<%@ Page Title="Asset Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssetRegister.aspx.cs" Inherits="PayRoll.Reports.AssetRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

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
                                    <span>Asset Register </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Asset Register
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
                            Asset Register
                        </div>

                        <div class="panel-body">
                            <div class="col-lg-12">
                                <div class="form-group col-lg-2">
                                    <label for="username">Type </label>
                                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" class="form-control" TabIndex="1"  OnSelectedIndexChanged="rblType_SelectedIndexChanged"  >
                                        <asp:ListItem Text="Individual" Value="1" Selected="True" ></asp:ListItem> 
                                        <asp:ListItem Text="All" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                  <div class="form-group col-lg-4">
                                    <asp:Label ID="lblEmpName" runat="server" Text="Employee Name" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="ddlEmpName" runat="server" class="form-control js-source-states" name="account"   AutoPostBack="True" TabIndex="4" OnSelectedIndexChanged="ddlEmpName_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="form-group col-lg-2">
                                   <asp:Label ID="lblEmpCode" runat="server" Text="Employee Code" Font-Bold="true"></asp:Label>
                                    <asp:TextBox ID="txtEmpCode" runat="server" type="textarea" MaxLength="80" placeholder="Employee Code" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Employee Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                </div>
                              
                                <div class="form-group col-lg-2">
                                    <asp:Label ID="lblIssueDate" runat="server" Text="Issue Date" Font-Bold="true" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txtDate" runat="server" type="textarea" MaxLength="80" placeholder="Enter Date" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2" Visible="false"></asp:TextBox>
                                    <Ajax:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RFVtxtDate" runat="server" ControlToValidate="txtDate" ErrorMessage="As On Date Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group col-lg-2">
                                    <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="true"></asp:Label>
                                    <asp:TextBox ID="txtToDate" runat="server" type="textarea" MaxLength="80" placeholder="Enter To Date" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2"></asp:TextBox>
                                    <Ajax:CalendarExtender ID="CEtxtToDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtToDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    
                                </div>
                                
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" TabIndex="13" OnClick="btnCancel_Click" CausesValidation="False" />
                                            <asp:Button ID="btnSave" runat="server" Text="Show" class="btn btn-outline btn-success" ValidationGroup="OK" TabIndex="12" OnClick="btnSave_Click" />
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
    </asp:UpdatePanel>


    <script type="text/javascript">
        var updateProgress = null;
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
    <script>
        $(function () {
            $(".js-source-states").select2();
        });
    </script>
</asp:Content>
