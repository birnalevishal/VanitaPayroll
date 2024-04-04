<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="PayRoll.TestPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--Page Layout -->
    <div class="small-header">
        <div class="hpanel">
            <div class="panel-body">
                <div id="hbreadcrumb" class="pull-right">
                    <ol class="hbreadcrumb breadcrumb">
                        <li><a href="index.html">Dashboard</a></li>
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
                        ********
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
                        ******
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
        </div>
    </div>

    <!--Page Layout -->

    
</asp:Content>
