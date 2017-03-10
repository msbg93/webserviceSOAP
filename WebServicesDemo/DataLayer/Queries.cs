using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Data;

public class Queries
{
    private static SqlTransaction transaction;
    public static string connstr = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ToString();
    SqlConnection conn = new SqlConnection(connstr);
    
    public void connnection()
    {
        conn.Open();
    }
    public void ExceutenonQuery(string Query)
    {
        conn.Close();
        conn.Open();
        transaction = conn.BeginTransaction();
        SqlCommand cmd = new SqlCommand(Query, conn);
        cmd.CommandText = Query;
        cmd.Transaction = transaction;
        cmd.CommandTimeout = 50000000;
        cmd.ExecuteNonQuery();
        transaction.Commit();
        conn.Close();

    }
    public void ExceutenonQuery2(string Query)
    {
        //conn2.Close();
        //conn2.Open();
        //transaction = conn2.BeginTransaction();
        //SqlCommand cmd = new SqlCommand(Query, conn2);
        //cmd.CommandText = Query;
        //cmd.Transaction = transaction;
        //cmd.CommandTimeout = 50000000;
        //cmd.ExecuteNonQuery();
        //transaction.Commit();
        //conn2.Close();

    }
    public void ExecuteStoredProcedure(string SPName, string Start, string End)
    {
            string Sql = SPName;
            DataSet ds = new DataSet();
            conn.Open();
            SqlCommand sqlcom = new SqlCommand();
            SqlParameter parameter;
            sqlcom.Connection = conn;
            sqlcom.CommandText = Sql;
            sqlcom.CommandType = CommandType.StoredProcedure;
            sqlcom.Parameters.Clear();
            parameter = new SqlParameter("@Start", Start);
            sqlcom.Parameters.Add(parameter);
            parameter = new SqlParameter("@End", End);
            sqlcom.Parameters.Add(parameter);
            sqlcom.CommandTimeout = 880000;
            int rows = sqlcom.ExecuteNonQuery();

            conn.Close();

        
    }
    public DataTable SPInquiryTransaction(string Username, string password, string Consumer_Number, string Bank_Mnemonic, string Reserved)
    {
        DataTable tb = new DataTable();
        string CS = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_1LINK_Inquiry_Transaction", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserName", Username));
            cmd.Parameters.Add(new SqlParameter("@Password", password));
            cmd.Parameters.Add(new SqlParameter("@ReferenceID", Consumer_Number));
            cmd.Parameters.Add(new SqlParameter("@Bank_Mnemonic", Bank_Mnemonic));
            cmd.Parameters.Add(new SqlParameter("@Reserved", Reserved));
            SqlDataReader reader = cmd.ExecuteReader();
            tb.Load(reader);
            con.Close();
            
        }
        return tb;
    }
    public DataTable SPPaymentDetails(string Username, string Password, string Consumer_Number, string Transaction_Auth_Id, string Transaction_Amount, string Tran_Date, string Tran_Time, string BankMnemonic,string Reserved)
    {
        DataTable tb = new DataTable();
        string CS = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {

            //string time = "031200";
            //string HH = time.Substring(0, 2);
            //string MM = time.Substring(2, 2);
            //string SS = time.Substring(4, 2);
            //string Time = HH + ":" + MM + ":" + SS;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Tran_Time.Length; i++)
            {
                if (i % 2 == 0 && i != 0)
                    sb.Append(':');
                sb.Append(Tran_Time[i]);

            }
            string formatted_Time = sb.ToString();
            string formated_Amount;
            formated_Amount = Transaction_Amount.Remove(Transaction_Amount.Length - 2).TrimStart('0');
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_1LINK_Payment_Details", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserName", Username));
            cmd.Parameters.Add(new SqlParameter("@Password", Password));
            cmd.Parameters.Add(new SqlParameter("@ReferenceID", Consumer_Number));
            cmd.Parameters.Add(new SqlParameter("@Tran_Auth_Id", Transaction_Auth_Id));
            cmd.Parameters.Add(new SqlParameter("@Tran_Amount", formated_Amount));
            cmd.Parameters.Add(new SqlParameter("@Tran_Date", Tran_Date));
            cmd.Parameters.Add(new SqlParameter("@Tran_Time", formatted_Time));
            cmd.Parameters.Add(new SqlParameter("@Bank_Mnemonic", BankMnemonic));
            cmd.Parameters.Add(new SqlParameter("@Reserved", Reserved));
            SqlDataReader reader = cmd.ExecuteReader();
            tb.Load(reader);
            con.Close();

        }
        return tb;
    }
    public DataTable SPEchoTransaction(string Username, string Password, string Ping)
    {
        DataTable dt = new DataTable();
      string CS = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
      using (SqlConnection con = new SqlConnection(CS))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SP_1LINK_Echo_Transaction", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Username", Username));
            cmd.Parameters.Add(new SqlParameter("@Password", Password));
            cmd.Parameters.Add(new SqlParameter("@Ping", Ping));
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            con.Close();

        }
      return dt;
    }
    public DataTable getvalues_dt(string query)
    {
        conn.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sda = new SqlDataAdapter(query, conn);
        sda.SelectCommand.CommandTimeout = 88000;
        sda.Fill(dt);
        return dt;
    
    }
    public DataTable getvalues_dt(string column, string table, string condition)
    {
        conn.Open();
        DataTable dt = new DataTable();
        string query = " select " + column + " from " + table + " where 1=1 " + condition;
        SqlDataAdapter sda = new SqlDataAdapter(query, conn);
        sda.Fill(dt);
        return dt;
        conn.Close();
    }
    public string getvalue(string query)
    {
        conn.Close();
        conn.Open();
        DataTable dt = new DataTable();
        SqlDataAdapter sda = new SqlDataAdapter(query, conn);
        sda.Fill(dt);
        string var = dt.Rows[0][0].ToString();
        return var;
        conn.Close();
    }
    public void MasterDetailEntry(DataSet ds)
    {
        conn.Close();
        conn.Open();
        string returnvalue = "";
        transaction = conn.BeginTransaction();
        try
        {

            foreach (DataTable dt in ds.Tables)
            {
                string tablename = "";
                string Colms = "";
                string Rows = "";
                string query = "";
                tablename = dt.ToString();
                foreach (DataColumn dc in dt.Columns)
                {
                    Colms = Colms + dc.ToString() + ",";
                }
                Colms = Colms.Substring(0, Colms.Length - 1);
                int totalcol = dt.Columns.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    Rows = "";
                    for (int i = 0; i < totalcol; i++)
                    {
                        if (dr[i].ToString() == "NULL")
                        {
                            Rows = Rows + "" + dr[i].ToString() + ",";
                        }
                        else
                        {
                            if (i == 6)
                            {
                                Rows = Rows + "'" + Convert.ToDateTime(dr[i]).ToString("yyyyMMdd") + "',";
                            }
                            else
                            {
                                Rows = Rows + "'" + dr[i].ToString() + "',";
                            }
                        }
                    }
                    Rows = Rows.Substring(0, Rows.Length - 1);
                    query = " insert into " + tablename + " (" + Colms + ") VALUES (" + Rows + ")";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandText = query;
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();
                }
            }
            transaction.Commit();
            returnvalue = "TRUE";
        }
        catch (Exception ex)
        {
            if (transaction != null)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);

            }
        }
        finally
        {
            transaction = null;
        }
        conn.Close();



    }
    
}

