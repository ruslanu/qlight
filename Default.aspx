<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .grid
        {
            font:16px Arial;
        }
        .grid td, .grid th
        {
            padding:10px;
        }
        .header
        {
            text-align:center;
            color:white;
            background-color:blue;
            font:13px Arial;
        }
        .row td
        {
            border:solid 1px blue;
            font:13px Arial;
            padding-left:6px;
            padding-right:6px;
        }
        .alternating
        {
            background-color:#eeeeee;
        }
        .alternating td
        {
            border:solid 1px blue;
            font:13px Arial;
            padding-left:6px;
            padding-right:6px;
        }
        .footer td
        {
            text-align:center;
            background-color: ivory;
            font:bold 13px Arial;
            border:solid 1px blue;
        }
        .TAPad
        {
            text-align:left;
        }
    </style>
    <title> Stocks List</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">  
    </asp:ScriptManager>
    <div>
    <i>Hello, friends</i><br/>
       <!-- Please use http://ruslanu.dlinkddns.com/friends<br/> -->
        -----------------<br/><br />
             <!--    <asp:GridView ID="GridView2" runat="server" >
        </asp:GridView> -->

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Timer ID="Timer1" runat="server" Interval="900000">
        </asp:Timer>
        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="false"
            HeaderStyle-CssClass="header" RowStyle-CssClass="row"
            AlternatingRowStyle-CssClass="alternating" FooterStyle-CssClass="footer"
            ShowFooter="true" OnRowDataBound="GridView3_RowDataBound" RowStyle-HorizontalAlign="Center" >
            <Columns>
                <asp:BoundField DataField="Symbol" HeaderText="Symbol" />
                <asp:BoundField DataField="ShareName" HeaderText="Name" ItemStyle-CssClass="TAPad" Visible="true" />
                <asp:BoundField DataField="NumShares" HeaderText="Shares" />             
                <asp:BoundField DataField="LastPrice" HeaderText="Last" DataFormatString="{0:C2}"/>
                <asp:BoundField DataField="LastValue" HeaderText="Value" DataFormatString="{0:C2}"/>
                <asp:BoundField DataField="LastPriceChange" HeaderText="Change" DataFormatString="{0:C2}"
                    HtmlEncode="false"  />
                <asp:BoundField DataField="LastValChange" HeaderText="Day Value" />
                <asp:BoundField DataField="UnrealizedGain" HeaderText="Unrealized<br/>Gain/Loss"
                    HtmlEncode="false" />                
                <asp:BoundField DataField="DateTimeLast" HeaderText="Last reported" />
            </Columns>
        </asp:GridView>
            <br />
            Last refresh: <%= RequestDT %><br />
            Next refresh: <%= NextUpdateDT %><br/><br/><br />
            WatchList<br />
            ----------------<br />
        <asp:GridView ID="gvWatch" runat="server" AutoGenerateColumns="false"
            HeaderStyle-CssClass="header" RowStyle-CssClass="row"
            AlternatingRowStyle-CssClass="alternating" FooterStyle-CssClass="footer"
            ShowFooter="false" RowStyle-HorizontalAlign="Center" >
            <Columns>
                <asp:BoundField DataField="DateAdded" HeaderText="Date Added" />
                <asp:BoundField DataField="Symbol" HeaderText="Symbol" />
                <asp:BoundField DataField="ShareName" HeaderText="Name" ItemStyle-CssClass="TAPad" />
                <asp:BoundField DataField="OrigPrice" HeaderText="Price Orig" DataFormatString="{0:C2}"/>
                <asp:BoundField DataField="LastPrice" HeaderText="Last" DataFormatString="{0:C2}"/>
                <asp:BoundField DataField="DateTimeLast" HeaderText="Last reported" />
            </Columns>
        </asp:GridView>    
        <br/><br/>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <table border="1">
            <tr class="header">
                <td>To do list:</td>
                <td>Done:</td>
            </tr>
            <tr class="row">
                <td>
                    - Colored price deltas in Watch List<br />
                </td>
                <td>
                    + Switched to nice fonts<br />
                    + Add totals to table<br/>
                    + Add green/red coloring for changes<br/>
                    + Added time of last refresh<br />
                    + Price and Percent change combined to one row<br />
                    + Autorefresh<br />
                    + Add company names<br/>
                    + WatchList<br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
