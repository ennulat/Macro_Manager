using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Timers;

namespace Macro_Manager.helper
{
    public class Database
    {
        //private SqlConnection connection;
        private SqlConnection connection;
        //private  connectionSqlite;
        private SqlCommand command;
        private SqlDataReader reader;
        public bool isSQLite { get; set; }
        private bool isError;
        private string _errorMessage;

        public int affectedrows;
        private string _databaseUsername;
        private string _databaseUserPassword;
        private string _databaseDatabaseName;
        private string _databaseDatabaseHost;

        private Timer timer;
        private string processID;

        public string blockedByUserName;

        private static Database db;
        public static Database GetInstance()
        {

            if (db == null)
            {
                db = new Database();
            }
            return db;
        }

        public Database(bool isSQLite = false)
        {
            isError = false;
            _errorMessage = "";

            this.connection = new SqlConnection();
            //Type t;
            //if (!isSQLite)
            //{
            //    t = Type.GetType("SqlConnection");
            //}
            //else {
            //    this.isSQLite = isSQLite;
            //    t = Type.GetType("SQLiteconnection");
            //}
            //this.connection = Activator.CreateInstance(t);
            //this.connection = new SqlConnection();
        }


        public string getErrorMessage
        {
            get
            {
                return this._errorMessage;
            }

        }

        public void resetError()
        {
            this._errorMessage = "";
            this.isError = false;
        }


        public void init()
        {
            processID = "";
            blockedByUserName = "";
            // Open
            this._errorMessage = "";
            if (this.connection.State.ToString() == "Closed")
            {
                if (this._databaseDatabaseHost.Length == 0)
                {
                    this.isError = true;
                    this._errorMessage = this._errorMessage + "Datenbank Server nicht gesetzt";
                }

                if (this._databaseUsername.Length == 0 && this.isSQLite.Equals(false))
                {
                    this.isError = true;
                    this._errorMessage = this._errorMessage + "Datenbank Benutzername nicht gesetzt";
                }

                if (this._databaseDatabaseName.Length == 0)
                {
                    this.isError = true;
                    this._errorMessage = this._errorMessage + "Datenbank Name nicht gesetzt";

                }

                if (this.isError == true)
                {
                    return;

                }

                string connectioTimout = "";

                string connectionString = @"Data Source=" + this._databaseDatabaseHost + ";Initial Catalog=" + this._databaseDatabaseName + ";User Id=" + this._databaseUsername + ";Password=" + this._databaseUserPassword + ";" + connectioTimout;

                //sqllite verbindungen werden ohne dbuser erstellt
                if (this.isSQLite.Equals(true))
                {
                    connectionString = @"Data Source=" + this._databaseDatabaseName;
                }

                try
                {
                    //Data Source=C:\Users\Jes\Desktop\diary2.db
                    this.connection.ConnectionString = connectionString;

                    //this.connection = new SqlConnection(connectionString);
                    this.connection.Open();

                    command = new SqlCommand("select @@spid", this.connection);
                    processID = command.ExecuteScalar().ToString();
                    command = null;

                }
                catch (Exception dbex)
                {
                    this._errorMessage = dbex.Message.ToString();
                    this.connection.Close();
                    this.isError = true;

                }
            }
        }


        public string getConnectionString()
        {
            string connectioTimout = "";
            string connectionString = @"Data Source=" + this._databaseDatabaseHost + ";Initial Catalog=" + this._databaseDatabaseName + ";User Id=" + this._databaseUsername + ";Password=" + this._databaseUserPassword + ";" + connectioTimout;
            return connectionString;
        }

        //public void setConnectionParameter(DatabaseParamtersStorage parameters)
        //{
        //    this._databaseDatabaseHost = parameters.databaseDatabaseHost.ToString();
        //    this._databaseDatabaseName = parameters.databaseDatabaseName.ToString();
        //    this._databaseUsername = parameters.databaseUsername.ToString();
        //    this._databaseUserPassword = parameters.databaseUserPassword.ToString();

        //}

        public bool test()
        {
            this.init();
            return this.isError;
        }

        /// <summary>
        /// Wird verwendet um den user herauszufinden der den datensatz sprerrt (wenn ein datensatz blockiert ist)
        /// </summary>
        public void initTimer()
        {
            this.timer = new Timer(800); // 0.8 sek
            this.timer.Elapsed += new ElapsedEventHandler(this.timerElapsed);
            this.timer.Start();
        }


        /// <summary>
        /// Führt eine prüfung aus ob ein block in der datenbank stattfindet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerElapsed(object sender, ElapsedEventArgs e)
        {

            this.blockedByUserName = "";
            this.timer.Stop();

            SqlConnection con = null;
            try
            {
                if (this.processID.Length > 0)
                {

                    string sql = "SELECT top 1 "
                                + "loginame as xLoginame, "
                                + "substring(hostname, 1, 12) "
                                + " as xHostname, "
                                + "tmpBl.spid, "
                                + "tmpBl.blocked "
                                + "FROM master..sysprocesses as tmpSys "
                                + "inner join  "
                                    + "(select spid, blocked from master..sysprocesses where blocked <> 0) as tmpBl on tmpSys.spid = tmpBl.blocked"
                                    + "  where tmpBl.spid = " + processID;

                    con = new SqlConnection(this.getConnectionString());
                    con.Open();
                    SqlCommand com = new SqlCommand(sql, con);
                    SqlDataReader reader = com.ExecuteReader();
                    reader.Read();
                    this.blockedByUserName = reader["xLoginame"].ToString().Trim();
                    reader.Close();

                }


            }
            catch (Exception ex)
            {
                // quite
            }
            finally
            {
                try
                {
                    con.Close();
                }
                catch (Exception)
                {

                }
            }
        }


        public ArrayList getArray(string sqlQuery)
        {
            isError = false;
            this.init();
            this._errorMessage = "";
            ArrayList list = new ArrayList();
            /*if (this.isError == true)
            {
                return list;
            }*/

            try
            {

                this.command = new SqlCommand(sqlQuery, this.connection);
                this.reader = this.command.ExecuteReader();

                //System.Windows.Forms.MessageBox.Show(this.reader.VisibleFieldCount.ToString());
                while (reader.Read())
                {
                    SortedList sList = new SortedList();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        sList.Add(reader.GetName(i).ToLower(), reader[i].ToString());
                    }
                    list.Add(sList);
                }
            }
            catch (Exception readerex)
            {
                this._errorMessage = this._errorMessage + readerex.Message.ToString() + "(sqlserver_con - ArrayList getArray ) ";
                this.isError = true;

            }
            finally
            {
                this.connection.Close();
            }
            return list;

        } // end function



        public Database getConnection()
        {

            this.init();
            return this;
        }

        /*public SqlCommand createCommand(string sqlQuery, int timeOut)
        {
            this.command = new SqlCommand(sqlQuery, this.connection);
            this.command.CommandTimeout = timeOut;
            return this.command;
        }*/

        public Database createCommand(string sqlQuery, int timeOut)
        {
            this.command = new SqlCommand(sqlQuery, this.connection);
            this.command.CommandTimeout = timeOut;
            return this;
        }


        public void ExecuteNonQuery(string sqlQuery)
        {

            try
            {
                this.init();
                this.command = new SqlCommand(sqlQuery, this.connection);
                this.command.CommandType = CommandType.Text;
                this.affectedrows = this.command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteNonQuery()
        {
            int _affectedrows = 0;
            try
            {
                _affectedrows = this.command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _affectedrows;
        }

        public object ExecuteScalarAnCloseConnection()
        {
            object result = null;
            try
            {
                // TODO create timer object an look up after one second who is locking

                this.initTimer();
                result = this.command.ExecuteScalar();
                this.timer.Stop();
            }
            catch (Exception ex)
            {
                this.connection.Close();
                throw ex;
            }

            return result;

        }

        public SortedList GetRowAndCloseConnection()
        {

            SortedList row = new SortedList();
            try
            {
                this.initTimer();
                this.reader = this.command.ExecuteReader();
                this.timer.Stop();

                //System.Windows.Forms.MessageBox.Show(this.reader.VisibleFieldCount.ToString());
                while (reader.Read())
                {

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i).ToLower(), reader[i]);
                    }
                    break;
                }
            }
            catch (Exception readerex)
            {
                throw readerex;

            }
            finally
            {
                this.connection.Close();
            }
            return row;

        }

        /// <summary>
        /// Gibt alle datensätze zu einem vorhergehend initialisierten command zurück
        /// </summary>
        /// <returns></returns>
        public ArrayList GetAllAndCloseConnection()
        {
            ArrayList list = new ArrayList();
            try
            {
                this.initTimer();
                this.reader = this.command.ExecuteReader();
                this.timer.Stop();

                while (reader.Read())
                {
                    SortedList sList = new SortedList();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        sList.Add(reader.GetName(i).ToLower(), reader[i].ToString());
                    }
                    list.Add(sList);
                }
            }
            catch (Exception readerex)
            {
                throw readerex;

            }
            finally
            {
                this.connection.Close();
            }

            return list;
        }



        public void closeConnection()
        {
            this.connection.Close();
        }


        public string getOne(string sqlQuery, int commandTimeout)
        {
            string ret = "";
            this.isError = false;
            this.init();
            this._errorMessage = "";
            try
            {
                this.command = new SqlCommand(sqlQuery, this.connection);
                this.command.CommandTimeout = commandTimeout;
                this.reader = this.command.ExecuteReader();

                while (reader.Read())
                {
                    ret = reader[0].ToString();
                }
            }
            catch (Exception readerex)
            {
                this._errorMessage = this._errorMessage + readerex.Message.ToString() + "(sqlserver_con - string getOne ) ";
                this.isError = true;


            }
            finally
            {
                this.connection.Close();
            }


            return ret;

        }

        /// <summary>
        /// use default command timeout 5 Sek
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns>string</returns>
        public string getOne(string sqlQuery)
        {
            return this.getOne(sqlQuery, 5);

        } // end function


        public bool query(string sqlQuery)
        {
            bool result = false;
            this.isError = false;
            this.init();
            this._errorMessage = "";

            try
            {

                this.command = new SqlCommand(sqlQuery, this.connection);
                this.command.CommandType = CommandType.Text;
                this.affectedrows = this.command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception readerex)
            {
                this._errorMessage = this._errorMessage + readerex.Message.ToString() + "";
                //MessageBox.Show(this._errorMessage);
                this.isError = true;

            }
            this.connection.Close();
            return result;

        } // end function

        public SortedList getRow(string sqlQuery)
        {
            this.isError = false;
            this.init();
            this._errorMessage = "";

            SortedList slist2 = new SortedList();

            try
            {

                this.command = new SqlCommand(sqlQuery, this.connection);
                this.reader = this.command.ExecuteReader();

                //System.Windows.Forms.MessageBox.Show(this.reader.VisibleFieldCount.ToString());
                while (reader.Read())
                {

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        slist2.Add(reader.GetName(i).ToLower(), reader[i]);
                    }

                    break;
                }
            }
            catch (Exception readerex)
            {
                this._errorMessage = this._errorMessage + readerex.Message.ToString() + "(sqlserver_con - ArrayList getArray ) ";
                this.isError = true;

            }
            this.connection.Close();
            return slist2;

        } // end function


        public DataTable getDataTable(string sql)
        {
            this.isError = false;
            this.init();

            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            this.connection.Close();
            DataTable dt = ds.Tables[0];
            return dt;

        }


        public string getSQLUpdateString(string TableName, SortedList list)
        {

            string updateString = " update " + TableName + " set ";
            string[] values = new string[list.Count];

            if (list.Count > 0)
            {
                int counter = 0;
                foreach (DictionaryEntry var in list)
                {
                    values[counter] = var.Key + "='" + var.Value.ToString() + "'";
                    counter++;
                }
                updateString += string.Join(",", values);
            }
            return updateString;
        }


        public string getSQLInsertString(string TableName, SortedList list)
        {

            string InsertString = " INSERT into " + TableName + " ";
            string[] valuesKeys = new string[list.Count];
            string[] valuesValues = new string[list.Count];

            if (list.Count > 0)
            {
                int counter = 0;
                foreach (DictionaryEntry var in list)
                {
                    string valueAdd = var.Value.ToString();

                    switch (var.Value.GetType().ToString())
                    {
                        case "System.Decimal":
                            valueAdd = var.Value.ToString().Replace(",", ".");
                            break;
                    }

                    valuesKeys[counter] = (string)var.Key;
                    valuesValues[counter] = "'" + (string)valueAdd + "'";
                    counter++;
                }
                InsertString += "(" + string.Join(",", valuesKeys) + ") VALUES (" + string.Join(",", valuesValues) + ")";
            }
            return InsertString;
        }

    }
}
