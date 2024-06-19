using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emprestimos.Core.src.Domain;

public class EmprestimoResult {
  public string PrimeiroNome { get; internal set; }
  public string UltimoNome { get; internal set; }
  public string Email { get; internal set; }
  public DateTime Data { get; internal set; }
}
