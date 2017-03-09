using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;

namespace SalesForce.Services
{
    public class AttachmentToFile
    {
        public byte[] CreateExcel()
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
            PopulateExcel(ws);

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
            //SALES DATA
            ws.Cells[1, 1] = "Sales Data Order No";
            ws.Cells[1, 2] = "Sales Data Sales Org";

            //SALES DATA SOLD TO COMPANY
            ws.Cells[1, 3] = "Company Westcon ID";
            ws.Cells[1, 4] = "Company Name";
            //SALES DATA SOLD TO ADDRESS
            ws.Cells[1, 5] = "Addres 1";
            ws.Cells[1, 6] = "Addres 2";
            ws.Cells[1, 7] = "Addres 3";
            ws.Cells[1, 8] = "Addres 4";
            ws.Cells[1, 9] = "City";
            ws.Cells[1, 10] = "State";
            ws.Cells[1, 11] = "Postal Code";
            ws.Cells[1, 12] = "Country";
            //SALES DATA SOLD TO COMPANY
            ws.Cells[1, 13] = "Company Country Prefix";
            ws.Cells[1, 14] = "Company Work Phone";
            ws.Cells[1, 15] = "Company Fax Number";
            ws.Cells[1, 16] = "Other Attibutes";


            //SALES DATA SOLD TO CONTACT
            ws.Cells[1, 17] = "Company Westcon ID";
            ws.Cells[1, 18] = "Company Country Prefix";
            ws.Cells[1, 19] = "Company Work Phone";
            ws.Cells[1, 20] = "Company Fax Number";
            ws.Cells[1, 21] = "Other Attributes";
            ws.Cells[1, 22] = "Contact Email Address";
            ws.Cells[1, 23] = "Contact Extension";
            ws.Cells[1, 24] = "Contact Mobile";

            //SALES DATA ORDER
            ws.Cells[1, 25] = "Sales Data Source System";
            ws.Cells[1, 26] = "Sales Data Extraction Rule Type";

            //SALES DATA LINE ITEM
            ws.Cells[1, 27] = "Line Item Line Number";
            ws.Cells[1, 28] = "Line Item  SKU";
            ws.Cells[1, 29] = "Line Item SKU Description";
            ws.Cells[1, 30] = "Line Item Product Type";
            ws.Cells[1, 31] = "Line Item Sales Descript";
            ws.Cells[1, 32] = "Line Ite Sales Practice";
            ws.Cells[1, 33] = "Line Item Created By";
            ws.Cells[1, 34] = "Line Item Acount Manager ID";
            ws.Cells[1, 35] = "Line Item Acount Manager Name";
            //SALES DATA LINE ITEM SHIP TO COMPANY
            ws.Cells[1, 36] = "Company Westcon ID";
            ws.Cells[1, 37] = "Company Name";
            //SALES DATA LINE ITEM SHIP TO CONTACT
            ws.Cells[1, 38] = "Addres 1";
            ws.Cells[1, 39] = "Addres 2";
            ws.Cells[1, 40] = "Addres 3";
            ws.Cells[1, 41] = "Addres 4";
            ws.Cells[1, 42] = "City";
            ws.Cells[1, 43] = "State";
            ws.Cells[1, 44] = "Postal Code";
            ws.Cells[1, 45] = "Country";
            //SALES DATA LINE ITEM SHIP TO COMPANY
            ws.Cells[1, 46] = "Company Country Prefix";
            ws.Cells[1, 47] = "Company Work Phone";
            ws.Cells[1, 48] = "Company Fax Number";
            ws.Cells[1, 49] = "Other Attibutes";
            ws.Cells[1, 50] = "Tack It Email Address";
            ws.Cells[1, 51] = "Ship It Email Address";
            //SALES DATA LINE ITEM SHIP TO COMPANY
            ws.Cells[1, 52] = "Company Westcon ID";
            ws.Cells[1, 53] = "Company Name";
            //SALES DATA SHIP TO COPMANY CONTACT
            ws.Cells[1, 54] = "Addres 1";
            ws.Cells[1, 55] = "Addres 2";
            ws.Cells[1, 56] = "Addres 3";
            ws.Cells[1, 57] = "Addres 4";
            ws.Cells[1, 58] = "City";
            ws.Cells[1, 59] = "State";
            ws.Cells[1, 60] = "Postal Code";
            ws.Cells[1, 61] = "Country";
            //SALES DATA SHIP TO COMPANY CONTACT
            ws.Cells[1, 62] = "Company Country Prefix";
            ws.Cells[1, 63] = "Company Work Phone";
            ws.Cells[1, 64] = "Company Fax Number";
            ws.Cells[1, 65] = "Company Other Attributes";
            ws.Cells[1, 66] = "Company Email Address";
            ws.Cells[1, 67] = "Company Extension";
            ws.Cells[1, 68] = "Company Mobile Phone";

            //SALES DATA END USER COMPANY
            ws.Cells[1, 69] = "Company Westcon ID";
            ws.Cells[1, 70] = "Contact Name";
            ws.Cells[1, 71] = "Addres 1";
            ws.Cells[1, 72] = "Addres 2";
            ws.Cells[1, 73] = "Addres 3";
            ws.Cells[1, 74] = "Addres 4";
            ws.Cells[1, 75] = "City";
            ws.Cells[1, 76] = "State";
            ws.Cells[1, 77] = "Postal Code";
            ws.Cells[1, 78] = "Country";
            //SALES DATA END USER CONTACT
            ws.Cells[1, 79] = "Company Country Prefix";
            ws.Cells[1, 80] = "Company Work Phone";
            ws.Cells[1, 81] = "Company Fax Number";
            ws.Cells[1, 82] = "Other Attributes";
            ws.Cells[1, 83] = "End User Id By Manufacturer";

            //SALES DATA END USER CONTACT
            ws.Cells[1, 84] = "Company Westcon ID";
            ws.Cells[1, 85] = "Contact Name";
            ws.Cells[1, 86] = "Addres 1";
            ws.Cells[1, 87] = "Addres 2";
            ws.Cells[1, 88] = "Addres 3";
            ws.Cells[1, 89] = "Addres 4";
            ws.Cells[1, 90] = "City";
            ws.Cells[1, 91] = "State";
            ws.Cells[1, 92] = "Postal Code";
            ws.Cells[1, 93] = "Country";
            //SALES DATA END USER COMPANY
            ws.Cells[1, 94] = "Company Country Prefix";
            ws.Cells[1, 95] = "Company Work Phone";
            ws.Cells[1, 96] = "Company Fax Number";
            ws.Cells[1, 97] = "Other Attributes";
            ws.Cells[1, 98] = "Contact Email Address";
            ws.Cells[1, 99] = "Contact Extension";
            ws.Cells[1, 100] = "Contact Mobile Phone";

            //SALES DATA LINE ITEM ORDER
            ws.Cells[1, 101] = "Line Item Sales Order Quantity";
            ws.Cells[1, 102] = "Line Item Sales Unit";
            ws.Cells[1, 103] = "Line Item Billing Cost";
            ws.Cells[1, 104] = "Line Item Billing Value";
            ws.Cells[1, 105] = "Line Item Document Currency";
            ws.Cells[1, 106] = "Line Item Contract No";
            ws.Cells[1, 107] = "Line Item Start Date";
            ws.Cells[1, 108] = "Line Item End Date";
            ws.Cells[1, 109] = "Line Item Manufacturer Queto No";
            ws.Cells[1, 110] = "Line Item Model No";
            ws.Cells[1, 111] = "Line Item NSP";
            ws.Cells[1, 112] = "Line Item Is Earliest Invoiced Item";
            ws.Cells[1, 113] = "Line Item Earliest Billing Post Date";
            ws.Cells[1, 114] = "Line Item Manufacturer ID";
            ws.Cells[1, 115] = "Line Item Manufacturer Name";
            ws.Cells[1, 116] = "Line Item Manufacturer Accreditation Level For Sold To";
            ws.Cells[1, 117] = "Line Item Discount";
            ws.Cells[1, 118] = "Line Item Promo ID";
            ws.Cells[1, 119] = "Line Item Promo 2 ID";
            ws.Cells[1, 120] = "Line Item Accreditation ID";
            ws.Cells[1, 121] = "Line Item Serial No";

            //SALES ORDER CONTRACT NUMBER
            ws.Cells[1, 122] = "Contract No";
            ws.Cells[1, 123] = "Contract Sales Order";
            ws.Cells[1, 124] = "Contract Manufacturer Invoice No";
            ws.Cells[1, 125] = "Contract Westcon Po No";
            ws.Cells[1, 126] = "Contract Manufacturer Quote No";
            ws.Cells[1, 127] = "Contract Model No";
            ws.Cells[1, 128] = "Contract NSP";
            ws.Cells[1, 129] = "Contract Start Date";
            ws.Cells[1, 130] = "Contract End Date";

            //HEADER FONT AND CELL COLOR
            ws.Range["A1", "BJ1"].Font.Color = ColorTranslator.ToOle(Color.Black);
            ws.Range["A1", "BJ1"].Borders.Color = ColorTranslator.ToOle(Color.Black);
            ws.Range["A1", "BJ1"].Borders.LineStyle = XlLineStyle.xlContinuous;
            ws.Range["A1", "BJ1"].Interior.Color = ColorTranslator.ToOle(Color.LightGray);

        }

        public void PopulateExcel(Worksheet ws)
        {
            //VARIÁVEL QUE CONTA A LINHA
            Int64 line = 2;

            //foreach(var item in obj)
            //{
            //POPULANDO O EXCEL
            ws.Cells[line, 1] = "";
            ws.Cells[line, 2] = "";
            ws.Cells[line, 3] = "";
            ws.Cells[line, 4] = "";
            ws.Cells[line, 5] = "";
            ws.Cells[line, 6] = "";
            ws.Cells[line, 7] = "";
            ws.Cells[line, 8] = "";
            ws.Cells[line, 9] = "";
            ws.Cells[line, 10] = "";

            ws.Cells[line, 11] = "";
            ws.Cells[line, 12] = "";
            ws.Cells[line, 13] = "";
            ws.Cells[line, 14] = "";
            ws.Cells[line, 15] = "";
            ws.Cells[line, 16] = "";
            ws.Cells[line, 17] = "";
            ws.Cells[line, 18] = "";
            ws.Cells[line, 19] = "";
            ws.Cells[line, 20] = "";

            ws.Cells[line, 21] = "";
            ws.Cells[line, 22] = "";
            ws.Cells[line, 23] = "";
            ws.Cells[line, 24] = "";
            ws.Cells[line, 25] = "";
            ws.Cells[line, 26] = "";
            ws.Cells[line, 27] = "";
            ws.Cells[line, 28] = "";
            ws.Cells[line, 29] = "";
            ws.Cells[line, 30] = "";

            ws.Cells[line, 31] = "";
            ws.Cells[line, 32] = "";
            ws.Cells[line, 33] = "";
            ws.Cells[line, 34] = "";
            ws.Cells[line, 35] = "";
            ws.Cells[line, 36] = "";
            ws.Cells[line, 37] = "";
            ws.Cells[line, 38] = "";
            ws.Cells[line, 39] = "";
            ws.Cells[line, 40] = "";

            ws.Cells[line, 41] = "";
            ws.Cells[line, 42] = "";
            ws.Cells[line, 43] = "";
            ws.Cells[line, 44] = "";
            ws.Cells[line, 45] = "";
            ws.Cells[line, 46] = "";
            ws.Cells[line, 47] = "";
            ws.Cells[line, 48] = "";
            ws.Cells[line, 49] = "";
            ws.Cells[line, 50] = "";

            ws.Cells[line, 51] = "";
            ws.Cells[line, 52] = "";
            ws.Cells[line, 53] = "";
            ws.Cells[line, 54] = "";
            ws.Cells[line, 55] = "";
            ws.Cells[line, 56] = "";
            ws.Cells[line, 57] = "";
            ws.Cells[line, 58] = "";
            ws.Cells[line, 59] = "";
            ws.Cells[line, 60] = "";

            ws.Cells[line, 61] = "";
            ws.Cells[line, 62] = "";
            ws.Cells[line, 63] = "";
            ws.Cells[line, 64] = "";
            ws.Cells[line, 65] = "";
            ws.Cells[line, 66] = "";
            ws.Cells[line, 67] = "";
            ws.Cells[line, 68] = "";
            ws.Cells[line, 69] = "";
            ws.Cells[line, 70] = "";

            ws.Cells[line, 71] = "";
            ws.Cells[line, 72] = "";
            ws.Cells[line, 73] = "";
            ws.Cells[line, 74] = "";
            ws.Cells[line, 75] = "";
            ws.Cells[line, 76] = "";
            ws.Cells[line, 77] = "";
            ws.Cells[line, 78] = "";
            ws.Cells[line, 79] = "";
            ws.Cells[line, 80] = "";

            ws.Cells[line, 81] = "";
            ws.Cells[line, 82] = "";
            ws.Cells[line, 83] = "";
            ws.Cells[line, 84] = "";
            ws.Cells[line, 85] = "";
            ws.Cells[line, 86] = "";
            ws.Cells[line, 87] = "";
            ws.Cells[line, 88] = "";
            ws.Cells[line, 89] = "";
            ws.Cells[line, 90] = "";

            ws.Cells[line, 91] = "";
            ws.Cells[line, 92] = "";
            ws.Cells[line, 93] = "";
            ws.Cells[line, 94] = "";
            ws.Cells[line, 95] = "";
            ws.Cells[line, 96] = "";
            ws.Cells[line, 97] = "";
            ws.Cells[line, 98] = "";
            ws.Cells[line, 99] = "";
            ws.Cells[line, 100] = "";

            ws.Cells[line, 101] = "";
            ws.Cells[line, 102] = "";
            ws.Cells[line, 103] = "";
            ws.Cells[line, 104] = "";
            ws.Cells[line, 105] = "";
            ws.Cells[line, 106] = "";
            ws.Cells[line, 107] = "";
            ws.Cells[line, 108] = "";
            ws.Cells[line, 109] = "";
            ws.Cells[line, 110] = "";

            ws.Cells[line, 111] = "";
            ws.Cells[line, 112] = "";
            ws.Cells[line, 113] = "";
            ws.Cells[line, 114] = "";
            ws.Cells[line, 115] = "";
            ws.Cells[line, 116] = "";
            ws.Cells[line, 117] = "";
            ws.Cells[line, 118] = "";
            ws.Cells[line, 119] = "";
            ws.Cells[line, 120] = "";

            ws.Cells[line, 121] = "";
            ws.Cells[line, 122] = "";
            ws.Cells[line, 123] = "";
            ws.Cells[line, 124] = "";
            ws.Cells[line, 125] = "";
            ws.Cells[line, 126] = "";
            ws.Cells[line, 127] = "";
            ws.Cells[line, 128] = "";
            ws.Cells[line, 129] = "";
            ws.Cells[line, 130] = "";

            //INCREMENTANDO A LINHA
            line++;
            //}
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