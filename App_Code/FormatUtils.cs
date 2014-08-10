using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for FormatUtils
/// </summary>
public static class FormatUtils
{
    public static string CurrencyStr(float val)
    {
        return string.Format("{0:S}${1:F2}", (val >= 0 ? "+" : "-"), Math.Abs(val));
    }

    public static string PercentStr(float val)
    {
        return string.Format("{0:S}{1:F2}%", (val >= 0 ? "+" : "-"), Math.Abs(val));
    }

    public static string CombiCellStr(float val, float percent)
    {
        return string.Format("{0}<br/>{1}", CurrencyStr(val), PercentStr(percent));
    }

    public static void ColorizeCell(GridViewRow row, int num)
    {
        if (row.Cells.Count > num)
        {
            TableCell cell = row.Cells[num];
            cell.ForeColor =
                (!string.IsNullOrEmpty(cell.Text) && cell.Text[0] == '-') ?
                                                                Color.Red : Color.Green;
        }
    }
}