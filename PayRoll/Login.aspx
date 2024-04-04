<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PayRoll.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">

    <!-- Page title -->
    <title>Login - PayRoll System</title>

    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <!--<link rel="shortcut icon" type="image/ico" href="favicon.ico" />-->

    <!-- Vendor styles -->
    <link rel="stylesheet" href="vendor/fontawesome/css/font-awesome.css" />
    <link rel="stylesheet" href="vendor/metisMenu/dist/metisMenu.css" />
    <link rel="stylesheet" href="vendor/animate.css/animate.css" />
    <link rel="stylesheet" href="vendor/bootstrap/dist/css/bootstrap.css" />

    <!-- App styles -->
    <link rel="stylesheet" href="fonts/pe-icon-7-stroke/css/pe-icon-7-stroke.css" />
    <link rel="stylesheet" href="fonts/pe-icon-7-stroke/css/helper.css" />
    <link rel="stylesheet" href="styles/style.css" />
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
                if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32))
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
</head>
<body class="blank">
    <div class="color-line"></div>

    <div class="login-container">
        <div class="row">
            <div class="col-md-12">
                <div class="text-center m-b-md">
                    <h3>PLEASE LOGIN TO APP</h3>
                </div>
                <div class="hpanel">
                    <div class="panel-body">
                        <form id="loginForm" runat="server" autocomplete="off">
                            <div class="form-group">
                                <label class="control-label" for="username">Username</label>
                                <%--<input type="text" placeholder="example@gmail.com" title="Please enter you username" required="" value="" name="username" id="username" class="form-control"/>--%>
                                <asp:TextBox ID="txtName" runat="server" placeholder="Enter User ID" class="form-control" name="name" TabIndex="1" AutoPostBack="True" MaxLength="10" OnTextChanged="txtName_TextChanged" onblur ="myFunction()" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="Username Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <label class="control-label" for="password">Password</label>
                                <%--<input type="password" title="Please enter your password" placeholder="******" required="" value="" name="password" id="password" class="form-control"/>--%>
                                <asp:TextBox ID="txtpassword" runat="server" type="password" placeholder="Enter Password" class="form-control" name="name" TabIndex="2" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtpassword" ErrorMessage="Password Required" Display="Dynamic" ForeColor="Red" ValidationGroup="OK"></asp:RequiredFieldValidator>
                            </div>

                            <div class="form-group" id="divddllist">
                                <%-- <label for="username">Organisation</label>--%>
                                <asp:Label ID="lblOrg" runat="server" Text="Organisation" Font-Bold="true"></asp:Label>
                                <asp:DropDownList ID="ddlList" runat="server" class="form-control m-b" name="account" CausesValidation="True" TabIndex="3"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="username">Year</label>
                                <asp:DropDownList ID="ddlYear" runat="server" class="form-control m-b" name="account" CausesValidation="True" TabIndex="4"></asp:DropDownList>
                            </div>
                            <%-- <div class="checkbox">
                                <input type="checkbox" class="i-checks" checked="checked" />
                                Remember login
                            </div>--%>
                            <div style="text-align: center; margin-bottom: 5px;">
                                <asp:Label ID="lblWarning" ForeColor="Red" runat="server" Text="" Visible="false"></asp:Label>
                            </div>
                            <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-success btn-block" OnClick="btnLogin_Click" TabIndex="5" />
                            <%--<asp:Button ID="btnForgetPasswd" runat="server" class="btn btn-default btn-block" Text="Forget Password?" OnClick="btnForgetPasswd_Click" />--%>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-center">
                <strong>HR Software</strong> - Maintained By Rubicon Systems
                   
            </div>
        </div>
    </div>

    <!-- Vendor scripts -->
    <script src="vendor/jquery/dist/jquery.min.js"></script>
    <script src="vendor/jquery-ui/jquery-ui.min.js"></script>
    <script src="vendor/slimScroll/jquery.slimscroll.min.js"></script>
    <script src="vendor/bootstrap/dist/js/bootstrap.min.js"></script>
    <script src="vendor/metisMenu/dist/metisMenu.min.js"></script>
    <script src="vendor/iCheck/icheck.min.js"></script>
    <script src="vendor/sparkline/index.js"></script>
     <script type="text/javascript"  lang="js">


    </script>
    <!-- App scripts -->
    <script src="scripts/homer.js"></script>

    <script>
        function myFunction() {
            __doPostBack("<%=txtName%>", txtName_TextChanged);
        }</script>

</body>
</html>
