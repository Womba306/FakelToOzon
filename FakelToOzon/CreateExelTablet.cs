using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace FakelToOzon
{
    public class CreateExcelTable
    {
        public static void CreateTable(JSONBuilder builder)
        {
            // Create a new Excel package
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                // Create a new Excel workbook
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.Add("Sheet1"); // Specify the first sheet

                // Set the file name
                string directoryPath = @"C:\Users\1\Downloads";
                string fileName = $"Report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
                string filePath = Path.Combine(directoryPath, fileName);

                // Create headers for the table
                string[] headers = new string[]
                {
                    "Name",
                    "Description",
                    "Price",
                    "Articule",
                    "CodeTNVED",
                    "Weight",
                    "Brand",
                    "Color",
                    "Size",
                    "Images",
                    "Hash",
                    "TRTS",
                    "GOST",
                    "Season",
                    "Zipper",
                    "Protects",
                    "AllParameters",
                    "Sex",
                    "Category"
                };

                // Fill in the headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                // Fill in the table with data from the Ozon API
                int row = 2;
                worksheet.Cells[row, 1].Value = builder.Name;
                worksheet.Cells[row, 2].Value = builder.Description;
                worksheet.Cells[row, 3].Value = string.Join(", ", builder.Price);
                // ...

                // Save the file
                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}