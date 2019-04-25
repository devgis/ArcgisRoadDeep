using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Threading;
using System.IO;
using System.Windows.Forms;
namespace ArcgisApp
{
    public static class OledbHelper
    {
        private static OleDbConnection conn;
        static OledbHelper()
        {
            if (conn == null)
            {
                conn = new OleDbConnection();
                string dbPath=Path.Combine(Application.StartupPath,"data.mdb");
                conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbPath;
            }
        }


        /// <summary>
        /// ��ѯ����DataTable
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">�����б�</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, OleDbParameter[] parameters = null)
        {
            try
            {
                lock (conn)
                {
                    DataTable dt = new DataTable();
                    OleDbCommand oleCmd = new OleDbCommand();
                    oleCmd.CommandText = sql;
                    if (parameters != null && parameters.Length > 0)
                    {
                        oleCmd.Parameters.AddRange(parameters);
                    }
                    oleCmd.Connection = conn;
                    OleDbDataAdapter da = new OleDbDataAdapter(oleCmd);
                    da.Fill(dt);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                Loger.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// ִ��SQL���
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="parameters">�����б�</param>
        /// <returns></returns>
        public static bool ExecSql(string sql, OleDbParameter[] parameters = null)
        {
            try
            {
                lock (conn)
                {
                    conn.Open();
                    OleDbCommand oleCmd = new OleDbCommand();
                    oleCmd.CommandText = sql;
                    if (parameters != null && parameters.Length > 0)
                    {
                        oleCmd.Parameters.AddRange(parameters);
                    }

                    oleCmd.Connection = conn;
                    if (oleCmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.WriteLog(ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// ������ʽִ��SQL���
        /// </summary>
        /// <param name="listsqls">SQL���list</param>
        /// <param name="listparameters">�����б�list</param>
        /// <returns>�ɹ�����true ���򷵻�false</returns>
        public static bool ExecSqlByTran(List<string> listsqls, List<OleDbParameter[]> listparameters = null)
        {
            if (listsqls == null || listparameters == null || listsqls.Count <= 0 || listparameters.Count <= 0 || listsqls.Count != listparameters.Count)
            {
                return false;
            }
            else
            {
                lock (conn)
                {
                    conn.Open();
                    OleDbTransaction tran = conn.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < listsqls.Count; i++)
                        {
                            OleDbCommand oleCmd = new OleDbCommand();
                            oleCmd.CommandText = listsqls[i];
                            if (listparameters != null && listparameters[i]!=null)
                            {
                                oleCmd.Parameters.AddRange(listparameters[i]);
                            }
                            oleCmd.Connection = conn;
                            oleCmd.Transaction = tran;
                            oleCmd.ExecuteNonQuery();
                        }
                        tran.Commit();
                        conn.Close();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        conn.Close();
                        Loger.WriteLog(ex);
                        return false;
                    }
                }
            }
        }

    }
}
