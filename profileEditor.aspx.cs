﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class profileEditor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["database"] != null)
        {
            Response.Write("<script>localStorage.setItem('dbres', '" + Session["database"] + "');</script>");
        }

        if (Session["ids"] != null)
        {
            Response.Write("<script>localStorage.setItem('ids', '" + Session["ids"] + "');</script>");
        }

        if (Session["idsc"] != null)
        {
            Response.Write("<script>localStorage.setItem('idsc', '" + Session["idsc"] + "');</script>");
        }

        Workers recv;
        if (Session["user"] != null)
        {
            recv = (Workers)Session["user"];
            Response.Write("<script>console.log('" + recv.FirstName + "');</script>");
            Response.Write("<script>localStorage.setItem('userid', '" + recv.Id + "');</script>");
            personalp.InnerHtml = "<form id = 'edit_form' class='form-vertical' role='form' method='post' data-toggle='validator'>"
                   +"<div class='form-group'>"
                        +"<label for='email' class='control-label'>Your email address: </label>"
                        + "<input type = 'email' name='emails' class='form-control' id='email' value='" + recv.Email + "'>"
                   + "<div class='help-block with-errors'></div>"
                   +"</div>"
                   +"<div class='form-group'>"
                       +"<label for='firstname' class='control-label'>Your first name: </label>"
                       +"<input type = 'text' name='firstname' class='form-control' id='firstname' data-minlength='2' value='"+ recv.FirstName +"'>"
                       +"<div class='help-block with-errors'></div>"
                   +"</div>"
                   +"<div class='form-group'>"
                       +"<label for='lastname' class='control-label'>Your last name: </label>"
                       + "<input type = 'text' name='lastname' class='form-control' id='lastname' data-minlength='2' value='" + recv.LastName + "'>"
                       + "<div class='help-block with-errors'></div>"
                   +"</div>"
                   + "<div class='form-group'>"
                       + "<label for='company' class='control-label'>Your Company: </label>"
                       + "<input type = 'text' name='company' class='form-control' id='firm' data-minlength='2' value='" + recv.CompanyName + "'>"
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
                   + "<div class='form-group' id='former'>"
                       + "<input id ='addformer' name = 'addformer' type='button' class='btn btn-default text-center' value='Add Former Employment'><hr/>"
                       + "</div>"
                   + "<div class='form-group' id='skil'>"
                       + "<input id ='addskil' name = 'addskil' type='button' class='btn btn-default text-center' value='Add Skill'><hr/>"
                       + "</div>"
                   + "<div class='form-group'>"
                       + "<div class='btn-group col-lg-12' data-toggle='buttons'>"
                            + "<label for='dbchoise' class='control-label col-lg-12'>Choose a database: </label>"
                            + "<div class='col-lg-4'></div>"
                            + "<label class='btn btn-primary' id='lab1'>"
                                    + "<input type = 'radio' name='dbchoise' value='raven' id='raven' autocomplete='off'> Raven (default)"
                            + "</label>"
                            + "<label class='btn btn-primary' id='lab2'>"
                                + "<input type = 'radio' name='dbchoise' value='mongo' id='mongo' autocomplete='off'> Mongo"
                       + "</label></div>"
                       + "<div class='col-lg-12'>"
                            +"<div class='checkbox form-inline'>"
                                +"<label>"
                                    + "<input type ='checkbox' name ='remember' id ='remember' checked data-toggle='toggle'> Remember me"
                                 + "</label>"     
                            + "</div><hr/>"
                            + "<input id ='change' type='button' class='btn btn-default' value='Change values'>"
                       + "</div>"
                   + "</div>"
               + "</form><hr class='col-lg-12' />"
               + "<input id ='delete' type='button' class='btn btn-default' value='Delete Profile'><hr/>";
        }

        Companies recvC;
        if (Session["company"] != null)
        {
            recvC = (Companies)Session["company"];
            Response.Write("<script>console.log('" + recvC.CompanyName + "');</script>");
            Response.Write("<script>localStorage.setItem('companyid', '" + recvC.Id + "');</script>");
            personalp.InnerHtml = "<form id = 'edit_form' class='form-vertical' role='form' method='post' data-toggle='validator'>"
                   + "<div class='form-group'>"
                        + "<label for='email' class='control-label'>Your email address: </label>"
                        + "<input type = 'email' name='emails' class='form-control' id='emails' value='" + recvC.Email + "'>"
                   + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='companyname' class='control-label'>Your Company name: </label>"
                       + "<input type = 'text' name='companyname' class='form-control' id='companyname' data-minlength='2' value='" + recvC.CompanyName + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='owner' class='control-label'>Company Owner: </label>"
                       + "<input type = 'text' name='owner' class='form-control' id='owner' data-minlength='2' value='" + recvC.Owner + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='type' class='control-label'>Company type: </label>"
                       + "<input type = 'text' name='type' class='form-control' id='type' data-minlength='2' value='" + recvC.Type + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='location' class='control-label'>Company type: </label>"
                       + "<input type = 'text' name='location' class='form-control' id='location' data-minlength='2' value='" + recvC.Location + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='password' class='control-label'>Choose a password: </label>"
                       + "<input type = 'text' name='passwords' class='form-control' id='passwords' data-minlength='5' value='" + recvC.Password + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='password' class='control-label'>Confirm a password: </label>"
                       + "<input type ='text' name='passwords' class='form-control' id='repeatedpwds' data-minlength='5' data-match='#password' required>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<div class='btn-group col-lg-12' data-toggle='buttons'>"
                            + "<label for='dbchoise' class='control-label col-lg-12'>Choose a database: </label>"
                            + "<div class='col-lg-4'></div>"
                            + "<label class='btn btn-primary' id='lab1'>"
                                    + "<input type = 'radio' name='dbchoise' value='raven' id='raven' autocomplete='off'> Raven (default)"
                            + "</label>"
                            + "<label class='btn btn-primary' id='lab2'>"
                                + "<input type = 'radio' name='dbchoise' value='mongo' id='mongo' autocomplete='off'> Mongo"
                       + "</label></div>"
                       + "<div class='col-lg-12'>"
                            + "<div class='checkbox form-inline'>"
                                + "<label>"
                                    + "<input type ='checkbox' name ='remembers' id ='remembers' checked data-toggle='toggle'> Remember me"
                                 + "</label>"
                            + "</div><hr/>"
                            + "<input id ='changeCom' type='button' class='btn btn-default' value='Change values'>"
                       + "</div>"
                   + "</div>"
               + "</form><hr class='col-lg-12' />"
               + "<input id ='deleteCom' type='button' class='btn btn-default' value='Delete Company Profile'><hr/>";
        }

        WorkersR recR;
        if (Session["userR"] != null)
        {
            recR = (WorkersR)Session["userR"];
            Response.Write("<script>console.log('" + recR.FirstName + "');</script>");
            Response.Write("<script>localStorage.setItem('userid', '" + recR.Id + "');</script>");
            personalp.InnerHtml = "<form id = 'edit_form' class='form-vertical' role='form' method='post' data-toggle='validator'>"
                   + "<div class='form-group'>"
                        + "<label for='email' class='control-label'>Your email address: </label>"
                        + "<input type = 'email' name='emails' class='form-control' id='email' value='" + recR.Email + "'>"
                   + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='firstname' class='control-label'>Your first name: </label>"
                       + "<input type = 'text' name='firstname' class='form-control' id='firstname' data-minlength='2' value='" + recR.FirstName + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='lastname' class='control-label'>Your last name: </label>"
                       + "<input type = 'text' name='lastname' class='form-control' id='lastname' data-minlength='2' value='" + recR.LastName + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='company' class='control-label'>Your Company: </label>"
                       + "<input type = 'text' name='company' class='form-control' id='firm' data-minlength='2' value='" + recR.CompanyName + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='password' class='control-label'>Choose a password: </label>"
                       + "<input type = 'text' name='passwords' class='form-control' id='password' data-minlength='5' value='" + recR.Password + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='password' class='control-label'>Confirm a password: </label>"
                       + "<input type ='text' name='passwords' class='form-control' id='repeatedpwd' data-minlength='5' data-match='#password' required>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div><hr/>"
                   + "<div class='form-group' id='former'>"
                       + "<input id ='addformer' name = 'addformer' type='button' class='btn btn-default text-center' value='Add Former Employment'><hr/>"
                       + "</div>"
                   + "<div class='form-group' id='skil'>"
                       + "<input id ='addskil' name = 'addskil' type='button' class='btn btn-default text-center' value='Add Skill'><hr/>"
                       + "</div>"
                   + "<div class='form-group'>"
                        + "<div class='btn-group col-lg-12' data-toggle='buttons'>"
                            + "<label for='dbchoise' class='control-label col-lg-12'>Choose a database: </label>"
                            + "<div class='col-lg-4'></div>"
                            + "<label class='btn btn-primary' id='lab1'>"
                                    + "<input type = 'radio' name='dbchoise' value='raven' id='raven' autocomplete='off'> Raven (default)"
                            + "</label>"
                            + "<label class='btn btn-primary' id='lab2'>"
                                + "<input type = 'radio' name='dbchoise' value='mongo' id='mongo' autocomplete='off'> Mongo"
                       + "</label></div>"
                       + "<div class='col-lg-12'>"
                            + "<div class='checkbox form-inline'>"
                                + "<label>"
                                    + "<input type ='checkbox' name ='remember' id ='remember' checked data-toggle='toggle'> Remember me"
                                 + "</label>"
                            + "</div><hr/>"
                            + "<input id ='change' type='button' class='btn btn-default' value='Change values'>"
                       + "</div>"
                   + "</div>"
               + "</form><hr class='col-lg-12' />"
               + "<input id ='delete' type='button' class='btn btn-default' value='Delete Profile'><hr/>";
        }

        CompaniesR reccC;
        if (Session["companyR"] != null)
        {
            reccC = (CompaniesR)Session["companyR"];
            Response.Write("<script>console.log('" + reccC.CompanyName + "');</script>");
            Response.Write("<script>localStorage.setItem('companyid', '" + reccC.Id + "');</script>");
            personalp.InnerHtml = "<form id = 'edit_form' class='form-vertical' role='form' method='post' data-toggle='validator'>"
                   + "<div class='form-group'>"
                        + "<label for='email' class='control-label'>Your email address: </label>"
                        + "<input type = 'email' name='emails' class='form-control' id='emails' value='" + reccC.Email + "'>"
                   + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='companyname' class='control-label'>Your Company name: </label>"
                       + "<input type = 'text' name='companyname' class='form-control' id='companyname' data-minlength='2' value='" + reccC.CompanyName + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='owner' class='control-label'>Company Owner: </label>"
                       + "<input type = 'text' name='owner' class='form-control' id='owner' data-minlength='2' value='" + reccC.Owner + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='type' class='control-label'>Company type: </label>"
                       + "<input type = 'text' name='type' class='form-control' id='type' data-minlength='2' value='" + reccC.Type + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='location' class='control-label'>Location: </label>"
                       + "<input type = 'text' name='location' class='form-control' id='location' data-minlength='2' value='" + reccC.Location + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='password' class='control-label'>Choose a password: </label>"
                       + "<input type = 'text' name='passwords' class='form-control' id='passwords' data-minlength='5' value='" + reccC.Password + "'>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<label for='password' class='control-label'>Confirm a password: </label>"
                       + "<input type ='text' name='passwords' class='form-control' id='repeatedpwds' data-minlength='5' data-match='#password' required>"
                       + "<div class='help-block with-errors'></div>"
                   + "</div>"
                   + "<div class='form-group'>"
                       + "<div class='btn-group col-lg-12' data-toggle='buttons'>"
                                + "<label for='dbchoise' class='control-label col-lg-12'>Choose a database: </label>"
                                + "<div class='col-lg-4'></div>"
                                + "<label class='btn btn-primary' id='lab1'>"
                                        + "<input type = 'radio' name='dbchoise' value='raven' id='raven' autocomplete='off'> Raven (default)"
                                + "</label>"
                                + "<label class='btn btn-primary' id='lab2'>"
                                    + "<input type = 'radio' name='dbchoise' value='mongo' id='mongo' autocomplete='off'> Mongo"
                       + "</label></div>"
                       + "<div class='col-lg-12'>"
                            + "<div class='checkbox form-inline'>"
                                + "<label>"
                                    + "<input type ='checkbox' name ='remembers' id ='remembers' checked data-toggle='toggle'> Remember me"
                                 + "</label>"
                            + "</div><hr/>"
                            + "<input id ='changeCom' type='button' class='btn btn-default' value='Change values'>"
                       + "</div>"
                   + "</div>"
               + "</form><hr class='col-lg-12' />"
               + "<input id ='deleteCom' type='button' class='btn btn-default' value='Delete Company Profile'><hr/>";
        }
    }
}