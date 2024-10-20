using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_ApiSemRedis.src.config.Connection;
using _01_ApiSemRedis.src.model;

namespace _01_ApiSemRedis.src.repository;

public interface IProductRepository {
  Task NovoProdutoAsync(Produtos produto);
}

public class ProductRepository : IProductRepository {
  private readonly IConnectionFactory _connectionFactory;

  public ProductRepository(IConnectionFactory connectionFactory) {
    _connectionFactory = connectionFactory;
  }

  public async Task NovoProdutoAsync(Produtos produto) {
    try {
      await _connectionFactory.ExecuteQuery<Produtos>("INSERT INTO produtos (nome, preco, status) VALUES (@Nome, @Preco, @Status)", produto);
    } catch (Exception e) {
      throw new Exception("Erro ao inserir produto", e);
    }
  }
}
