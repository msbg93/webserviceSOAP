using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Runtime.Serialization;
using System.ServiceModel;
namespace DataLayer
{
    //public class ATMCustomer
    //{
      
    //    public string PortfolioID { get; set; }
    //    public string CustomerName { get; set; }
    //    public string Address { get; set; }
    //    public string Phone { get; set; }
    //    public string UserName { get; set; }
    //    public string Password { get; set; }
    //    public string ConsumerNumber { get; set; }
    //    public string BankMnemonic { get; set; }
    //    public string Reserved { get; set; }
    //    public string Response_Code { get; set; }
    //    public string Consumer_Detail { get; set; }
    //    public string Bill_Status { get; set; }
    //    public string Due_Date { get; set; }
    //    public string Amount_within_DueDate { get; set; }
    //    public string Amount_after_DueDate { get; set; }
    //    public string Date_Paid { get; set; }
    //    public string Amount_Paid { get; set; }
    //    public string Tran_Auth_Id { get; set; }
    //    public string TransactionAmount { get; set; }
    //    public string Tran_Date { get; set; }
    //    public string Tran_Time { get; set; }
    //    public string IdentificationParameter { get; set; }
    //    public string Result { get; set; }
    //    public string billingMonth { get; set; }

    //    private string _strConnectionString;
    //    private SqlConnection _openConnection()
    //    {
    //        _strConnectionString = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
    //        SqlConnection oCnn = new SqlConnection(_strConnectionString);
    //        oCnn.Open();
    //        return oCnn;
    //    }
    //    private void _closeConnection(SqlConnection oCnn)
    //    {
    //        oCnn.Close();
    //        oCnn = null;
    //    }
    //    public  ATMCustomer GetInquiry(string ReferenceID, string Username, string password)
    //    {

    //        SqlConnection oCnn = _openConnection();
    //        string CS = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
    //        using (SqlConnection con = new SqlConnection(CS))
    //        {
    //            con.Open();
    //            SqlCommand cmd = new SqlCommand("SP_1LINK_Inquiry_Transaction", con);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.Add(new SqlParameter("@ReferenceID", ReferenceID));
    //            cmd.Parameters.Add(new SqlParameter("@UserName", Username));
    //            cmd.Parameters.Add(new SqlParameter("@Password", password));
    //            ATMCustomer customer = new ATMCustomer();
    //            SqlDataReader reader = cmd.ExecuteReader();
    //            DataTable tb = new DataTable();
    //            tb.Load(reader);
    //            if (tb.Rows[0]["UserName"].ToString() == Username && tb.Rows[0]["Password"].ToString() == password)
    //            {
    //                customer.Response_Code = tb.Rows[0]["ErrID"].ToString();
    //                customer.Consumer_Detail = tb.Rows[0]["Portfolio_Id"].ToString() + "~" + tb.Rows[0]["Fund_ID"].ToString() + "~" + tb.Rows[0]["Agent_Id"].ToString();
    //                customer.Bill_Status = tb.Rows[0]["Bill_status"].ToString();
    //                customer.Due_Date = tb.Rows[0]["DueDate"].ToString();
    //                customer.Amount_within_DueDate = tb.Rows[0]["Amount_within_DueDate"].ToString();
    //                customer.Amount_after_DueDate = tb.Rows[0]["Amount_after_DueDate"].ToString();
    //                customer.billingMonth = tb.Rows[0]["Billing_Month"].ToString();
    //                customer.Date_Paid = tb.Rows[0]["Date_Paid"].ToString();
    //                customer.Amount_Paid = tb.Rows[0]["Amount_Paid"].ToString();
    //                customer.Tran_Auth_Id = tb.Rows[0]["Tran_Auth_Id"].ToString();
    //                customer.Reserved = tb.Rows[0]["Reserved"].ToString();

    //            }
    //            else
    //            {
    //                customer.Response_Code = tb.Rows[0]["ErrID"].ToString();
    //            }
    //            con.Close();
    //            return customer;

    //        }
    //    }
    //    public  ATMCustomer PaymentDetails(string ReferenceID, string Username, string Password, string Transaction_Auth_Id, string Transaction_Amount, string Tran_Date, string Tran_Time, string BankMnemonic)
    //    {
    //        SqlConnection oCnn = _openConnection();
    //        SqlCommand cmd = new SqlCommand("SP_1LINK_Payment_Details", oCnn);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.Add(new SqlParameter("@ReferenceID", ReferenceID));
    //        cmd.Parameters.Add(new SqlParameter("@UserName", Username));
    //        cmd.Parameters.Add(new SqlParameter("@Password", Password));
    //        cmd.Parameters.Add(new SqlParameter("@Tran_Auth_Id", Transaction_Auth_Id));
    //        cmd.Parameters.Add(new SqlParameter("@Tran_Amount", Transaction_Amount));
    //        cmd.Parameters.Add(new SqlParameter("@Tran_Date", Tran_Date));
    //        cmd.Parameters.Add(new SqlParameter("@Tran_Time", Tran_Time));
    //        cmd.Parameters.Add(new SqlParameter("@Bank_Mnemonic", BankMnemonic));
    //        ATMCustomer customer = new ATMCustomer();
    //        SqlDataReader reader = cmd.ExecuteReader();
    //        DataTable tb = new DataTable();
    //        tb.Load(reader);
    //        if (tb.Rows[0]["UserName"].ToString() == Username && tb.Rows[0]["Password"].ToString() == Password)
    //        {
    //            customer.Response_Code = tb.Rows[0]["ErrID"].ToString();
    //        }
    //        else
    //        {
    //            customer.Response_Code = tb.Rows[0]["ErrID"].ToString();
    //        }



    //        _closeConnection(oCnn);
    //        return customer;



    //    }
    //    public ATMCustomer   GetInquiryForPractice(string ReferenceID);
        
    //}
     [ServiceContract]
    public interface ATMService
    {
      [OperationContract]
      DataTable BillInquiry(string ReferenceID, string Username, string password);
      [OperationContract]
      DataTable BillPayment(string ReferenceID, string Username, string Password, string Transaction_Auth_Id, string Transaction_Amount, string Tran_Date, string Tran_Time, string BankMnemonic);
      [OperationContract]
      string insertandUpdate(string PortfolioID, string agentID, string FundIDFrom, string AgentIDTo, string FundIDTo, string Transdesc, string TransType, string Dealdate, string Units, string Amount, string CellNo, string Status);

     
    }
}
