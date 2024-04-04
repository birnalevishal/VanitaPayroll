<%@ Page Title="Hamali Salary Processing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HamaliSalaryProcessing.aspx.cs" Inherits="PayRoll.Masters.HamaliSalaryProcessing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

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
                                    <span>Hamali Salary Processing </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Hamali Salary Processing
                        </h2>

                    </div>
                </div>
            </div>

            <div class="content">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                               Hamali Salary Processing
                            </div>

                            <div class="panel-body">
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2">
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
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"   TabIndex="2"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-2">
                                        <label for="username">Employee Code</label>
                                        <asp:TextBox ID="txtEmpCode" runat="server" MaxLength="6" type="text" placeholder="Enter Emp. Code" class="form-control" name="name"   ValidationGroup="OK" TabIndex="1" AutoPostBack="True" onkeypress="return onlyNumbers(event,this);" OnTextChanged="txtEmpCode_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVtxtEmpCode" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="Employee Code Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                     <div class="form-group col-lg-4">
                                        <label for="Canteen">Employee Name</label>
                                        <asp:DropDownList ID="ddlEmpName" runat="server" class="form-control" name="account"   AutoPostBack="True" TabIndex="2" OnSelectedIndexChanged="ddlEmpName_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                    <div class="form-group col-lg-1" style="margin-top: 22px">
                                        <asp:Button ID="btnGetData" runat="server" Text="Get Data" class="btn btn-outline btn-success" OnClick="btnGetData_Click" TabIndex="999" ValidationGroup="OK" />
                                    </div>
                                </div>
                           

                            <asp:Panel ID="pnlSalaryData" runat="server" Enabled="false" Visible="false">
                                <div class="hpanel hblue">
                                    <div class="panel-heading hbuilt">
                                        Attendence
                                    </div>
                                    <div class="panel-body">
                                        <div class="col-lg-12">
                                            <div class="form-group col-lg-2">
                                                <label for="username">Month Days</label>
                                                <asp:TextBox ID="txtDaysInMonth" runat="server" type="text" MaxLength="25" placeholder="Pay Days" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="form-group col-lg-2">
                                                <label for="username">Pay Days</label>
                                                <asp:TextBox ID="txtPayDays" runat="server" type="text" MaxLength="25" placeholder="Pay Days" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="form-group col-lg-2">
                                                <label for="username">Present Days</label>
                                                <asp:TextBox ID="txtPresentDays" runat="server" type="text" MaxLength="25" placeholder="Present Days" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>

                                            <div class="form-group col-lg-2">
                                                <label for="username">Absent Days</label>
                                                <asp:TextBox ID="txtAbsentDays" runat="server" type="text" MaxLength="25" placeholder="Absent Days" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="form-group col-lg-2">
                                                <label for="username">PL</label>
                                                <asp:TextBox ID="txtPL" runat="server" type="text" MaxLength="25" placeholder="PL" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="form-group col-lg-2">
                                                <label for="username">Weekly Off</label>
                                                <asp:TextBox ID="txtWeeklyOff" runat="server" type="text" MaxLength="25" placeholder="Weekly Off" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>

                                        </div>
                                        <div class="col-lg-12">
                                            <div class="form-group col-lg-2">
                                                <label for="username">Payable Days</label>
                                                <asp:TextBox ID="txtPaybleDays" runat="server" type="text" MaxLength="25" placeholder="Payble Days" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>

                                            <div class="form-group col-lg-2">
                                                <label for="username">Pay Holidays</label>
                                                <asp:TextBox ID="txtPayHolidays" runat="server" type="text" MaxLength="25" placeholder="Pay Holidays" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="form-group col-lg-2">
                                                <label for="username">COff</label>
                                                <asp:TextBox ID="txtCOff" runat="server" type="text" MaxLength="25" placeholder="COff" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="hpanel hblue">
                                        <div class="panel-heading hbuilt">
                                            Earnings
                                        </div>
                                        <div class="panel-body">
                                            <div class="col-lg-12">
                                                <div class="form-group col-lg-2">
                                                    <label for="username">Basic+DA</label>
                                                    <asp:TextBox ID="txtBasicDA" runat="server" type="text" MaxLength="25" placeholder="Basic+DA" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="form-group col-lg-2">
                                                    <label for="username">HRA</label>
                                                    <asp:TextBox ID="txtHRA" runat="server" type="text" MaxLength="25" placeholder="HRA" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>

                                                <div class="form-group col-lg-2">
                                                    <label for="username">Conveyance</label>
                                                    <asp:TextBox ID="txtConveyance" runat="server" type="text" MaxLength="25" placeholder="CA" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="form-group col-lg-2">
                                                    <label for="username">Education</label>
                                                    <asp:TextBox ID="txtEducation" runat="server" type="text" MaxLength="25" placeholder="Education" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="form-group col-lg-2">
                                                    <label for="username">Medical</label>
                                                    <asp:TextBox ID="txtMedical" runat="server" type="text" MaxLength="25" placeholder="Medical" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="form-group col-lg-2">
                                                    <label for="username">Tea & Tiffin</label>
                                                    <asp:TextBox ID="txtCanteen" runat="server" type="text" MaxLength="25" placeholder="Canteen" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-lg-12">
                                                <div class="form-group col-lg-2">
                                                    <label for="username">Washing</label>
                                                    <asp:TextBox ID="txtWashing" runat="server" type="text" MaxLength="25" placeholder="Washing" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="form-group col-lg-2">
                                                    <label for="username">Uniform & Shoes</label>
                                                    <asp:TextBox ID="txtUniform" runat="server" type="text" MaxLength="25" placeholder="Uniform" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>

                                                 <div class="form-group col-lg-2">
                                                    <asp:Label ID="lblAdd1" runat="server" Font-Bold="true" Text="."></asp:Label>
                                                    <asp:TextBox ID="txtAdd1" runat="server" type="text" MaxLength="25" placeholder="" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    
                                                </div>
                                                <div class="form-group col-lg-2">
                                                    <asp:Label ID="lblAdd2" runat="server" Font-Bold="true" Text="."></asp:Label>
                                                    <asp:TextBox ID="txtAdd2" runat="server" type="text" MaxLength="25" placeholder="" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    
                                                </div>
                                                 <div class="form-group col-lg-2">
                                                    <asp:Label ID="lblAdd3" runat="server" Font-Bold="true" Text="."></asp:Label>
                                                    <asp:TextBox ID="txtAdd3" runat="server" type="text" MaxLength="25" placeholder="" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    
                                                </div>
                                                <div class="form-group col-lg-2">
                                                    <label for="username">Incentive</label>
                                                    <asp:TextBox ID="txtIsentive" runat="server" type="text" MaxLength="25" placeholder="Washing" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                </div>

                                            </div>


                                        </div>

                                        <div class="hpanel hblue">
                                            <div class="panel-heading hbuilt">
                                                Deduction
                                            </div>
                                            <div class="panel-body">
                                                <div class="col-lg-12">
                                                    <div class="form-group col-lg-2">
                                                        <label for="username">Salary Advance</label>
                                                        <asp:TextBox ID="txtAdvance" runat="server" type="text" MaxLength="25" placeholder="Salary Advance" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                    </div>
                                                    <div class="form-group col-lg-2">
                                                        <label for="username">Povidend Fund</label>
                                                        <asp:TextBox ID="txtPf" runat="server" type="text" MaxLength="25" placeholder="Povidend Fund" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                    </div>

                                                    <div class="form-group col-lg-2">
                                                        <label for="username">Professional Tax</label>
                                                        <asp:TextBox ID="txtProfessionalTax" runat="server" type="text" MaxLength="25" placeholder="Professional Tax" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                    </div>
                                                    <div class="form-group col-lg-2">
                                                        <label for="username">ESI Contribution</label>
                                                        <asp:TextBox ID="txtESI" runat="server" type="text" MaxLength="25" placeholder="ESI Contribution" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                    </div>

                                                    <div class="form-group col-lg-2">
                                                        <label for="username">Loan</label>
                                                        <asp:TextBox ID="txtLoan" runat="server" type="text" MaxLength="25" placeholder="Loan" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                    </div>
                                                    <div class="form-group col-lg-2">
                                                        <label for="username">Pathsanstha</label>
                                                        <asp:TextBox ID="txtPathSanstha" runat="server" type="text" MaxLength="25" placeholder="Pathsanstha" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                    </div>
                                                </div>

                                                <div class="col-lg-12">
                                                    <div class="form-group col-lg-2">
                                                        <label for="username">TDS</label>
                                                        <asp:TextBox ID="txtTDS" runat="server" type="text" MaxLength="25" placeholder="TDS" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                    </div>
                                                    <div class="form-group col-lg-2">
                                                        <label for="username">LWF</label>
                                                        <asp:TextBox ID="txtLWF" runat="server" type="text" MaxLength="25" placeholder="LWF" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                        <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="hpanel hblue">
                                                <div class="panel-heading hbuilt">
                                                    Net
                                                </div>
                                                <div class="panel-body">

                                                    <div class="col-lg-12">
                                                        <div class="form-group col-lg-3">
                                                            <label for="username">Gross</label>
                                                            <asp:TextBox ID="txtGross" runat="server" type="text" MaxLength="25" placeholder="Gross Salary" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                        <div class="form-group col-lg-3">
                                                            <label for="username">Deduction</label>
                                                            <asp:TextBox ID="txtDeduction" runat="server" type="text" MaxLength="25" placeholder="Total Deduction" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="RFVtxtFName" runat="server" ControlToValidate="txtFName" ErrorMessage="First Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                        </div>

                                                        <div class="form-group col-lg-3">
                                                            <label for="username">Net Salary</label>
                                                            <asp:TextBox ID="txtNetAmount" runat="server" type="text" MaxLength="25" placeholder="Net Salary" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyAlphabets(event,this);" onchange="setEmployeeName()"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="RFVtxtMName" runat="server" ControlToValidate="txtMName" ErrorMessage="Middle Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlGVList" runat="server" Enabled="false" Visible="false">
                                <div class="col-lg-12">
                                    <div class="hpanel">
                                        <div class="panel-heading">
                                            <div class="panel-tools">
                                                <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                                <a class="closebox"><i class="fa fa-times"></i></a>
                                            </div>
                                            List Of Employees Having Net Salary Below 50%
                                        </div>
                                        <div class="panel-body">
                                            <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" Width="100%" DataKeyNames="EmpCode">
                                                <Columns>
                                                    <asp:BoundField DataField="EmpCode" HeaderText="Employee Code" HeaderStyle-CssClass="col-lg-6">
                                                        <HeaderStyle CssClass="col-lg-2"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="EmpName" HeaderText="Name" HeaderStyle-CssClass="col-lg-4">
                                                        <HeaderStyle CssClass="col-lg-8"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="net" HeaderText="NetSalary" HeaderStyle-CssClass="col-lg-4">
                                                        <HeaderStyle CssClass="col-lg-2"></HeaderStyle>
                                                    </asp:BoundField>
                                                </Columns>
                                                <PagerStyle CssClass="GridPager" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <div class="col-lg-12">
                                <div class="hpanel">
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="37" />
                                            <asp:Button ID="btnSave" runat="server" Text="Salary Process" class="btn btn-outline btn-success" OnClick="btnSave_Click" TabIndex="36" />
                                            <asp:HiddenField ID="HFEmployee" runat="server" />
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
                    alert("Enter Valid PAN No.");
                    // invalid pan card number
                }
            }
        }
    </script>
</asp:Content>
