using Dapper;
using EmployeeManagement.Common.Enums;
using EmployeeManagement.Database.Context.DbOptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Database.Dapper
{
    public static class DapperExtensions
    {
        private static ISqlConnectionFactory _sqlConnectionFactory;

        public static void Initialize(DbOptions dapperOptions)
        {
            _sqlConnectionFactory = new SqlConnectionFactory(dapperOptions);
        }

        public static IEnumerable<T> QueryDapper<T>(string sql, object param = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return con.Query<T>(sql, param);
        }

        public static IEnumerable<T> QueryDapperReport<T>(string sql, object param = null, DatabaseNames dbNameReport = DatabaseNames.DatabaseServer1)
        {
            var con = _sqlConnectionFactory.GetOpenConnection(dbNameReport);
            return con.Query<T>(sql, param);
        }

        public static T QueryFirstOrDefault<T>(string sql, object parameters = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return con.QueryFirstOrDefault<T>(sql, parameters);
        }

        public static T QueryFirstOrDefaultStoreProc<T>(string store, object parameters = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return con.QueryFirstOrDefault<T>(store, parameters, commandType: CommandType.StoredProcedure);
        }

        public static IEnumerable<T> QueryDapperStoreProc<T>(string store, object param = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return con.Query<T>(store, param, commandType: CommandType.StoredProcedure);
        }

        public static IEnumerable<T> QueryDapperStoreProcReport<T>(string store, object param = null, DatabaseNames dbNameReport = DatabaseNames.DatabaseServer1)
        {
            var con = _sqlConnectionFactory.GetOpenConnection(dbNameReport);
            var result = con.Query<T>(store, param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> QueryMultiple<T1, T2, T3, T4>(string store, object param = null)
        {
            Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>> result = null;
            try
            {
                var con = _sqlConnectionFactory.GetOpenConnection();

                using (var qr = con.QueryMultiple(store, param, commandType: CommandType.StoredProcedure))
                {
                    var t1 = qr.Read<T1>();
                    var t2 = qr.Read<T2>();
                    var t3 = qr.Read<T3>();
                    var t4 = qr.Read<T4>();

                    result = new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>(t1, t2, t3, t4);
                }
            }
            catch
            {
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>(new List<T1>(), new List<T2>(), new List<T3>(), new List<T4>());
            }
            return result;
        }

        public static Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> QueryMultiple<T1, T2, T3>(string store, object param = null)
        {
            Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> result = null;

            try
            {
                var con = _sqlConnectionFactory.GetOpenConnection();

                using (var qr = con.QueryMultiple(store, param, commandType: CommandType.StoredProcedure))
                {
                    var t1 = qr.Read<T1>();
                    var t2 = qr.Read<T2>();
                    var t3 = qr.Read<T3>();

                    result = new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(t1, t2, t3);
                }
            }
            catch
            {
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(new List<T1>(), new List<T2>(), new List<T3>());
            }
            return result;
        }

        /// <summary>
        /// thực thi các store proceduce
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="isReport">if set to <c>true</c> [is report].</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  1/3/2019   created
        /// </Modified>
        public static int ExecuteDapperStoreProc(string store, object param = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            int result = 0;
            try
            {
                if (param == null)
                    param = new DynamicParameters();

                ((DynamicParameters)param).Add(name: "@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                con.Execute(store, param, commandType: CommandType.StoredProcedure);

                result = ((DynamicParameters)param).Get<int>("@Result");
            }
            catch
            {
                return result;
            }
            return result;
        }

        public static int ExecuteNonQueryDapperStoreProc(string store, object param = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            int result = 0;
            try
            {
                if (param == null)
                    param = new DynamicParameters();
                result = con.Execute(store, param, commandType: CommandType.StoredProcedure);
            }
            catch
            {
                return result;
            }
            return result;
        }

        public static int ExecuteNonQueryDapperStoreProcReport(string store, object param = null, DatabaseNames dbNameReport = DatabaseNames.DatabaseServer1)
        {
            var con = _sqlConnectionFactory.GetOpenConnection(dbNameReport);
            int result = 0;
            try
            {
                if (param == null)
                    param = new DynamicParameters();
                result = con.Execute(store, param, commandType: CommandType.StoredProcedure);
            }
            catch
            {
                return result;
            }
            return result;
        }

        public static object ExecuteScalarDapperStoreProc(string store, object param = null)
        {
            try
            {
                var con = _sqlConnectionFactory.GetOpenConnection();
                if (param == null)
                    param = new DynamicParameters();

                var result = con.ExecuteScalar(store, param, commandType: CommandType.StoredProcedure);

                return result;
            }
            catch
            {
                return 0;
            }
        }

        public static int ExecuteDapper(string sql, object param = null)
        {
            try
            {
                var con = _sqlConnectionFactory.GetOpenConnection();
                if (param == null)
                    param = new DynamicParameters();

                var result = con.Execute(sql, param);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        public static async Task<IEnumerable<T>> QueryDapperAsync<T>(string sql, object param = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return await con.QueryAsync<T>(sql, param);
        }

        public static async Task<IEnumerable<T>> QueryDapperStoreProcAsync<T>(string store, object param = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return await con.QueryAsync<T>(store, param, commandType: CommandType.StoredProcedure);
        }

        public static async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return await con.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public static async Task<T> QueryFirstOrDefaultStoreProcAsync<T>(string store, object parameters = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return await con.QueryFirstOrDefaultAsync<T>(store, parameters, commandType: CommandType.StoredProcedure);
        }

        public static async Task<int> ExecuteDapperAsync(string sql, object param = null)
        {
            var con = _sqlConnectionFactory.GetOpenConnection();
            return await con.ExecuteAsync(sql, param);
        }
    }

}

