using Emprestimos.Core.src.Domain;

namespace Emprestimos.Core.src.Processor;

public class EmprestimoProcessor {
  public EmprestimoResult LerDados(EmprestimoReq req) {
    if (req == null) throw new ArgumentNullException(nameof(req));

    return new EmprestimoResult() {
      PrimeiroNome = req.PrimeiroNome,
      UltimoNome = req.UltimoNome,
      Email = req.Email,
      Data = req.Data
    };
  }
}
