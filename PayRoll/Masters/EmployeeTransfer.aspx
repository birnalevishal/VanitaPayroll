﻿<%@ Page Title="Employee Transfer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeTransfer.aspx.cs" Inherits="PayRoll.Masters.EmployeeTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
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
            if (txt.which != 8 && txt.which != 0 && (txt.which < 48 || txt.which > 57)) {
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
                                    <span>Masters</span>
                                </li>
                                <li class="active">
                                    <span>Employee Transfer </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Employee Transfer
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
                                Add/Edit Employee Transfer
                            </div>

                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <label for="username">Prv Organisation </label>
                                        <asp:DropDownList ID="ddlPrvOrg" runat="server" class="form-control m-b" name="account" TabIndex="1"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlPrvOrg" runat="server" InitialValue="0" ControlToValidate="ddlPrvOrg" ErrorMessage="Previous Organisation Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Prv Employee Code</label>
                                        <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Enter Employee Code" class="form-control" name="name" ValidationGroup="OK" TabIndex="2" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Previous Emp Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <label for="username">Employee Name</label>
                                        <asp:TextBox ID="txtEmpName" runat="server" ReadOnly="true" MaxLength="75" type="text" placeholder="Enter Employee Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpName" runat="server" ControlToValidate="txtEmpName" ErrorMessage="Employee Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                   
                                    <div class="form-group col-lg-4">
                                        <label for="username">New Organisation</label>
                                        <asp:DropDownList ID="ddlNewOrg" runat="server" class="form-control m-b" name="account" TabIndex="3"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlNewOrg" runat="server" InitialValue="0" ControlToValidate="ddlNewOrg" ErrorMessage="New Organisation Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">New Employee Code</label>
                                        <asp:TextBox ID="txtEmpCodeNew" runat="server" MaxLength="6" type="text" placeholder="Enter Employee Code" class="form-control" name="name" ValidationGroup="OK" TabIndex="4" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCodeNew_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpCodeNew" runat="server" ControlToValidate="txtEmpCodeNew" ErrorMessage="New Emp Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <label for="username">Employee Name</label>
                                        <asp:TextBox ID="txtEmpNameNew" runat="server" ReadOnly="true" MaxLength="75" type="text" placeholder="Enter Employee Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="2"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RFVtxtEmpNameNew" runat="server" ControlToValidate="txtEmpNameNew" ErrorMessage="Employee Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="98" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="99" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
               <%-- <div class="row">
                    <div class="col-lg-12">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Employee Configuration List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="Employeecd" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand" TabIndex="6">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-2">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Docdate" HeaderText="Doc. Date" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="Divcd" HeaderText="Division" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="LocDepCd" HeaderText="Department" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="Desigcd" HeaderText="Designation" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="HodInchEmpcd" HeaderText="HOD" HeaderStyle-CssClass="col-lg-2" />
                                        <asp:BoundField DataField="Categcd" HeaderText="Category" HeaderStyle-CssClass="col-lg-2" />
                                        <asp:BoundField DataField="Skillcd" HeaderText="Skill" HeaderStyle-CssClass="col-lg-2" />
                                        <asp:BoundField DataField="Stacd" HeaderText="Status" HeaderStyle-CssClass="col-lg-2" />
                                    </Columns>
                                    <PagerStyle CssClass="GridPager" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>--%>


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


</asp:Content>