﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class companyInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["idsc"] != null)
        {
            Response.Write("<script>localStorage.setItem('idsc', '" + Session["idsc"] + "');</script>");
        }
    }
}