<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BanjiNet - Login</title>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="Shared/loginn.css">
    <script src="Scripts/logins.js"></script>
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
    <div id="fields" class="tab-content nav-tabs container text-center">
        <div class="tab-pane active" id="tab1">
            <div class="col-lg-3"></div>
            <div class="col-lg-6 center-block">
                <form id="signin" class="form-vertical" role="form" method="post">
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
            <div class="col-lg-3"></div>
        </div>
        <!--sign up-->
        <div class="tab-pane text-center" id="tab2">
            <%--<div class="col-lg-3"></div>--%>
            <div class="col-lg-6 center-block">
                <h3 class="control-label">User Sign Up </h3><hr />
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
                        <input id="signupbtn" type="button" class="btn btn-default" value="Sign Up User">
                        <span id="msgsignup">You already have an account please <a id="trans2" href="#tab1">Sign in</a></span>
                    </div>
                </form>
            </div>
            <!--company sign up-->
            <div class="col-lg-6 center-block">
                <h3 class="control-label">Company Sign Up </h3><hr />
                <form id="signup-form-company" class="form-vertical" role="form" method="post" data-toggle="validator">
                    <div class="form-group">
                        <label for="emailc" class="control-label">Your email address: </label>
                        <input type="email" name="emailc" class="form-control" id="email-company" placeholder="e.g. office@gmail.com" value="" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="companyname" class="control-label">Company name: </label>
                        <input type="text" name="companyname" class="form-control" id="companyname" placeholder="e.g. BanjiSoft" data-minlength="2" value="" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="ceo" class="control-label">Owner: </label>
                        <input type="text" name="ceo" class="form-control" id="ceo" placeholder="e.g. Nikola Stevanovic" data-minlength="2" value="" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="typec" class="control-label">Company type: </label>
                        <input type="text" name="typec" class="form-control" id="typec" placeholder="e.g. Software Development" data-minlength="2" value="" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="located" class="control-label">Location: </label>
                        <input type="text" name="located" class="form-control" id="located" placeholder="e.g. Nis" data-minlength="2" value="" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="passwordc" class="control-label">Choose a password: </label>
                        <input type="password" name="passwordc" class="form-control" id="passwordc" data-minlength="5" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <label for="repeatedpwdc" class="control-label">Confirm a password: </label>
                        <input type="password" name="repeatedpwdc" class="form-control" id="repeatedpwdc" data-minlength="5" data-match="#password" required>
                        <div class="help-block with-errors"></div>
                    </div>
                    <div class="form-group">
                        <input id="signupbtnc" type="button" class="btn btn-default" value="Sign Up Company">
                        <span id="msgsignupc">You already have an account please <a id="trans3" href="#tab1">Sign in</a></span>
                    </div>
                </form>
            </div>
        </div>
    </div>
        
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
    </form>
</body>
</html>
