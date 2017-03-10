using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using DataLayer;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text;
using System.Diagnostics;


namespace WebServicesDemo
{
    /// <summary>
    /// Summary description for CustomerService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CustomerService : System.Web.Services.WebService
    {
        public struct EchoResponce
        {
            public string PingResponse;
        }
        public struct StringValuesBillInqiury
        {
            public string Response_Code, Consumer_Detail, Bill_Status, Due_Date, Amount_Within_DueDate, Amount_After_DueDate, Billing_Month, Date_Paid, Amount_Paid, Tran_Auth_Id, PID, F_Name, Reserved;
        }
        public struct StringValuesBillPayment
        {
            public string Response_Code, Reserved, Identification_Parameter;
        }
        public struct BillResponce
        {

            public string BillResult;

        }
        Queries objquery = new Queries();
        private FtpClient ftp = null;
        private string _servername = ConfigurationManager.AppSettings["Ftpservername"];
        private string _userid = ConfigurationManager.AppSettings["Ftpuserid"];
        private string _password = ConfigurationManager.AppSettings["Ftppassword"];

        DataTable dtCustomer;
        DataTable dt;
        DataTable dtfund;
        DataTable dtOrder;
        [WebMethod]
        public string EchoTransaction(string Username, string Password, string Ping)
        {
            DataTable dt = objquery.SPEchoTransaction(Username, Password, Ping);
            dt.TableName = "EchoTransaction";
            EchoResponce sv = new EchoResponce();
            string result;
            try
            {
                //sv.PingResponse = dt.Rows[0]["ping"].ToString();
                result = dt.Rows[0]["ping"].ToString();
            }
            catch (Exception)
            {
                //sv.PingResponse = dt.Rows[0]["Response_Code"].ToString();
                result = dt.Rows[0]["ping"].ToString();
            }
            return result;

        }
        [WebMethod]
        public string BillInquiry(string Username, string password, string Consumer_Number, string Bank_Mnemonic, string Reserved)
        {

            DataTable dt = objquery.SPInquiryTransaction(Username, password, Consumer_Number, Bank_Mnemonic, Reserved);
            dt.TableName = "BillInquiry";
            StringValuesBillInqiury sv = new StringValuesBillInqiury();
            BillResponce BR = new BillResponce();
            StringBuilder SB = new StringBuilder(299);
            string result;
            try
            {
                sv.Response_Code = dt.Rows[0]["Response_Code"].ToString();
                sv.Consumer_Detail = dt.Rows[0]["Consumer_Detail"].ToString();
                sv.Bill_Status = dt.Rows[0]["Bill_Status"].ToString();
                sv.Due_Date = dt.Rows[0]["Due_Date"].ToString();
                sv.Amount_Within_DueDate = dt.Rows[0]["Amount_Within_DueDate"].ToString();
                sv.Amount_After_DueDate = dt.Rows[0]["Amount_After_DueDate"].ToString();
                sv.Billing_Month = dt.Rows[0]["Billing_Month"].ToString();
                sv.Date_Paid = dt.Rows[0]["Date_Paid"].ToString();
                sv.Amount_Paid = dt.Rows[0]["Amount_Paid"].ToString();
                sv.Tran_Auth_Id = dt.Rows[0]["Tran_Auth_Id"].ToString();
                sv.PID = dt.Rows[0]["PID"].ToString();
                sv.F_Name = dt.Rows[0]["F_Name"].ToString();
                sv.Reserved = sv.PID.PadRight(30) + sv.F_Name.PadRight(30);
                if (string.IsNullOrEmpty(sv.Date_Paid))
                {
                    BR.BillResult = sv.Response_Code.PadRight(2) + sv.Consumer_Detail.PadRight(30) + sv.Bill_Status.PadRight(1) + Convert.ToDateTime(sv.Due_Date).ToString("yyyyMMdd").PadRight(8) + sv.Amount_Within_DueDate.PadLeft(11, '0').PadLeft(12, '+').PadRight(14,'0') + sv.Amount_After_DueDate.PadLeft(11, '0').PadLeft(12, '+').PadRight(14,'0') + sv.Billing_Month.PadRight(4) + sv.Date_Paid.PadRight(8) + sv.Amount_Paid.PadLeft(12) + sv.Tran_Auth_Id.PadRight(6) + sv.Reserved.PadRight(200);
                    //SB.Append(BR.BillResult);
                    result = BR.BillResult;
                }
                else
                {
                    BR.BillResult = sv.Response_Code.PadRight(2) + sv.Consumer_Detail.PadRight(30) + sv.Bill_Status.PadRight(1) + Convert.ToDateTime(sv.Due_Date).ToString("yyyyMMdd").PadRight(8) + sv.Amount_Within_DueDate.PadLeft(11, '0').PadLeft(12, '+').PadRight(14, '0') + sv.Amount_After_DueDate.PadLeft(11, '0').PadLeft(12, '+').PadRight(14, '0') + sv.Billing_Month.PadRight(4) + Convert.ToDateTime(sv.Date_Paid).ToString("yyyyMMdd").PadRight(8) + sv.Amount_Paid.PadLeft(10, '0').PadRight(12, '0') + sv.Tran_Auth_Id.PadRight(6) + sv.Reserved.PadRight(200);
                    //SB.Append(BR.BillResult);
                    result = BR.BillResult;
                }
            }
            catch (Exception)
            {
                sv.Response_Code = dt.Rows[0]["Response_Code"].ToString();
                BR.BillResult = sv.Response_Code.PadRight(299) ;
                //SB.Append(BR.BillResult);
                result = BR.BillResult;
            }
            return result;
        }
        [WebMethod]
        public string BillPayment(string Username, string Password, string Consumer_Number, string Transaction_Auth_Id, string Transaction_Amount, string Tran_Date, string Tran_Time, string BankMnemonic, string Reserved)
        {
            DataTable dtrepso = new DataTable();
            dtrepso.TableName = "BillPayment";
            DataTable dt = objquery.SPPaymentDetails(Username, Password, Consumer_Number, Transaction_Auth_Id, Transaction_Amount, Tran_Date, Tran_Time, BankMnemonic, Reserved);
            dt.TableName = "BillPayment";
            StringValuesBillPayment sv = new StringValuesBillPayment();
            BillResponce BP = new BillResponce();
            StringBuilder SB = new StringBuilder(222);
            string result;
            if (dt.Columns.Count > 1)
            {

                //string Result = dt.Rows[0]["Ref_ID"].ToString();
                //string appPath = HttpContext.Current.Request.PhysicalApplicationPath;
                //string filePath = appPath + "EOrdersFiles\\" + Result.ToString() + ".txt";
                //StreamWriter w;
                //w = File.CreateText(filePath);
                //w.WriteLine(dt.Rows[0]["Ref_ID"].ToString() + "#" + dt.Rows[0]["Portfolio_id"].ToString() + "#" + dt.Rows[0]["AgentID"].ToString() + "#" + dt.Rows[0]["Fund_ID"].ToString() + "#" + dt.Rows[0]["Tran_Amount"].ToString() + "#" + dt.Rows[0]["OrderDatetime"].ToString());
                //w.Flush();
                //w.Close();
                //FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create("ftp://" + _servername + "/BP/" + Result.ToString() + ".txt");
                //requestFTPUploader.Credentials = new NetworkCredential(_userid, _password);
                //requestFTPUploader.UsePassive = true;
                //requestFTPUploader.KeepAlive = false;
                //requestFTPUploader.Proxy = null;
                //requestFTPUploader.UseBinary = false;
                //requestFTPUploader.Timeout = 90000;
                //requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                //requestFTPUploader.Proxy = null;
                //FileInfo fileInfo = new FileInfo(filePath);
                //FileStream fileStream = fileInfo.OpenRead();

                //int bufferLength = 2048;
                //byte[] buffer = new byte[bufferLength];

                //Stream uploadStream = requestFTPUploader.GetRequestStream();
                //int contentLength = fileStream.Read(buffer, 0, bufferLength);

                //while (contentLength != 0)
                //{
                //    uploadStream.Write(buffer, 0, contentLength);
                //    contentLength = fileStream.Read(buffer, 0, bufferLength);
                //}

                //uploadStream.Close();
                //fileStream.Close();

                DataColumn dtcol = new DataColumn();
                dtcol.ColumnName = "Response_code";
                dtrepso.Columns.Add(dtcol);
                dtcol = new DataColumn();
                dtcol.ColumnName = "Identification_Parameter";
                dtrepso.Columns.Add(dtcol);
                dtcol = new DataColumn();
                dtcol.ColumnName = "Reserved";
                dtrepso.Columns.Add(dtcol);
                DataRow dr = dtrepso.NewRow();
                dr["Response_code"] = "00";
                //dr["Identification_Parameter"] = dt.Rows[0]["Ref_ID"].ToString();
                //dr["Reserved"] = dtrepso.Rows[0]["Reserved"].ToString();
                dtrepso.Rows.Add(dr);
                sv.Response_Code = dtrepso.Rows[0]["Response_code"].ToString();
                sv.Identification_Parameter = dtrepso.Rows[0]["Identification_Parameter"].ToString();
                sv.Reserved = dtrepso.Rows[0]["Reserved"].ToString();
                BP.BillResult = sv.Response_Code.PadRight(2) + sv.Identification_Parameter.PadRight(20) + sv.Reserved.PadRight(200);
                //SB.Append(BP.BillResult);
                result = BP.BillResult;
            }
            else
            {
                DataColumn dtcol = new DataColumn();
                dtcol.ColumnName = "Response_code";
                dtrepso.Columns.Add(dtcol);
                dtcol = new DataColumn();
                dtcol.ColumnName = "Identification_Parameter";
                dtrepso.Columns.Add(dtcol);
                dtcol = new DataColumn();
                dtcol.ColumnName = "Reserved";
                dtrepso.Columns.Add(dtcol);
                DataRow dr = dtrepso.NewRow();
                dr["Response_code"] = dt.Rows[0]["Response_code"].ToString();
                //dr["Identification_Paramter"] = dt.Rows[0]["Ref_ID"].ToString();
                //dr["Reserved"] = dt.Rows[0]["Reserved"].ToString();
                dtrepso.Rows.Add(dr);
                sv.Response_Code = dtrepso.Rows[0]["Response_code"].ToString();
                BP.BillResult = sv.Response_Code.PadRight(222) ;
                //SB.Append(BP.BillResult);
                result = BP.BillResult;

            }

            return result;
        }





    }

}









