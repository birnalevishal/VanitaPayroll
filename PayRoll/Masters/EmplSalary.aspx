<%@ Page Title="Salary Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmplSalary.aspx.cs" Inherits="PayRoll.Masters.EmplSalary" %>

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
            if (txt.which != 8 && txt.which != 0 && (txt.which < 48 || txt.which > 57)) {
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
                                    <span>Salary </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Salary Master
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
                                Add/Edit Salary Master
                            </div>

                            <div class="panel-body">
                                <asp:Panel ID="pnlSalaryData" runat="server">
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-2">
                                            <label for="username">Employee Code</label>
                                            <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Employee Code" class="form-control" name="name"     TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Required" Display="Dynamic" ForeColor="Red"  ></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group col-lg-4">
                                            <label for="username">Employee Name</label>
                                            <asp:TextBox ID="txtEmpName" runat="server" ReadOnly="true" type="text" placeholder="Employee Name" class="form-control" name="name"     TabIndex="2"></asp:TextBox>
                                        </div>
                                        <%--<div class="form-group col-lg-2">
                                            <label for="username">Document Date</label>
                                            <asp:TextBox ID="txtDocDate" runat="server" type="text" placeholder="(dd/MM/yyyy)" class="form-control" name="name"     TabIndex="8"></asp:TextBox>
                                            <Ajax:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDocDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RFVtxtDocDate" runat="server" ControlToValidate="txtDocDate" ErrorMessage="Required" Display="Dynamic" ForeColor="Red"  ></asp:RequiredFieldValidator>
                                        </div>--%>
                                        <div class="form-group col-lg-2">
                                            <label for="username">Month Name</label>
                                            <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account"   TabIndex="3">
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
                                            <asp:RequiredFieldValidator ID="RFVddlMon" runat="server" ControlToValidate="ddlMon" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red"  ></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group col-lg-2">
                                            <label for="username">Year</label>
                                            <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"   TabIndex="4"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RFVddlYear" runat="server" ControlToValidate="ddlYear" InitialValue="0000" ErrorMessage="Required" Display="Dynamic" ForeColor="Red"  ></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-3">
                                            <label for="username">Basic+DA</label>
                                            <asp:TextBox ID="txtBasic" runat="server" MaxLength="10" type="text" placeholder="Enter Basic" class="form-control" name="name"     TabIndex="5" onkeypress="return onlyNumbers(event,this); " onfocusout = "total(event,this)" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RFVtxtBasic" runat="server" ControlToValidate="txtBasic" ErrorMessage="Required" Display="Dynamic" ForeColor="Red"  ></asp:RequiredFieldValidator>
                                            
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">HRA</label>
                                            <asp:TextBox ID="txtHRA" runat="server" MaxLength="10" type="text" placeholder="Enter HRA" class="form-control" name="name"     TabIndex="6" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">Conveyance</label>
                                            <asp:TextBox ID="txtConveyance" runat="server" MaxLength="10" type="text" placeholder="Enter Conveyance" class="form-control" name="name"     TabIndex="7" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">Education</label>
                                            <asp:TextBox ID="txtEducation" runat="server" MaxLength="10" type="text" placeholder="Enter Education" class="form-control" name="name"     TabIndex="8" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-3">
                                            <label for="username">Medical</label>
                                            <asp:TextBox ID="txtMedical" runat="server" MaxLength="10" type="text" placeholder="Enter Medical" class="form-control" name="name"     TabIndex="9" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>

                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">Tea & Tiffin</label>
                                            <asp:TextBox ID="txtCanteen" runat="server" MaxLength="10" type="text" placeholder="Enter Canteen" class="form-control" name="name"     TabIndex="10" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">Washing</label>
                                            <asp:TextBox ID="txtWashing" runat="server" MaxLength="10" type="text" placeholder="Enter Washing" class="form-control" name="name"     TabIndex="11" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">Uniform & Shoes</label>
                                            <asp:TextBox ID="txtUniform" runat="server" MaxLength="10" type="text" placeholder="Enter Uniform" class="form-control" name="name"     TabIndex="12" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>
                                        </div>
                                    </div>
                                     <div class="col-lg-12">
                                        <div class="form-group col-lg-3">
                                            <asp:Label ID="lblAdd1" runat="server" Text="" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtAdd1" runat="server" MaxLength="10" type="text" placeholder="" class="form-control" name="name"     TabIndex="13" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>

                                        </div>
                                        <div class="form-group col-lg-3">
                                            <asp:Label ID="lblAdd2" runat="server" Text="" Font-Bold="true" ></asp:Label>
                                            <asp:TextBox ID="txtAdd2" runat="server" MaxLength="10" type="text" placeholder="" class="form-control" name="name"     TabIndex="14" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-3">
                                            <asp:Label ID="lblAdd3" runat="server" Text="" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtAdd3" runat="server" MaxLength="10" type="text" placeholder="" class="form-control" name="name"     TabIndex="15" onkeypress="return onlyNumbers(event,this);" onfocusout = "total()"></asp:TextBox>
                                        </div>
                                       
                                    </div>
                                    <div class="col-lg-12">
                                        <div class="form-group col-lg-3">
                                            <label for="username">Total</label>
                                            <asp:TextBox ID="txtTotal" runat="server" MaxLength="10" type="text" placeholder="Total"  class="form-control" name="name"     TabIndex="16" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>

                                        </div>  

                                    
                                        <div class="form-group col-lg-3">
                                            <label for="username">PF Man</label>
                                            <asp:TextBox ID="txtPFMan" runat="server" MaxLength="10" type="text" placeholder="Enter PF Man" class="form-control" name="name"     TabIndex="17" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>

                                        </div>
                                        <div class="form-group col-lg-3">
                                            <label for="username">PF Emp Percentage</label>
                                            <asp:TextBox ID="txtPfEmpPct" runat="server" MaxLength="10" type="text" placeholder="Enter PF Emp %" class="form-control" name="name"     TabIndex="18" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        </div>
                                        <div class="form-group col-lg-1" style="margin-top:22px" >
                                            <asp:CheckBox ID="chkIsActive" Text="Is Active" runat="server" type="checkbox" class="i-checks" Checked="True" TabIndex="19"/>
                                        </div>
                                        <div class="form-group col-lg-1"  id="divApproval" style="margin-top:17px" >
                                            <asp:CheckBox ID="chkApproval" Visible="false" Text="Approval" runat="server" type="checkbox" class="i-checks" TabIndex="20" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="23" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click"   TabIndex="22" />
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
                                Salary List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="Employeecd" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-1">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show" CausesValidation="False">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Employeecd" HeaderText="Employee Code" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MonYrCd" HeaderText="MonthYear" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Docdate" HeaderText="Document Date" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Employeename" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-6" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsActive" HeaderText="Active" HeaderStyle-CssClass="col-lg-2">
                                            <HeaderStyle CssClass="col-lg-2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Approval" HeaderText="Approval" HeaderStyle-CssClass="col-lg-2">
                                            <HeaderStyle CssClass="col-lg-2" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridPager" />
                                </asp:GridView>

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
    <script type="text/javascript">
        
        function total() {
          debugger
            var txtBasic = document.getElementById('<%=txtBasic.ClientID%>').value;
            var txtHRA = document.getElementById('<%=txtHRA.ClientID%>').value;
            var txtConveyance = document.getElementById('<%=txtConveyance.ClientID%>').value;
            var txtEducation = document.getElementById('<%=txtEducation.ClientID%>').value;
            var txtMedical = document.getElementById('<%=txtMedical.ClientID%>').value;
            var txtCanteen = document.getElementById('<%=txtCanteen.ClientID%>').value;
            var txtWashing = document.getElementById('<%=txtWashing.ClientID%>').value;
            var txtUniform = document.getElementById('<%=txtUniform.ClientID%>').value;

            var txtAdd1 = document.getElementById('<%=txtAdd1.ClientID%>').value;
            var txtAdd2 = document.getElementById('<%=txtAdd2.ClientID%>').value;
            var txtAdd3 = document.getElementById('<%=txtAdd3.ClientID%>').value;
           
            if (txtBasic == "")
                txtBasic = 0;
            if (txtHRA == "")
                txtHRA = 0;
            if (txtConveyance == "")
                txtConveyance = 0;
            if (txtEducation == "")
                txtEducation = 0;
            if (txtMedical == "")
                txtMedical = 0;
            if (txtCanteen == "")
                txtCanteen = 0;
            if (txtWashing == "")
                txtWashing = 0;
            if (txtUniform == "")
                txtUniform = 0;

            if (txtAdd1 == "")
                txtAdd1 = 0;
            if (txtAdd2 == "")
                txtAdd2 = 0;
            if (txtAdd3 == "")
                txtAdd3 = 0;
            
            var result = parseFloat(txtBasic) + parseFloat(txtHRA) + parseFloat(txtConveyance) + parseFloat(txtEducation) + parseFloat(txtMedical) + parseFloat(txtCanteen) + parseFloat(txtWashing) + parseFloat(txtUniform) + parseFloat(txtAdd1) + parseFloat(txtAdd2) + parseFloat(txtAdd3);
            if (!isNaN(result)) {
                document.getElementById('<%=txtTotal.ClientID%>').value = result;
            }
        }
    </script>

</asp:Content>
