<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BanjiNet - Login</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="Shared/login.css">
    <script src="Scripts/loginsign.js"></script>
</head>
<body> <!--background="src/backgroundlogin.jpg"-->
    <form id="form1" runat="server">
    <div class="container text-center nav-tabs">
        <div class="jumbotron">
            <h1>Welcome to BanjiNet</h1>
            <p>This is the example project for benchmarking MongoDB and RaptorDB</p>
        </div>
    </div>
    <div class="container">
        <ul class="nav nav-tabs">
            <li class="active" id="logbtn"><a href="#tab1" data-toggle="tab">Login</a></li>
            <li id="regbtn"><a href="#tab2" data-toggle="tab">Registration</a></li>
        </ul>
    </div>
    <!--login-->
    <div id="fields" class="tab-content nav-tabs container">
        <div class="tab-pane active" id="tab1">
            <div class="col-lg-6 center-block">
                <form action="php/login.php" id="signin" class="form-vertical" role="form" method="post">
                    <div class="form-group">
                        <label for="emailsignin" class="control-label">E-mail: </label>
                        <input type="email" name="email" class="form-control" id="emailsignin" required>
                    </div>
                    <div class="form-group">
                        <label for="passwordsignin" class="control-label">Password: </label>
                        <input type="password" name="password" class="form-control" id="passwordsignin" required>
                    </div>
                    <div class="checkbox form-inline">
                        <label>
                            <input type="checkbox" name="remember" id="remember"> Remember me
                        </label>
                    </div>
                    <div class="form-group">
                        <input id="signinbtn" type="Button" class="btn btn-default" value="Sign in">
                        <span id="msgsignin">You don't have account please <a href="#tab2" id="trans1">Sign up</a></span>
                    </div>
                </form>
            </div>
        </div>
        <!--sign up-->
        <div class="tab-pane" id="tab2">
            <div class="col-lg-6 center-block">
                <form id="signup-form" class="form-vertical" role="form" method="post" data-toggle="validator">
                    <div class="form-group">
                        <label for="email" class="control-label">Your email address: </label>
                        <input type="email" name="emails" class="form-control" id="email" placeholder="e.g. banjac275@gmail.com" value="" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="firstname" class="control-label">Your first name: </label>
                        <input type="text" name="firstnames" class="form-control" id="firstname" placeholder="e.g. Nikola" data-minlength="2" value="" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="lastname" class="control-label">Your last name: </label>
                        <input type="text" name="lastnames" class="form-control" id="lastname" placeholder="e.g. Stevanovic" data-minlength="2" value="" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="password" class="control-label">Choose a password: </label>
                        <input type="password" name="passwords" class="form-control" id="password" data-minlength="5" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="password" class="control-label">Confirm a password: </label>
                        <input type="password" name="passwords" class="form-control" id="repeatedpwd" data-minlength="5" data-match="#password" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <input id="signupbtn" type="button" class="btn btn-default" value="Sign Up">
                        <span id="msgsignup">You already have an account please <a id="trans2" href="#tab1">Sign in</a></span>
                    </div>
                </form>
            </div>
        </div>
    </div>
        <div id="res" class="hide" runat="server"></div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="bootstrap/js/bootstrap.min.js"></script>
    </form>
</body>
</html>
