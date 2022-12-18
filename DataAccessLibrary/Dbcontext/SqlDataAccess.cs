using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccessLibrary.Models;
using System.IO.Pipes;

namespace DataAccessLibrary.Dbcontext
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration config;

        public SqlDataAccess(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = config.GetConnectionString(connectionStringName);

            using IDbConnection connection = new SqlConnection(connectionString);

            var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            return rows.ToList();
        }

        public async Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = config.GetConnectionString(connectionStringName);

            using IDbConnection connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<UserModel>> LoadUserWithAddress<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = config.GetConnectionString(connectionStringName);

            using IDbConnection connection = new SqlConnection(connectionString);

            var userDict = new Dictionary<int, UserModel>();

            var rows = await connection.QueryAsync<UserModel,AddressModel , UserModel>(
                storedProcedure,
                (user, address) =>
                {
                    if(!userDict.TryGetValue(user.Id, out var currentUser))
                    {
                        currentUser = user;
                        userDict.Add(currentUser.Id, currentUser);
                    }

                    currentUser.AddressModels.Add(address);
                    return currentUser;
                },
                parameters,
                commandType: CommandType.StoredProcedure);

            return rows.Distinct().ToList();
        }

    }
}
