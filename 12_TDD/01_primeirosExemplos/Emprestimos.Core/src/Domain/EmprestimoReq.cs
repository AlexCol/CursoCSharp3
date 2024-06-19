using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emprestimos.Core.src.Domain;

public class EmprestimoReq {
  public string PrimeiroNome { get; set; }
  public string UltimoNome { get; set; }
  public string Email { get; set; }
  public DateTime Data { get; set; }
}
