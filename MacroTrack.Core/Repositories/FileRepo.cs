using MacroTrack.Core.Infrastructure;

using ClosedXML.Excel;
using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using System.Data;
using System.IO;

namespace MacroTrack.Core.Repositories
{
    /// <summary>
    /// Responsible for general file side things, backups, exports, imports, &c., was originally called "ExportRepo" for ExportDataBaseToExcel();
    /// </summary>
    public class FileRepo : RepoBase
    {
        private readonly string _connectionString;

        public FileRepo(string connectionString, CoreContext ctx) : base(ctx)
        {
            _connectionString = connectionString;            
        }

        /// <summary>
        /// Backup mathod. Please note that, if destinationPath already exists, it will override it.
        /// </summary>
        /// <param name="sourcePath">Existing data you want to back up.</param>
        /// <param name="destinationPath">Where you would like the backup.</param>
        /// <exception cref="FileNotFoundException"></exception>
        public void BackupDatabase(string sourcePath, string destinationPath)
        {
            // Make sure dest dir exists:
            string? destDir = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(destDir)) Directory.CreateDirectory(destDir);

            // ConnStrings:
            string srcConnString = $"Data Source={sourcePath}";
            string destConnString = $"Data Source={destinationPath}";

            // Connections:
            using var source = new SqliteConnection(srcConnString);
            using var destination = new SqliteConnection(destConnString);

            // Open connections:
            source.Open();
            destination.Open();

            // Backup:
            source.BackupDatabase(destination);
        }

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
