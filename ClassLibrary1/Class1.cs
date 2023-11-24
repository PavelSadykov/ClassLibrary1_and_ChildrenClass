using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace ClassLibrary1
{
    public abstract class DBJob
    {

        public DbConnection Conn;
        public DbDataAdapter DataAdapter;
        public string ConnString { get; set; }
        public abstract DbConnection GetConnection();
        public abstract DbDataAdapter GetDataAdapter(string sql);
        public abstract bool HasTable(string tableName);// запрос имени таблицы для проверки наличия ее
    }


    public class SQLiteDBJob : DBJob
    {
        public SQLiteConnection _conn;
        private SQLiteDataAdapter _adapter;
        public SQLiteDBJob(string constr)
        {
            ConnString = "DataSource=" + constr;
            _conn = new SQLiteConnection(ConnString);
            Conn = _conn;
        }

        public override DbConnection GetConnection()
        {
            _conn = new SQLiteConnection(ConnString);
            Conn = _conn;
            return Conn;
        }
        public override bool HasTable(string tableName)
        {
                string checkTableQuery = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";

                using (SQLiteCommand cmd = new SQLiteCommand(checkTableQuery, (SQLiteConnection)Conn))
                {
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        return true;
                  
                }
                    else
                    {
                        return false;
                    }
               
            }
           
        }

        public override DbDataAdapter GetDataAdapter (string sql)
        {
          
            _adapter = new SQLiteDataAdapter(sql, _conn);
            DataAdapter = _adapter;
            return DataAdapter;
        }
    }

    public class SQLDBJob : DBJob
    {
        public SqlConnection _conn;
        private SqlDataAdapter _adapter;
        public SQLDBJob(string constr)
        {
            ConnString = @"Data Source=DESKTOP-EKD1ADK;Initial Catalog=Library;Integrated Security=True"; ;
            _conn = new SqlConnection (ConnString);
            Conn = _conn;
        }
        public override bool HasTable(string tableName)
        {

            string checkTableQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{tableName}';";
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand command = new SqlCommand(checkTableQuery, conn))
                {
                   
                        conn.Open();
                        int tableCount = Convert.ToInt32(command.ExecuteScalar());
                        if (tableCount > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    
                }
            }

            
        }
        public override DbConnection GetConnection()
        {
            _conn = new SqlConnection(ConnString);
            Conn = _conn;
            return Conn;
        }
        public override DbDataAdapter GetDataAdapter(string sql)
        {
            _adapter = new SqlDataAdapter(sql, _conn);
            DataAdapter = _adapter;
            return DataAdapter;
        }
    }



}
