using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using Newtonsoft.Json;

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
        Companies recc;
        if (Session["company"] != null)
        {
            recc = (Companies)Session["company"];
            Response.Write("<script>console.log('" + recc.CompanyName + "');</script>");
            Response.Write("<script>localStorage.setItem('job', '" + null + "');</script>");
            List<ObjectId> objects = new List<ObjectId>();
            if (recc.Employees != null)
            {
                for (int j = 0; j < recc.Employees.Length; j++)
                {
                    objects.Add(recc.Employees[j]);
                }
                Response.Write("<script>localStorage.setItem('firm', '" + JsonConvert.SerializeObject(objects) + "');</script>");
            }
            personal.InnerHtml = "<div>Company Name: " + recc.CompanyName + "</div><hr/>" +
                    "<div>Email: " + recc.Email + "</div><hr/>" +
                    "<div>Owner: " + recc.Owner + "</div><hr/>" +
                    "<div>Type: " + recc.Type + "</div><hr/>" +
                    "<div>Location: " + recc.Location + "</div>";
        }
    }
}