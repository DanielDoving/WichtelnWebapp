using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace WichtelnWebapp.Server
{
    public class SqlController : ISqlController
    {
        private readonly IConfiguration _config;

        public string conStr_name { get; set; } = "DefaultConnection";
        public SqlController(IConfiguration config)
        {
            _config = config;
        }

        // Get Query Data from DB as List (generic)
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            string conStr = _config.GetConnectionString(conStr_name);
            using (IDbConnection connection = new SqlConnection(conStr))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);
                return data.ToList();
            }
        }

        // Update, Delete, Insert to DB
        public async Task SaveData<T>(string sql, T parameters)
        {
            string conStr = _config.GetConnectionString(conStr_name);
            using (IDbConnection connection = new SqlConnection(conStr))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
