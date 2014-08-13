using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class QMasterPage : System.Web.UI.MasterPage
{
    public Quser CurrUser;
    public Portfolio Portfolio { get; set; }
    public List<LastShareData> LastData { get; set; }
    public TotalData Totals { get; set; }
    public DateTime RequestDT { get; set; }
    public DateTime NextUpdateDT;

    public void GenerateWatchList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("DateAdded");
        dt.Columns.Add("Symbol");
        dt.Columns.Add("ShareName");
        dt.Columns.Add("OrigPrice");
        dt.Columns.Add("LastPrice");
        dt.Columns.Add("PriceChange");
        dt.Columns.Add("DateTimeLast");

        foreach(LastShareData last in LastData)
        {
            WatchItem item = null;
            Portfolio.WatchList.TryGetValue(last.Symbol, out item);
            if (item != null)
            {
                DataRow row = dt.NewRow();
                DateTime DateAdded = Convert.ToDateTime(item.WatchDate);
                DateTime DateNow = DateTime.UtcNow;
                int age = (DateNow.Year - DateAdded.Year) * 12 + (DateNow.Month - DateAdded.Month);
                row["DateAdded"] = String.Format("{0}<br/>{1} months", item.WatchDate, age);
                row["Symbol"] = last.Symbol;
                row["ShareName"] = last.ShareName;
                row["OrigPrice"] = item.Price.ToString("C2");
                row["LastPrice"] = last.LastPrice.ToString("C2");
                row["PriceChange"] = FormatUtils.CombiCellStr(last.LastPrice - item.Price,
                    100 * (last.LastPrice - item.Price) / item.Price);
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
        //dt.Rows[1].Visible = CurrUser.ShowStockNames;
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
            if (!Portfolio.Shares.Keys.Contains(symbol))
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
            //tmp += quoteline;
            string[] quotecontent = quoteline.Split(',');
            if (quotecontent.Length > 4 && quotecontent[2] != "N/A")
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
        Portfolio.ReadPortfolio(CurrUser);
        Portfolio.ReadWatch(CurrUser);

        GetQuotes();
        GenerateView();
        if (CurrUser.ShowWatchlist)
        {
            GenerateWatchList();
        }
    }

    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = CurrUser.ShowStockNames;
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "TOTAL:";
            e.Row.Cells[4].Text = Totals.LastValue.ToString("C2");
            e.Row.Cells[6].Text = FormatUtils.CurrencyStr(Totals.LastChange);
            e.Row.Cells[7].Text = FormatUtils.CombiCellStr(Totals.LastValue - Totals.AckValue,
                       100 * (Totals.LastValue - Totals.AckValue) / Totals.AckValue);
        }
        if (e.Row.RowType == DataControlRowType.Footer || e.Row.RowType == DataControlRowType.DataRow)
        {
            FormatUtils.ColorizeCell(e.Row, 5);
            FormatUtils.ColorizeCell(e.Row, 6);
            FormatUtils.ColorizeCell(e.Row, 7);
        }
    }
    protected void gvWatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            FormatUtils.ColorizeCell(e.Row, 5);
        }
    }
}
