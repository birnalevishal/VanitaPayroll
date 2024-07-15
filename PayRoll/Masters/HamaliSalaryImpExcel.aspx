<%@ Page Title="Import Excel For Hamali Salary" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HamaliSalaryImpExcel.aspx.cs" Inherits="PayRoll.Masters.HamaliSalaryImpExcel" %>

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
                                    <span>Transaction</span>
                                </li>
                                <li class="active">
                                    <span>Import Excel For Hamali Salary </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Import Excel For Hamali Salary
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
                                Import Excel For Hamali Salary
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">Month Name</label>
                                        <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account" TabIndex="1">
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
                                    <div class="form-group col-lg-3">
                                        <label for="username">Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"  TabIndex="2"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlYear" runat="server" ControlToValidate="ddlYear" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">Select File</label>
                                        <asp:FileUpload ID="FUExcel" runat="server" TabIndex="3" />
                                    </div>

                                    <div class="form-group col-lg-3" style="padding-top:10px; float:right">
                                        <a href="DownloadFile.ashx?file=~/ImportFormatFiles/HamaliSalaryImportFormat.xlsx" class="btn btn-outline btn-warning" TabIndex="6">Download Import Format</a>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>
                                 <asp:Panel ID="pnlGVList" runat="server" Enabled="false" Visible="false">
                                    <div class="row">
                                         <div class="col-lg-6">
                                            <div class="hpanel">
                                                <div class="panel-heading">
                                                    <div class="panel-tools">
                                                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                                        <a class="closebox"><i class="fa fa-times"></i></a>
                                                    </div>
                                                    <asp:Label ID="lblHamaliHeading" runat="server" Text="Employee Not Found List" Font-Bold="true"></asp:Label>
                                                </div>
                                                <div class="panel-body">
                                                    <asp:GridView ID="gvHamali" runat="server" class="table table-striped table-bordered table-hover"  AutoGenerateColumns="False" Width="100%" DataKeyNames="Employeecd">
                                                        <Columns>
                                                            <asp:BoundField DataField="Employeecd" HeaderText="Employee Code" HeaderStyle-CssClass="col-lg-6">
                                                                <HeaderStyle CssClass="col-lg-2"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Employeename" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-6"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Attendence" HeaderText="Employee" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-4"></HeaderStyle>
                                                            </asp:BoundField>

                                                        </Columns>
                                                        <PagerStyle CssClass="GridPager" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6">
                                            <div class="hpanel">
                                                <div class="panel-heading">
                                                    <div class="panel-tools">
                                                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                                        <a class="closebox"><i class="fa fa-times"></i></a>
                                                    </div>
                                                    <asp:Label ID="lblAttHeading" runat="server" Text="Attendence Not Found List" Font-Bold="true"></asp:Label>
                                                </div>
                                                <div class="panel-body">
                                                    <asp:GridView ID="gvAttendence" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" Width="100%" DataKeyNames="Employeecd">
                                                        <Columns>
                                                            <asp:BoundField DataField="Employeecd" HeaderText="Employee Code" HeaderStyle-CssClass="col-lg-6">
                                                                <HeaderStyle CssClass="col-lg-2"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Employeename" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-6"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Attendence" HeaderText="Attendence" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-4"></HeaderStyle>
                                                            </asp:BoundField>

                                                        </Columns>
                                                        <PagerStyle CssClass="GridPager" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6">
                                            <div class="hpanel">
                                                <div class="panel-heading">
                                                    <div class="panel-tools">
                                                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                                        <a class="closebox"><i class="fa fa-times"></i></a>
                                                    </div>
                                                    Salary Not Found Or Not Approved List
                                                </div>
                                                <div class="panel-body">
                                                    <asp:GridView ID="gvSalary" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" Width="100%" DataKeyNames="Employeecd">
                                                        <Columns>
                                                            <asp:BoundField DataField="Employeecd" HeaderText="Employee Code" HeaderStyle-CssClass="col-lg-6">
                                                                <HeaderStyle CssClass="col-lg-2"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Employeename" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-6"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Attendence" HeaderText="Salary" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-4"></HeaderStyle>
                                                            </asp:BoundField>

                                                        </Columns>
                                                        <PagerStyle CssClass="GridPager" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="5" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="4" />
                                        </div>
                                    </div>
                                </div>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
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
