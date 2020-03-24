using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Utils
{
    public class SqlHelper
    {


        private static readonly string constr = ConfigurationManager.ConnectionStrings["strCon"].ConnectionString;


      
        public static int ExecuteNonQuery(IDbTransaction tran, string sql, params SqlParameter[] pms)
        {

            using (SqlCommand cmd = new SqlCommand(sql, (SqlConnection)tran.Connection, (SqlTransaction)tran))
            {
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }

                return cmd.ExecuteNonQuery();

            }


        }


        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] pms)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, constr);
            if (pms != null)
            {
                adapter.SelectCommand.Parameters.AddRange(pms);
            }
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;

        }


        protected static System.Data.Common.DbConnection CreateConnection()
        {
            SqlConnection con = new SqlConnection(constr);
            return con;
        }

    }

    public class TransactionDal : SqlHelper
    {

        public DbConnection dbconnection = null;
        public DbTransaction transaction = null;

        public void BeginTransaction()
        {
            dbconnection = SqlHelper.CreateConnection();
            dbconnection.Open();
            transaction = dbconnection.BeginTransaction();

        }

        public void CommitTransaction()
        {
            if (null != transaction)
            {
                transaction.Commit();
            }

        }

        public void RollbackTransaction()
        {
            if (null != transaction)
            {
                transaction.Rollback();
            }

        }

        public void DisposeTransaction()
        {
            if (dbconnection.State == ConnectionState.Open)
            {
                dbconnection.Close();
            }
            if (null != transaction)
            {
                transaction.Dispose();
            }

        }
    }


}
