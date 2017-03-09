using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;
using SalesForce.Models;

namespace SalesForce.Services
{
    public class AttachmentToFile
    {
        public byte[] CreateExcel(SalesData salesData)
        {
            //INICIANDO O EXCEL
            Application excel = new Application();

            //ABRINDO O EXCEL
            excel.Visible = true;

            Workbook wb = excel.Workbooks.Add(Type.Missing);

            Worksheet ws = (Worksheet)wb.Worksheets[1];

            //FUNÇÃO QUE CRIA O CABEÇALHO DO DOCUMENTO DO EXCEL
            CreateExcelHeader(ws);

            //PREENCHENDO O EXCEL COM OS DADOS RECEBIDOS
            PopulateExcel(ws,salesData);

            //ESTILIZANDO O TAMANHO DA FONTE USADA
            ws.Cells.Font.Size = 14;

            string tempPath = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + "_temp";

            wb.SaveAs(tempPath, wb.FileFormat);
            tempPath = wb.FullName;            
            wb.Close();
            excel.Quit();

            byte[] byteFile = File.ReadAllBytes(tempPath);
            File.Delete(tempPath);

            //RETORNO DA FUNÇÃO
            return byteFile;
        }

        public void CreateExcelHeader(Worksheet ws)
        {
            Int32 cellIndex = 1;

            //SALES DATA
            ws.Cells[2, cellIndex++] = "Sales Data Order No";
            ws.Cells[2, cellIndex++] = "Sales Data Sales Org";
            //SALES DATA ORDER
            ws.Cells[2, cellIndex++] = "Sales Data Source System";
            ws.Cells[2, cellIndex++] = "Sales Data Extraction Rule Type";

            //SALES DATA SOLD TO COMPANY
            ws.Range[ws.Cells[cellIndex], ws.Cells[cellIndex + 14]].Merge();
            ws.Range[ws.Cells[cellIndex], ws.Cells[cellIndex + 14]].Interior = ColorTranslator.ToOle(Color.LightBlue);
            ws.Cells[1, cellIndex] = "Sold to";
            ws.Cells[2, cellIndex++] = "Company Westcon ID";
            ws.Cells[2, cellIndex++] = "Company Name";
            //SALES DATA SOLD TO AddressS
            ws.Cells[2, cellIndex++] = "Address 1";
            ws.Cells[2, cellIndex++] = "Address 2";
            ws.Cells[2, cellIndex++] = "Address 3";
            ws.Cells[2, cellIndex++] = "Address 4";
            ws.Cells[2, cellIndex++] = "City";
            ws.Cells[2, cellIndex++] = "State";
            ws.Cells[2, cellIndex++] = "Postal Code";
            ws.Cells[2, cellIndex++] = "Country";
            //SALES DATA SOLD TO COMPANY
            ws.Cells[2, cellIndex++] = "Company Country Prefix";
            ws.Cells[2, cellIndex++] = "Company Work Phone";
            ws.Cells[2, cellIndex++] = "Company Fax Number";  
            //SALES DATA SOLD TO CONTACT         
            ws.Cells[2, cellIndex++] = "Contact Email Addresss";
            ws.Cells[2, cellIndex++] = "Contact Extension";
            ws.Cells[2, cellIndex++] = "Contact Mobile";          

            //SALES DATA LINE ITEM
            ws.Cells[2, cellIndex++] = "Line Item Line Number";
            ws.Cells[2, cellIndex++] = "Line Item  SKU";
            ws.Cells[2, cellIndex++] = "Line Item SKU Description";
            ws.Cells[2, cellIndex++] = "Line Item Product Type";
            ws.Cells[2, cellIndex++] = "Line Item Sales Descript";
            ws.Cells[2, cellIndex++] = "Line Ite Sales Practice";
            ws.Cells[2, cellIndex++] = "Line Item Created By";
            ws.Cells[2, cellIndex++] = "Line Item Acount Manager ID";
            ws.Cells[2, cellIndex++] = "Line Item Acount Manager Name";
            
            //SALES DATA LINE ITEM SHIP TO COMPANY
            ws.Range[ws.Cells[cellIndex], ws.Cells[cellIndex + 12]].Merge();
            ws.Range[ws.Cells[cellIndex], ws.Cells[cellIndex + 12]].Interior = ColorTranslator.ToOle(Color.LightYellow);
            ws.Cells[1, cellIndex] = "Ship To";
            ws.Cells[2, cellIndex++] = "Company Westcon ID";
            ws.Cells[2, cellIndex++] = "Company Name";
            //SALES DATA LINE ITEM SHIP TO CONTACT
            ws.Cells[2, cellIndex++] = "Address 1";
            ws.Cells[2, cellIndex++] = "Address 2";
            ws.Cells[2, cellIndex++] = "Address 3";
            ws.Cells[2, cellIndex++] = "Address 4";
            ws.Cells[2, cellIndex++] = "City";
            ws.Cells[2, cellIndex++] = "State";
            ws.Cells[2, cellIndex++] = "Postal Code";
            ws.Cells[2, cellIndex++] = "Country";
            //SALES DATA LINE ITEM SHIP TO COMPANY
            ws.Cells[2, cellIndex++] = "Company Country Prefix";
            ws.Cells[2, cellIndex++] = "Company Work Phone";
            ws.Cells[2, cellIndex++] = "Company Fax Number";         
           


            //SALES DATA END USER COMPANY
            ws.Cells.Range[ws.Cells[cellIndex], ws.Cells[cellIndex + 12]].Merge();
            ws.Range[ws.Cells[cellIndex], ws.Cells[cellIndex + 12]].Interior = ColorTranslator.ToOle(Color.LightGray);
            ws.Cells[1, cellIndex] = "End User";
            ws.Cells[2, cellIndex++] = "Company Westcon ID";
            ws.Cells[2, cellIndex++] = "Contact Name";
            ws.Cells[2, cellIndex++] = "Address 1";
            ws.Cells[2, cellIndex++] = "Address 2";
            ws.Cells[2, cellIndex++] = "Address 3";
            ws.Cells[2, cellIndex++] = "Address 4";
            ws.Cells[2, cellIndex++] = "City";
            ws.Cells[2, cellIndex++] = "State";
            ws.Cells[2, cellIndex++] = "Postal Code";
            ws.Cells[2, cellIndex++] = "Country";
            //SALES DATA END USER CONTACT
            ws.Cells[2, cellIndex++] = "Company Country Prefix";
            ws.Cells[2, cellIndex++] = "Company Work Phone";
            ws.Cells[2, cellIndex++] = "Company Fax Number";         
         

            //SALES DATA LINE ITEM ORDER
            ws.Cells[2, cellIndex++] = "Line Item Sales Order Quantity";
            ws.Cells[2, cellIndex++] = "Line Item Sales Unit";
            ws.Cells[2, cellIndex++] = "Line Item Billing Cost";
            ws.Cells[2, cellIndex++] = "Line Item Billing Value";
            ws.Cells[2, cellIndex++] = "Line Item Document Currency";
            ws.Cells[2, cellIndex++] = "Line Item Contract No";
            ws.Cells[2, cellIndex++] = "Line Item Start Date";
            ws.Cells[2, cellIndex++] = "Line Item End Date";
            ws.Cells[2, cellIndex++] = "Line Item Manufacturer Queto No";
            ws.Cells[2, cellIndex++] = "Line Item Model No";
            ws.Cells[2, cellIndex++] = "Line Item NSP";
            ws.Cells[2, cellIndex++] = "Line Item Is Earliest Invoiced Item";
            ws.Cells[2, cellIndex++] = "Line Item Earliest Billing Post Date";
            ws.Cells[2, cellIndex++] = "Line Item Manufacturer ID";
            ws.Cells[2, cellIndex++] = "Line Item Manufacturer Name";
            ws.Cells[2, cellIndex++] = "Line Item Manufacturer Accreditation Level For Sold To";
            ws.Cells[2, cellIndex++] = "Line Item Discount";
            ws.Cells[2, cellIndex++] = "Line Item Promo ID";
            ws.Cells[2, cellIndex++] = "Line Item Promo 2 ID";
            ws.Cells[2, cellIndex++] = "Line Item Accreditation ID";
            ws.Cells[2, cellIndex++] = "Line Item Serial No";

            //SALES ORDER CONTRACT NUMBER
            ws.Cells.Range[ws.Cells[cellIndex], ws.Cells[cellIndex + 8]].Merge();
            ws.Range[ws.Cells[cellIndex], ws.Cells[cellIndex + 8]].Interior = ColorTranslator.ToOle(Color.LightGreen);
            ws.Cells[1, cellIndex] = "Contract";
            ws.Cells[2, cellIndex++] = "Contract No";
            ws.Cells[2, cellIndex++] = "Contract Sales Order";
            ws.Cells[2, cellIndex++] = "Contract Manufacturer Invoice No";
            ws.Cells[2, cellIndex++] = "Contract Westcon Po No";
            ws.Cells[2, cellIndex++] = "Contract Manufacturer Quote No";
            ws.Cells[2, cellIndex++] = "Contract Model No";
            ws.Cells[2, cellIndex++] = "Contract NSP";
            ws.Cells[2, cellIndex++] = "Contract Start Date";
            ws.Cells[2, cellIndex++] = "Contract End Date";           

        }

        public void PopulateExcel(Worksheet ws, SalesData salesData)
        {
            int cellIndex = 1;

            for (Int32 line = 2, len = salesData.LineItems.Count; line < len; line++)
            {
                LineItem item = salesData.LineItems.ToList()[line];
                
                //POPULANDO O EXCEL
                ws.Cells[line, cellIndex++] = salesData.SalesOrderNo;
                ws.Cells[line, cellIndex++] = salesData.SalesOrg;
                ws.Cells[line, cellIndex++] = salesData.SourceSystem;
                ws.Cells[line, cellIndex++] = salesData.ExtractionRuleType;

                //Sold to info
                ws.Cells[line, cellIndex++] = salesData.SoldTo.WestconId;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Name;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Addresss.Addr1;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Addresss.Addr2;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Addresss.Addr3;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Addresss.Addr4;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Addresss.City;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Addresss.State;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Addresss.PostalCode;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Addresss.Country;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.CountryPrefix;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.WorkPhone;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.FaxNumber;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Contact.EmailAddresss;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Contact.Extension;
                ws.Cells[line, cellIndex++] = salesData.SoldTo.Contact.MobilePhone;

                //Line Item Data
                ws.Cells[line, cellIndex++] = item.LineNumber;
                ws.Cells[line, cellIndex++] = item.SKU;
                ws.Cells[line, cellIndex++] = item.SKUDescription;
                ws.Cells[line, cellIndex++] = item.ProductType;
                ws.Cells[line, cellIndex++] = item.SalesDistrict;
                ws.Cells[line, cellIndex++] = item.SalesPractice; 
                ws.Cells[line, cellIndex++] = item.CreatedBy;
                ws.Cells[line, cellIndex++] = item.AccountManagerId;
                ws.Cells[line, cellIndex++] = item.AccountManagerName;

                //Ship to
                ws.Cells[line, cellIndex++] = item.ShipTo.WestconId;
                ws.Cells[line, cellIndex++] = item.ShipTo.Name;
                ws.Cells[line, cellIndex++] = item.ShipTo.Addresss.Addr1;
                ws.Cells[line, cellIndex++] = item.ShipTo.Addresss.Addr2;
                ws.Cells[line, cellIndex++] = item.ShipTo.Addresss.Addr3;
                ws.Cells[line, cellIndex++] = item.ShipTo.Addresss.Addr4 ;
                ws.Cells[line, cellIndex++] = item.ShipTo.Addresss.City;
                ws.Cells[line, cellIndex++] = item.ShipTo.Addresss.State;
                ws.Cells[line, cellIndex++] = item.ShipTo.Addresss.PostalCode;
                ws.Cells[line, cellIndex++] = item.ShipTo.Addresss.Country;
                ws.Cells[line, cellIndex++] = item.ShipTo.CountryPrefix;
                ws.Cells[line, cellIndex++] = item.ShipTo.WorkPhone;
                ws.Cells[line, cellIndex++] = item.ShipTo.FaxNumber;

                //End User
                ws.Cells[line, cellIndex++] = item.EndUser.WestconId;
                ws.Cells[line, cellIndex++] = item.EndUser.Name;
                ws.Cells[line, cellIndex++] = item.EndUser.Addresss.Addr1;
                ws.Cells[line, cellIndex++] = item.EndUser.Addresss.Addr2;
                ws.Cells[line, cellIndex++] = item.EndUser.Addresss.Addr3;
                ws.Cells[line, cellIndex++] = item.EndUser.Addresss.Addr4;
                ws.Cells[line, cellIndex++] = item.EndUser.Addresss.City;
                ws.Cells[line, cellIndex++] = item.EndUser.Addresss.State;
                ws.Cells[line, cellIndex++] = item.EndUser.Addresss.PostalCode;
                ws.Cells[line, cellIndex++] = item.EndUser.Addresss.Country;
                ws.Cells[line, cellIndex++] = item.EndUser.CountryPrefix;
                ws.Cells[line, cellIndex++] = item.EndUser.WorkPhone;
                ws.Cells[line, cellIndex++] = item.EndUser.FaxNumber;


                ws.Cells[line, cellIndex++] = item.SalesOrderQty;
                ws.Cells[line, cellIndex++] = item.SalesUnit;
                ws.Cells[line, cellIndex++] = item.BillingCost;
                ws.Cells[line, cellIndex++] = item.BillingValue;
                ws.Cells[line, cellIndex++] = item.DocumentCurrency;
                ws.Cells[line, cellIndex++] = item.ContractNo;
                ws.Cells[line, cellIndex++] = item.StartDate;
                ws.Cells[line, cellIndex++] = item.EndDate;
                ws.Cells[line, cellIndex++] = item.ManufacturerQuoteNo;
                ws.Cells[line, cellIndex++] = item.ModelNo;
                ws.Cells[line, cellIndex++] = item.NSP;
                ws.Cells[line, cellIndex++] = item.IsEarliestInvoicedItem;
                ws.Cells[line, cellIndex++] = item.EarliestBillingPostDate;
                ws.Cells[line, cellIndex++] = item.ManufacturerID;
                ws.Cells[line, cellIndex++] = item.ManufacturerName;
                ws.Cells[line, cellIndex++] = item.ManufacturerAccreditationLevelForSoldTo;
                ws.Cells[line, cellIndex++] = item.Discount;
                ws.Cells[line, cellIndex++] = item.PromoID;
                ws.Cells[line, cellIndex++] = item.Promo2ID;
                ws.Cells[line, cellIndex++] = item.AccreditationID;
                ws.Cells[line, cellIndex++] = item.LineItemSerialNo;

                //Contract
                ws.Cells[line, cellIndex++] = item.Contract.ContractNo;
                ws.Cells[line, cellIndex++] = item.Contract.SalesOrderNo;
                ws.Cells[line, cellIndex++] = item.Contract.ManufacturerInvoiceNo;
                ws.Cells[line, cellIndex++] = item.Contract.WestconPONo;
                ws.Cells[line, cellIndex++] = item.Contract.ManufacturerQuoteNo;
                ws.Cells[line, cellIndex++] = item.Contract.ModelNo;
                ws.Cells[line, cellIndex++] = item.Contract.NSP;
                ws.Cells[line, cellIndex++] = item.Contract.StartDate;
                ws.Cells[line, cellIndex++] = item.Contract.EndDate;
            }
        }

        public SalesForceSVC.Attachment AttachFile(byte[] byteFile)
        {

            SalesForceSVC.Attachment att = new SalesForceSVC.Attachment();

            att.Body = byteFile;
            att.Name = "Attachment Excel File";
            att.IsPrivate = false;
                       
            
            return att;

        }
       
    }

}