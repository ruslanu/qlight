using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

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

public class Portfolio
{
    public Dictionary<string, Stake> Shares = new Dictionary<string, Stake>();

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
        if (Item == null)
        {
            Item = new WatchItem();
            WatchList.Add(symbol, Item);
        }
        Item.Price = Price;
        Item.WatchDate = WatchDate;
    }

    public void ReadPortfolio(Quser curruser, XDocument xmlDoc)
    {
        Shares = new Dictionary<string, Stake>();

        foreach (XElement port in xmlDoc.Element("doc").Elements("portfolio"))
        {
            if (curruser.Username == "rusel" || curruser.Username == (string)port.Attribute("user"))
            {
                foreach (XElement elem in port.Elements("purchase"))
                {
                    AddPurchase((string)elem.Element("symbol"), (float)elem.Element("shares"), (float)elem.Element("price"));
                }
            }
        }
    }

    public void ReadWatch(Quser curruser, XDocument xmlDoc)
    {
        WatchList = new Dictionary<string, WatchItem>();

        foreach (XElement wl in xmlDoc.Element("doc").Elements("watchlist"))
        {
            if (curruser.Username == (string)wl.Attribute("user"))
            {
                foreach (XElement elem in wl.Elements("watch"))
                {
                    AddWatch((string)elem.Element("symbol"), (float)elem.Element("price"), (string)elem.Element("date"));
                }
                break;
            }
        }
    }
}

public class TotalData
{
    public float AckValue;
    public float LastValue;
    public float LastChange;
}

