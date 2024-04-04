<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CountryMaster.aspx.cs" Inherits="PayRoll.Masters.CountryMaster" %>

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
                                    <span>Masters</span>
                                </li>
                                <li class="active">
                                    <span>Country </span>
                                </li>
                            </ol>
                        </div>
                        <h2 class="font-light m-b-xs">Country Master
                        </h2>

                    </div>
                </div>
            </div>

            <div class="content">
                <div class="row">
                    <div class="col-lg-6">
                        <div class="hpanel">
                            <div class="panel-heading">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Add/Edit Country
                            </div>
                            <div class="panel-body">
                                <form role="form" id="form1">
                                    <div class="form-group">
                                        <label for="username">Country Name</label>
                                        <input type="text" placeholder="enter country name" class="form-control" name="name" id="countryName">
                                    </div>
                                    <div class="hr-line-dashed"></div>


                                    <div class="form-group">
                                        <label for="Isactive">Is Active</label>
                                        <input type="checkbox" class="i-checks" id="isActive">
                                    </div>
                                    <div class="hr-line-dashed"></div>

                                    <div class="form-group">
                                        <div class="col-sm-8 col-sm-offset-0">
                                            <button class="btn btn-outline btn-warning" type="submit">Cancel</button>
                                            <button class="btn btn-outline btn-success" type="submit" id="btnSave">Save</button>

                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6">
                        <div class="hpanel">
                            <div class="panel-heading">
                                <div class="panel-tools">
                                    <a class="showhide"><i class="fa fa-chevron-up"></i></a>
                                    <a class="closebox"><i class="fa fa-times"></i></a>
                                </div>
                                Country List
                            </div>
                            <div class="panel-body">
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

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://cdn.jsdelivr.net/json2/0.1/json2.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#btnSave").on("click", function () {
                var country = {};
                country.Country = $("#countryName").val();
                country.IsActive = $("#isActive").val();
                alert("hi");
                $.ajax({
                    type: "POST",
                    url: "CountryMaster/InsertRecord",
                    data: '{obj: ' + JSON.stringify(country) + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Data added successfully.");
                        window.location.reload();
                    }
                });
                return false;
            });
        });
    </script>

    
    <script type="text/javascript">
        var updateProgress = null;
        function postbackButtonClick() {
            updateProgress = $find("<%= UpdateProgress1.ClientID %>");
            window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());
            return true;
        }
    </script>
</asp:Content>
