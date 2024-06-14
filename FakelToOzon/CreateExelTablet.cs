using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Security.Cryptography;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.ExternalReferences;

namespace FakelToOzon
{
    public class CreateExcelTable 
    {
        int row = 4;
        private ExcelPackage package;
        private string filePath;

       
        int cnt =0;

        public  void CreateTable() 
        {
            // Create a new Excel package
            JsonToOzon jsonToOzon = new JsonToOzon();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            package = new ExcelPackage();

            // Create a new Excel workbook
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("Sheet1"); // Specify the first sheet

            // Set the file name
            string directoryPath = @"C:\Users\1\Downloads";
            string fileName = $"Report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
            filePath = Path.Combine(directoryPath, fileName);

            // Create headers for the table
            string[] headers = new string[]
            {
                // ...
            };

            // Fill in the headers
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            // Fill in the table with data from the Ozon API
            foreach (var builder in jsonToOzon.CreateJson())
            {
                foreach (var price in builder.Price)
                {
                    double over_price = (price * 0.64 * 3);
                    if (over_price >= 1500)
                    {
                        using (var md5 = MD5.Create())
                        {
                            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(builder.Name));
                            var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();




                            foreach (var size in builder.Size)
                            {

                                worksheet.Cells[row, 1].Value = row - 1;
                                worksheet.Cells[row, 2].Value = builder.Articule;
                                worksheet.Cells[row, 3].Value = builder.Name;
                                worksheet.Cells[row, 4].Value = price;
                                worksheet.Cells[row, 5].Value = price;
                                worksheet.Cells[row, 6].Value = "Не облагается";
                                worksheet.Cells[row, 7].Value = "";
                                worksheet.Cells[row, 8].Value = "";
                                worksheet.Cells[row, 9].Value = builder.Weight;
                                worksheet.Cells[row, 10].Value = "";
                                worksheet.Cells[row, 11].Value = "";
                                worksheet.Cells[row, 12].Value = "";
                                worksheet.Cells[row, 13].Value = builder.Images;
                                worksheet.Cells[row, 14].Value = string.Join(";", builder.Images.Skip(1));
                                worksheet.Cells[row, 15].Value = "";
                                worksheet.Cells[row, 16].Value = "";
                                worksheet.Cells[row, 17].Value = builder.Brand;
                                worksheet.Cells[row, 18].Value = hashString;
                                worksheet.Cells[row, 19].Value = size;
                                worksheet.Cells[row, 20].Value = builder.Color;
                                worksheet.Cells[row, 21].Value = size;
                                worksheet.Cells[row, 22].Value = builder.Color;
                                worksheet.Cells[row, 23].Value = builder.Category;
                                worksheet.Cells[row, 24].Value = builder.Sex;
                                worksheet.Cells[row, 25].Value = "";
                                worksheet.Cells[row, 26].Value = builder.Description;
                                worksheet.Cells[row, 27].Value = "";
                                worksheet.Cells[row, 28].Value = "";
                                worksheet.Cells[row, 29].Value = builder.Under;
                                worksheet.Cells[row, 30].Value = "Россия";
                                worksheet.Cells[row, 31].Value = builder.Season;
                                worksheet.Cells[row, 40].Value = builder.Zipper;
                                row++;
                            }

                        }
                        cnt++;
                        if (cnt == 30)
                        {
                            package.SaveAs(new FileInfo(filePath));
                            cnt = 0;
                            row = 4;
                        }
                    }
                }
            }
            
        }
        
            
        


    }
}