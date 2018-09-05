using System;
using System.Collections.Generic;
using System.Text;

namespace SourcePoint.Core.Helpers.Excel
{
    public class Excel<TEntity>
    {
        public Excel() { }
        public Excel(int row, int col, int toRow, int toCol, string megeName) : this(row, col, null, ExcelType.Header)
        {
            this.ToRow = toRow;
            this.ToCol = toCol;
            this.Merge = true;
            this.MergeName = megeName;
        }
        public Excel(int row, int col, string[] header, ExcelType type = ExcelType.Header)
        {
            this.Row = row;
            this.Col = col;
            this.Type = type;
            this.Headers = header;
        }
        /// <summary>
        /// sheet名称
        /// </summary>
        public string SheetName { get; set; }
        public int Row { get; set; } = 1;
        public int Col { get; set; } = 1;
        public int ToRow { get; set; } = 1;
        public int ToCol { get; set; } = 1;
        public bool Merge { get; set; }
        public string MergeName { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        public List<TEntity> Data { get; set; } = new List<TEntity>();
        public ExcelType Type { get; set; }
        /// <summary>
        /// 表头
        /// </summary>
        public string[] Headers { get; set; }
    }
}
