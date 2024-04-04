<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="PayRoll.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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

    <!--Controls -->
    <div class="hr-line-dashed"></div>

    <div class="form-group">
        <label>Email</label>
        <input type="email" placeholder="Enter email" class="form-control" required>
    </div>

    <div class="form-group">
        <label>Password</label>
        <input type="password" placeholder="Password" class="form-control" name="password">
    </div>

    <div class="form-group">
        <label>Url</label>
        <input type="text" placeholder="Enter email" class="form-control" name="url">
    </div>

    <div class="form-group">
        <label>Number</label>
        <input type="text" placeholder="Enter email" class="form-control" name="number">
    </div>

    <div>
        <button class="btn btn-sm btn-primary m-t-n-xs" type="submit"><strong>Submit</strong></button>
    </div>

    <!--Controls -->

    <div class="form-group">
        <label class="col-sm-2 control-label">Normal</label>

        <div class="col-sm-10">
            <input type="text" class="form-control"></div>
    </div>


</asp:Content>
