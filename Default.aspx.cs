using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



[Serializable]
public class Purchase
{
    //public string Symbol { get; set; }
    public float NumShares { get; set; }
    public float Price { get; set; }
    public string AcquisitionDate { get; set; }
}

public class LastShareData
{
    public string Symbol { get; set; }
    public string ShareName { get; set; }
    public float LastPrice;
    public float PriceChange;
    public DateTime RepDT { get; set; }
}

[Serializable]
public class Stake
{
    public float TotShares { get; set; }
    public float AvgPrice { get; set; }
    public List<Purchase> Orders { get; set; }

    public void AddPurchase(float num, float price)
    {
        Purchase p = new Purchase();
        p.NumShares = num;
        p.Price = price;

        float amount = TotShares * AvgPrice + num * price;
        TotShares += num;
        AvgPrice = amount / TotShares;
    }
}

public class WatchItem
{
    public float Price;
    public string WatchDate;
}

[Serializable]
public class Portfolio
{
    public Dictionary<string, Stake> Shares = new Dictionary<string,Stake>();

    public Dictionary<string, WatchItem> WatchList = new Dictionary<string, WatchItem>();

    void AddStake(string symbol, float num, float price)
    {
        Stake s = new Stake();
        Purchase p = new Purchase();
        p.NumShares = num;
        p.Price = price;
        s.Orders = new List<Purchase>();
        s.Orders.Add(p);
        s.TotShares = p.NumShares;
        s.AvgPrice = p.Price;
        Shares.Add(symbol, s);
    }

    void AddPurchase(string symbol, float num, float price)
    {
        Stake Stake = null;
        Shares.TryGetValue(symbol, out Stake);
        if (Stake == null)
        {
            Stake = new Stake();
            Shares.Add(symbol, Stake);
        }
        Stake.AddPurchase(num, price);
    }

    void AddWatch(string symbol, float Price, string WatchDate)
    {
        WatchItem Item = null;
        WatchList.TryGetValue(symbol, out Item);
        if(Item == null)
        {
            Item = new WatchItem();
            WatchList.Add(symbol, Item);
        }
        Item.Price = Price;
        Item.WatchDate = WatchDate;
    }

    public void ReadPortfolio()
    {
        Shares = new Dictionary<string, Stake>();

        // Friends
        AddPurchase("HLF", 50, 38.48F); // 04/12/2013
        AddPurchase("MSFT", 20, 31.39F); // 09/04/2013
        AddPurchase("MSFT", 60, 31.2299F); // 09/04/2013        
        AddPurchase("MSFT", 20, 32.69F); // 09/24/2013
        AddPurchase("EA", 50, 23.65F); // 12/23/2013
        AddPurchase("ATVI", 50, 17.95F); // 12/23/2013
        AddPurchase("TTWO", 50, 17.61F); // 12/23/2013
        AddPurchase("DIS", 10, 71.45F); // 12/09/2013
        AddPurchase("ORCL", 75, 41.00F); // 07/07/2014

        ///*
        // Ruslanu
        AddPurchase("RSH", 100, 2.39F); // 01/08/2014
        AddPurchase("ORCL", 25, 41.00F); // 07/07/2014
        AddPurchase("SYMC", 100, 22.65F); // 07/14/2014
        AddPurchase("F", 200, 17.65F); // 07/21/2014
        AddPurchase("HLF", 50, 54.27F); // 07/22/2014
        AddPurchase("BA", 10, 121.07F); // 08/04/2014
        AddPurchase("MIDD", 15, 72.24F); // 08/04/2014
        AddPurchase("RBL", 40, 25.96F); // 06/09/2014
        AddPurchase("RSXJ", 40, 35.76F); // 06/09/2014

        //Mama
        AddPurchase("CHK", 100, 20.91F); // 06/17/2013
        AddPurchase("RBL", 20, 25.33F); // 01/28/2014
        AddPurchase("RBL", 20, 25.57F); // 02/18/2014
        AddPurchase("RBL", 20, 22.10F); // 05/02/2014
        AddPurchase("RSXJ", 20, 38.14F); // 01/28/2014
        AddPurchase("RSXJ", 20, 37.77F); // 02/18/2014
        AddPurchase("RSXJ", 20, 30.42F); // 05/02/2014
        AddPurchase("VOO", 33, 76.56F * 2); // 07/11/2013
        AddPurchase("VOO", 25, 172.58F); // 05/08/2014
        AddPurchase("VEA", 100, 41.30F); // 11/18/2013
        AddPurchase("VEA", 100, 41.68F); // 04/29/2014
        AddPurchase("VEA", 100, 42.56F); // 06/04/2014
        AddPurchase("VEA", 100, 42.715F); // 07/07/2014
        AddPurchase("VWO", 100, 42.15F); // 11/18/2013
        AddPurchase("VWO", 100, 42.56F); // 05/30/2014
        AddPurchase("VWO", 100, 45.045F); // 07/24/2014

        // Roth IRA
        AddPurchase("VOO", 23, 168.67F); // 12/27/2013
        AddPurchase("VOO", 1, 181.08F); // 07/17/2014
        AddPurchase("VEA", 48, 37.91F); // 09/04/2013
        AddPurchase("VEA", 1, 39.48F); // 09/30/2013
        AddPurchase("VEA", 1, 41.53F); // 12/30/2013
        AddPurchase("VEA", 50, 40.23F); // 01/27/2014
        AddPurchase("VWO", 100, 38.14F); // 09/04/2013
        
        //*/

    }

    public void ReadWatch()
    {
        WatchList = new Dictionary<string, WatchItem>();

        AddWatch("F", 15.29F, "01/01/2014");
        AddWatch("INTC", 25.38F, "01/20/2014");
        AddWatch("GLCNF", 5.24F, "01/14/2014");
        AddWatch("GLNCY", 10.36F, "01/14/2014");
        AddWatch("RBL", 26.57F, "01/21/2014");
        AddWatch("RSXJ", 39.94F, "01/21/2014");
    }
}

public class TotalData
{
    public float AckValue;
    public float LastValue;
    public float LastChange;
}

public partial class _Default : System.Web.UI.Page
{
    public Portfolio Portfolio { get; set; }
    public List<LastShareData> LastData { get; set; }
    public TotalData Totals { get; set; }
    public DateTime RequestDT { get; set; }
    public DateTime NextUpdateDT;

    public string tmp;

    public void GenerateWatchList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DateAdded");
        dt.Columns.Add("Symbol");
        dt.Columns.Add("ShareName");
        dt.Columns.Add("OrigPrice");
        dt.Columns.Add("LastPrice");
        dt.Columns.Add("DateTimeLast");

        foreach(LastShareData last in LastData)
        {
            WatchItem item = null;
            Portfolio.WatchList.TryGetValue(last.Symbol, out item);
            if (item != null)
            {
                DataRow row = dt.NewRow();
                row["DateAdded"] = item.WatchDate;
                row["Symbol"] = last.Symbol;
                row["ShareName"] = last.ShareName;
                row["OrigPrice"] = item.Price.ToString("C2");
                row["LastPrice"] = last.LastPrice.ToString("C2");
                row["DateTimeLast"] = last.RepDT.ToString();

                dt.Rows.Add(row);
            }
        }

        gvWatch.DataSource = dt;
        gvWatch.DataBind();
    }

    public void GenerateView()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Symbol");
        dt.Columns.Add("ShareName");
        dt.Columns.Add("NumShares");
        dt.Columns.Add("LastPrice");
        dt.Columns.Add("LastValue");
        dt.Columns.Add("LastPriceChange");
        dt.Columns.Add("LastValChange");
        dt.Columns.Add("UnrealizedGain");
        dt.Columns.Add("DateTimeLast");

        Totals = new TotalData();

        foreach(LastShareData last in LastData)
        {
            Stake s = null;
            Portfolio.Shares.TryGetValue(last.Symbol, out s);
            if (s != null)
            {
                float acqval = s.TotShares * s.AvgPrice;
                float lastval = s.TotShares * last.LastPrice;
                float lastvalchange = s.TotShares * last.PriceChange;

                Totals.AckValue += acqval;
                Totals.LastValue += lastval;
                Totals.LastChange += lastvalchange;

                DataRow row = dt.NewRow();
                row["Symbol"] = last.Symbol;
                row["ShareName"] = last.ShareName;
                row["NumShares"] = s.TotShares;
                row["LastPrice"] = last.LastPrice.ToString("C2");
                row["LastValue"] = lastval.ToString("C2");
                row["LastPriceChange"] = FormatUtils.CombiCellStr(last.PriceChange,
                                     100 * last.PriceChange / (last.LastPrice - last.PriceChange));
                row["LastValChange"] = FormatUtils.CurrencyStr(lastvalchange);
                row["UnrealizedGain"] = FormatUtils.CombiCellStr(lastval - acqval,
                                                                 100 * (lastval - acqval) / acqval);
                row["DateTimeLast"] = last.RepDT.ToString();

                dt.Rows.Add(row);
            }
        }

        GridView3.DataSource = dt;
        GridView3.DataBind();
    }

    private void GetQuotes()
    {
        StringBuilder sb = new StringBuilder(@"http://download.finance.yahoo.com/d/quotes.csv?s=");
        foreach (string symbol in Portfolio.Shares.Keys)
        {
            sb.Append(symbol);
            sb.Append(',');
        }
        foreach (string symbol in Portfolio.WatchList.Keys)
        {
            if(!Portfolio.Shares.Keys.Contains(symbol))
            {
               sb.Append(symbol);
               sb.Append(',');
            }
        }
        sb.Append("&f=sl1d1t1c1n");

        // Initialize a new WebRequest.
        System.Net.HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(sb.ToString());
        // Get the response from the Internet resource.
        System.Net.HttpWebResponse webresp = (HttpWebResponse)webreq.GetResponse();
        // Read the body of the response from the server.
        System.IO.StreamReader strm =
          new StreamReader(webresp.GetResponseStream(), Encoding.ASCII);

        LastData = new List<LastShareData>();
        while (strm.Peek() >= 0)
        {
            string quoteline = strm.ReadLine().Replace("\"", "");
            tmp += quoteline;
            string[] quotecontent = quoteline.Split(',');
            if (quotecontent.Length > 4 &&   quotecontent[2] != "N/A")
            {
                LastShareData share = new LastShareData();

                share.Symbol = quotecontent[0];

                float.TryParse(quotecontent[1], out share.LastPrice);

                share.RepDT = Convert.ToDateTime(quotecontent[2] + ' ' + quotecontent[3]);

                float.TryParse(quotecontent[4], out share.PriceChange);

                share.ShareName = quotecontent[5];

                LastData.Add(share); 
            }
        }


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        RequestDT = NextUpdate.ConvertToEST(DateTime.UtcNow);
        NextUpdateDT = NextUpdate.GetNextUpdateDT();
        Timer1.Interval = Convert.ToInt32((NextUpdateDT - RequestDT).TotalMilliseconds);

        Portfolio = new Portfolio();
        Portfolio.ReadPortfolio();
        Portfolio.ReadWatch();

        GetQuotes();
        GenerateView();
        GenerateWatchList();
    }


    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "TOTAL:";
            e.Row.Cells[4].Text = Totals.LastValue.ToString("C2");
            e.Row.Cells[6].Text = FormatUtils.CurrencyStr(Totals.LastChange);
            e.Row.Cells[7].Text = FormatUtils.CombiCellStr(Totals.LastValue - Totals.AckValue,
                       100 * (Totals.LastValue - Totals.AckValue) / Totals.AckValue);   
        }
        if(e.Row.RowType == DataControlRowType.Footer || e.Row.RowType == DataControlRowType.DataRow)
        {
            FormatUtils.ColorizeCell(e.Row, 5);
            FormatUtils.ColorizeCell(e.Row, 6);
            FormatUtils.ColorizeCell(e.Row, 7);
        }
    }
}

// Comments
/*
        <!--
            <asp:BoundField DataField="Value.UnrealizedGain" HeaderText="Gain/Loss" />
            <asp:BoundField DataField="Value.TotalCost" HeaderText="Cost" DataFormatString="{0:C2}"/>
            <asp:BoundField DataField="Value.TotalValue" HeaderText="Value" DataFormatString="{0:C2}"/>
                            <asp:BoundField DataField="TotalShares" HeaderText="Shares" ItemStyle-Width="15%" />

                <asp:BoundField DataField="Value.TotalValue" HeaderText="Base Price" DataFormatString="{0:C2}"/>
                            <asp:BoundField DataField="AquisitionDate" HeaderText="Held Since"
                    DataFormatString="{0:d}"/>
            <asp:BoundField DataField="PercentGain" HeaderText="% G/L" />
                            <asp:BoundField DataField="LastPerChange" HeaderText="% Change" />
                    < %= tmp  %>
                    < %= "Total Day Value Change: " + Totals.LastChange + " ( " + " " + " )" %><br/>
        < %= "Total Unrealized Gain/Loss: " + " " + " ( " + " " + " )" %><br/>
            -->
 * 
 *         <br/>       
        Last refreshed: <%= RequestDT %><br/><br/><br/><br/>
*/