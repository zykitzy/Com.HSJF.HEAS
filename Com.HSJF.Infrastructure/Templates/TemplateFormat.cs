using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Templates
{
    public class TemplateFormat : BaseFormat
    {
        public TemplateFormat()
        {
            this.AllowAutoFilter = true;
            this.AllowDeleteColumns = true;
            this.AllowDeleteRows = true;
            this.AllowEditObject = true;
            this.AllowEditScenarios = true;
            this.AllowFormatCells = true;
            this.AllowFormatColumns = true;
            this.AllowFormatRows = true;
            this.AllowInsertColumns = true;
            this.AllowInsertHyperlinks = true;
            this.AllowInsertRows = true;
            this.AllowPivotTables = true;
            this.AllowSelectLockedCells = true;
            this.AllowSelectUnlockedCells = true;
            this.AllowSort = true;
            this.IsProtected = false;
            this.HiddenColumnHeads = false;
            this.ColumnFormats = new HashSet<ColumnFormat>();
            this.RowFormats = new HashSet<RowFormat>();
            this.CellDataItems = new HashSet<CellData>();
        }

        public bool AllowAutoFilter { get; set; }
        public bool AllowDeleteColumns { get; set; }
        public bool AllowDeleteRows { get; set; }
        public bool AllowEditObject { get; set; }
        public bool AllowEditScenarios { get; set; }
        public bool AllowFormatCells { get; set; }
        public bool AllowFormatColumns { get; set; }
        public bool AllowFormatRows { get; set; }
        public bool AllowInsertColumns { get; set; }
        public bool AllowInsertHyperlinks { get; set; }
        public bool AllowInsertRows { get; set; }
        public bool AllowPivotTables { get; set; }
        public bool AllowSelectLockedCells { get; set; }
        public bool AllowSelectUnlockedCells { get; set; }
        public bool AllowSort { get; set; }
        public bool IsProtected { get; set; }

        public string SheetName { get; set; }
        public bool HiddenColumnHeads { get; set; }

        public ICollection<ColumnFormat> ColumnFormats { get; private set; }
        public ICollection<RowFormat> RowFormats { get; private set; }
        public ICollection<CellData> CellDataItems { get; set; }
    }

    public class ColumnHeadFormat : BaseFormat
    {
        internal ColumnHeadFormat()
        {
            this.BackgroundColor = Color.Silver;
            this.Font = new Font(_defaultFont, FontStyle.Bold);
            this.FontColor = SystemColors.WindowText;
            this.Readonly = true;
        }

        public Color? BackgroundColor { get; set; }
        public Font Font { get; set; }
        public Color FontColor { get; set; }
        public bool Readonly { get; set; }
        public bool HiddenColumn { get; set; }
    }

    public class ColumnFormat : BaseFormat
    {
        public ColumnFormat()
        {
            this.Font = new Font(_defaultFont, FontStyle.Regular);
            this.FontColor = SystemColors.WindowText;
            this.HeadFormat = new ColumnHeadFormat();
        }

        public string ColumnName { get; set; }
        public string Group1Name { get; set; }
        public string Group2Name { get; set; }
        public string Group3Name { get; set; }
        public int ColumnIndex { get; set; }
        public object State { get; set; }

        public Color? BackgroundColor { get; set; }
        public Font Font { get; set; }
        public Color FontColor { get; set; }
        public string Numberformat { get; set; }
        public bool Readonly { get; set; }

        public ColumnHeadFormat HeadFormat { get; set; }
    }

    public class RowFormat : BaseFormat
    {
        public RowFormat()
        {
            this.Font = new Font(_defaultFont, FontStyle.Regular);
            this.FontColor = SystemColors.WindowText;
        }

        public int RowIndex { get; set; }
        public object State { get; set; }

        public Color? BackgroundColor { get; set; }
        public Font Font { get; set; }
        public Color FontColor { get; set; }
        public bool Readonly { get; set; }
        public bool Hidden { get; set; }
        public string Numberformat { get; set; }
    }

    public class CellData : BaseFormat
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public IEnumerable<string> Data { get; set; }
    }
}
