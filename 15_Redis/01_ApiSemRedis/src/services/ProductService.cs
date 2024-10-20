using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _01_ApiSemRedis.src.model;
using _01_ApiSemRedis.src.repository;

namespace _01_ApiSemRedis.src.services;

public interface IProductService {
  Task NovoProdutoAsync(Produtos produto);
}

public class ProductService : IProductService {
  private readonly IProductRepository _productRepository;

  public ProductService(IProductRepository productRepository) {
    _productRepository = productRepository;
  }

  public async Task NovoProdutoAsync(Produtos produto) {

    /*realiza validações - erros disparam throw*/
    await _productRepository.NovoProdutoAsync(produto);
  }
}
