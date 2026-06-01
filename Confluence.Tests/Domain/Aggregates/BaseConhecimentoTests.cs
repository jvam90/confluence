namespace Confluence.Tests.Domain.Aggregates;

using Confluence.Lib.Domain.Aggregates;
using Confluence.Lib.Domain.Entities;
using Confluence.Lib.Domain.ValueObjects;
using Xunit;
using FluentAssertions;

/// <summary>
/// Testes para o Aggregate Root BaseConhecimento.
/// Cobre: invariantes, operações com artigos, ciclo de vida.
/// </summary>
public sealed class BaseConhecimentoTests
{
    private readonly UsuarioId _usuarioId = UsuarioId.Criar();
    private readonly Categoria _categoria = Categoria.Criar("Backend");
    private readonly ConteudoArtigo _conteudo = ConteudoArtigo.Criar(
        "Resumo do artigo",
        "# Conteúdo");

    // ====== TESTES POSITIVOS ======

    [Fact(DisplayName = "Criar deve retornar BaseConhecimento válida")]
    public void Criar_ComDadosValidos_DeveRetornarBaseValida()
    {
        // Act
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Descrição válida");

        // Assert
        base_.Should().NotBeNull();
        base_.Nome.Should().Be("Minha Base");
        base_.Descricao.Should().Be("Descrição válida");
        base_.IdUsuarioPropietario.Should().Be(_usuarioId);
        base_.IsPublica.Should().BeFalse();
        base_.Artigos.Should().BeEmpty();
    }

    [Fact(DisplayName = "AdicionarArtigo deve adicionar artigo ao agregado")]
    public void AdicionarArtigo_ComArtigoValido_DeveAdicionarAoAgregado()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");
        var artigo = Artigo.Criar("Título", _conteudo, _categoria);

        // Act
        base_.AdicionarArtigo(artigo);

        // Assert
        base_.Artigos.Should().HaveCount(1);
        base_.Artigos.Should().Contain(artigo);
    }

    [Fact(DisplayName = "RemoverArtigo deve remover artigo do agregado")]
    public void RemoverArtigo_ComArtigoExistente_DeveRemover()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");
        var artigo = Artigo.Criar("Título", _conteudo, _categoria);
        base_.AdicionarArtigo(artigo);

        // Act
        base_.RemoverArtigo(artigo.Id);

        // Assert
        base_.Artigos.Should().BeEmpty();
    }

    [Fact(DisplayName = "ObterArtigo deve retornar artigo pelo ID")]
    public void ObterArtigo_ComIdExistente_DeveRetornarArtigo()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");
        var artigo = Artigo.Criar("Título", _conteudo, _categoria);
        base_.AdicionarArtigo(artigo);

        // Act
        var recuperado = base_.ObterArtigo(artigo.Id);

        // Assert
        recuperado.Should().NotBeNull();
        recuperado.Should().Be(artigo);
    }

    [Fact(DisplayName = "ObterArtigosPorCategoria deve retornar artigos da categoria")]
    public void ObterArtigosPorCategoria_ComArticulosDaCategoria_DeveRetornar()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");
        var cat1 = Categoria.Criar("Backend");
        var cat2 = Categoria.Criar("Frontend");

        var artigo1 = Artigo.Criar("Artigo 1", _conteudo, cat1);
        var artigo2 = Artigo.Criar("Artigo 2", _conteudo, cat1);
        var artigo3 = Artigo.Criar("Artigo 3", _conteudo, cat2);

        base_.AdicionarArtigo(artigo1);
        base_.AdicionarArtigo(artigo2);
        base_.AdicionarArtigo(artigo3);

        // Act
        var artigosBackend = base_.ObterArtigosPorCategoria(cat1);

        // Assert
        artigosBackend.Should().HaveCount(2);
        artigosBackend.Should().Contain(artigo1);
        artigosBackend.Should().Contain(artigo2);
        artigosBackend.Should().NotContain(artigo3);
    }

    [Fact(DisplayName = "Publicar deve marcar como pública")]
    public void Publicar_ComBasePrivada_DevePublicar()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição válida", isPublica: false);

        // Act
        base_.Publicar();

        // Assert
        base_.IsPublica.Should().BeTrue();
    }

    [Fact(DisplayName = "Despublicar deve marcar como privada")]
    public void Despublicar_ComBasePublica_DeveDespublicar()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição válida", isPublica: true);

        // Act
        base_.Despublicar();

        // Assert
        base_.IsPublica.Should().BeFalse();
    }

    [Fact(DisplayName = "AtualizarNome deve alterar nome")]
    public void AtualizarNome_ComNovoNome_DeveAlterarNome()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Nome Original", "Descrição aqui");

        // Act
        base_.AtualizarNome("Novo Nome");

        // Assert
        base_.Nome.Should().Be("Novo Nome");
    }

    [Fact(DisplayName = "AtualizarDescricao deve alterar descrição")]
    public void AtualizarDescricao_ComNovaDescricao_DeveAlterarDescricao()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição válida aqui");

        // Act
        base_.AtualizarDescricao("Nova descrição muito mais completa");

        // Assert
        base_.Descricao.Should().Be("Nova descrição muito mais completa");
    }

    [Fact(DisplayName = "ContarArtigos deve retornar quantidade correta")]
    public void ContarArtigos_DeveRetornarQuantidadeCorreta()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");
        var artigo1 = Artigo.Criar("Artigo 1", _conteudo, _categoria);
        var artigo2 = Artigo.Criar("Artigo 2", _conteudo, _categoria);

        // Act
        base_.AdicionarArtigo(artigo1);
        base_.AdicionarArtigo(artigo2);

        // Assert
        base_.ContarArtigos().Should().Be(2);
    }

    // ====== TESTES DE INVARIANTES ======

    [Fact(DisplayName = "AdicionarArtigo duplicado deve lançar exceção")]
    public void AdicionarArtigo_ComIdDuplicado_DeveLancarInvalidOperationException()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");
        var artigo = Artigo.Criar("Artigo", _conteudo, _categoria);
        base_.AdicionarArtigo(artigo);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => base_.AdicionarArtigo(artigo))
            .Message.Should().Contain("já existe");
    }

    [Fact(DisplayName = "RemoverArtigo inexistente deve lançar exceção")]
    public void RemoverArtigo_ComIdInexistente_DeveLancarInvalidOperationException()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");
        var idInexistente = ArtigoId.Criar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => base_.RemoverArtigo(idInexistente))
            .Message.Should().Contain("não encontrado");
    }

    [Fact(DisplayName = "Publicar base já publicada deve lançar exceção")]
    public void Publicar_ComBaseJaPublicada_DeveLancarInvalidOperationException()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição válida", isPublica: true);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => base_.Publicar())
            .Message.Should().Contain("já está publicada");
    }

    [Fact(DisplayName = "Despublicar base já privada deve lançar exceção")]
    public void Despublicar_ComBaseJaPrivada_DeveLancarInvalidOperationException()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição válida", isPublica: false);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => base_.Despublicar())
            .Message.Should().Contain("não está publicada");
    }

    // ====== TESTES NEGATIVOS ======

    [Fact(DisplayName = "Criar com nome vazio deve lançar exceção")]
    public void Criar_ComNomeVazio_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            BaseConhecimento.Criar(_usuarioId, "", "Descrição válida"));
    }

    [Fact(DisplayName = "Criar com nome muito curto deve lançar exceção")]
    public void Criar_ComNomeMusitoCurto_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            BaseConhecimento.Criar(_usuarioId, "AB", "Descrição válida"))
            .Message.Should().Contain("entre 3 e 100 caracteres");
    }

    [Fact(DisplayName = "Criar com descrição vazia deve lançar exceção")]
    public void Criar_ComDescricaoVazia_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            BaseConhecimento.Criar(_usuarioId, "Nome", ""))
            .Message.Should().Contain("não pode ser vazia");
    }

    [Fact(DisplayName = "Criar com usuário null deve lançar exceção")]
    public void Criar_ComUsuarioNull_DeveLancarArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            BaseConhecimento.Criar(null!, "Nome", "Descrição"));
    }

    [Fact(DisplayName = "ObterArtigo com null deve lançar exceção")]
    public void ObterArtigo_ComNull_DeveLancarArgumentNullException()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => base_.ObterArtigo(null!));
    }

    [Fact(DisplayName = "Artigos retornado deve ser read-only")]
    public void Artigos_DeveSerReadOnly()
    {
        // Arrange
        var base_ = BaseConhecimento.Criar(_usuarioId, "Minha Base", "Uma descrição completa e válida para testes");

        // Act
        var artigos = base_.Artigos;

        // Assert - Verificar que é somente leitura
        artigos.Should().BeAssignableTo<IReadOnlyList<Artigo>>();
        artigos.GetType().Name.Should().Contain("ReadOnly");
    }
}
