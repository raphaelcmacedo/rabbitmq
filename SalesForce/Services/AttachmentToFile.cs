﻿using System;
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

            //Local test only
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
            NPOI.SS.Util.CellRangeAddress mergeSoldTo = new NPOI.SS.Util.CellRangeAddress(0, 0, 4, 31);
            sheet.AddMergedRegion(mergeSoldTo);

            NPOI.SS.Util.CellRangeAddress mergeShipTo = new NPOI.SS.Util.CellRangeAddress(0, 0, 41, 68);
            sheet.AddMergedRegion(mergeShipTo);

            NPOI.SS.Util.CellRangeAddress mergeEndUser = new NPOI.SS.Util.CellRangeAddress(0, 0, 69, 94);
            sheet.AddMergedRegion(mergeEndUser);

            NPOI.SS.Util.CellRangeAddress mergeSerial = new NPOI.SS.Util.CellRangeAddress(0, 0, 138, 141);
            sheet.AddMergedRegion(mergeSerial);

            style.FillForegroundColor = (short)IndexedColors.BlueGrey.Index;
            ICell cellSoldTo = this.CreateCell(row, style, 4, "Sold To");
            CellUtil.SetAlignment(cellSoldTo, workbook, 2);

            style.FillForegroundColor = (short)IndexedColors.Grey25Percent.Index;
            ICell cellShipTo = this.CreateCell(row, style, 41, "Ship To");
            CellUtil.SetAlignment(cellShipTo, workbook, 2);

            style.FillForegroundColor = (short)IndexedColors.Plum.Index;
            ICell cellEndUser = this.CreateCell(row, style, 69, "End User");
            CellUtil.SetAlignment(cellEndUser, workbook, 2);

            style.FillForegroundColor = (short)IndexedColors.Grey50Percent.Index;
            ICell cellSerial = this.CreateCell(row, style, 138, "Serial Numbers");
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
                "Contact Name",
                "Contact Westcon ID",
                "Contact Work Phone",
                "Contact Fax Number",
                "Contact Email Address",
                "Contact Extension",
                "Contact Mobile",
                "Contact Address 1",
                "Contact Address 2",
                "Contact Address 3",
                "Contact Address 4",
                "Contact City",
                "Contact State",
                "Contact Postal Code",
                "Contact Country",
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
                "Contact Name",
                "Contact Westcon ID",
                "Contact Work Phone",
                "Contact Fax Number",
                "Contact Email Address",
                "Contact Extension",
                "Contact Mobile",
                "Contact Address 1",
                "Contact Address 2",
                "Contact Address 3",
                "Contact Address 4",
                "Contact City",
                "Contact State",
                "Contact Postal Code",
                "Contact Country",//64
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
                "Contact Name",
                "Contact Westcon ID",
                "Contact Work Phone",
                "Contact Fax Number",
                "Contact Email Address",
                "Contact Extension",
                "Contact Mobile",
                "Contact Address 1",
                "Contact Address 2",
                "Contact Address 3",
                "Contact Address 4",
                "Contact City",
                "Contact State",
                "Contact Postal Code",
                "Contact Country",//91
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
                "Line Item Deal ID AgreementNo",
                "Line Item Deal ID SBANo",
                "Line Item Deal ID RoutingIndicator",
                "Line Item Deal ID Discount",
                "Line Item Promo ID AgreementNo",
                "Line Item Promo ID SBANo",
                "Line Item Promo ID RoutingIndicator",
                "Line Item Promo ID Discount",
                "Line Item Promo 2 ID AgreementNo",
                "Line Item Promo 2 ID SBANo",
                "Line Item Promo 2 ID RoutingIndicator",
                "Line Item Promo 2 ID Discount",
                "Line Item Accreditation ID AgreementNo",
                "Line Item Accreditation ID SBANo",
                "Line Item Accreditation ID RoutingIndicator",
                "Line Item Accreditation ID Discount",               
                "Contract No",
                "Contract Sales Order",
                "Contract Manufacturer Invoice No",
                "Contract Westcon Po No",
                "Contract Manufacturer Quote No",
                "Contract Model No",
                "Contract NSP",
                "Contract Start Date",
                "Contract End Date",
                "Line Item Serial No",
                "Serial Number",
                "Equi Manufacturer Serial Number",
                "Equi Serial Number"
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
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Name);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.WestconId);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.WorkPhone);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.FaxNumber);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.EmailAddress);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Extension);
                        this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.MobilePhone);

                        if (salesData.SoldTo.Contact.Address != null)
                        {
                            this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Address.Addr1);
                            this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Address.Addr2);
                            this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Address.Addr3);
                            this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Address.Addr4);
                            this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Address.City);
                            this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Address.State);
                            this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Address.PostalCode);
                            this.CreateCell(row, null, cellIndex++, salesData.SoldTo.Contact.Address.Country);
                        }
                        else
                        {
                            cellIndex += 8;
                        }
                    }
                    else
                    {
                        cellIndex += 14;
                    }
                }
                else
                {
                    cellIndex += 27;
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

                    if (item.ShipTo.Contact != null)
                    {
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Name);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.WestconId);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.WorkPhone);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.FaxNumber);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.EmailAddress);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Extension);
                        this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.MobilePhone);

                        if (item.ShipTo.Contact.Address != null)
                        {
                            this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Address.Addr1);
                            this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Address.Addr2);
                            this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Address.Addr3);
                            this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Address.Addr4);
                            this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Address.City);
                            this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Address.State);
                            this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Address.PostalCode);
                            this.CreateCell(row, null, cellIndex++, item.ShipTo.Contact.Address.Country);
                        }
                        else
                        {
                            cellIndex += 8;
                        }
                    }
                    else
                    {
                        cellIndex += 14;
                    }
                }
                else
                {
                    cellIndex += 27;
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

                    if (item.EndUser.Contact != null)
                    {
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Name);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.WestconId);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.WorkPhone);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.FaxNumber);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.EmailAddress);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Extension);
                        this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.MobilePhone);

                        if (item.EndUser.Contact.Address != null)
                        {
                            this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Address.Addr1);
                            this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Address.Addr2);
                            this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Address.Addr3);
                            this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Address.Addr4);
                            this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Address.City);
                            this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Address.State);
                            this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Address.PostalCode);
                            this.CreateCell(row, null, cellIndex++, item.EndUser.Contact.Address.Country);
                        }
                        else
                        {
                            cellIndex += 8;
                        }
                    }else
                    {
                        cellIndex += 14;
                    }
                   

                }
                else
                {
                    cellIndex += 27;
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

                this.CreateCell(row, null, cellIndex++, item.DealID.AgreementNo);
                this.CreateCell(row, null, cellIndex++, item.DealID.SBANo);
                this.CreateCell(row, null, cellIndex++, item.DealID.RoutingIndicator);
                this.CreateCell(row, null, cellIndex++, item.DealID.Value);

                this.CreateCell(row, null, cellIndex++, item.PromoIDDiscount.AgreementNo);
                this.CreateCell(row, null, cellIndex++, item.PromoIDDiscount.SBANo);
                this.CreateCell(row, null, cellIndex++, item.PromoIDDiscount.RoutingIndicator);
                this.CreateCell(row, null, cellIndex++, item.PromoIDDiscount.Value);

                this.CreateCell(row, null, cellIndex++, item.Promo2IDDiscount.AgreementNo);
                this.CreateCell(row, null, cellIndex++, item.Promo2IDDiscount.SBANo);
                this.CreateCell(row, null, cellIndex++, item.Promo2IDDiscount.RoutingIndicator);
                this.CreateCell(row, null, cellIndex++, item.Promo2IDDiscount.Value);

                this.CreateCell(row, null, cellIndex++, item.AccreditationIDDiscount.AgreementNo);
                this.CreateCell(row, null, cellIndex++, item.AccreditationIDDiscount.SBANo);
                this.CreateCell(row, null, cellIndex++, item.AccreditationIDDiscount.RoutingIndicator);
                this.CreateCell(row, null, cellIndex++, item.AccreditationIDDiscount.Value);

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
                }else
                {
                    cellIndex += 9;
                }

                this.CreateCell(row, null, cellIndex++, item.LineItemSerialNo);
                this.CreateCell(row, null, cellIndex++, item.SerialNo);
                this.CreateCell(row, null, cellIndex++, item.EquiManufacturerSerialNo);
                this.CreateCell(row, null, cellIndex++, item.EquiSerialNo);

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


