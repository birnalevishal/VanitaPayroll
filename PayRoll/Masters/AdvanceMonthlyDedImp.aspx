<%@ Page Title="Advance Monthly Deduction Import" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdvanceMonthlyDedImp.aspx.cs" Inherits="PayRoll.Masters.AdvanceMonthlyDedImp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="small-header">
                <div class="hpanel">
                    <div class="panel-body">
                        <div id="hbreadcrumb" class="pull-right">
                            <ol class="hbreadcrumb breadcrumb">
                                <li><a href="index.html">Dashboard</a></li>
                                <li>
                                    <span>Transaction</span>
                                </li>
                                <li class="active">
                                    <span>Advance Monthly Deduction Import </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Advance Monthly Deduction Import
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
                                Advance Monthly Deduction Import
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">Month Name</label>
                                        <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account"   TabIndex="1">
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
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"   TabIndex="2"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlYear" runat="server" ControlToValidate="ddlYear" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">Select File</label>
                                        <asp:FileUpload ID="FUExcel" runat="server" TabIndex="3" />
                                    </div>
                                    <div class="form-group col-lg-3" style="padding-top:10px; float:right">
                                        <a href="DownloadFile.ashx?file=~/ImportFormatFiles/AdvanceMonthlyDeductionImportFormat.xlsx" class="btn btn-outline btn-warning" TabIndex="6">Download Import Format</a>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>
                                <asp:Panel ID="pnlGVList" runat="server" Enabled="false" Visible="false">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="hpanel">
                                                <div class="panel-heading">
                                                    <div class="panel-tools">
                                                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                                        <a class="closebox"><i class="fa fa-times"></i></a>
                                                    </div>
                                                    <asp:Label ID="lblAttHeading" runat="server" Text="Attendence Not Found List" Font-Bold="true"></asp:Label>
                                                </div>
                                                <div class="panel-body">
                                                    <asp:GridView ID="gvEmployee" runat="server" class="table table-striped table-bordered table-hover" AutoGenerateColumns="False" Width="100%" DataKeyNames="Employeecd">
                                                        <Columns>
                                                            <asp:BoundField DataField="SrNo" HeaderText="Sr.No." HeaderStyle-CssClass="col-lg-6">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Employeecd" HeaderText="Employee Code" HeaderStyle-CssClass="col-lg-6">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Employeename" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-3"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OrgJoinDate" HeaderText="Orignal Joining Date" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PrvGross" HeaderText="Previous Gross" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="IncrementAmt" HeaderText="Increment Amount" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="wef" HeaderText="W.E.F." HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RevisedGross" HeaderText="Revised Gross" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Arrears" HeaderText="Arrears" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark1" HeaderText="Remark1" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark2" HeaderText="Remark2" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RevisedGross1" HeaderText="Revised Gross" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="H" HeaderText="" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="BasicDA" HeaderText="Basic+DA" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="HRA" HeaderText="HRA" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Medical" HeaderText="Medical Allowance" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Educatonal" HeaderText="Educatonal Allowance" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="Conveyance" HeaderText="Conveyance/Travelling Allowance" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Canteen" HeaderText="Tea & Tiffin Allowance" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>

                                                            <asp:BoundField DataField="Uniform" HeaderText="Uniform & Shoes Allowance" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                             <asp:BoundField DataField="Washing" HeaderText="Washing Allowance" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="GrossEarning" HeaderText="Gross Earning" HeaderStyle-CssClass="col-lg-4">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
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
