namespace Confluence.Tests.Domain.Entities;

using Confluence.Lib.Domain.Entities;
using Confluence.Lib.Domain.ValueObjects;
using Xunit;
using FluentAssertions;

/// <summary>
/// Testes para a Entity Artigo.
/// Cobre: criação, validação, atualização, ciclo de vida (publicação).
/// </summary>
public sealed class ArtigoTests
{
    private readonly Categoria _categoriaValida = Categoria.Criar("Backend");
    private readonly ConteudoArtigo _conteudoValido = ConteudoArtigo.Criar(
        "Este é um resumo do artigo",
        "# Conteúdo do Artigo\n\nMarkdown válido");

    // ====== TESTES POSITIVOS ======

    [Fact(DisplayName = "Criar deve retornar Artigo válido")]
    public void Criar_ComDadosValidos_DeveRetornarArtigoValido()
    {
        // Act
        var artigo = Artigo.Criar("Título do Artigo", _conteudoValido, _categoriaValida);

        // Assert
        artigo.Should().NotBeNull();
        artigo.Titulo.Should().Be("Título do Artigo");
        artigo.Conteudo.Should().Be(_conteudoValido);
        artigo.Categoria.Should().Be(_categoriaValida);
        artigo.IsPublico.Should().BeFalse();
        artigo.Id.Should().NotBeNull();
        artigo.DataCriacao.Should().BeBefore(DateTime.UtcNow.AddSeconds(1));
    }

    [Fact(DisplayName = "Artigo deve ter identidade única")]
    public void Criar_DeveRetornarArtigosComIdsUnicos()
    {
        // Act
        var artigo1 = Artigo.Criar("Artigo 1", _conteudoValido, _categoriaValida);
        var artigo2 = Artigo.Criar("Artigo 2", _conteudoValido, _categoriaValida);

        // Assert
        artigo1.Id.Should().NotBe(artigo2.Id);
    }

    [Fact(DisplayName = "AtualizarTitulo deve alterar título")]
    public void AtualizarTitulo_ComValorValido_DeveAlterarTitulo()
    {
        // Arrange
        var artigo = Artigo.Criar("Título Original", _conteudoValido, _categoriaValida);
        var novoTitulo = "Título Atualizado";
        var dataAtualizacaoAnterior = artigo.DataAtualizacao;

        // Act
        System.Threading.Thread.Sleep(1); // Garante que há diferença de tempo
        artigo.AtualizarTitulo(novoTitulo);

        // Assert
        artigo.Titulo.Should().Be(novoTitulo);
        artigo.DataAtualizacao.Should().BeOnOrAfter(dataAtualizacaoAnterior);
    }

    [Fact(DisplayName = "AtualizarConteudo deve alterar conteúdo")]
    public void AtualizarConteudo_ComValorValido_DeveAlterarConteudo()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida);
        var novoConteudo = ConteudoArtigo.Criar(
            "Novo resumo",
            "# Novo Conteúdo");

        // Act
        artigo.AtualizarConteudo(novoConteudo);

        // Assert
        artigo.Conteudo.Should().Be(novoConteudo);
    }

    [Fact(DisplayName = "AtualizarCategoria deve alterar categoria")]
    public void AtualizarCategoria_ComValorValido_DeveAlterarCategoria()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida);
        var novaCategoria = Categoria.Criar("Frontend");

        // Act
        artigo.AtualizarCategoria(novaCategoria);

        // Assert
        artigo.Categoria.Should().Be(novaCategoria);
    }

    [Fact(DisplayName = "Publicar deve marcar artigo como público")]
    public void Publicar_ComArtigoPrivado_DevePublicar()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida, isPublico: false);

        // Act
        artigo.Publicar();

        // Assert
        artigo.IsPublico.Should().BeTrue();
    }

    [Fact(DisplayName = "Despublicar deve marcar artigo como privado")]
    public void Despublicar_ComArtigoPublico_DeveDespublicar()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida, isPublico: true);

        // Act
        artigo.Despublicar();

        // Assert
        artigo.IsPublico.Should().BeFalse();
    }

    [Fact(DisplayName = "Igualdade deve comparar por identidade")]
    public void Igualdade_DeveCompararPorIdentidade()
    {
        // Arrange
        var id = ArticuloId.Criar();
        var artigo1 = Artigo.ReconstruirDoRepositorio(
            id, "Título 1", _conteudoValido, _categoriaValida, false,
            DateTime.UtcNow, DateTime.UtcNow);
        var artigo2 = Artigo.ReconstruirDoRepositorio(
            id, "Título 2", _conteudoValido, _categoriaValida, false,
            DateTime.UtcNow, DateTime.UtcNow);

        // Act & Assert
        artigo1.Should().Be(artigo2);
    }

    // ====== TESTES NEGATIVOS ======

    [Fact(DisplayName = "Criar com título vazio deve lançar exceção")]
    public void Criar_ComTituloVazio_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Artigo.Criar("", _conteudoValido, _categoriaValida))
            .Message.Should().Contain("não pode ser vazio");
    }

    [Fact(DisplayName = "Criar com título muito curto deve lançar exceção")]
    public void Criar_ComTituloMusitoCurto_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Artigo.Criar("ABC", _conteudoValido, _categoriaValida))
            .Message.Should().Contain("entre 5 e 200 caracteres");
    }

    [Fact(DisplayName = "Criar com título muito longo deve lançar exceção")]
    public void Criar_ComTituloMuitoLongo_DeveLancarArgumentException()
    {
        // Arrange
        var tituloLongo = new string('A', 201);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Artigo.Criar(tituloLongo, _conteudoValido, _categoriaValida))
            .Message.Should().Contain("entre 5 e 200 caracteres");
    }

    [Fact(DisplayName = "Criar com conteúdo null deve lançar exceção")]
    public void Criar_ComConteudoNull_DeveLancarArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            Artigo.Criar("Título", null!, _categoriaValida));
    }

    [Fact(DisplayName = "Publicar artigo já publicado deve lançar exceção")]
    public void Publicar_ComArtigoJaPublicado_DeveLancarInvalidOperationException()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida, isPublico: true);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => artigo.Publicar())
            .Message.Should().Contain("já está publicado");
    }

    [Fact(DisplayName = "Despublicar artigo já privado deve lançar exceção")]
    public void Despublicar_ComArtigoJaPrivado_DeveLancarInvalidOperationException()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida, isPublico: false);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => artigo.Despublicar())
            .Message.Should().Contain("não está publicado");
    }

    [Fact(DisplayName = "AtualizarTitulo com null deve lançar exceção")]
    public void AtualizarTitulo_ComNull_DeveLancarArgumentNullException()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => artigo.AtualizarTitulo(null!));
    }

    [Fact(DisplayName = "AtualizarConteudo com null deve lançar exceção")]
    public void AtualizarConteudo_ComNull_DeveLancarArgumentNullException()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => artigo.AtualizarConteudo(null!));
    }

    [Fact(DisplayName = "AtualizarCategoria com null deve lançar exceção")]
    public void AtualizarCategoria_ComNull_DeveLancarArgumentNullException()
    {
        // Arrange
        var artigo = Artigo.Criar("Título", _conteudoValido, _categoriaValida);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => artigo.AtualizarCategoria(null!));
    }
}
