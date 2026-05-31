namespace Confluence.Tests.Domain.Factories;

using Confluence.Lib.Domain.Factories;
using Confluence.Lib.Domain.ValueObjects;
using Xunit;
using FluentAssertions;

/// <summary>
/// Testes para a Factory FabricaBaseConhecimento.
/// Cobre: criação com validação, criação com artigo inicial.
/// </summary>
public sealed class FabricaBaseConhecimentoTests
{
    private readonly IFabricaBaseConhecimento _fabrica = new FabricaBaseConhecimento();
    private readonly UsuarioId _usuarioValido = UsuarioId.Criar();
    private readonly Categoria _categoriaValida = Categoria.Criar("Backend");
    private readonly ConteudoArtigo _conteudoValido = ConteudoArtigo.Criar(
        "Resumo válido aqui",
        "# Conteúdo válido");

    // ====== TESTES POSITIVOS ======

    [Fact(DisplayName = "CriarNovaBase deve criar BaseConhecimento validada")]
    public void CriarNovaBase_ComDadosValidos_DeveRetornarBaseValida()
    {
        // Act
        var base_ = _fabrica.CriarNovaBase(_usuarioValido, "Nova Base", "Descrição válida");

        // Assert
        base_.Should().NotBeNull();
        base_.Nome.Should().Be("Nova Base");
        base_.Descricao.Should().Be("Descrição válida");
        base_.IdUsuarioPropietario.Should().Be(_usuarioValido);
        base_.IsPublica.Should().BeFalse();
    }

    [Fact(DisplayName = "CriarNovaBase com isPublica=true deve criar pública")]
    public void CriarNovaBase_ComIsPublica_DeveRetornarPublica()
    {
        // Act
        var base_ = _fabrica.CriarNovaBase(_usuarioValido, "Base", "Descrição válida para testes", isPublica: true);

        // Assert
        base_.IsPublica.Should().BeTrue();
    }

    [Fact(DisplayName = "CriarBaseComArtigoInicial deve criar base com artigo")]
    public void CriarBaseComArtigoInicial_ComDadosValidos_DeveRetornarComArtigo()
    {
        // Act
        var base_ = _fabrica.CriarBaseComArtigoInicial(
            _usuarioValido,
            "Minha Base",
            "Descrição completa",
            "Meu Artigo",
            _conteudoValido,
            _categoriaValida);

        // Assert
        base_.Should().NotBeNull();
        base_.Nome.Should().Be("Minha Base");
        base_.Artigos.Should().HaveCount(1);
        base_.Artigos[0].Titulo.Should().Be("Meu Artigo");
    }

    [Fact(DisplayName = "ValidarDadosBase não deve lançar para dados válidos")]
    public void ValidarDadosBase_ComDadosValidos_NaoDeveLancar()
    {
        // Act
        var acao = () => _fabrica.ValidarDadosBase(_usuarioValido, "Nome", "Descrição válida");

        // Assert
        acao.Should().NotThrow();
    }

    // ====== TESTES NEGATIVOS ======

    [Fact(DisplayName = "CriarNovaBase com usuário null deve lançar exceção")]
    public void CriarNovaBase_ComUsuarioNull_DeveLancarArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _fabrica.CriarNovaBase(null!, "Nome", "Descrição"));
    }

    [Fact(DisplayName = "CriarNovaBase com nome vazio deve lançar exceção")]
    public void CriarNovaBase_ComNomeVazio_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _fabrica.CriarNovaBase(_usuarioValido, "", "Descrição"))
            .Message.Should().Contain("não pode ser vazio");
    }

    [Fact(DisplayName = "CriarNovaBase com descrição vazia deve lançar exceção")]
    public void CriarNovaBase_ComDescricaoVazia_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _fabrica.CriarNovaBase(_usuarioValido, "Nome", ""))
            .Message.Should().Contain("não pode ser vazia");
    }

    [Fact(DisplayName = "CriarNovaBase com nome muito curto deve lançar exceção")]
    public void CriarNovaBase_ComNomeMusitoCurto_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _fabrica.CriarNovaBase(_usuarioValido, "AB", "Descrição válida"))
            .Message.Should().Contain("entre 3 e 100 caracteres");
    }

    [Fact(DisplayName = "CriarNovaBase com descrição muito curta deve lançar exceção")]
    public void CriarNovaBase_ComDescricaoMusitoCurta_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _fabrica.CriarNovaBase(_usuarioValido, "Nome", "Curta"))
            .Message.Should().Contain("entre 10 e 500 caracteres");
    }

    [Fact(DisplayName = "CriarBaseComArtigoInicial com conteúdo null deve lançar exceção")]
    public void CriarBaseComArtigoInicial_ComConteudoNull_DeveLancarArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _fabrica.CriarBaseComArtigoInicial(
                _usuarioValido,
                "Minha Base",
                "Descrição completa aqui",
                "Artigo",
                null!,
                _categoriaValida));
    }

    [Fact(DisplayName = "CriarBaseComArtigoInicial com categoria null deve lançar exceção")]
    public void CriarBaseComArtigoInicial_ComCategoriaNull_DeveLancarArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _fabrica.CriarBaseComArtigoInicial(
                _usuarioValido,
                "Minha Base",
                "Descrição completa aqui",
                "Artigo",
                _conteudoValido,
                null!));
    }

    [Fact(DisplayName = "ValidarDadosBase com usuário null deve lançar exceção")]
    public void ValidarDadosBase_ComUsuarioNull_DeveLancarArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            _fabrica.ValidarDadosBase(null!, "Nome", "Descrição"));
    }
}
