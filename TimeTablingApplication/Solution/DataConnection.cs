using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
namespace TimeTablingApplication
{
    public class DataConnection
    {
        string connectionStr;
        OleDbConnection myConnection;

           public DataConnection()
        {
            connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" 
                + System.Windows.Forms.Application.StartupPath + "\\juryTTL.accdb";
            myConnection = new OleDbConnection(connectionStr);
        }
           public void ExecuteUpdateSQL(string strSQL) //Hàm ghi và cập nhật dữ liệu
           {
               
               myConnection.Open();

               OleDbCommand myCommand = new OleDbCommand(strSQL, myConnection);
               myCommand.ExecuteNonQuery();

               myConnection.Close();
           }
        

           public DataSet OpenDataSet(string strSQL)
           {
               myConnection.Open();

               OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(strSQL, myConnection);
               DataSet ds = new DataSet();
               
               myDataAdapter.Fill(ds);

               myConnection.Close();
               return ds;

           }
    }
 
}
