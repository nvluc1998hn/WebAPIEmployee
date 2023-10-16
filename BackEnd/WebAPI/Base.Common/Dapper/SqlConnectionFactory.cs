using Base.Common.DBContext;
using Base.Common.Enum;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Dapper
{
    public interface ISqlConnectionFactory
    {
        /// <summary>
        /// Trả về chuổi kết nối theo database name
        /// </summary>
        /// <param name="database">Nếu tham số này không truyền vào thì mặc định là sử dụng ConnString</param>
        /// <returns></returns>
        IDbConnection GetOpenConnection(DatabaseNames database = DatabaseNames.Default);
    }

        public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
        {
            private readonly DbOptions _dbOptions;
            private IDbConnection _connection;

            public SqlConnectionFactory(DbOptions dbOptions)
            {
                _dbOptions = dbOptions;
            }

            /// <summary>
            /// Trả về chuổi kết nối theo database name
            /// </summary>
            /// <param name="database"></param>
            /// <returns></returns>
            public IDbConnection GetOpenConnection(DatabaseNames database = DatabaseNames.Default)
            {
                string _connString = _dbOptions.ConnString;
                switch (database)
                {
                    case DatabaseNames.DatabaseServer1:
                        _connString = _dbOptions.ConnString1;
                        break;

                    case DatabaseNames.DatabaseServer2:
                        _connString = _dbOptions.ConnString2;
                        break;

                    case DatabaseNames.DatabaseServer3:
                        _connString = _dbOptions.ConnString3;
                        break;

                    case DatabaseNames.DatabaseServer4:
                        _connString = _dbOptions.ConnString4;
                        break;

                    case DatabaseNames.DatabaseServer5:
                        _connString = _dbOptions.ConnString5;
                        break;

                    case DatabaseNames.DatabaseServer6:
                        _connString = _dbOptions.ConnString6;
                        break;
                    case DatabaseNames.DatabaseServer100:
                        _connString = _dbOptions.ConnString100;
                        break;

                    default:
                        _connString = _dbOptions.ConnString;
                        break;
                }

                _connection = new SqlConnection(_connString);
                _connection.Open();

                return _connection;
            }

            public void Dispose()
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Dispose();
                }
            }
        }
}
