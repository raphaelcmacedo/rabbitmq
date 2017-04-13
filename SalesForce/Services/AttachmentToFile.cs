using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;
using Main.Models;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Main.Services
{
    public class AttachmentToFile
    {
        private IDataFormat dataFormatCustom;

        public string CreateExcel(SalesData salesData)
        {

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Sales Data " + salesData.SalesOrderNo);
            dataFormatCustom = workbook.CreateDataFormat();

            this.CreateExcelHeaderTop(workbook, sheet);
            this.CreateExcelHeader(workbook, sheet);
            this.PopulateExcel(sheet, salesData);

            Byte[] byteFile = null;
            using (var stream = new MemoryStream())
            {
                workbook.Write(stream);
                byteFile = stream.ToArray();
            }

           // File.WriteAllBytes("D:\\Desenvolvimento\\Teste.xlsx", byteFile);

            return System.Convert.ToBase64String(byteFile);
        }

        private void CreateExcelHeaderTop(XSSFWorkbook workbook, ISheet sheet)
        {
            var row = sheet.CreateRow(0);
            
            //Style
            var style = workbook.CreateCellStyle();
            
            style.FillPattern = FillPattern.SolidForeground;
            var font = workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            style.SetFont(font);

            //Merge cells
            NPOI.SS.Util.CellRangeAddress mergeSoldTo = new NPOI.SS.Util.CellRangeAddress(0, 0, 4, 20);
            sheet.AddMergedRegion(mergeSoldTo);

            NPOI.SS.Util.CellRangeAddress mergeShipTo = new NPOI.SS.Util.CellRangeAddress(0, 0, 30, 42);
            sheet.AddMergedRegion(mergeShipTo);

            NPOI.SS.Util.CellRangeAddress mergeEndUser = new NPOI.SS.Util.CellRangeAddress(0, 0, 43, 55);
            sheet.AddMergedRegion(mergeEndUser);

            style.FillForegroundColor = (short)IndexedColors.BlueGrey.Index;
            ICell cellSoldTo = this.CreateCell(row, style, 4, "Sold To");
            CellUtil.SetAlignment(cellSoldTo, workbook, 2);

            style.FillForegroundColor = (short)IndexedColors.Grey25Percent.Index;
            ICell cellShipTo = this.CreateCell(row, style, 30, "Ship To");
            CellUtil.SetAlignment(cellShipTo, workbook, 2);

            style.FillForegroundColor = (short)IndexedColors.Plum.Index;
            ICell cellEndUser = this.CreateCell(row, style, 43, "End User");
            CellUtil.SetAlignment(cellEndUser, workbook, 2);
        }

        private void CreateExcelHeader(XSSFWorkbook workbook, ISheet sheet)
        {
            Int32 cellIndex = 0;

            var row = sheet.CreateRow(1);

            //Style
            var style = workbook.CreateCellStyle();
            style.FillForegroundColor = (short)IndexedColors.LightBlue.Index;
            style.FillPattern = FillPattern.SolidForeground;
            
            string[] headers = new string[]
            {
                "Sales Data Order No",
                "Sales Data Sales Org",
                "Sales Data Source System",
                "Sales Data Extraction Rule Type",
                "Company Westcon ID",
                "Company Name",
                "Address 1",
                "Address 2",
                "Address 3",
                "Address 4",
                "City",
                "State",
                "Postal Code",
                "Country",
                "Company Country Prefix",
                "Company Work Phone",
                "Company Fax Number",
                "Contact Email Address",
                "Contact Extension",
                "Contact Mobile",
                "Line Item Line Number",
                "Line Item  SKU",
                "Line Item SKU Description",
                "Line Item Product Type",
                "Line Item Sales Descript",
                "Line Ite Sales Practice",
                "Line Item Created By",
                "Line Item Acount Manager ID",
                "Line Item Acount Manager Name",
                "Company Westcon ID",
                "Company Name",
                "Address 1",
                "Address 2",
                "Address 3",
                "Address 4",
                "City",
                "State",
                "Postal Code",
                "Country",
                "Company Country Prefix",
                "Company Work Phone",
                "Company Fax Number",
                "Company Westcon ID",
                "Company Name",
                "Address 1",
                "Address 2",
                "Address 3",
                "Address 4",
                "City",
                "State",
                "Postal Code",
                "Country",
                "Company Country Prefix",
                "Company Work Phone",
                "Company Fax Number",
                "Line Item Sales Order Quantity",
                "Line Item Sales Unit",
                "Line Item Billing Cost",
                "Line Item Billing Value",
                "Line Item Document Currency",
                "Line Item Contract No",
                "Line Item Start Date",
                "Line Item End Date",
                "Line Item Manufacturer Queto No",
                "Line Item Model No",
                "Line Item NSP",
                "Line Item Is Earliest Invoiced Item",
                "Line Item Earliest Billing Post Date",
                "Line Item Manufacturer ID",
                "Line Item Manufacturer Name",
                "Line Item Manufacturer Accreditation Level For Sold To",
                "Line Item Discount",
                "Line Item Promo ID",
                "Line Item Promo 2 ID",
                "Line Item Accreditation ID",
                "Line Item Serial No",
                "Contract No",
                "Contract Sales Order",
                "Contract Manufacturer Invoice No",
                "Contract Westcon Po No",
                "Contract Manufacturer Quote No",
                "Contract Model No",
                "Contract NSP",
                "Contract Start Date",
                "Contract End Date"
            };

            foreach (string header in headers)
            {
                sheet.SetColumnWidth(cellIndex, 6000);
                this.CreateCell(row, style, cellIndex++,header);
            }
            

        }

        private void PopulateExcel(ISheet sheet, SalesData salesData)
        {
            int cellIndex = 0;
            int line = 2;

            foreach (LineItem item in salesData.LineItems)
            {
                var row = sheet.CreateRow(line);
                
                //Sales Data
                this.CreateCell(row, null, cellIndex++, salesData.SalesOrderNo);
                this.CreateCell(row, null, cellIndex++, salesData.SalesOrg);
                this.CreateCell(row, null, cellIndex++, salesData.SourceSystem);
                this.CreateCell(row, null, cellIndex++, salesData.ExtractionRuleType);

                //Sold to info
                if (salesData.SoldTo != null)
                {
                    this.CreateCell(row, null, cellIndex++, salesData.SoldTo.WestconId);
                    this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Name);

                    if (salesData.SoldTo.Address != null)
                    {
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Address.Addr1);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Address.Addr2);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Address.Addr3);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Address.Addr4);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Address.City);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Address.State);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Address.PostalCode);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Address.Country);
                    }
                    else
                    {
                        cellIndex += 8;
                    }

                    this.CreateCell(row, null, cellIndex++, salesData.SoldTo.CountryPrefix);
                    this.CreateCell(row, null, cellIndex++, salesData.SoldTo.WorkPhone);
                    this.CreateCell(row, null, cellIndex++, salesData.SoldTo.FaxNumber);

                    if (salesData.SoldTo.Contact != null)
                    {
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.EmailAddress);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Extension);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.MobilePhone);
                    }
                    else
                    {
                        cellIndex += 3;
                    }
                }
                else
                {
                    cellIndex += 16;
                }

                //Line Item Data
                this.CreateCell(row, null, cellIndex++, item.LineNumber);
                this.CreateCell(row, null, cellIndex++, item.SKU);
                this.CreateCell(row, null, cellIndex++, item.SKUDescription);
                this.CreateCell(row, null, cellIndex++, item.ProductType);
                this.CreateCell(row, null, cellIndex++, item.SalesDistrict);
                this.CreateCell(row, null, cellIndex++, item.SalesPractice);
                this.CreateCell(row, null, cellIndex++, item.CreatedBy);
                this.CreateCell(row, null, cellIndex++, item.AccountManagerId);
                this.CreateCell(row, null, cellIndex++, item.AccountManagerName);

                //Ship to
                if (item.ShipTo != null)
                {
                    this.CreateCell(row, null, cellIndex++, item.ShipTo.WestconId);
                    this.CreateCell(row, null, cellIndex++, item.ShipTo.Name);
                    if (item.ShipTo.Address != null)
                    {
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Address.Addr1);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Address.Addr2);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Address.Addr3);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Address.Addr4);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Address.City);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Address.State);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Address.PostalCode);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Address.Country);
                    }else
                    {
                        cellIndex += 8;
                    }
                    this.CreateCell(row, null, cellIndex++, item.ShipTo.CountryPrefix);
                    this.CreateCell(row, null, cellIndex++, item.ShipTo.WorkPhone);
                    this.CreateCell(row, null, cellIndex++, item.ShipTo.FaxNumber);

                }
                else
                {
                    cellIndex += 13;
                }

                //End User
                if (item.EndUser != null)
                {
                    this.CreateCell(row, null, cellIndex++, item.EndUser.WestconId);
                    this.CreateCell(row, null, cellIndex++, item.EndUser.Name);
                    if (item.EndUser.Address != null)
                    {
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Address.Addr1);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Address.Addr2);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Address.Addr3);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Address.Addr4);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Address.City);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Address.State);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Address.PostalCode);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Address.Country);
                    }else
                    {
                        cellIndex += 8;
                    }
                    this.CreateCell(row, null, cellIndex++, item.EndUser.CountryPrefix);
                    this.CreateCell(row, null, cellIndex++, item.EndUser.WorkPhone);
                    this.CreateCell(row, null, cellIndex++, item.EndUser.FaxNumber);

                }
                else
                {
                    cellIndex += 13;
                }

                this.CreateCell(row, null, cellIndex++, item.SalesOrderQty);
                this.CreateCell(row, null, cellIndex++, item.SalesUnit);
                this.CreateCell(row, null, cellIndex++, item.BillingCost);
                this.CreateCell(row, null, cellIndex++, item.BillingValue);
                this.CreateCell(row, null, cellIndex++, item.DocumentCurrency);
                this.CreateCell(row, null, cellIndex++, item.ContractNo);
                this.CreateCell(row, null, cellIndex++, item.StartDate);
                this.CreateCell(row, null, cellIndex++, item.EndDate);
                this.CreateCell(row, null, cellIndex++, item.ManufacturerQuoteNo);
                this.CreateCell(row, null, cellIndex++, item.ModelNo);
                this.CreateCell(row, null, cellIndex++, item.NSP);
                this.CreateCell(row, null, cellIndex++, item.IsEarliestInvoicedItem);
                this.CreateCell(row, null, cellIndex++, item.EarliestBillingPostDate);
                this.CreateCell(row, null, cellIndex++, item.ManufacturerID);
                this.CreateCell(row, null, cellIndex++, item.ManufacturerName);
                this.CreateCell(row, null, cellIndex++, item.ManufacturerAccreditationLevelForSoldTo);
                this.CreateCell(row, null, cellIndex++, item.Discount);
                this.CreateCell(row, null, cellIndex++, item.PromoID);
                this.CreateCell(row, null, cellIndex++, item.Promo2ID);
                this.CreateCell(row, null, cellIndex++, item.AccreditationID);
                this.CreateCell(row, null, cellIndex++, item.LineItemSerialNo);

                //Contract
                if (item.Contract != null)
                {
                    this.CreateCell(row, null, cellIndex++, item.Contract.ContractNo);
                    this.CreateCell(row, null, cellIndex++, item.Contract.SalesOrderNo);
                    this.CreateCell(row, null, cellIndex++, item.Contract.ManufacturerInvoiceNo);
                    this.CreateCell(row, null, cellIndex++, item.Contract.WestconPONo);
                    this.CreateCell(row, null, cellIndex++, item.Contract.ManufacturerQuoteNo);
                    this.CreateCell(row, null, cellIndex++, item.Contract.ModelNo);
                    this.CreateCell(row, null, cellIndex++, item.Contract.NSP);
                    this.CreateCell(row, null, cellIndex++, item.Contract.StartDate);
                    this.CreateCell(row, null, cellIndex++, item.Contract.EndDate);
                }
               
                line++;
                cellIndex = 0;
            }
        }

        public SalesForceSVC.Attachment Base64ToSalesForceAttachment(string base64File, string parentId)
        {
            byte[] byteFile = Convert.FromBase64String(base64File);
            SalesForceSVC.Attachment att = new SalesForceSVC.Attachment();

            att.Body = byteFile;
            att.Name = "SalesData.xlsx";
            att.IsPrivate = false;
            att.ParentId = parentId;

            return att;

        }

        private ICell CreateCell(IRow row, ICellStyle cellStyle, int index, string text)
        {
            ICell cell = row.CreateCell(index);
            cell.SetCellValue(text);
            cell.CellStyle = cellStyle;
            return cell;
        }

        private void CreateCell(IRow row, ICellStyle cellStyle, int index, decimal value)
        {
            ICell cell = row.CreateCell(index);
            cell.SetCellValue((double) value);
            cell.CellStyle = cellStyle;
        }

        private void CreateCell(IRow row, ICellStyle cellStyle, int index, DateTime? value)
        {
            if (value != null)
            {
                string newValue = (value ?? DateTime.MinValue).ToString("MM/dd/yyyy");

                ICell cell = row.CreateCell(index);
                cell.SetCellValue(newValue);
                cell.CellStyle = cellStyle;
            }
        }

        /*private void FillForegroundColor(IWorkbook workbook, IRow row, int foregroundColor = 0, int jumpColumn = -1)
        {
            for (int i = 0; i < numberOfColumns; i++)
            {
                if (i != jumpColumn)
                {
                    this.CreateCell(workbook, row, i, "", false, 0, 0, IndexedColors.DarkBlue.Index);
                }
            }
        }*/

    }

}


