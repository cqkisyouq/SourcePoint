using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;

namespace SourcePoint.Core.Helpers.Excel
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public static byte[] ExportExcel<TEntity>(IEnumerable<TEntity> data, string[] headers = null)
        {
            using (ExcelPackage ep = new ExcelPackage())
            {
                ExcelWorksheet ws = ep.Workbook.Worksheets.Add("Sheet1");
                ws.Cells["A1"].LoadFromCollection(data, true, TableStyles.None);
                if (headers != null && headers.Length > 0)
                {
                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = headers[i];
                    }
                }
                return ep.GetAsByteArray();
            }
        }
        /// <summary>
        /// 导出excel多表头
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static byte[] ExportMultiHeader<TEntity, TData>(List<Excel<TEntity>> list, List<Excel<TData>> data, string name = "Sheet1")
        {
            using (ExcelPackage ep = new ExcelPackage())
            {
                ExcelWorksheet ws = ep.Workbook.Worksheets.Add(name);
                ConfigruationReport(ws, list);
                if (data != null) ConfigruationReport(ws, data);
                return ep.GetAsByteArray();
            }
        }
        private static void ConfigruationReport<TEntity>(ExcelWorksheet ws, List<Excel<TEntity>> list)
        {
            foreach (var item in list)
            {
                var row = item.Row;
                var col = item.Col;

                if (item.Type == ExcelType.Data || item.Type == ExcelType.HeaderAndData)
                {
                    if (item.Merge) row = item.ToRow;
                    if (item.Type == ExcelType.HeaderAndData) row++;

                    ws.Cells[row, col].LoadFromCollection(item.Data, false, TableStyles.None);
                }

                row = item.Row;

                if (item.Type == ExcelType.Header || item.Type == ExcelType.HeaderAndData)
                {
                    if (item.Merge)
                    {
                        var cell = ws.Cells[row, col, item.ToRow, item.ToCol];
                        cell.Merge = item.Merge;
                        cell.Value = item.MergeName;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    if (item.Headers != null)
                    {
                        for (int i = 0; i < item.Headers.Length; i++)
                        {
                            var header = ws.Cells[row, col + i];
                            header.Value = item.Headers[i];
                            header.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            header.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        }
                    }
                }
            }
        }

        public static string CreateFileNameFor(string pre, DateTime? start, DateTime? end)
        {
            List<string> names = new List<string>();
            names.Add(pre);
            if (start.HasValue) names.Add(start.Value.ToString("yyyy_MM_dd"));
            if (end.HasValue) names.Add(end.Value.ToString("yyyy_MM_dd"));
            return $"{string.Join("_", names)}.xlsx";
        }
        public const string ExcelContentType = "application/octet-stream";
    }
}
