<%@ Page Title="Asset Transanction" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssetTransanction.aspx.cs" Inherits="PayRoll.Masters.AssetTransanction" %>
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
                                    <span>Asset Transanction </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Asset Transanction
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
                                Add/Edit Asset Transanction
                            </div>

                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-2">
                                            <label for="username">Employee Code</label>
                                            <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Employee Code" class="form-control" name="name"  ValidationGroup="OK" TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                        </div>
                                         <div class="form-group col-lg-4">
                                            <label for="Canteen">Employee Name</label>
                                            <asp:DropDownList ID="ddlEmpName" runat="server" class="form-control js-source-states" name="account"   AutoPostBack="True" TabIndex="4" OnSelectedIndexChanged="ddlEmpName_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                         <div class="form-group col-lg-2">
                                            <label for="username">Designation</label>
                                            <asp:TextBox ID="txtDesignation" runat="server" ReadOnly="true" type="text" placeholder="Employee Name" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" ></asp:TextBox>
                                        </div>
                                         <div class="form-group col-lg-2" style="visibility:hidden">
                                             <label for="username">Date</label>
                                                <asp:TextBox ID="txtIssueDate" class="form-control" runat="server" Text='<%# Eval("IssueDate") %>' AutoPostBack="True" OnTextChanged="txtIssueDate_TextChanged"></asp:TextBox>
                                                <Ajax:CalendarExtender ID="CEtxtIssueDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtIssueDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Enabled="false" runat="server" ControlToValidate="txtIssueDate" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                        </div>

                                    </div>
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-2">
                                            <label for="username">Department</label>
                                            <asp:TextBox ID="txtDepartment" runat="server" ReadOnly="true" type="text" placeholder="Employee Name" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" ></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-2">
                                            <label for="username">Mobile No</label>
                                            <asp:TextBox ID="txtMobileNo" runat="server" type="text" ReadOnly="true" MaxLength="10" placeholder="Mobile No" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK"  onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="REVtxtMobileNo" Display="Dynamic" ControlToValidate="txtMobileNo" ValidationExpression="^[\d]{10,10}$" runat="server" ErrorMessage="10 Digits Required." ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="form-group col-lg-4">
                                            <label for="username">Email ID</label>
                                            <asp:TextBox ID="txtEmailID" runat="server" type="text" ReadOnly="true" MaxLength="50" placeholder="Email ID" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" ></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="REVtxtEmailID" runat="server" ControlToValidate="txtEmailID" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" ErrorMessage="Enter Valid Email ID" ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">Driving Licence No</label>
                                            <asp:TextBox ID="txtDrivingLicence" runat="server" ReadOnly="true" type="text" placeholder="Employee Name" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-12">
                                            <label for="username">Present Address</label>
                                            <asp:TextBox ID="txtWorkingAdd" runat="server" type="text" ReadOnly="true" MaxLength="10" placeholder="Present Address" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-12">
                                            <label for="username">Permenet Address</label>
                                            <asp:TextBox ID="txtPermenentAdd" runat="server" type="text" ReadOnly="true" MaxLength="50" placeholder="Permenet Address" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-3"></div>
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
                                Asset List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="Gridview1" runat="server" ShowFooter="true" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="10" Width="100%" onrowdatabound="GridView1_RowDataBound" >
                                    <Columns>
                                        <asp:BoundField DataField="SrNo" HeaderText="Sr No" />
                                       <%-- <asp:TemplateField HeaderText="IssueDate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtIssueDate" class="form-control" runat="server" Text='<%# Eval("IssueDate") %>'></asp:TextBox>
                                                <Ajax:CalendarExtender ID="CEtxtIssueDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtIssueDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                          <asp:TemplateField HeaderText="Item" >
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlItem" runat="server" class="form-control"  name="account" ></asp:DropDownList>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item")%>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Material Description" >
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDescription" class="form-control" runat="server" Text='<%# Eval("MaterialDetails") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="50">
                                            <ItemTemplate >
                                                <asp:TextBox ID="txtQuantity" MaxLength="3" class="form-control" runat="server" Text='<%# Eval("Quantity") %>' onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" />
                                        </asp:TemplateField>
                                        
                                         <asp:TemplateField HeaderText="Value in Rs" ItemStyle-Width="100">
                                             <ItemTemplate>
                                                <asp:TextBox ID="txtAmount" MaxLength="10"  class="form-control" runat="server" Text='<%# Eval("Amount") %>' onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                             <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remark">
                                           <ItemTemplate>
                                                <asp:TextBox ID="txtRemark" class="form-control" runat="server" Text='<%# Eval("Remark") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonAdd" class="form-control" runat="server" OnClick="ButtonAdd_Click" Text="Add New Row" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issue Date" ItemStyle-Width="120">
                                             <ItemTemplate>
                                                <asp:TextBox ID="txtIssueDate" MaxLength="10"  class="form-control" runat="server" Text='<%# Eval("IssueDate") %>' ></asp:TextBox>
                                                <Ajax:CalendarExtender ID="CEtxtIssueDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtIssueDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                            </ItemTemplate>
                                             <ItemStyle Width="120px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Handover Date" ItemStyle-Width="120">
                                             <ItemTemplate>
                                                <asp:TextBox ID="txtHandOverDate" MaxLength="10"  class="form-control" runat="server" Text='<%# Eval("HandOverDate") %>' ></asp:TextBox>
                                                <Ajax:CalendarExtender ID="CEtxtHandOverDate" PopupButtonID="imgPopup" runat="server" TargetControlID="txtHandOverDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                            </ItemTemplate>
                                             <ItemStyle Width="120px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                             <ItemTemplate>
                                                <asp:LinkButton ID="lbtnRemove" runat="server" Text="Remove" OnClick="lbtnRemove_Click"   ></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="13" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="12" />
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
