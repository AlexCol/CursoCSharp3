using System;
using Emprestimos.Core.src.Domain;
using Emprestimos.Core.src.Processor;
using Xunit;

namespace Emprestimos.Core.Tests.src {
    public class EmprestimosTests {
        EmprestimoProcessor _processor;
        public EmprestimosTests() {
            _processor = new EmprestimoProcessor();
        }
        [Fact]
        public void DeveRetornarDadosEmprestimosComValoresDaRequisicao() {
            // Organizar
            var req = new EmprestimoReq() {
                PrimeiroNome = "Diogo",
                UltimoNome = "Santos",
                Email = "lalaala@yahoo.com.br",
                Data = DateTime.Now
            };

            // Agir
            EmprestimoResult result = _processor.LerDados(req);

            // Afirmar
            Assert.NotNull(result);
            Assert.Equal(req.PrimeiroNome, result.PrimeiroNome);
            Assert.Equal(req.UltimoNome, result.UltimoNome);
            Assert.Equal(req.Email, result.Email);
            Assert.Equal(req.Data, result.Data);
        }

        [Fact]
        public void DeveRetornarUmaExceptionSeOReqForNulo() {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.LerDados(null));
            Assert.Equal("req", exception.ParamName);
        }
    }
}
