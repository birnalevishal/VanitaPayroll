<%@ Page Title="PF Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PFConfig.aspx.cs" Inherits="PayRoll.Masters.PFConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                                    <span>PF  </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">PF Master
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
                                Add/Edit PF
                            </div>

                            <div class="panel-body">
                                 <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Month Name</label>
                                        <asp:DropDownList ID="ddlMon" runat="server" class="form-control" name="account"   AutoPostBack="True" OnSelectedIndexChanged="ddlMon_SelectedIndexChanged" TabIndex="1">
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
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"   AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" TabIndex="2"></asp:DropDownList>

                                    </div>
                                </div>
                                 <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">PF %</label>
                                        <asp:TextBox ID="PF" runat="server" MaxLength="4" type="text" placeholder="Enter PF In % " class="form-control" name="name"   ValidationGroup="OK" TabIndex="3" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="PF" ErrorMessage="PF Is Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">EPF %</label>
                                        <asp:TextBox ID="Epf" runat="server" MaxLength="4" type="text" placeholder="Enter EPF In %" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Epf" ErrorMessage="EPF Is Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">EPS %</label>
                                        <asp:TextBox ID="EPS" runat="server" MaxLength="4" type="text" placeholder="Enter EPS In %" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="EPS" ErrorMessage="EPS Is Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="PfCondition">PF Condition</label>
                                        <asp:TextBox ID="PfCondition" runat="server" MaxLength="4" type="text" placeholder="Enter PF Condition" class="form-control" name="name"   ValidationGroup="OK" TabIndex="6" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="PfAmount">PF Amount Limit</label>
                                        <asp:TextBox ID="PfAmount" runat="server" MaxLength="10" type="text" placeholder="Enter PF Amount" class="form-control" name="name"   ValidationGroup="OK" TabIndex="7" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="AgeLimit">Age Limit</label>
                                        <asp:TextBox ID="AgeLimit" runat="server" MaxLength="2" type="text" placeholder="Enter Age Limit" class="form-control" name="name"   ValidationGroup="OK" TabIndex="8" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="PfCondition">HRA %</label>
                                        <asp:TextBox ID="HRA" runat="server" MaxLength="10" type="text" placeholder="Enter HRA" class="form-control" name="name"   ValidationGroup="OK" TabIndex="9" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="PfCondition">Admin Charge 1 %</label>
                                        <asp:TextBox ID="AdCharge1" runat="server" MaxLength="10" type="text" placeholder="Enter Admin Charge 1 in %" class="form-control" name="name"   ValidationGroup="OK" TabIndex="10" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="PfCondition">Admin Minimum Amount</label>
                                        <asp:TextBox ID="AdminCharge" runat="server" MaxLength="10" type="text" placeholder="Enter Admin Minimum Amount" class="form-control" name="name"   ValidationGroup="OK" TabIndex="11" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="PfCondition">Admin Charge 2 %</label>
                                        <asp:TextBox ID="AdCharge2" runat="server" MaxLength="10" type="text" placeholder="Enter Admin Charge 2 In %" class="form-control" name="name"   ValidationGroup="OK" TabIndex="12" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="col-lg-12">
                                    <div class="hr-line-dashed"></div>
                                    <div class="form-group col-lg-2"></div>
                                    
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="14" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="13" />
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
                                PF List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="OrgId" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                    <Columns>

                                        <asp:BoundField DataField="monthname" HeaderText="Month" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="year" HeaderText="Year" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PF" HeaderText="PF" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EPF" HeaderText="EPF" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EPS" HeaderText="EPS" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PFConditional" HeaderText="PF Conditional" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PFAmtLimit" HeaderText="PF Amount Limit" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PFAgeLimit" HeaderText="PF Age Limit" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HRA" HeaderText="HRA" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AdminCharge1" HeaderText="Admin Charge 1" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AdminCharge1min" HeaderText="Admin Minimum Charge 1" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AdminCharge2" HeaderText="Admin Charge 2" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
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

</asp:Content>
