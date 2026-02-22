using MacroTrack.Core.Infrastructure;

using ClosedXML.Excel;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using System.Data;
using System.IO;

namespace MacroTrack.Core.Repositories
{
    public class ExportRepo : RepoBase
    {
        private readonly string _connectionString;

        public ExportRepo(string connectionString, CoreContext ctx) : base(ctx)
        {
            _connectionString = connectionString;            
        }

        /*
        public void ExportToExcel()
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx)",
                FileName = "MTDatabaseExport.xlsx"
            };

            if (dlg.ShowDialog() != true) return;

            ExportDataBaseToExcel(dlg.FileName);
        }*/

        public void ExportDataBaseToExcel(string outputPath)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var workbook = new XLWorkbook();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';";
            using var tableReader = tableCmd.ExecuteReader();
            while (tableReader.Read())
            {
                string tableName = tableReader.GetString(0);
                var cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM [{tableName}]";
                using var reader = cmd.ExecuteReader();
                var ws = workbook.Worksheets.Add(tableName);

                for (int c = 0; c < reader.FieldCount; c++) ws.Cell(1, c + 1).SetValue(reader.GetName(c));
                int row = 2;
                while (reader.Read())
                {
                    for (int c = 0; c < reader.FieldCount; c++)
                    {
                        object val = reader.GetValue(c);
                        ws.Cell(row, c + 1).SetValue(val == DBNull.Value ? Blank.Value : XLCellValue.FromObject(val));
                    }
                    row++;
                }
                ws.Columns().AdjustToContents();
            }
            workbook.SaveAs(outputPath);
        }
    }

    

}
