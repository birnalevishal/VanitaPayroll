<%@ Page Title="Financial Year Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="financialYear1.aspx.cs" Inherits="PayRoll.Masters.financialYear1" %>

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
                                    <span>Masters</span>
                                </li>
                                <li class="active">
                                    <span>Financial Year </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Financial Year Master
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
                                Add/Edit Financial Year
                            </div>

                            <div class="panel-body">
                                <div class="form-group">
                                    <label for="username">From Date </label>
                                    <asp:TextBox ID="txtFromDate" runat="server" type="text" placeholder="Enter From Date(dd/MM/yyyy)" class="form-control" name="name"   ValidationGroup="OK" TabIndex="1"></asp:TextBox>
                                    <Ajax:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtFromDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    &nbsp;<asp:RequiredFieldValidator ID="RFVtxtFromDate" runat="server" ControlToValidate="txtFromDate" ErrorMessage="Enter From Date " Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <label for="username">To Date </label>

                                    <asp:TextBox ID="txtToDate" runat="server" type="text" placeholder="Enter To Date(dd/MM/yyyy)" class="form-control" name="name"   ValidationGroup="OK" TabIndex="2"></asp:TextBox>
                                    <Ajax:CalendarExtender ID="CalendarExtender1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtToDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RFVtxtToDate" runat="server" ControlToValidate="txtToDate" ErrorMessage="Enter To Date" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CV" ControlToCompare="txtFromDate" ControlToValidate="txtToDate" Type="Date" Operator="GreaterThan" ErrorMessage="To Date Should Be Greater Than From Date" runat="server"></asp:CompareValidator>
                                </div>

                                <div class="form-group">
                                    <label for="Isactive">Is Active</label>
                                    <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" type="checkbox" class="i-checks" TabIndex="3" />
                                </div>
                                <div class="hr-line-dashed"></div>

                                <div class="form-group">
                                    <div class="col-sm-8 col-sm-offset-0">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="5" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="4" />
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
                                Financial Year List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="YearId" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-2">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-2"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Frdate" HeaderText="From Date" HeaderStyle-CssClass="col-lg-4" />
                                        <asp:BoundField DataField="Todate" HeaderText="To Date" HeaderStyle-CssClass="col-lg-4" />
                                        <asp:BoundField DataField="IsActive" HeaderText="Active" HeaderStyle-CssClass="col-lg-2" />
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
