using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_ApiSemRedis.src.config.Connection;
using Dapper;

namespace _01_ApiSemRedis.src.repository;

public interface IConnectRepository {
  string TestConnect();
}

public class ConnectRepository : IConnectRepository {
  private readonly IConnectionFactory _connectionFactory;

  public ConnectRepository(IConnectionFactory connectionFactory) {
    _connectionFactory = connectionFactory;
  }

  public string TestConnect() {
    using var connection = _connectionFactory.Connect();
    connection.Query("SELECT 1");
    return "Connected to database";
  }
}
