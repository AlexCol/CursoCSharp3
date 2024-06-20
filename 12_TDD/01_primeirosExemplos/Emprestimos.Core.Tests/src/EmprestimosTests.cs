using System;
using Emprestimos.Core.src.Domain;
using Emprestimos.Core.src.Processor;
using Emprestimos.Core.src.Repository;
using Moq;
using Xunit;

namespace Emprestimos.Core.Tests.src;

public class EmprestimosTests {
    EmprestimoProcessor _processor;
    Mock<IEmprestimoRepository> _repoMock;
    EmprestimoReq emprestimoReq;

    public EmprestimosTests() {
        _repoMock = new Mock<IEmprestimoRepository>();
        _processor = new EmprestimoProcessor(_repoMock.Object);
        emprestimoReq = new EmprestimoReq() {
            PrimeiroNome = "Diogo",
            UltimoNome = "Santos",
            Email = "lalaala@yahoo.com.br",
            Data = DateTime.Now
        };
    }

    [Fact]
    public void DeveRetornarDadosEmprestimosComValoresDaRequisicao() {
        // Agir
        EmprestimoResult result = _processor.SalvarDados(emprestimoReq);

        // Afirmar
        Assert.NotNull(result);
        Assert.Equal(emprestimoReq.PrimeiroNome, result.PrimeiroNome);
        Assert.Equal(emprestimoReq.UltimoNome, result.UltimoNome);
        Assert.Equal(emprestimoReq.Email, result.Email);
        Assert.Equal(emprestimoReq.Data, result.Data);
    }

    [Fact]
    public void DeveRetornarUmaExceptionSeOReqForNulo() {
        // Organizar e Agir
        var exception = Assert.Throws<ArgumentNullException>(() => _processor.SalvarDados(null));

        // Afirmar
        Assert.Equal("req", exception.ParamName);
    }

    [Fact]
    public void DeveSalvarEmprestimoNoBancoDeDados() {
        // Organizar
        //! Variável para armazenar o empréstimo que será salvo
        Emprestimo emprestimoSalvo = null;

        //! Configurando o mock para o método Salvar do repositório
        //! It.IsAny<Emprestimo>() permite qualquer instância de Emprestimo
        //! Callback captura o argumento passado para o método Salvar e armazena na variável emprestimoSalvo (ou pode ser qualquer código que simule o que o salvar faria)
        _repoMock.Setup(x => x.Salvar(It.IsAny<Emprestimo>()))
            .Callback<Emprestimo>(x => emprestimoSalvo = x);

        // Agir        
        //! Chama o método SalvarDados no _processor, que internamente chama _repository.Salvar
        _processor.SalvarDados(emprestimoReq);

        // Afirmar
        //! Verifica se o método Salvar foi chamado exatamente uma vez no mock _repoMock
        _repoMock.Verify(x => x.Salvar(It.IsAny<Emprestimo>()), Times.Once());

        Assert.NotNull(emprestimoSalvo);
        Assert.Equal(emprestimoReq.PrimeiroNome, emprestimoSalvo.PrimeiroNome);
        Assert.Equal(emprestimoReq.UltimoNome, emprestimoSalvo.UltimoNome);
        Assert.Equal(emprestimoReq.Email, emprestimoSalvo.Email);
        Assert.Equal(emprestimoReq.Data, emprestimoSalvo.Data);
    }

    //Indica que o método é um teste baseado em teoria, permitindo executar o mesmo teste com diferentes conjuntos de dados.
    //Metodos com Theory PRECISAM ter parametros, que serão alimentados com InlineData
    [Theory]
    [InlineData(1, false)]
    [InlineData(18, true)]
    [InlineData(19, true)]
    public void DeveSerMaiorDeIdade(int idade, bool valorEsperado) {
        var resposta = _processor.MaiorIdade(idade);
        Assert.Equal(valorEsperado, resposta);
    }

}