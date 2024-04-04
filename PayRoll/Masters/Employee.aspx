<%@ Page Title="Employee Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Employee.aspx.cs" Inherits="PayRoll.Masters.Employee" %>

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
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32) || (charCode == 45) || (charCode == 47))
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
            if (txt.which != 8 && txt.which != 0 && txt.which != 46 && (txt.which < 48 || txt.which > 57)) {
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
                                    <span>Employee </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Employee Master
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
                                Add/Edit Employee
                            </div>

                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">Employee Code</label>
                                        <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Employee Code" class="form-control" name="name" ValidationGroup="OK" TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Employee Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group col-lg-2">
                                        <label for="username">Aadhar No</label>
                                        <asp:TextBox ID="txtAadharNo" runat="server" type="text" MaxLength="12" placeholder="Enter Aadhar No" class="form-control" name="name" ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtAadharNo" runat="server" ControlToValidate="txtAadharNo" ErrorMessage="Aadhar No. Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="REVtxtAadharNo" Display="Dynamic" ControlToValidate="txtAadharNo" ValidationExpression="^[\d]{12,12}$" runat="server" ErrorMessage="12 Digits Required." ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Orignal Joining Date</label>
                                        <asp:TextBox ID="txtDOJ" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="3"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CEtxtDOJ" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDOJ" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RFVtxtDOJ" runat="server" ControlToValidate="txtDOJ" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">New Joining Date</label>
                                        <asp:TextBox ID="txtJoinTrf1" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="4"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CalendarExtender3" PopupButtonID="imgPopup" runat="server" TargetControlID="txtJoinTrf1" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RFVtxtJoinTrf1" runat="server" ControlToValidate="txtJoinTrf1" ErrorMessage="Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK" Enabled="false"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-4" style="margin-top: 20px">
                                        <asp:Button ID="btnGetData" runat="server" Text="Get Data" class="btn btn-outline btn-success" OnClick="btnGetData_Click" TabIndex="999" />
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" class="btn btn-outline btn-success" OnClick="btnUpdate_Click" TabIndex="999" />
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <label for="username">First Name</label>
                                        <asp:TextBox ID="txtFName" runat="server" type="text" MaxLength="25" placeholder="Enter First Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group col-lg-4">
                                        <label for="username">Middle Name</label>
                                        <asp:TextBox ID="txtMName" runat="server" type="text" MaxLength="25" placeholder="Enter Middle Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="6" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group col-lg-4">
                                        <label for="username">Last Name</label>
                                        <asp:TextBox ID="txtLName" runat="server" type="text" MaxLength="25" placeholder="Enter Last Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="7" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtLName" runat="server" ControlToValidate="txtLName" ErrorMessage="Last Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <label for="username">Employee Name</label>
                                        <asp:TextBox ID="txtEmpName" runat="server" ReadOnly="true" type="text" placeholder="Employee Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="8"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Birthdate</label>
                                        <asp:TextBox ID="txtBirthdate" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="9"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="Calendar1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtBirthdate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RFVtxtBirthdate" runat="server" ControlToValidate="txtBirthdate" ErrorMessage="Birthdate Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Gender</label>
                                        <asp:DropDownList ID="ddlGender" runat="server" class="form-control" name="account" TabIndex="10"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Blood Group </label>
                                        <asp:DropDownList ID="ddlBloodGroup" runat="server" class="form-control" name="account" TabIndex="11">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="A+" Value="A+"></asp:ListItem>
                                            <asp:ListItem Text="B+" Value="B+"></asp:ListItem>
                                            <asp:ListItem Text="AB+" Value="AB+"></asp:ListItem>
                                            <asp:ListItem Text="O+" Value="O+"></asp:ListItem>
                                            <asp:ListItem Text="A-" Value="A-"></asp:ListItem>
                                            <asp:ListItem Text="B-" Value="B-"></asp:ListItem>
                                            <asp:ListItem Text="AB-" Value="AB-"></asp:ListItem>
                                            <asp:ListItem Text="O-" Value="O-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Marrital Status </label>
                                        <asp:DropDownList ID="ddlMaritalStatus" runat="server" class="form-control" name="account" TabIndex="12">
                                            <asp:ListItem Text="Unmarried" Value="U"></asp:ListItem>
                                            <asp:ListItem Text="Married" Value="M"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <label for="username">Email ID</label>
                                        <asp:TextBox ID="txtEmailID" runat="server" type="text" MaxLength="50" placeholder="Email ID" class="form-control" name="name" ValidationGroup="OK" TabIndex="13"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVtxtEmailID" runat="server" ControlToValidate="txtEmailID" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" ErrorMessage="Enter Valid Email ID" ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Mobile No</label>
                                        <asp:TextBox ID="txtMobileNo" runat="server" type="text" MaxLength="10" placeholder="Mobile No" class="form-control" name="name" ValidationGroup="OK" TabIndex="14" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVtxtMobileNo" Display="Dynamic" ControlToValidate="txtMobileNo" ValidationExpression="^[\d]{10,10}$" runat="server" ErrorMessage="10 Digits Required." ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Qualification</label>
                                        <asp:TextBox ID="txtQualification" runat="server" type="text" MaxLength="50" placeholder="Qualification" class="form-control" name="name" ValidationGroup="OK" TabIndex="15"></asp:TextBox>

                                    </div>

                                    <div class="form-group col-lg-2">
                                        <label for="username">Previous Exp</label>
                                        <asp:TextBox ID="txtPreviousExp" runat="server" type="text" MaxLength="10" placeholder="Previous Exp" class="form-control" name="name" ValidationGroup="OK" TabIndex="16"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Designation</label>
                                        <asp:TextBox ID="txtDesignation" ReadOnly="true" runat="server" type="text" MaxLength="10" placeholder="Designation" class="form-control" name="name" ValidationGroup="OK" TabIndex="17"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-12">
                                    <div class="form-group col-lg-6">
                                        <div class="form-group">
                                            <label for="username">Permenent Address</label>
                                            <asp:TextBox ID="txtPAddress" runat="server" type="text" MaxLength="250" placeholder="Enter Address" class="form-control" name="name" ValidationGroup="OK" TabIndex="18"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group col-lg-6">
                                        <div class="form-group">
                                            <label for="username">Working Address</label>
                                            <asp:TextBox ID="txtWAddress" runat="server" type="text" MaxLength="250" placeholder="Enter Address" class="form-control" name="name" ValidationGroup="OK" TabIndex="24" AutoPostBack="True" OnTextChanged="txtWAddress_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">State Name</label>
                                        <asp:DropDownList ID="ddlPState" runat="server" class="form-control" name="account" TabIndex="19" OnSelectedIndexChanged="ddlPState_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">District Name</label>
                                        <asp:DropDownList ID="ddlPDist" runat="server" class="form-control" name="account" TabIndex="20" AutoPostBack="True" OnSelectedIndexChanged="ddlPDist_SelectedIndexChanged"></asp:DropDownList>
                                    </div>



                                    <div class="form-group col-lg-3">
                                        <label for="username">State Name</label>
                                        <asp:DropDownList ID="ddlWState" runat="server" class="form-control" name="account" TabIndex="25" AutoPostBack="True" OnSelectedIndexChanged="ddlWState_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlWState" runat="server" ControlToValidate="ddlWState" InitialValue="0" ErrorMessage="State Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>



                                    <div class="form-group col-lg-3">
                                        <label for="username">District Name</label>
                                        <asp:DropDownList ID="ddlWDist" runat="server" class="form-control" name="account" TabIndex="26" AutoPostBack="True" OnSelectedIndexChanged="ddlWDist_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>


                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
                                        <label for="username">Taluka Name</label>
                                        <asp:DropDownList ID="ddlPTaluka" runat="server" class="form-control" name="account" TabIndex="21" AutoPostBack="True" OnSelectedIndexChanged="ddlPTaluka_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">City Name</label>
                                        <asp:DropDownList ID="ddlPCity" runat="server" class="form-control" name="account" TabIndex="22" AutoPostBack="True" OnSelectedIndexChanged="ddlPCity_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Pincode</label>
                                        <asp:TextBox ID="txtPPinCode" runat="server" type="text" placeholder="Pincode" MaxLength="6" class="form-control" name="name" ValidationGroup="OK" TabIndex="23" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Taluka Name</label>
                                        <asp:DropDownList ID="ddlWTaluka" runat="server" class="form-control" name="account" TabIndex="27" AutoPostBack="True" OnSelectedIndexChanged="ddlWTaluka_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">City Name</label>
                                        <asp:DropDownList ID="ddlWCity" runat="server" class="form-control" name="account" TabIndex="28" AutoPostBack="True" OnSelectedIndexChanged="ddlWCity_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Pincode</label>
                                        <asp:TextBox ID="txtWPinCode" runat="server" type="text" placeholder="Pincode" MaxLength="6" class="form-control" name="name" ValidationGroup="OK" TabIndex="29" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">Bank </label>
                                        <asp:DropDownList ID="ddlBank" runat="server" class="form-control" name="account" TabIndex="30" AutoPostBack="True" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Bank Branch</label>
                                        <asp:DropDownList ID="ddlBankBranch" runat="server" class="form-control" name="account" TabIndex="31"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Bank A/C No.</label>
                                        <asp:TextBox ID="txtBankAcNo" runat="server" type="text" MaxLength="25" placeholder="Enter Bank A/C No." class="form-control" name="name" ValidationGroup="OK" TabIndex="32"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">PAN No.</label>
                                        <asp:TextBox ID="txtpanNo" runat="server" type="text" MaxLength="10" placeholder="Enter PAN No." Style="text-transform: uppercase;" class="form-control" name="name" ValidationGroup="OK" TabIndex="33" onfocusout="PANValidation(this.value)"></asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="REVtxtpanNo" runat="server" ControlToValidate="txtpanNo" ValidationExpression="/^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/" ErrorMessage="Enter Valid PAN No." ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                        --%>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">PF Applicable </label>
                                        <asp:DropDownList ID="ddlPFApplicable" runat="server" class="form-control" name="account" TabIndex="33" AutoPostBack="true" OnSelectedIndexChanged="ddlPFApplicable_SelectedIndexChanged">
                                            <asp:ListItem Text="Applicable" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Not Applicable" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="PNRPY" Value="P"></asp:ListItem>
                                            <asp:ListItem Text="At actual after Limit" Value="A"></asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3" id="divPFJoinDt">
                                        <label for="username">PF Joining Date</label>
                                        <asp:TextBox ID="txtPFJoindDt" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="34"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CEtxtPFJoindDt" PopupButtonID="imgPopup" runat="server" TargetControlID="txtPFJoindDt" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    </div>
                                    <div class="form-group col-lg-3" id="divPF">
                                        <label for="username">PF No.</label>
                                        <asp:TextBox ID="txtPFNo" runat="server" type="text" MaxLength="22" placeholder="Enter PF No." class="form-control" name="name" ValidationGroup="OK" TabIndex="35"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVtxtPFNo" Display="Dynamic" ControlToValidate="txtPFNo" ValidationExpression="^[\S\d]{22,22}$" runat="server" ErrorMessage="22 Digits Required." ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="form-group col-lg-3" id="divUAN">
                                        <label for="username">UAN No.</label>
                                        <asp:TextBox ID="txtUANNo" runat="server" type="text" MaxLength="12" placeholder="Enter UAN No." class="form-control" name="name" ValidationGroup="OK" TabIndex="36" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVtxtUANNo" Display="Dynamic" ControlToValidate="txtUANNo" ValidationExpression="^[\d]{12,12}$" runat="server" ErrorMessage="12 Digits Required." ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                    </div>
                                </div>

                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">ESI Applicable </label>
                                        <%--<asp:DropDownList ID="ddlESI" runat="server" class="form-control" name="account" TabIndex="32" onchange="ddlESI(this.value)" AutoPostBack="True" OnSelectedIndexChanged="ddlESI_SelectedIndexChanged">--%>
                                        <asp:DropDownList ID="ddlESI" runat="server" class="form-control" name="account" TabIndex="37" AutoPostBack="True" OnSelectedIndexChanged="ddlESI_SelectedIndexChanged">
                                            <asp:ListItem Text="Applicable" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Not Applicable" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="username">Exempted Date</label>
                                        <asp:TextBox ID="txtESIEffectDt" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="38" AutoPostBack="True" OnTextChanged="txtESIEffectDt_TextChanged"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CalendarExtender4" PopupButtonID="imgPopup" runat="server" TargetControlID="txtESIEffectDt" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    </div>
                                    <div class="form-group col-lg-3" id="divESI">
                                        <label for="username">ESI No</label>
                                        <asp:TextBox ID="txtESINo" runat="server" type="text" MaxLength="10" placeholder="Enter ESI No." class="form-control" name="name" ValidationGroup="OK" TabIndex="39" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="REVtxtESINo" Display="Dynamic" ControlToValidate="txtESINo" ValidationExpression="^[\d]{10,10}$" runat="server" ErrorMessage="10 Digits Required." ForeColor="Red" ValidationGroup="OK"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-lg-3" id="divESIRadio">
                                        <label for="username">ESI Calculation</label>
                                        <asp:RadioButtonList ID="rblESICal" runat="server" RepeatDirection="Horizontal" class="form-control i-checks" TabIndex="40">
                                            <asp:ListItem Text="Basic" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Gross" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>

                                    <%-- <div class="form-group col-lg-3">
                                        <div class="form-group">
                                            <label for="Isactive">HOD Applicable</label>
                                            <asp:CheckBox ID="chkHODApplicable" runat="server" Checked="false" type="checkbox" class="i-checks" TabIndex="35" />
                                        </div>
                                    </div>--%>
                                </div>

                                <div class="col-lg-12">

                                    <div class="col-lg-3">
                                        <label>Regional</label>
                                        <asp:DropDownList ID="ddlRegional" runat="server" class="form-control" TabIndex="41" AutoPostBack="true" OnSelectedIndexChanged="ddlRegional_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="col-lg-3">
                                        <label>Sub-Regional</label>
                                        <asp:DropDownList ID="ddlSubRegional" runat="server" TabIndex="42" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSubRegional_SelectedIndexChanged1"></asp:DropDownList>
                                    </div>

                                    <div class="form-group col-lg-3">
                                        <label for="username">HRA Applicable </label>
                                        <asp:DropDownList ID="ddlHRAApplicable" runat="server" class="form-control" name="account" TabIndex="43">
                                            <asp:ListItem Text="Applicable" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Not Applicable" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Professional Tax Applicable</label>
                                        <asp:DropDownList ID="ddlProfessionalTax" runat="server" class="form-control" name="account" TabIndex="44">
                                            <asp:ListItem Text="Applicable" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Not Applicable" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-lg-12">

                                    <div class="form-group col-lg-3">
                                        <label for="username">Labour Walfare Applicable</label>
                                        <asp:DropDownList ID="ddlLabourWalfare" runat="server" class="form-control" name="account" TabIndex="45" AutoPostBack="True" OnSelectedIndexChanged="ddlLabourWalfare_SelectedIndexChanged">
                                            <asp:ListItem Text="Applicable" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Not Applicable" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">LIN No</label>
                                        <asp:TextBox ID="txtLINNo" runat="server" type="text" placeholder="Enter LIN No." class="form-control" name="name" TabIndex="46"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">HOD Applicable</label>
                                        <asp:DropDownList ID="ddlHODApplicable" runat="server" class="form-control" name="account" TabIndex="47">
                                            <asp:ListItem Text="Applicable" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Not Applicable" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-lg-3">
                                        <label for="username">Gratuity Joining Date</label>
                                        <asp:TextBox ID="txtGratuityDate" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="48"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CalendarExtender1" PopupButtonID="imgPopup" runat="server" TargetControlID="txtGratuityDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    </div>

                                </div>

                                <div class="col-lg-12">

                                    <div class="form-group col-lg-3">
                                        <label for="username">Driving Licence No</label>
                                        <asp:TextBox ID="txtDrivingLicence" runat="server" type="text" MaxLength="15" placeholder="Driving Licence No" class="form-control" name="name" ValidationGroup="OK" TabIndex="49"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-3">
                                        <label>D/I</label>
                                        <asp:DropDownList ID="ddlDI" runat="server" TabIndex="50" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlDI_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">LIC ID</label>
                                        <asp:TextBox ID="txtLICID" runat="server" type="text" MaxLength="15" placeholder="LIC ID" class="form-control" name="name" ValidationGroup="OK" TabIndex="51"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">LIC Policy No</label>
                                        <asp:TextBox ID="txtLICPolicyNo" runat="server" type="text" MaxLength="15" placeholder="Policy No" class="form-control" name="name" ValidationGroup="OK" TabIndex="52" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="col-lg-12">
                                    <div class="form-group col-lg-3">
                                        <label for="username">Caste</label>
                                        <asp:DropDownList ID="ddlCast" runat="server" class="form-control" name="account" TabIndex="53">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">User Role</label>
                                        <asp:DropDownList ID="ddlRole" runat="server" class="form-control" name="account" TabIndex="54"></asp:DropDownList>
                                    </div>

                                   
                                </div>

                                <div class="col-lg-12">
                                    <div class="col-lg-3">
                                        <label for="username">Resignation Date</label>
                                        <asp:TextBox ID="txtResignation" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="57"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CR" PopupButtonID="imgPopup" runat="server" TargetControlID="txtResignation" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    </div>
                                    <div class="col-lg-3">
                                        <label for="username">Leave Date</label>
                                        <asp:TextBox ID="txtLeaveDate" runat="server" type="text" placeholder="DD/MM/YYYY" class="form-control" name="name" ValidationGroup="OK" TabIndex="58"></asp:TextBox>
                                        <Ajax:CalendarExtender ID="CalendarExtender2" PopupButtonID="imgPopup" runat="server" TargetControlID="txtLeaveDate" Format="dd/MM/yyyy"></Ajax:CalendarExtender>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Leave Reason</label>
                                        <asp:DropDownList ID="ddlLeaveReason" runat="server" class="form-control" name="account" TabIndex="59">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Resign" Value="Resign"></asp:ListItem>
                                            <asp:ListItem Text="Retirement" Value="Retirement"></asp:ListItem>
                                            <asp:ListItem Text="Termination" Value="Termination"></asp:ListItem>
                                            <asp:ListItem Text="Death" Value="Death"></asp:ListItem>
                                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtLeaveReason" runat="server" type="text" MaxLength="15" placeholder="Leave Reason" class="form-control" name="name"  ValidationGroup="OK" TabIndex="33"></asp:TextBox>--%>
                                    </div>

                                     <div class="col-lg-3">
                                        <label for="Isactive">Is Active</label>
                                        <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" type="checkbox" class="form-control i-checks" TabIndex="60" />
                                    </div>
                                </div>

                                

                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="88" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="80" />

                                        <asp:HiddenField ID="HFEmployee" runat="server" />
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
        function setEmployeeName() {
            var fName1 = document.getElementById("MainContent_txtFName").value;
            var mName1 = document.getElementById("MainContent_txtMName").value;
            var lName1 = document.getElementById("MainContent_txtLName").value;

            fName1 = fName1.charAt(0).toUpperCase() + fName1.slice(1);
            document.getElementById("MainContent_txtFName").value = fName1;

            mName1 = mName1.charAt(0).toUpperCase() + mName1.slice(1);
            document.getElementById("MainContent_txtMName").value = mName1;

            lName1 = lName1.charAt(0).toUpperCase() + lName1.slice(1);
            document.getElementById("MainContent_txtLName").value = lName1;

            var empName1 = fName1 + " " + mName1 + " " + lName1;
            document.getElementById("MainContent_txtEmpName").value = empName1;

        }
    </script>

    <script>
        function pfApplicable(reqid) {
            var ddlPFApplicable = document.getElementById("MainContent_ddlPFApplicable").value;

            if (ddlPFApplicable == "N") {
                document.getElementById('MainContent_txtPFJoindDt').setAttribute('readonly', 'readonly')
                document.getElementById('MainContent_txtPFNo').setAttribute('readonly', 'readonly')
                document.getElementById('MainContent_txtUANNo').setAttribute('readonly', 'readonly')
                document.getElementById('MainContent_txtPFJoindDt').disabled = true;

                document.getElementById('MainContent_txtPFJoindDt').value = ''
                document.getElementById('MainContent_txtPFNo').value = ''
                document.getElementById('MainContent_txtUANNo').value = ''
            }
            else {
                document.getElementById('MainContent_txtPFJoindDt').removeAttribute('readonly')
                document.getElementById('MainContent_txtPFNo').removeAttribute('readonly')
                document.getElementById('MainContent_txtUANNo').removeAttribute('readonly')

                document.getElementById('MainContent_txtPFJoindDt').disabled = false;
            }
        }


    </script>

    <script>
        function ddlESI(reqid) {
            var ddlESI = document.getElementById("MainContent_ddlESI").value;

            document.getElementById('MainContent_rblESICal_0').removeAttribute('checked');
            document.getElementById('MainContent_rblESICal_1').removeAttribute('checked');

            if (ddlESI == "N") {

                document.getElementById('MainContent_txtESINo').setAttribute('readonly', 'readonly');
                document.getElementById('MainContent_rblESICal').setAttribute('readonly', 'readonly');
                document.getElementById('MainContent_txtESINo').value = '';

                document.getElementById('MainContent_rblESICal_0').disabled = true;
                document.getElementById('MainContent_rblESICal_1').disabled = true;
            }
            else {
                document.getElementById('MainContent_txtESINo').removeAttribute('readonly');
                document.getElementById('MainContent_rblESICal').removeAttribute('readonly');

                document.getElementById('MainContent_rblESICal_0').disabled = false;
                document.getElementById('MainContent_rblESICal_1').disabled = false;
            }
        }
    </script>
    <script>
        function PANValidation(panVal) {
            var panVal = document.getElementById("MainContent_txtpanNo").value;
            //var panVal = $('#panNumber').val();
            var regpan = /^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$/;
            if (panVal.length > 0) {
                if (regpan.test(panVal)) {
                    // valid pan card number
                } else {
                    document.getElementById('MainContent_txtpanNo').value = '';
                    alert("Enter Valid PAN No.");

                    // invalid pan card number
                }
            }
        }
    </script>

</asp:Content>
