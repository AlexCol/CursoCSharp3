using System.Data;
using MySqlConnector;

namespace _01_ApiSemRedis.src.config.Connection;

public class MySqlFactory : ConnectionFactory {

  public MySqlFactory(IConfiguration _configuration) : base(_configuration) { }

  public override IDbConnection Connect() {
    string connectionString = configuration.GetConnectionString("MySql") ?? throw new ArgumentNullException("MySql connection string not found");
    connection = new MySqlConnection(connectionString);
    connection.Open();
    return connection;
  }
}
