using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class UserProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Workers recv;
        if (Session["user"] != null)
        {
            recv = (Workers)Session["user"];
            Response.Write("<script>console.log('" + recv.FirstName + "');</script>");
            Response.Write("<script>localStorage.setItem('job', '"+ recv.CompanyName +"');</script>");
            personal.InnerHtml = "<div>Name: " + recv.FirstName + " " + recv.LastName + "</div><hr/>" +
                "<div>Email: " + recv.Email + "</div><hr/>" +
                "<div id='cname'>Company: " + recv.CompanyName + "</div>";
        }
    }
}