<%@ Page Title="Leave Balance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LeaveBalance.aspx.cs" Inherits="PayRoll.Masters.LeaveBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
        function onlyNumbers(txt) {
            if (txt.which != 8 && txt.which != 0 && txt.which != 46 &&(txt.which < 48 || txt.which > 57)) {
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
                                    <span>Leave Balance</span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Leave Balance Master
                        </h2>

                    </div>
                </div>
            </div>

            <div class="content">
                <div class="row">
                    <div class="col-lg-8">
                         <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Add/Edit Leave Balance
                            </div>

                            <div class="panel-body">
                                <%--<div class="row">
                                 <div class="form-group col-lg-6">
                                        <label for="username">Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"   AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged1">
                                        </asp:DropDownList>
                                    </div>
                                </div>--%>
                                <div class="row">
                                    <div class="form-group col-lg-3">
                                            <label for="username">Month Name</label>
                                            <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account"   TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlMon_SelectedIndexChanged">
                                                <asp:ListItem Text="select" Value="00" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="January" Value="01"></asp:ListItem>
                                               <%-- <asp:ListItem Text="February" Value="02"></asp:ListItem>
                                                <asp:ListItem Text="March" Value="03"></asp:ListItem>
                                                <asp:ListItem Text="April" Value="04"></asp:ListItem>
                                                <asp:ListItem Text="May" Value="05"></asp:ListItem>
                                                <asp:ListItem Text="June" Value="06"></asp:ListItem>
                                                <asp:ListItem Text="July" Value="07"></asp:ListItem>
                                                <asp:ListItem Text="August" Value="08"></asp:ListItem>
                                                <asp:ListItem Text="September" Value="09"></asp:ListItem>
                                                <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="December" Value="12"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RFVddlMon" runat="server" ControlToValidate="ddlMon" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">Year</label>
                                            <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"   TabIndex="2" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RFVddlYear" runat="server" ControlToValidate="ddlYear" InitialValue="0000" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                        </div>
                                    <div class="form-group col-lg-6">
                                        <label for="Canteen">Employee Name</label>
                                        <asp:DropDownList ID="ddlEmpName" runat="server" class="form-control js-source-states" name="account"   AutoPostBack="True" TabIndex="3" OnSelectedIndexChanged="ddlEmpName_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    
                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-6">
                                        <label for="Canteen">COff</label>
                                        <asp:TextBox ID="COff" runat="server" class="form-control" MaxLength="4" placeholder="Enter COff" name="account"   TabIndex="4" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-6">
                                        <label for="Canteen">PL</label>
                                        <asp:TextBox ID="PL" runat="server" class="form-control" MaxLength="4" placeholder="Enter PL" name="account"   TabIndex="5" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                    <div class="form-group">
                                        <div class="col-sm-12 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="7" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="6" />
                                            <asp:Button ID="btnTransfer" runat="server" Text="Transfer" class="btn btn-outline btn-success pull-right" OnClick="btnTransfer_Click"  OnClientClick = "return confirm('Are you sure you want Transfer? Previous change will be lost')" TabIndex="6" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-4">
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
                                        <asp:BoundField DataField="COff" HeaderText="COff" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PL" HeaderText="PL" HeaderStyle-CssClass="col-lg-4">
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

     <script>
        $(function () {
            $(".js-source-states").select2();
        });
    </script>

</asp:Content>
