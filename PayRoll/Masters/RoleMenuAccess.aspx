<%@ Page Title="Role Menu Access" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleMenuAccess.aspx.cs" Inherits="PayRoll.Masters.RoleMenuAccess" %>

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
                                    <span>Role Menu Access </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Role Menu Access
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
                                Role Name
                            </div>

                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <label class="col-sm-4 control-label">Role Name</label>
                                            <div class="col-sm-4">
                                                <asp:DropDownList ID="ddlList" runat="server" class="form-control m-b" CausesValidation="True" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlList_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-4 col-sm-offset-0">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="4" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Employee Reports
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkEmpRptAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkEmpRptAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkEmpRptList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>


                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Employee Masters
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkEmpMastersAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkEmpMastersAll_CheckedChanged" Text="Select All" />
                                <asp:CheckBoxList ID="chkEmpMasterList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Common Masters
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkCommMasterAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkCommMasterAll_CheckedChanged" Text="Select All" />
                                <asp:CheckBoxList ID="chkCommMastList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>


                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Configuration
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkConfigAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkConfigAll_CheckedChanged" Text="Select All" />
                                <asp:CheckBoxList ID="chkConfigList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Transaction
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkTranFormsAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkTranFormsAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkTranList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Salary Process
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkSalaryProcessAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkSalaryProcessAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkSalaryProcessList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Salary Reports
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkSalReportsAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkSalReportsAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkSalReportsList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Legal
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkLegalAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkLegalAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkLegalList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>


                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Employee life cycle
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkEmpLifeCycleAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkEmpLifeCycleAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkEmpLifeCycleList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Bonus
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkBonusAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkBonusAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkBonusList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Insentive
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkInsentiveAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkInsentiveAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkInsentiveList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Increment
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkIncrAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkIncrAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkIncrList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>


                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Access
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkAccessAll" runat="server" AutoPostBack="True" Text="Select All" OnCheckedChanged="chkAccessAll_CheckedChanged" />
                                <asp:CheckBoxList ID="chkAccessList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-3">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Utilities
                            </div>
                            <div class="panel-body">
                                <asp:CheckBox ID="chkUtilAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkUtilAll_CheckedChanged" Text="Select All" />
                                <asp:CheckBoxList ID="chkUtilList" runat="server" AutoPostBack="True" AppendDataBoundItems="True">
                                </asp:CheckBoxList>
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
