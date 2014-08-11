using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Quser
/// </summary>
public class Quser
{
    public String Username { get; set; }
    public String Greeting { get; set; }
    public bool ShowWatchlist = true;
    public bool ShowStockNames = true;

	public Quser(String username)
	{
        Username = username;

		//
		// TODO: Add constructor logic here
		//
	}
}