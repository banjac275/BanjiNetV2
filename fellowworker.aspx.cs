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
        WorkersR recR = (WorkersR)Session["userR"];
        Response.Write("<script>localStorage.setItem('userTemp', '" + JsonConvert.SerializeObject(recR) + "');</script>");
    }
}