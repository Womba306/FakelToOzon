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
using Newtonsoft;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;

using OfficeOpenXml.DataValidation;
using Microsoft.Office.Interop.Excel;
using SoftEtherApi.SoftEtherModel;
using System.Linq;
using System.Collections.Immutable;

namespace FakelToOzon
{
    public class CreateExcelTable
    {
        int row = 4;
        int count_row = 2;
        private ExcelPackage package;
        private string bfilePath;
        private string cfilePath;
        private string bootsfilePath;
        int count = 0;
        int cnt = 0;

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
                    string bootsFilePath = @"..\data\EXEL\boots.xlsx";
                    string baseFilePath = @"..\data\EXEL\base.xlsx";
                    string baseDirectoryPath = @"..\data\OUT\Data";
                    string countDirectoryPath = @"..\data\OUT\Count";

                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        bootsFilePath = @"..\..\..\data\EXEL\boots.xlsx";
                        baseFilePath = @"..\..\..\data\EXEL\base.xlsx";
                        baseDirectoryPath = @"..\..\..\data\OUT\Data";
                    }

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    string baseFileName = $"Data_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
                    string bootsFileName = $"Boots_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
                    bfilePath = Path.Combine(baseDirectoryPath, bootsFileName);
                    bootsfilePath = Path.Combine(baseDirectoryPath, baseFileName);

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
                    File.Copy(bootsFilePath, bootsfilePath, true);
                }
                using (var count_package = new ExcelPackage(new FileInfo(cfilePath)))
                {
                    var count_workbook = count_package.Workbook;
                    var count_worksheet = count_workbook.Worksheets["Остатки на складе"];
                    // Ensure count is an integer or numeric value

                    using (var package = new ExcelPackage(new FileInfo(bfilePath)))
                    {
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets["Шаблон"];
                        var worksheetvalidations = workbook.Worksheets["validation"];

                        using (var boots_package = new ExcelPackage(new FileInfo(bootsfilePath)))
                        {
                            var boots_workbook = boots_package.Workbook;
                            var boots_worksheet = boots_workbook.Worksheets["Шаблон"];
                            var boots_worksheetvalidations = boots_workbook.Worksheets["validation"];

                            // Get the workbook and worksheet

                            List<string?> columnData = new List<string?>();

                            List<string?> boots_columnData = new List<string?>();


                            // Переберите ячейки в столбце
                            for (int row = 1; row <= 763; row++)
                            {
                                // Добавьте значение ячейки в список
                                columnData.Add(worksheetvalidations.Cells[row, 55].Value.ToString());
                                boots_columnData.Add(boots_worksheetvalidations.Cells[row, 59].Value.ToString());

                            }

                            // Fill in the table with data
                            using (var md5 = MD5.Create())
                            {
                                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(builder.Name));
                                var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
                                double wight = builder.Width;
                                double height = builder.Height;
                                double lenght = builder.Length;
                                int weight = builder.Weight;
                                int item_count_last;
                                for (int i = 0; i < builder.Size.Count; i++)
                                {
                                    var size = builder.Size[i];
                                    var item_count = builder.Count[i];
                                    if (item_count < 20)
                                    {
                                        item_count_last = 0;

                                    }
                                    else
                                    {
                                        item_count_last = item_count;
                                    }

                                    double over_price = (builder.Price[builder.Size.IndexOf(size)] * 0.64 * 2.72);
                                    if (over_price >= 1200)
                                    {


                                        count_worksheet.Cells[row, 2].Value = $"{builder.Articule}.{count}";
                                        count_worksheet.Cells[row, 3].Value = $"{builder.Name} | {size.Split(" ")[0]}{size.Split(" ")[1]}{size.Split(" ")[2]}";
                                        count_worksheet.Cells[row, 4].Value = item_count_last;
                                      
                                            boots_worksheet.Cells[row, 1].Value = row - 3;
                                            boots_worksheet.Cells[row, 2].Value = $"{builder.Articule}.{count}";
                                            boots_worksheet.Cells[row, 3].Value = $"{builder.Name} | {size.Split(" ")[0]}{size.Split(" ")[1]}{size.Split(" ")[2]}"; ;
                                            boots_worksheet.Cells[row, 4].Value = over_price;
                                            boots_worksheet.Cells[row, 5].Value = over_price;
                                            boots_worksheet.Cells[row, 6].Value = "Не облагается";
                                            boots_worksheet.Cells[row, 7].Value = "";
                                            boots_worksheet.Cells[row, 8].Value = "";
                                            boots_worksheet.Cells[row, 9].Value = Convert.ToInt32(weight);
                                            boots_worksheet.Cells[row, 10].Value = wight;
                                            boots_worksheet.Cells[row, 11].Value = height;
                                            boots_worksheet.Cells[row, 12].Value = lenght;
                                            boots_worksheet.Cells[row, 13].Value = builder.Images;
                                            boots_worksheet.Cells[row, 14].Value = string.Join(";", builder.Images.Skip(1));
                                            boots_worksheet.Cells[row, 15].Value = "";
                                            boots_worksheet.Cells[row, 16].Value = "";
                                            boots_worksheet.Cells[row, 17].Value = builder.Brand;
                                            boots_worksheet.Cells[row, 18].Value = hashString;
                                            boots_worksheet.Cells[row, 20].Value = builder.MainColor;
                                            boots_worksheet.Cells[row, 21].Value = $"{size.Split(" ")[0]}{size.Split(" ")[1]}{size.Split(" ")[2]}";
                                            boots_worksheet.Cells[row, 22].Value = $"{size.Split(" ")[0]}{size.Split(" ")[1]}{size.Split(" ")[2]}";
                                            boots_worksheet.Cells[row, 23].Value = builder.Color;
                                            boots_worksheet.Cells[row, 24].Value = builder.Category;
                                            boots_worksheet.Cells[row, 25].Value = builder.Season;
                                            boots_worksheet.Cells[row, 26].Value = builder.Sex;
                                            boots_worksheet.Cells[row, 27].Value = "";
                                            boots_worksheet.Cells[row, 28].Value = builder.Description;
                                            boots_worksheet.Cells[row, 29].Value = builder.Under;
                                            boots_worksheet.Cells[row, 30].Value = "Россия";
                                            boots_worksheet.Cells[row, 46].Value = "Россия";
                                            var result = boots_columnData.Where(line => line.Contains(builder.CodeTNVED)).ToList();
                                            if (result.Count > 0)
                                            {
                                                boots_worksheet.Cells[row, 59].Value = result[0];
                                            }
                                             worksheet.Cells[row, 1].Value = row - 3;
                                            worksheet.Cells[row, 2].Value = $"{builder.Articule}.{count}";
                                            worksheet.Cells[row, 3].Value = $"{builder.Name} | {size.Split(" ")[0]}{size.Split(" ")[1]}{size.Split(" ")[2]}"; ;
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
                                            worksheet.Cells[row, 21].Value = $"{size.Split(" ")[0]}{size.Split(" ")[1]}{size.Split(" ")[2]}";
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



                                            var result2 = columnData.Where(line => line.Contains(builder.CodeTNVED)).ToList();
                                            if (result.Count > 0)
                                            {
                                                worksheet.Cells[row, 55].Value = result2[0];
                                            }

                                            count++;
                                            row++;

                                        


                                    }
                                    boots_worksheet.DataValidations.Clear();
                                    boots_package.Save();
                                    worksheet.DataValidations.Clear();
                                    package.Save();
                                }
                                count_worksheet.DataValidations.Clear();
                                count_package.Save();
                            }
                        }
                    }


                    cnt++;
                    count = 0;
                    Console.WriteLine(cnt);

                }
            }

        }
    }
}