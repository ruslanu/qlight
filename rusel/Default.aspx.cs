using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rusel : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.CurrUser = new Quser("rusel");
        Master.CurrUser.Greeting = "Hello, rusel!";
        Master.CurrUser.ShowWatchlist = false;
        Master.CurrUser.ShowStockNames = false;
    }
}