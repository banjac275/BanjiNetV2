using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class fellowworker : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userR"] != null)
        {
            WorkersR recR = (WorkersR)Session["userR"];
            Response.Write("<script>localStorage.setItem('userTemp', '" + JsonConvert.SerializeObject(recR) + "');</script>");
            string recvR = (string)Session["workerR"];
            if (recvR != null)
            {
                Response.Write("<script>localStorage.setItem('workerViewR', '" + recvR + "');</script>");
                Session["workerR"] = null;
            }
        }

        if (Session["user"] != null)
        {
            Workers recR = (Workers)Session["user"];
            Response.Write("<script>localStorage.setItem('userTempM', '" + JsonConvert.SerializeObject(recR) + "');</script>");
            string reccR = (string)Session["worker"];
            if (reccR != null)
            {
                Response.Write("<script>localStorage.setItem('workerView', '" + reccR + "');</script>");
                Session["worker"] = null;
            }
        }

        if (Session["idsc"] != null)
        {
            Response.Write("<script>localStorage.setItem('idsc', '" + Session["idsc"] + "');</script>");
        }

        if (Session["ids"] != null)
        {
            Response.Write("<script>localStorage.setItem('ids', '" + Session["ids"] + "');</script>");
        }
    }
}