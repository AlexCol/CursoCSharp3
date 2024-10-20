using _01_ApiSemRedis.src.model;
using _01_ApiSemRedis.src.services;
using Microsoft.AspNetCore.Mvc;

namespace _01_ApiSemRedis.src.controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase {
  private readonly IProductService _productService;

  public ProductController(IProductService productService) {
    _productService = productService;
  }

  [HttpPost]
  public async Task<ActionResult> NovoProdutoAsync([FromBody] Produtos produto) {
    try {
      await _productService.NovoProdutoAsync(produto);
      return Ok(new { mensagem = "Produto inserido com sucesso" });
    } catch (Exception e) {
      return BadRequest(new { mensagen = e.Message });
    }
  }
}
