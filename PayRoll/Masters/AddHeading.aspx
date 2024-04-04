<%@ Page Title="Add Heading Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddHeading.aspx.cs" Inherits="PayRoll.Masters.AddHeading" %>

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
                                    <span>Bank </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Additional Heading Master
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
                                 Additional Heading Master
                            </div>

                            <div class="panel-body">
                                <div class="form-group">
                                    <label for="username">Add1 Heading </label>
                                    <asp:TextBox ID="txtAdd1" runat="server" type="text" MaxLength="50" TabIndex="1" placeholder="Enter Add1 Heading" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                   
                                </div>
                                <div class="form-group">
                                    <label for="username">Add2 Heading </label>
                                    <asp:TextBox ID="txtAdd2" runat="server" type="text" MaxLength="50" TabIndex="2" placeholder="Enter Add2 Heading" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                    
                                </div>
                                <div class="form-group">
                                    <label for="username">Add3 Heading</label>
                                    <asp:TextBox ID="txtAdd3" runat="server" type="text" MaxLength="10" TabIndex="3" placeholder="Enter Add3 Heading" class="form-control" name="name" CausesValidation="True" ValidationGroup="OK" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                    
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
                                Additional Heading List
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="gvList" runat="server" class="table table-striped table-bordered table-hover" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" Width="100%" DataKeyNames="OrgID" OnPageIndexChanging="gvList_PageIndexChanging" OnRowCommand="gvList_RowCommand">
                                    <Columns>
                                       
                                        <asp:BoundField DataField="Add1Heading" HeaderText="Add1 Heading" HeaderStyle-CssClass="col-lg-4" />
                                        <asp:BoundField DataField="Add2Heading" HeaderText="Add2 Heading" HeaderStyle-CssClass="col-lg-4" />
                                        <asp:BoundField DataField="Add3Heading" HeaderText="Add3 Heading" HeaderStyle-CssClass="col-lg-4" />
                                        
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
