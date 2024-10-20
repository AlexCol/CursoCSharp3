using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace _01_ApiSemRedis.src.config.Connection;

public interface IConnectionFactory {
  IDbConnection Connect();
  void Dispose();
  Task<IEnumerable<T>> ExecuteQuery<T>(string query, object parameters);
}

public abstract class ConnectionFactory : IConnectionFactory {
  protected readonly IConfiguration configuration;
  protected IDbConnection? connection;

  public ConnectionFactory(IConfiguration _configuration) {
    configuration = _configuration;
  }

  public abstract IDbConnection Connect();

  public void Dispose() {
    connection?.Close();
  }

  public async Task<IEnumerable<T>> ExecuteQuery<T>(string query, object parameters) {
    using var connection = Connect();
    return await connection.QueryAsync<T>(query, parameters);
  }
}
