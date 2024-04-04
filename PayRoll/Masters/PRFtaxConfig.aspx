<%@ Page Title="Professional Tax Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PRFtaxConfig.aspx.cs" Inherits="PayRoll.Masters.PRFtaxConfig" %>

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
                                    <span>Professional Tax  </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Professional Tax Master
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
                                Add/Edit Professional Tax
                            </div>

                            <div class="panel-body">
                                <div class="row">
                                    <div class="form-group col-lg-3"></div>
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
                                        <label for="Year">Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"   AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" TabIndex="2"></asp:DropDownList>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="form-group col-lg-3"></div>

                                    <div class="form-group col-lg-3">
                                        <label for="State">State</label>
                                        <asp:DropDownList ID="ddlState" runat="server" class="form-control" name="account"   AutoPostBack="True" TabIndex="3" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="Gender">Gender</label>
                                        <asp:DropDownList ID="ddlGender" runat="server" class="form-control" name="account"   AutoPostBack="True" TabIndex="4" OnSelectedIndexChanged="ddlGender_SelectedIndexChanged"></asp:DropDownList>
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
                                Professional Tax List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="Gridview1" runat="server" ShowFooter="true" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="SrNo" HeaderText="Sr No" />
                                        <asp:TemplateField HeaderText="From Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox1" MaxLength="10" class="form-control" runat="server" Text='<%# Eval("FrAmount") %>' onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox2" MaxLength="10" class="form-control" runat="server" Text='<%# Eval("ToAmount") %>' onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox3" MaxLength="5" class="form-control" runat="server" Text='<%# Eval("TaxAmount") %>' onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Month">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="Month" runat="server" class="form-control" SelectedValue='<%# Bind("mon") %>'>
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
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Month Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TextBox4" MaxLength="5" class="form-control" runat="server" Text='<%# Eval("MonAmount") %>' onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Button ID="ButtonAdd" class="form-control" runat="server" OnClick="ButtonAdd_Click" Text="Add New Row" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="">
                                             <ItemTemplate>
                                                <asp:LinkButton ID="lbtnRemove" runat="server" Text="Remove" OnClick="lbtnRemove_Click"   ></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
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

</asp:Content>
