<%@ Page Title="Std. Deduction, Rebate" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="F16StdDedRebate.aspx.cs" Inherits="PayRoll.Masters.F16StdDedRebate" %>

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
                                    <span>Form 16 Deduction, Rebate </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Form 16 Standard Deduction/ Rebate Amt.
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
                                Form 16 Standard Deduction/ Rebate Amt.
                            </div>

                            <div class="panel-body">
                                <div class="form-group">
                                    <label for="username">Standard Deduction</label>
                                    <asp:TextBox ID="txtStdDed" runat="server" type="text" MaxLength="50" TabIndex="1" placeholder="Enter Standard Deduction" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>

                                </div>
                                <div class="form-group">
                                    <label for="username">Rebate</label>
                                    <asp:TextBox ID="txtRebate" runat="server" type="text" MaxLength="50" TabIndex="2" placeholder="Enter Rebate Amount" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>

                                </div>
                                <div class="form-group">
                                    <label for="username">Rebate Limit Amount</label>
                                    <asp:TextBox ID="txtRebateAmt" runat="server" type="text" MaxLength="10" TabIndex="3" placeholder="Enter Rebate Limit Amount" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>

                                </div>


                                <div class="hr-line-dashed"></div>

                                <div class="form-group">
                                    <div class="col-sm-8 col-sm-offset-0">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="6" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="5" />
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
                                Form 16 Standard Deduction/ Rebate List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="Yrno" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="StdDed" HeaderText="Standard Deduction" HeaderStyle-CssClass="col-lg-4" DataFormatString="{0:0.00}" >
                                        <HeaderStyle CssClass="col-lg-4" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Rebate" HeaderText="Rebate" HeaderStyle-CssClass="col-lg-4" DataFormatString="{0:0.00}" >
                                        <HeaderStyle CssClass="col-lg-4" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RebateLimit" HeaderText="Rebate Limit" HeaderStyle-CssClass="col-lg-4" DataFormatString="{0:0.00}" >
                                        <HeaderStyle CssClass="col-lg-4" />
                                        </asp:BoundField>
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
