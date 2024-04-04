<%@ Page Title="ESI Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ESI.aspx.cs" Inherits="PayRoll.Masters.ESI" %>

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
                                    <span>ESI  </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">ESI Master
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
                                Add/Edit ESI
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
                                        <asp:RequiredFieldValidator ID="RFVddlMon" runat="server" ControlToValidate="ddlMon" InitialValue="00" ErrorMessage="Required" Display="Dynamic" ForeColor="Red"  ></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Year</label>
                                        <asp:DropDownList ID="ddlYear" runat="server" class="form-control" name="account"   AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" TabIndex="2"></asp:DropDownList>
                                        
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">ESI Employee %</label>
                                        <asp:TextBox ID="ESIEmp" runat="server" MaxLength="4" type="text" placeholder="ESI Emp. % " class="form-control" name="name"   ValidationGroup="OK" TabIndex="3" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">ESI Company %</label>
                                        <asp:TextBox ID="ESIComp" runat="server" MaxLength="4" type="text" placeholder="ESI Company %" class="form-control" name="name"   ValidationGroup="OK" TabIndex="4" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">Amount</label>
                                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" type="text" placeholder="Enter Amount" class="form-control" name="name"   ValidationGroup="OK" TabIndex="5" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                </div>
                               <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">1 Half Year From Month</label>
                                        <asp:TextBox ID="txtOhfm" runat="server" MaxLength="2" type="text" placeholder="Enter 1 Half Year From Month" class="form-control" name="name"   ValidationGroup="OK" TabIndex="6" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">1 Half Year To Month</label>
                                        <asp:TextBox ID="txtOhtm" runat="server" MaxLength="2" type="text" placeholder="Enter 1 Half Year To Month" class="form-control" name="name"   ValidationGroup="OK" TabIndex="7" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div class="form-group col-lg-2"></div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">2 Half Year From Month</label>
                                        <asp:TextBox ID="txtthfm" runat="server" MaxLength="2" type="text" placeholder="Enter 2 Half Year From Month" class="form-control" name="name"   ValidationGroup="OK" TabIndex="8" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="form-group col-lg-3">
                                        <label for="username">2 Half Year To Month</label>
                                        <asp:TextBox ID="txtthtm" runat="server" MaxLength="2" type="text" placeholder="Enter 2 Half Year To Month" class="form-control" name="name"   ValidationGroup="OK" TabIndex="9" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="col-lg-12">
                                      <div class="hr-line-dashed"></div>
                                    <div class="form-group col-lg-2"></div>
                                  
                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="10" />
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="11" />
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
                                ESI List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="OrgId" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                    <Columns>
                                        <%--<asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-2">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="col-lg-1"></HeaderStyle>
                                      </asp:TemplateField>--%>
                                        <asp:BoundField DataField="monthname" HeaderText="Month" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="year" HeaderText="Year" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESIEmpPer" HeaderText="ESI Employee %" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ESICompPer" HeaderText="ESI Company %" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HYr1FrMon" HeaderText="1st Half Year From Month" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HYr1ToMon" HeaderText="1st Half Year To Month" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HYr2FrMon" HeaderText="2nd Half Year From Month" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HYr2ToMon" HeaderText="2nd Half Year To Month" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-1" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-CssClass="col-lg-4">
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
