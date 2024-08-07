﻿<%@ Page Title="City Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="City.aspx.cs" Inherits="PayRoll.Masters.City" %>

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
                                    <span>City </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">City Master
                        </h2>

                    </div>
                </div>
            </div>

            <div class="content">
                <div class="row">
                    <div class="col-lg-6">
                       <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Add/Edit City Name
                            </div>

                            <div class="panel-body">
                                 <div class="form-group">
                                    <label for="username">Taluka Name</label>
                                    <asp:DropDownList ID="ddlList" runat="server" class="form-control m-b" name="account" CausesValidation="True" TabIndex="1"></asp:DropDownList>
                                </div>
                                <div class="form-group">

                                    <label for="username">City Name</label>
                                    <asp:TextBox ID="txtName" runat="server" type="text" MaxLength="50" placeholder="Enter City Name" class="form-control" name="name" ValidationGroup="OK" TabIndex="1" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="City Name Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                </div>
                                
                                <div class="form-group">
                                    <label for="username">Pin Code</label>
                                    <asp:TextBox ID="txtName1" runat="server" type="text" MaxLength="6" placeholder="Enter Pin Code" class="form-control" name="name"  ValidationGroup="OK" TabIndex="2" onkeypress="return onlyNumbers(event,this);"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName" ErrorMessage="Pin Code Is Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                                </div>
                              
                                <div class="form-group">
                                    <label for="Isactive">Is Active</label>
                                    <asp:CheckBox ID="chkIsActive" runat="server" type="checkbox" class="i-checks" Checked="True" TabIndex="4" />
                                </div>
                                <div class="hr-line-dashed"></div>

                                <div class="form-group">
                                    <div class="col-sm-8 col-sm-offset-0">
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-outline btn-warning" OnClick="btnCancel_Click" TabIndex="6" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-outline btn-success" OnClick="btnSave_Click" ValidationGroup="OK" TabIndex="5" />
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>

                    <div class="col-lg-6">
                       <div class="hpanel hblue">
                            <div class="panel-heading hbuilt">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                City List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvCityList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="10" Width="100%" DataKeyNames="Citycd" OnRowCommand="gvCityList_RowCommand" OnPageIndexChanging="gvCityList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="col-lg-2">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnYear" runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Show">Edit</asp:LinkButton>
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="col-lg-2"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="City" HeaderText="City Name" HeaderStyle-CssClass="col-lg-6">
                                            <HeaderStyle CssClass="col-lg-8"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Pincode" HeaderText="Pin Code" HeaderStyle-CssClass="col-lg-6">
                                            <HeaderStyle CssClass="col-lg-8"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsActive" HeaderText="Active" HeaderStyle-CssClass="col-lg-4">
                                            <HeaderStyle CssClass="col-lg-2"></HeaderStyle>
                                        </asp:BoundField>
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
