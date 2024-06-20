using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emprestimos.Core.src.Domain;

namespace Emprestimos.Core.src.Repository;

public interface IEmprestimoRepository {
  public void Salvar(Emprestimo emprestimo);
}
