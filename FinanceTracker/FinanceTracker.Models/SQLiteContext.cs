using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SQLite;

namespace FinanceTracker.Models
{
    internal static class SQLiteContext
    {
        private static SQLiteAsyncConnection _conn = null;

        private static void Initialize()
        {
            if (_conn != null)
                return;

            string connStr = ConfigurationManager.AppSettings["dbConnection"];
            _conn = new SQLiteAsyncConnection(connStr);
            _conn.CreateTableAsync<TransactionModel>();
        }
        private static void EnsureValidMap(Type t)
        {
            var mappings = _conn.TableMappings;
            foreach (var mapping in mappings)
            {
                if (mapping.MappedType == t)
                    return;
            }
            _conn.CreateTableAsync(t);
        }

        public static int Insert<Model>(Model m)
        {
            Initialize();
            EnsureValidMap(typeof(Model));

            return _conn.InsertAsync(m).Result;
        }
    }
}
