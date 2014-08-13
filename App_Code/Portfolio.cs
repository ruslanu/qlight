using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    public void ReadPortfolio(Quser curruser)
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

        if (curruser.Username == "rusel")
        {
            // Ruslanu
            AddPurchase("RSH", 100, 2.39F); // 01/08/2014
            AddPurchase("ORCL", 25, 41.00F); // 07/07/2014
            AddPurchase("SYMC", 100, 22.65F); // 07/14/2014
            AddPurchase("F", 200, 17.65F); // 07/21/2014
            AddPurchase("HLF", 50, 54.27F); // 07/22/2014
            AddPurchase("BA", 10, 121.07F); // 08/04/2014
            AddPurchase("MIDD", 15, 72.24F); // 08/04/2014
            AddPurchase("CSCO", 50, 25.21F); // 08/12/2014
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

        }
    }

    public void ReadWatch(Quser curruser)
    {
        WatchList = new Dictionary<string, WatchItem>();

        if (curruser.Username != "ruslanu")
        {
            AddWatch("F", 15.29F, "01/01/2014");
            AddWatch("INTC", 25.38F, "01/20/2014");
            AddWatch("GLCNF", 5.24F, "01/14/2014");
            AddWatch("GLNCY", 10.36F, "01/14/2014");
            AddWatch("RBL", 26.57F, "01/21/2014");
            AddWatch("RSXJ", 39.94F, "01/21/2014");
            AddWatch("MHG", 12.56F, "08/08/2014");
        }
    }
}

public class TotalData
{
    public float AckValue;
    public float LastValue;
    public float LastChange;
}

