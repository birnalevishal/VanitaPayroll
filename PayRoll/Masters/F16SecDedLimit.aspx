<%@ Page Title="Sectionwise Deduction Limit" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="F16SecDedLimit.aspx.cs" Inherits="PayRoll.Masters.F16SecDedLimit" %>

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
                                    <span>Section Dedcuction Limit </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Fotm 16 Section Dedcuction Limit 
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
                                Add/Edit Section Deduction Limit
                            </div>

                            <div class="panel-body">
                                <div class="form-group">
                                    <label for="username">Section</label>
                                    <asp:DropDownList ID="ddlList" runat="server" class="form-control m-b" name="account" CausesValidation="True" TabIndex="1"></asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label for="username">Deduction Limit in Rs.</label>
                                    <asp:TextBox ID="txtDedInRs" runat="server" type="text" MaxLength="50" placeholder="Enter Limit in Rs." class="form-control" name="name" ValidationGroup="OK" TabIndex="1" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label for="username">Deduction Limit in %</label>
                                    <asp:TextBox ID="txtDedInPct" runat="server" type="text" MaxLength="6" placeholder="Enter Limit in %" class="form-control" name="name" ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
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
                                Sectionwise Deduction Limit List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="False" AutoGenerateColumns="False" PageSize="10" Width="100%" DataKeyNames="YearId" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-1">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnYear" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                        </asp:TemplateField>
                                         <asp:BoundField DataField="subSrNo" HeaderText="SrNo" HeaderStyle-CssClass="col-lg-1">
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Section" HeaderText="Section" HeaderStyle-CssClass="col-lg-3">
                                            <HeaderStyle CssClass="col-lg-3"></HeaderStyle>
                                        </asp:BoundField>
                                        
                                         <asp:BoundField DataField="Narr" HeaderText="Narr" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-4"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LimitInAmt" HeaderText="Limit in Rs." HeaderStyle-CssClass="col-lg-3" DataFormatString="{0:0.00}">
                                            <HeaderStyle CssClass="col-lg-3"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LimitInPct" HeaderText="Limit in %" HeaderStyle-CssClass="col-lg-3" DataFormatString="{0:0.00}">
                                            <HeaderStyle CssClass="col-lg-3"></HeaderStyle>
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
