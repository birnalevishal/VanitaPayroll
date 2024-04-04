<%@ Page Title="Authority Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Authority.aspx.cs" Inherits="PayRoll.Masters.Authority" %>

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
                                    <span>Authority</span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Authority Master
                        </h2>

                    </div>
                </div>
            </div>

            <div class="content">
                <div class="row">
                    <div class="col-lg-6">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Add/Edit Authority
                            </div>

                            <div class="panel-body">
                                <%--<div class="row">
                                 <div class="form-group col-lg-6">
                                        <label for="username">Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" CausesValidation="True" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged1">
                                        </asp:DropDownList>
                                    </div>
                                </div>--%>
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <label for="username">Employee Code </label>
                                        <asp:TextBox ID="txtEmpCode" runat="server" type="text" placeholder="Enter Employee Code" class="form-control" name="name"   ValidationGroup="OK" AutoPostBack="True" OnTextChanged="txtEmpCode_TextChanged" TabIndex="1" onkeypress="return onlyNumbers(event,this);" MaxLength="6"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Employee Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <label for="username">Employee Name </label>
                                        <asp:TextBox ID="txtEmpName" runat="server" ReadOnly="true" type="text" placeholder="Employee Name" class="form-control" name="name"   ValidationGroup="OK" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpName" runat="server" ControlToValidate="txtEmpName" ErrorMessage="Employee Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <label for="Canteen">Form Name</label>
                                        <asp:DropDownList ID="ddlformname" runat="server" class="form-control" name="account" CausesValidation="True" TabIndex="3"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlformname" runat="server" ControlToValidate="ddlformname" InitialValue="0" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="14" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="13" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Employee List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="OrgId" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-1">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnYear" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show">Delete</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="YearId" HeaderText="YearId" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="Employeename" HeaderText="Employee Name" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Title" HeaderText="Form Access" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
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
