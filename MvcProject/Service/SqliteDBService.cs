using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Configuration;

namespace MvcProject.Service
{
    class SqliteDBService
    {
        private static SQLiteConnection connection;
        protected static SqliteDBService instance = null;

        public static SqliteDBService GetInstance()
        {
            if (SqliteDBService.instance == null)
            {
                SqliteDBService.instance = new SqliteDBService();
            }

            return SqliteDBService.instance;
        }

        public static string getConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Hashtable DbRequestToGiveData(string query) // метод с получением данных. Запросы SELECT
        {
            SqliteDBService.connection = new SQLiteConnection(SqliteDBService.getConnectionString());
            try
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Hashtable hashtable = new Hashtable();

                    while (reader.Read())
                    {
                        string[] values = new string[reader.FieldCount];
                        NameValueCollection row = reader.GetValues();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            values[i] = row[i];
                        }
                        hashtable.Add(reader.StepCount-1, values);
                    }
                    reader.Close();
                    SqliteDBService.connection.Close();

                    return hashtable;
                }
                reader.Close();
                SqliteDBService.connection.Close();

                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                SqliteDBService.connection.Dispose();
            }
        }

        public int DbRequestWithoutGiveData(string query) // метод без получения данных. Остальные запросы
        {
            try
            {
                SqliteDBService.connection = new SQLiteConnection(SqliteDBService.getConnectionString());
                SQLiteCommand command = new SQLiteCommand(query, connection);
                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();

                return result;
            }
            catch
            {
                return -1;
            }
            finally
            {
                SqliteDBService.connection.Dispose();
            }
        }
    }
}
