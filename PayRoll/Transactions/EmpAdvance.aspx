<%@ Page Title="Employee Advance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmpAdvance.aspx.cs" Inherits="PayRoll.Transactions.EmpAdvance" %>

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
                                    <span>Transactions</span>
                                </li>
                                <li class="active">
                                    <span>Employee Advance </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Employee Advance
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
                                Add/Edit Employee Advance
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
                                        <asp:TextBox ID="txtEmpName" runat="server" ReadOnly="true" MaxLength="75" type="text" placeholder="Employee Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpName" runat="server" ControlToValidate="txtEmpName" ErrorMessage="Employee Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Advance Type </label>
                                        <asp:DropDownList ID="ddlAdvType" runat="server" class="form-control m-b" name="account" TabIndex="3"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVAdvType" runat="server" ControlToValidate="ddlAdvType" InitialValue="0" ErrorMessage="Select Advance Type" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Advance Date</label>
                                        <asp:TextBox ID="txtDocDate" runat="server" type="text" placeholder="Advance Date(dd/MM/yyyy)" class="form-control" name="name" ValidationGroup="OK" TabIndex="4"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CEtxtDocDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDocDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RFVtxtDocDate" runat="server" ControlToValidate="txtDocDate" ErrorMessage="Select Advance Date" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Advance Amount </label>
                                        <asp:TextBox ID="txtAdvAmt" runat="server" type="text" MaxLength="10" placeholder="Enter Amount" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="5" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAdvAmt" ErrorMessage="Enter Advance Amount" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">Deduction Amount</label>
                                        <asp:TextBox ID="txtDedAmt" runat="server" type="text" MaxLength="10" placeholder="Enter Ded. Amount" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="6" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDedAmt" ErrorMessage="Enter Deduction Amount" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Ded. Start Month</label>
                                        <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account" TabIndex="6">
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
                                        <asp:RequiredFieldValidator ID="RFVddlMon" runat="server" ControlToValidate="ddlMon" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Ded. Start Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="7"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlYear" runat="server" ControlToValidate="ddlYear" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Balance Amount</label>
                                        <asp:TextBox ID="txtBalAmt" runat="server" ReadOnly="true" type="text" MaxLength="10" placeholder="Bal. Amount" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="7" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2" id="divApproveDt" runat="server" visible="false">
                                        <label for="username">Approve Date</label>
                                        <asp:TextBox ID="txtApproveDt" runat="server" type="text" placeholder="Approve Date(dd/MM/yyyy)" class="form-control" name="name" ValidationGroup="OK" TabIndex="4"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CalendarExtender1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtApproveDt" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    </div>
                                    <div class="form-group col-lg-2" style="padding-top: 30px;" id="divApprove" runat="server" visible="false">
                                        <asp:CheckBox ID="chkApproved" runat="server" Checked="False" type="checkbox" class="i-checks" TabIndex="8" />
                                        <label for="Canteen">Approved</label>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                </div>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="21" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="20" />
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
                                Employee Advance List
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">Employee Code</label>
                                        <asp:TextBox ID="txtEmpCodeSearch" runat="server" MaxLength="6" type="text" placeholder="Enter Employee Code" class="form-control" name="name"  TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Employee Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <label for="username">Employee Name</label>
                                        <asp:TextBox ID="txtEmpNameSearch" runat="server" ReadOnly="true" MaxLength="75" type="text" placeholder="Employee Name" class="form-control" name="name" TabIndex="1"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmpName" ErrorMessage="Employee Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group col-sm-2 col-sm-offset-0 " style="margin-top: 22px">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-outline btn-warning2" TabIndex="40" OnClick="btnSearch_Click" />
                                    </div>
                                </div>

                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>

                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="10" Width="100%" DataKeyNames="AdvId" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand" TabIndex="6">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-1">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# Eval("AdvId") %>' CommandName="Show">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Employeecd" HeaderText="Emp. Code" HeaderStyle-CssClass="col-lg-1"  />
                                        <asp:BoundField DataField="Employeename" HeaderText="Emp Name" HeaderStyle-CssClass="col-lg-3" />
                                        <asp:BoundField DataField="AdvName" HeaderText="Adv. Type" HeaderStyle-CssClass="col-lg-2" />
                                        <asp:BoundField DataField="AdvDt" HeaderText="Adv. Date" HeaderStyle-CssClass="col-lg-1" DataFormatString="{0:dd/MM/yyyy}"/>
                                        <asp:BoundField DataField="AdvAmount" HeaderText="Adv. Amount" HeaderStyle-CssClass="col-lg-1" DataFormatString="{0:0.00}"/>
                                        <asp:BoundField DataField="DedAmount" HeaderText="Ded. Amount" HeaderStyle-CssClass="col-lg-1" DataFormatString="{0:0.00}"/>
                                        <asp:BoundField DataField="DedMonYrcd" HeaderText="Ded. Start" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="AdvApproved" HeaderText="Approved" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="ApproveDt" HeaderText="Approve Date" HeaderStyle-CssClass="col-lg-1" DataFormatString="{0:dd/MM/yyyy}"/>

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
