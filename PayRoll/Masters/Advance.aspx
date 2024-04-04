<%@ Page Title="Advance Transanction" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Advance.aspx.cs" Inherits="PayRoll.Masters.Advance" %>

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
                                    <span>Masters</span>
                                </li>
                                <li class="active">
                                    <span>Advance Transanction </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Advance Transanction
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
                                Add/Edit Advance Transanction
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-2">
                                            <label for="username">Employee Code</label>
                                            <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Employee Code" class="form-control" name="name" ValidationGroup="OK" TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group col-lg-4">
                                            <label for="username">Employee Name</label>
                                            <asp:TextBox ID="txtEmpName" runat="server" ReadOnly="true" type="text" placeholder="Employee Name" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK"></asp:TextBox>
                                        </div>

                                        <div class="form-group col-lg-2">
                                            <label for="username">Amount</label>
                                            <asp:TextBox ID="txtAmount" runat="server" type="text" placeholder="Advance Amount" class="form-control" name="name" ValidationGroup="OK"  TabIndex="2" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVtxtAmount" runat="server" ControlToValidate="txtAmount" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>

                                        </div>
                                        <div class="form-group col-lg-2">
                                            <label for="username">Application Date</label>
                                            <asp:TextBox ID="txtApplicationDate" class="form-control" runat="server"  TabIndex="3" MaxLength="10"></asp:TextBox>
                                            <Ajax:CalendarExtender ID="CEtxtApplicationDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtApplicationDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RFVtxtApplicationDate" runat="server" ControlToValidate="txtApplicationDate" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group col-lg-2">
                                            <label for="username">Sanction Date</label>
                                            <asp:TextBox ID="txtSanctionDt" class="form-control" runat="server"  TabIndex="4" MaxLength="10"></asp:TextBox>
                                            <Ajax:CalendarExtender ID="CEtxtSanctionDt" PopupButtonID="imgPopup" runat="server" TargetControlID="txtSanctionDt" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                            <%--<asp:RequiredFieldValidator ID="RFVtxtSanctionDt" runat="server" ControlToValidate="txtSanctionDt" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-2">
                                            <label for="username">Deduction </label>
                                            <asp:TextBox ID="txtDeduction" runat="server" type="text" placeholder="Deduction Amount" class="form-control" name="name" ValidationGroup="OK" onkeypress="return onlyNumbers(event,this);"  TabIndex="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVtxtDeduction" runat="server" ControlToValidate="txtDeduction" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group col-lg-2">
                                        <label for="username">Month Name</label>
                                        <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account"  TabIndex="6">
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
                                        <label for="username">Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="7"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlYear" runat="server" ControlToValidate="ddlYear" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    </div>
                                    <div class="col-lg-12">
                                        <div class="hr-line-dashed"></div>
                                        <div class="form-group">
                                            <div class="col-sm-8 col-sm-offset-0">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="13" />
                                                <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="12" />
                                            </div>
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
                                Advance List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="Gridview1" runat="server" ShowFooter="true" class="table table-striped table-bordered table-hover" AutoGenerateColumns="False" Width="100%" OnRowCommand="Gridview1_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-1">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Advamount" HeaderText="Advance Amount" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="AdvApplicationDate" HeaderText="Application Date" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="SanctionDate" HeaderText="Sanction Date" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="DeductionAmt" HeaderText="Deduction" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="DeductionFrom" HeaderText="Deduction From" HeaderStyle-CssClass="col-lg-1" />
                                    </Columns>
                                    <PagerStyle CssClass="GridPager" />
                                </asp:GridView>
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
