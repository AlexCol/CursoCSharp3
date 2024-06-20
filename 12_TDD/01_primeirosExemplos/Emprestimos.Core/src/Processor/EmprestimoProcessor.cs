using Emprestimos.Core.src.Domain;
using Emprestimos.Core.src.Repository;

namespace Emprestimos.Core.src.Processor;

public class EmprestimoProcessor {
  IEmprestimoRepository _repository;

  public EmprestimoProcessor(IEmprestimoRepository repository) {
    _repository = repository;
  }

  public EmprestimoResult SalvarDados(EmprestimoReq req) {
    if (req == null) throw new ArgumentNullException(nameof(req));

    _repository.Salvar(new Emprestimo() {
      PrimeiroNome = req.PrimeiroNome,
      UltimoNome = req.UltimoNome,
      Email = req.Email,
      Data = req.Data
    });

    return new EmprestimoResult() {
      PrimeiroNome = req.PrimeiroNome,
      UltimoNome = req.UltimoNome,
      Email = req.Email,
      Data = req.Data
    };
  }

  public bool MaiorIdade(int idade) {
    return idade >= 18;
  }
}
