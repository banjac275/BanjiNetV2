using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class profileEditor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Workers recv;
        if (Session["user"] != null)
        {
            recv = (Workers)Session["user"];
            Response.Write("<script>console.log('" + recv.FirstName + "');</script>");
            personalp.InnerHtml = "<form id = 'edit_form' class='form-vertical' role='form' method='post' data-toggle='validator'>"
                   +"<div class='form-group'>"
                        +"<label for='email' class='control-label'>Your email address: </label>"
                        + "<input type = 'email' name='emails' class='form-control' id='email' value='" + recv.Email + "'>"
                   + "<div class='help-block with-errors'></div>"
                   +"</div>"
                   +"<div class='form-group'>"
                       +"<label for='firstname' class='control-label'>Your first name: </label>"
                       +"<input type = 'text' name='firstnames' class='form-control' id='firstname' data-minlength='2' value='"+ recv.FirstName +"'>"
                       +"<div class='help-block with-errors'></div>"
                   +"</div>"
                   +"<div class='form-group'>"
                       +"<label for='lastname' class='control-label'>Your last name: </label>"
                       + "<input type = 'text' name='lastnames' class='form-control' id='lastname' data-minlength='2' value='" + recv.LastName + "'>"
                       + "<div class='help-block with-errors'></div>"
                   +"</div>"
                   + "<div class='form-group'>"
                       + "<label for='company' class='control-label'>Your Company: </label>"
                       + "<input type = 'text' name='company' class='form-control' id='firm' data-minlength='2' value='" + recv.CompanyId + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       +"<label for='password' class='control-label'>Choose a password: </label>"
                       + "<input type = 'text' name='passwords' class='form-control' id='password' data-minlength='5' value='" + recv.Password + "'>"
                       + "<div class='help-block with-errors'></div>"
                   +"</div>"
                   +"<div class='form-group'>"
                       +"<label for='password' class='control-label'>Confirm a password: </label>"
                       +"<input type ='text' name='passwords' class='form-control' id='repeatedpwd' data-minlength='5' data-match='#password' required>"
                       +"<div class='help-block with-errors'></div>"
                   +"</div>"
                   +"<div class='form-group'>"
                       +"<div class='col-lg-6'>"
                            +"<div class='checkbox form-inline'>"
                                +"<label>"
                                    + "<input type ='checkbox' name ='remember' id ='remember' checked data-toggle='toggle'> Remember me"
                                 + "</label>"     
                            + "</div><hr/>"
                            + "<label class='control-label'>Pick Database(s): (default is MongoDB)</label>"
                            + "<label class='checkbox-inline'>"
                                +"<input type ='checkbox' checked data-toggle='toggle'> MongoDB"
                            +"</label>"
                            + "<label class='checkbox-inline'>"
                                +"<input type ='checkbox' data-toggle='toggle'> RaptorDB"
                            + "</label><hr/>"
                            + "<input id ='change' type='button' class='btn btn-default' value='Change values'>"
                       + "</div>"
                   + "</div>"
               +"</form><hr/>"
               + "<input id ='delete' type='button' class='btn btn-default' value='Delete Profile'><hr/>";
        }
    }
}