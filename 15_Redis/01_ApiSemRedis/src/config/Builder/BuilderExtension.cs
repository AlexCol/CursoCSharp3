using _01_ApiSemRedis.src.config.Connection;
using _01_ApiSemRedis.src.repository;
using _01_ApiSemRedis.src.services;

namespace _01_ApiSemRedis.src.config.Builder;

public static class BuilderExtension {
  public static void AddDependencies(this WebApplicationBuilder builder) {
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddControllers();

    builder.Services.AddScoped<IConnectionFactory, MySqlFactory>();
    builder.Services.AddScoped<IConnectRepository, ConnectRepository>();
    builder.Services.AddScoped<IConnectService, ConnectService>();

    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();
  }
}
