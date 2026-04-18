<%@ Page Title="Increment Export-Import" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IncrementImpExp.aspx.cs" Inherits="PayRoll.Masters.IncrementImpExp" %>

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
                                    <span>Increment Export-Import </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Increment Export-Import
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
                                Increment Export-Import
                            </div>
                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">Date</label>
                                        <asp:TextBox ID="txtDate" runat="server" type="textarea" MaxLength="80" placeholder="Enter Date" class="form-control" name="name" ValidationGroup="OK" TabIndex="2"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RFVtxtDate" runat="server" ControlToValidate="txtDate" ErrorMessage="As On Date Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
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
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account" TabIndex="2"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlYear" runat="server" ControlToValidate="ddlYear" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-4" style="padding-top: 10px; float: right">
                                        <a href="DownloadFile.ashx?file=~/ImportFormatFiles/IncrementImportFormat.xlsx" class="btn btn-outline btn-warning" style="float: right" tabindex="6">Download Import Format</a>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <label for="username">Select File to Upload</label>
                                        <asp:FileUpload ID="FUExcel" runat="server" TabIndex="3" />
                                    </div>
                                </div>
                                <div class="col-sm-8 col-sm-offset-0">
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" class="btn btn-outline btn-success" ValidationGroup="OK" TabIndex="4" OnClick="btnUpload_Click" />
                                </div>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>
                                <asp:Panel ID="pnlGVList" runat="server" Enabled="false" Visible="true">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="hpanel">
                                                <div class="panel-heading">
                                                    <div class="panel-tools">
                                                        <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                                        <a class="closebox"><i class="fa fa-times"></i></a>
                                                    </div>
                                                    <asp:Label ID="lblUploadHeading" runat="server" Text="Upload List" Font-Bold="true"></asp:Label>
                                                </div>
                                                <div class="panel-body">
                                                    <asp:GridView ID="gvUploadList" runat="server" class="table table-striped table-bordered table-hover" AutoGenerateColumns="False" Width="100%" DataKeyNames="EmpCode" OnRowDataBound="gvUploadList_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="EmpCode" HeaderText="Emp Code">
                                                                <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Name" HeaderText="Employee Name">
                                                                <HeaderStyle CssClass="col-lg-4"></HeaderStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="wef" HeaderText="wef" DataFormatString="{0:dd/MM/yyyy}" />
                                                            <asp:BoundField DataField="Arrears" HeaderText="Arrears"></asp:BoundField>
                                                            <asp:BoundField DataField="BASICDA" HeaderText="BASIC+DA"></asp:BoundField>
                                                            <asp:BoundField DataField="HRA" HeaderText="HRA"></asp:BoundField>
                                                            <asp:BoundField DataField="MedicalAllowance" HeaderText="Medical"></asp:BoundField>
                                                            <asp:BoundField DataField="EducationalAllowance" HeaderText="Education"></asp:BoundField>
                                                            <asp:BoundField DataField="ConveyanceTravellingAllowance" HeaderText="Conveyance"></asp:BoundField>
                                                            <asp:BoundField DataField="TeaTiffinAllowance" HeaderText="TeaTiffin"></asp:BoundField>
                                                            <asp:BoundField DataField="UniformShoesAllowance" HeaderText="UniformShoes"></asp:BoundField>
                                                            <asp:BoundField DataField="WashingAllowance" HeaderText="Washing"></asp:BoundField>
                                                            <asp:BoundField DataField="Add1" HeaderText="Add1"></asp:BoundField>
                                                            <asp:BoundField DataField="Add2" HeaderText="Add2"></asp:BoundField>
                                                            <asp:BoundField DataField="Add3" HeaderText="Add3"></asp:BoundField>
                                                            <asp:BoundField DataField="GrossEarning" HeaderText="Gross"></asp:BoundField>
                                                            <%--<asp:BoundField DataField="Uploaded" HeaderText="Uploaded" Visible="false" ></asp:BoundField>--%>
                                                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="col-lg-1" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Uploaded") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="col-lg-1" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="GridPager" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="form-group">
                                    <div class="col-sm-12 col-sm-offset-0 pull-right">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning " OnClick="btnCancel_Click" TabIndex="5" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success " OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="4" />

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
            <asp:PostBackTrigger ControlID="btnUpload" />
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
