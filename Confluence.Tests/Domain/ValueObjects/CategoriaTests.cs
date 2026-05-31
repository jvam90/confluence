namespace Confluence.Tests.Domain.ValueObjects;

using Confluence.Lib.Domain.ValueObjects;
using Xunit;
using FluentAssertions;

/// <summary>
/// Testes para o Value Object Categoria.
/// Cobre: validação de comprimento, igualdade, imutabilidade.
/// </summary>
public sealed class CategoriaTests
{
    // ====== TESTES POSITIVOS ======

    [Fact(DisplayName = "Criar com dados válidos deve retornar Categoria válida")]
    public void Criar_ComDadosValidos_DeveRetornarCategoriaValida()
    {
        // Arrange
        var nome = "Backend";
        var descricao = "Tópicos de backend";

        // Act
        var categoria = Categoria.Criar(nome, descricao);

        // Assert
        categoria.Should().NotBeNull();
        categoria.Nome.Should().Be(nome);
        categoria.Descricao.Should().Be(descricao);
    }

    [Fact(DisplayName = "Criar sem descrição deve retornar Categoria válida")]
    public void Criar_SemDescricao_DeveRetornarCategoriaValida()
    {
        // Arrange
        var nome = "Frontend";

        // Act
        var categoria = Categoria.Criar(nome);

        // Assert
        categoria.Should().NotBeNull();
        categoria.Nome.Should().Be(nome);
        categoria.Descricao.Should().BeNull();
    }

    [Fact(DisplayName = "Igualdade deve comparar apenas o nome")]
    public void Igualdade_DeveCompararApenasNome()
    {
        // Arrange
        var categoria1 = Categoria.Criar("Backend", "Descrição 1");
        var categoria2 = Categoria.Criar("Backend", "Descrição 2");

        // Act & Assert
        categoria1.Should().Be(categoria2);
    }

    [Fact(DisplayName = "Categorias com nomes diferentes devem ser desiguais")]
    public void Desigualdade_ComNomesDiferentes_DeveSerjFalso()
    {
        // Arrange
        var categoria1 = Categoria.Criar("Backend");
        var categoria2 = Categoria.Criar("Frontend");

        // Act & Assert
        categoria1.Should().NotBe(categoria2);
    }

    [Fact(DisplayName = "ToString deve retornar o nome")]
    public void ToString_DeveRetornarNome()
    {
        // Arrange
        var nome = "DevOps";
        var categoria = Categoria.Criar(nome);

        // Act
        var resultado = categoria.ToString();

        // Assert
        resultado.Should().Be(nome);
    }

    [Theory(DisplayName = "Nomes com comprimento válido devem ser aceitos")]
    [InlineData("SQL")]      // 3 chars
    [InlineData("Backend")]  // 7 chars
    [InlineData("C Sharp")] // Espaço allowed, 7 chars
    public void Criar_ComComprimentoValido_DeveAceitar(string nome)
    {
        // Act & Assert
        var categoria = Categoria.Criar(nome);
        categoria.Should().NotBeNull();
    }

    // ====== TESTES NEGATIVOS ======

    [Fact(DisplayName = "Criar com nome vazio deve lançar exceção")]
    public void Criar_ComNomeVazio_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Categoria.Criar(""))
            .Message.Should().Contain("não pode ser vazio");
    }

    [Fact(DisplayName = "Criar com nome whitespace deve lançar exceção")]
    public void Criar_ComNomeWhitespace_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Categoria.Criar("   "))
            .Message.Should().Contain("não pode ser vazio");
    }

    [Fact(DisplayName = "Criar com nome muito curto deve lançar exceção")]
    public void Criar_ComNomeMusitoCurto_DeveLancarArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Categoria.Criar("AB"))
            .Message.Should().Contain("entre 3 e 50 caracteres");
    }

    [Fact(DisplayName = "Criar com nome muito longo deve lançar exceção")]
    public void Criar_ComNomeMuitoLongo_DeveLancarArgumentException()
    {
        // Arrange
        var nomeLongo = new string('A', 51);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Categoria.Criar(nomeLongo))
            .Message.Should().Contain("entre 3 e 50 caracteres");
    }

    [Fact(DisplayName = "GetHashCode deve ser igual para categorias iguais")]
    public void GetHashCode_ComNomesIguais_DeveRetornarHashIgual()
    {
        // Arrange
        var categoria1 = Categoria.Criar("Backend");
        var categoria2 = Categoria.Criar("Backend");

        // Act
        var hash1 = categoria1.GetHashCode();
        var hash2 = categoria2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact(DisplayName = "Nome deve ser trimmed")]
    public void Criar_ComEspacosAoRedor_DeveTrimEspacos()
    {
        // Arrange
        var nomeComEspacos = "  Backend  ";

        // Act
        var categoria = Categoria.Criar(nomeComEspacos);

        // Assert
        categoria.Nome.Should().Be("Backend");
    }
}
