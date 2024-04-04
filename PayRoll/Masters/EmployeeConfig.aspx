<%@ Page Title="Employee Configuration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeConfig.aspx.cs" Inherits="PayRoll.Masters.EmployeeConfig" %>

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
                                    <span>Employee Configuration </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Employee Configuration Master
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
                                Add/Edit Employee Configuration
                            </div>

                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">Employee Code</label>
                                        <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Enter Employee Code" class="form-control" name="name" ValidationGroup="OK" TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Employee Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <label for="username">Employee Name</label>
                                        <asp:TextBox ID="txtEmpName" runat="server" ReadOnly="true" MaxLength="75" type="text" placeholder="Enter Employee Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpName" runat="server" ControlToValidate="txtEmpName" ErrorMessage="Employee Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Document Date</label>
                                        <asp:TextBox ID="txtDocDate" runat="server" type="text" placeholder="Enter Doc. Date(dd/MM/yyyy)" class="form-control" name="name" ValidationGroup="OK" TabIndex="3"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CEtxtDocDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDocDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RFVtxtDocDate" runat="server" ControlToValidate="txtDocDate" ErrorMessage="Doc Date Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">Division </label>
                                        <asp:DropDownList ID="ddlDivision" runat="server" class="form-control m-b" name="account" TabIndex="4"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RFVddlDivision" runat="server" ControlToValidate="ddlDivision" ErrorMessage="Division Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Department</label>
                                        <asp:DropDownList ID="ddlDepartment" runat="server" class="form-control m-b" name="account" TabIndex="5"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RFVddlDepartment" runat="server" ControlToValidate="ddlDepartment" ErrorMessage="Department Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Designation </label>
                                        <asp:DropDownList ID="ddlDesignation" runat="server" class="form-control m-b" name="account" TabIndex="6"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RFVddlDesignation" runat="server" ControlToValidate="ddlDesignation" ErrorMessage="Designation Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Category </label>
                                        <asp:DropDownList ID="ddlCategory" runat="server" class="form-control m-b" name="account" TabIndex="7"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RFVddlCategory" runat="server" ControlToValidate="ddlCategory" ErrorMessage="Category Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Skill</label>
                                        <asp:DropDownList ID="ddlSkill" runat="server" class="form-control m-b" name="account" TabIndex="8"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RFVddlSkill" runat="server" ControlToValidate="ddlSkill" ErrorMessage="Skill Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Status </label>
                                        <asp:DropDownList ID="ddlStatus" runat="server" class="form-control m-b" name="account" TabIndex="9"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RFVddlStatus" runat="server" ControlToValidate="ddlStatus" ErrorMessage="Status Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">HOD Employee Code </label>
                                        <asp:TextBox ID="txtHODEmpCode" runat="server" type="text" placeholder="HOD Employee Code" class="form-control" name="name" ValidationGroup="OK" AutoPostBack="True" OnTextChanged="txtHODEmpCode_TextChanged" TabIndex="10" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <label for="username">HOD Employee Name </label>
                                        <asp:TextBox ID="txtHODEmpName" runat="server" ReadOnly="true" type="text" placeholder="HOD Name" class="form-control" name="name" TabIndex="11"></asp:TextBox>
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
                <div class="row">
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
                                        <asp:BoundField DataField="Docdate" HeaderText="Doc. Date" HeaderStyle-CssClass="col-lg-1" DataFormatString="{0:dd/MM/yyyy}" />
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


</asp:Content>
