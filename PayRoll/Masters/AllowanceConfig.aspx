<%@ Page Title="Allowance Configuration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllowanceConfig.aspx.cs" Inherits="PayRoll.Masters.AllowanceConfig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
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
                                    <span>Allowance Configuration </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Allowance Configuration Master
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
                                Add/Edit Allowance Configuration
                            </div>

                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">Employee Code</label>
                                        <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Enter Employee Code" class="form-control" name="name"  ValidationGroup="OK" TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Employee Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Employee Name</label>
                                        <asp:TextBox ID="txtEmpName" runat="server" ReadOnly="true" MaxLength="75" type="text" placeholder="Enter Employee Name" class="form-control" name="name"  ValidationGroup="OK" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpName" runat="server" ControlToValidate="txtEmpName" ErrorMessage="Employee Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Document Date</label>
                                        <asp:TextBox ID="txtDocDate" runat="server" type="text" placeholder="Enter Doc. Date(dd/MM/yyyy)" class="form-control" name="name"  ValidationGroup="OK" TabIndex="2"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CEtxtDocDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDocDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RFVtxtDocDate" runat="server" ControlToValidate="txtDocDate" ErrorMessage="Doc Date Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">TA </label>
                                        <asp:TextBox ID="txtTA" runat="server" type="text" MaxLength="10" placeholder="Enter TA" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);" AutoPostBack="True" OnTextChanged="txtTA_TextChanged"></asp:TextBox>

                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">DA</label>
                                        <asp:TextBox ID="txtDA" runat="server" type="text" MaxLength="10" placeholder="Enter DA" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);" AutoPostBack="True" OnTextChanged="txtDA_TextChanged"></asp:TextBox>

                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Phone </label>
                                        <asp:TextBox ID="txtPhone" runat="server" type="text" MaxLength="10" placeholder="Enter Phone" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);" AutoPostBack="True" OnTextChanged="txtPhone_TextChanged"></asp:TextBox>

                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Lodge</label>
                                        <asp:TextBox ID="txtLodge" runat="server" type="text" MaxLength="10" placeholder="Enter Lodge" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>

                                    </div>
                                </div>
                                 <div class="col-lg-12">
                                      <div class="form-group col-lg-3">
                                         <label for="username">Total(TA+DA+Phone)</label>
                                        <asp:TextBox ID="txtTotal" runat="server" type="text" ReadOnly="true"  placeholder="Total" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>

                                     </div>
                                     <div class="form-group col-lg-3">
                                         <label for="username">Other</label>
                                        <asp:TextBox ID="txtOther" runat="server" type="text" MaxLength="10" placeholder="Enter Other" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>

                                     </div>
                                     <div class="form-group col-lg-3">
                                         <label for="username">Remark</label>
                                        <asp:TextBox ID="txtRemark" runat="server" type="text" placeholder="Enter Remark" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" TabIndex="2" ></asp:TextBox>
                                     </div>
                                      <div class="form-group col-lg-1"  id="divApproval" style="margin-top:17px" >
                                            <asp:CheckBox ID="chkApproval" Visible="false" Text="Approval" runat="server" type="checkbox" class="i-checks" TabIndex="20" />
                                        </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="10" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="9" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Allowance Configuration List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="Docdate" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand" TabIndex="6">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-2">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Docdate" HeaderText="Doc. Date" HeaderStyle-CssClass="col-lg-1" DataFormatString="{0:dd/MM/yyyy}"/>
                                        <asp:BoundField DataField="TA" HeaderText="TA" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="DA" HeaderText="DA" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="phone" HeaderText="Phone" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="lodge" HeaderText="Lodge" HeaderStyle-CssClass="col-lg-1" />

                                        <asp:BoundField DataField="Other" HeaderText="Other" HeaderStyle-CssClass="col-lg-1" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-CssClass="col-lg-2" />
                                        <asp:BoundField DataField="Approval" HeaderText="Approval" HeaderStyle-CssClass="col-lg-1" />
                                        
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
