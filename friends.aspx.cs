﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class friends : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.CurrUser = new Quser("friends");
        Master.CurrUser.Greeting = "Hello, friends!";
    }
}