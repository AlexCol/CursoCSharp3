using System.ComponentModel.DataAnnotations;

namespace _01_ApiSemRedis.src.model;

public class Produtos {
  [Key]
  public Guid Id { get; set; }
  public string Nome { get; set; } = "";
  public decimal Preco { get; set; }
  public bool Status { get; set; }
}
