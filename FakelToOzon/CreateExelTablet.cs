using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Security.Cryptography;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.ExternalReferences;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FakelToOzon
{
    public class CreateExcelTable 
    {
        int row = 4;
        int count_row = 2;
        private ExcelPackage package;
        private string bfilePath;
        private string cfilePath;
        int count = 0;
        int cnt =0;

        public void CreateTable()
        {
            // Create a new Excel package
            JsonToOzon jsonToOzon = new JsonToOzon();

            // Open the new file

            // Fill in the table with data from the Ozon API
            foreach (var builder in jsonToOzon.CreateJson())
            {
                if (cnt == 0)
                {
                    string baseFilePath = @"..\data\EXEL\base.xlsx";
                    string baseDirectoryPath = @"..\data\OUT\Data";
                    string countDirectoryPath = @"..\data\OUT\Count";

                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        baseFilePath = @"..\..\..\data\EXEL\base.xlsx";
                        baseDirectoryPath = @"..\..\..\data\OUT\Data";
                    }

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    string baseFileName = $"Data_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
                    bfilePath = Path.Combine(baseDirectoryPath, baseFileName);

                    string countFilePath = @"..\data\EXEL\count.xlsx";
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        countDirectoryPath = @"..\..\..\data\OUT\Count";
                        countFilePath = @"..\..\..\data\EXEL\count.xlsx";
                    }
                    string countFileName = $"Count_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
                    cfilePath = Path.Combine(countDirectoryPath, countFileName);

                    // Copy the files
                    File.Copy(baseFilePath, bfilePath, true);
                    File.Copy(countFilePath, cfilePath, true);
                }
                using (var count_package = new ExcelPackage(new FileInfo(cfilePath)))
                {
                    var count_workbook = count_package.Workbook;
                    var count_worksheet = count_workbook.Worksheets["Остатки на складе"];
                     // Ensure count is an integer or numeric value
                   
                    using (var package = new ExcelPackage(new FileInfo(bfilePath)))
                    {
                        // Get the workbook and worksheet
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets["Шаблон"];

                        // Fill in the table with data
                        using (var md5 = MD5.Create())
                        {
                            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(builder.Name));
                            var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
                            string wight = builder.Width;
                            string height = builder.Height;
                            string lenght = builder.Length;
                            int weight = builder.Weight;
                            foreach (var size in builder.Size)
                            {
                                double over_price = (builder.Price[builder.Size.IndexOf(size)] * 0.64 * 3);
                                if (over_price >= 1000)
                                {
                                    count_worksheet.Cells[row, 2].Value = $"{builder.Articule}.{count}";
                                    count_worksheet.Cells[row, 3].Value = builder.Name;
                                    count_worksheet.Cells[row, 4].Value = builder.Count[count];
                                    worksheet.Cells[row, 1].Value = row - 3;
                                    worksheet.Cells[row, 2].Value = $"{builder.Articule}.{count}";
                                    worksheet.Cells[row, 3].Value = builder.Name;
                                    worksheet.Cells[row, 4].Value = over_price;
                                    worksheet.Cells[row, 5].Value = over_price;
                                    worksheet.Cells[row, 6].Value = "Не облагается";
                                    worksheet.Cells[row, 7].Value = "";
                                    worksheet.Cells[row, 8].Value = "";
                                    worksheet.Cells[row, 9].Value = Convert.ToInt32(weight);
                                    worksheet.Cells[row, 10].Value = wight;
                                    worksheet.Cells[row, 11].Value = height;
                                    worksheet.Cells[row, 12].Value = lenght;
                                    worksheet.Cells[row, 13].Value = builder.Images;
                                    worksheet.Cells[row, 14].Value = string.Join(";", builder.Images.Skip(1));
                                    worksheet.Cells[row, 15].Value = "";
                                    worksheet.Cells[row, 16].Value = "";
                                    worksheet.Cells[row, 17].Value = builder.Brand;
                                    worksheet.Cells[row, 18].Value = hashString;
                                    worksheet.Cells[row, 19].Value = size.TrimEnd(' ', '\n').Replace("-", ";").Replace("/", ";");
                                    worksheet.Cells[row, 20].Value = builder.MainColor;
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
                                    count++;
                                    row++;
                                }
                            }
                        }

                        worksheet.DataValidations.Clear();
                        package.Save();
                    }
                    count_worksheet.DataValidations.Clear();
                    count_package.Save();
                }


                
                cnt++;
                count = 0;

                if (cnt == 100)
                {
                    Console.WriteLine("Одна страница отработала ");
                    cnt = 0;
                    row = 4;
                }
            }
        }





    }
}