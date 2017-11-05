<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Search" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
    <head runat="server">
        <title>BanjiNet - Search</title>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="~/Shared/user.css"/>
        <script src="Scripts/searche.js"></script>
    </head>
    <body>
        <!-- top nav -->
           <div class="container">
              <nav class="navbar navbar-default nav-tabs">
                <div class="container-fluid">
                  <div id="navbar1" class="navbar-header">
                    <ul class="nav navbar-nav">
                      <li class="navbar-brand navbar-left"><a href="#">BanjiNet</a></li>
                      <li><a href="#">Home</a></li>
                      <li class="active"><a href="Search.aspx">Search</a></li>
                      <li><a href="workerslist.aspx">Workers</a></li>
                      <li><a href="companylist.aspx">Companies</a></li>
                      <li><a href="profileEditor.aspx">Edit Profile</a></li>
                      <li><a href="UserProfile.aspx">User Profile</a></li>
                      <li><a href="logout.aspx">Log Out</a></li>
                    </ul>
                  </div>
                  <!--/.nav-collapse -->
                    </div>
                <!--/.container-fluid -->
              </nav>
            </div>                     
        <!-- /top nav -->

        <!-- Search bar -->
        <div class="row">
          <div class="col-lg-3"></div>
          <div class="input-group col-lg-6">
              <div class="input-group-btn">
                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Specify <span class="caret"></span></button>
                <ul class="dropdown-menu">
                  <li><span class="input-group-addon"><input type="checkbox" aria-label="..." id="first">First Name</span></li>
                  <li><span class="input-group-addon"><input type="checkbox" aria-label="..." id="last">Last Name</span></li>
                  <li><span class="input-group-addon"><input type="checkbox" aria-label="..." id="mmail">E-mail</span></li>
                  <li><span class="input-group-addon"><input type="checkbox" aria-label="..." id="comp">Company</span></li>
                  <li><span class="input-group-addon"><input type="checkbox" aria-label="..." id="skiill">Skill</span></li>
                </ul>
              </div><!-- /btn-group -->
              <input type="text" class="form-control" id="srcinput" placeholder="Search..."/>
              <span class="input-group-btn">
                <button class="btn btn-default" type="button" id="srcbttn">Go!</button>
              </span>
            </div><!-- /input-group -->
            <div class="col-lg-3" style="margin-right: 5.9em;"></div>
            <div class="col-lg-5" id="livesearch"></div>
        </div><!-- /.row -->

        <div class="row" id="listW">
        <div class="col-lg-2"></div>
        <div class="col-lg-8">
               <h3 class="text-center">Workers: </h3>
               <table class="table table-hover">
                  <thead class="thead-inverse">
                    <tr>
                      <th>#</th>
                      <th>First Name</th>
                      <th>Last Name</th>
                      <th>Email</th>
                      <th>Company</th>
                    </tr>
                  </thead>
                  <tbody id="listingW">
                                        
                  </tbody>
                </table>
           </div>
         </div>

        <div class="row" id="listC">
        <div class="col-lg-2"></div>
        <div class="col-lg-8">
               <h3 class="text-center">Companies: </h3>
               <table class="table table-hover">
                  <thead class="thead-inverse">
                    <tr>
                      <th>#</th>
                      <th>Company Name</th>
                      <th>Email</th>
                      <th>Type</th>
                      <th>Location</th>
                    </tr>
                  </thead>
                  <tbody id="listingC">
                                        
                  </tbody>
                </table>
           </div>
         </div>

        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js" integrity="sha256-T0Vest3yCU7pafRw9r+settMBX6JkKN06dqBnpQ8d30=" crossorigin="anonymous"></script>
        <script src="https://code.jquery.com/jquery-3.2.1.js" integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=" crossorigin="anonymous"></script>
    </body>
</html>
