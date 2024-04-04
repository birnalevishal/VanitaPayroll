<%@ Page Title="Form 16 Deductions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="F16Ded.aspx.cs" Inherits="PayRoll.Transactions.F16Ded" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--Page Layout -->
            <div class="small-header">
                <div class="hpanel">
                    <div class="panel-body">
                        <div id="hbreadcrumb" class="pull-right">
                            <ol class="hbreadcrumb breadcrumb">
                                <li><a runat="server" href="~/Default.aspx">Dashboard</a></li>
                                <li>
                                    <span>Transactions</span>
                                </li>
                                <li class="active">
                                    <span>Form 16 Deductions</span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Form 16 Deductions
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
                                Form 16 Deductions
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-lg-12">
                                        <div class="form-group">
                                            <label class="col-sm-2 control-label">Employee Code </label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtEmpCode" runat="server" type="text" placeholder="" class="form-control" name="name" TabIndex="1" ReadOnly="True"  AutoPostBack="True" OnTextChanged="txtEmpCode_TextChanged" ></asp:TextBox>
                                            </div>
                                            <label class="col-sm-2 control-label">Employee Name </label>
                                            <div class="col-sm-6">
                                                <asp:TextBox ID="txtEmpName" runat="server" type="text" placeholder="" class="form-control" name="name" TabIndex="2" ReadOnly="True"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-lg-12">
                                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AutoGenerateColumns="False" Width="100%" DataKeyNames="Srno" OnRowDataBound="gvList_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="Srno" HeaderText="Sr No." >
                                                            <HeaderStyle CssClass="col-lg-1" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="SubSrno" HeaderText="Sub SrNo." >
                                                            <HeaderStyle CssClass="col-lg-1" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Section" HeaderText="Section" >
                                                            <HeaderStyle CssClass="col-lg-1" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DeductionOn" HeaderText="Deduction">
                                                            <HeaderStyle CssClass="col-lg-7" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAmt" class="form-control" runat="server" Text='<%# Bind("Amt") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="col-lg-3" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <div class="col-sm-10">
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" ValidationGroup="ValOK" TabIndex="30" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" TabIndex="31" OnClick="btnCancel_Click" />
                                            </div>
                                           
                                        </div>
                                    </div>
                                </div>
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

            <!--Page Layout -->
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
