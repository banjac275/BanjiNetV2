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
        if(Session["database"] != null)
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
            string skillset = null;
            Response.Write("<script>console.log('" + recv.FirstName + "');" +
                "localStorage.setItem('job', '" + recv.CompanyName + "');" +
                "localStorage.setItem('userTemp', '" + JsonConvert.SerializeObject(recv) + "');" +
                "</script>");
            Response.Write("<script>localStorage.setItem('companyCheck', 'faulty');</script>");
            if (recv.Skills != null)
            {
                skillset = String.Join(", ", recv.Skills);
            }
            personal.InnerHtml = "<div>Name: " + recv.FirstName + " " + recv.LastName + "</div><hr/>" +
                "<div>Email: " + recv.Email + "</div><hr/>" +
                "<div id='cname'>Company: " + recv.CompanyName + "</div><hr/>" +
                "<div>Skills: " + skillset + "</div>";

            if (recv.Friends != null)
            {
                Response.Write("<script>localStorage.setItem('friends', '" + JsonConvert.SerializeObject(recv.Friends) + "');</script>");
            }
        }

        Companies recc;
        if (Session["company"] != null)
        {
            recc = (Companies)Session["company"];
            Response.Write("<script>console.log('" + recc.CompanyName + "');</script>");
            Response.Write("<script>localStorage.setItem('companyCheck', 'vraiment');</script>");
            if (recc.Employees != null)
            {
                Response.Write("<script>localStorage.setItem('firm', '" + JsonConvert.SerializeObject(recc.Employees) + "');</script>");
            }
            else
                Response.Write("<script>localStorage.setItem('firm', '1');</script>");
            personal.InnerHtml = "<div>Company Name: " + recc.CompanyName + "</div><hr/>" +
                    "<div>Email: " + recc.Email + "</div><hr/>" +
                    "<div>Owner: " + recc.Owner + "</div><hr/>" +
                    "<div>Type: " + recc.Type + "</div><hr/>" +
                    "<div>Location: " + recc.Location + "</div>";
        }

        WorkersR recR;
        if (Session["userR"] != null)
        {
            recR = (WorkersR)Session["userR"];
            string skillset = null;
            Response.Write("<script>console.log('" + recR.FirstName + "');" +
                "localStorage.setItem('jobR', '" + recR.CompanyName + "');" +
                "localStorage.setItem('userTemp', '" + JsonConvert.SerializeObject(recR) + "');" +
                "</script>");
            Response.Write("<script>localStorage.setItem('companyCheck', 'faulty');</script>");
            if (recR.Skills != null)
            {
                skillset = String.Join(", ", recR.Skills);
            }
            personal.InnerHtml = "<div>Name: " + recR.FirstName + " " + recR.LastName + "</div><hr/>" +
                "<div>Email: " + recR.Email + "</div><hr/>" +
                "<div id='cname'>Company: " + recR.CompanyName + "</div><hr/>" +
                "<div>Skills: " + skillset + "</div>";
            
            if (recR.Friends != null)
            {
                Response.Write("<script>localStorage.setItem('friendsR', '" + JsonConvert.SerializeObject(recR.Friends) + "');</script>");
            }
        }

        CompaniesR reccR;
        if (Session["companyR"] != null)
        {
            reccR = (CompaniesR)Session["companyR"];
            Response.Write("<script>console.log('" + reccR.CompanyName + "');</script>");
            Response.Write("<script>localStorage.setItem('companyCheck', 'vraiment');</script>");
            //List<Guid> objects = new List<Guid>();
            if (reccR.Employees != null)
            {
                Response.Write("<script>localStorage.setItem('firmR', '" + JsonConvert.SerializeObject(reccR.Employees) + "');</script>");
            }
            else
                Response.Write("<script>localStorage.setItem('firmR', '1');</script>");
            personal.InnerHtml = "<div>Company Name: " + reccR.CompanyName + "</div><hr/>" +
                    "<div>Email: " + reccR.Email + "</div><hr/>" +
                    "<div>Owner: " + reccR.Owner + "</div><hr/>" +
                    "<div>Type: " + reccR.Type + "</div><hr/>" +
                    "<div>Location: " + reccR.Location + "</div>";
        }
    }
}