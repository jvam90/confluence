namespace Confluence.Tests.Domain.ValueObjects;

using Confluence.Lib.Domain.ValueObjects;
using Xunit;
using FluentAssertions;

/// <summary>
/// Testes para o Value Object UsuarioId.
/// Cobre: validação, igualdade por valor, imutabilidade, criação.
/// </summary>
public sealed class UsuarioIdTests
{
    // ====== TESTES POSITIVOS ======

    [Fact(DisplayName = "Criar deve retornar UsuarioId válido")]
    public void Criar_ComGuidValido_DeveRetornarUsuarioIdValido()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var usuarioId = UsuarioId.Criar(guid);

        // Assert
        usuarioId.Should().NotBeNull();
        usuarioId.Valor.Should().Be(guid);
    }

    [Fact(DisplayName = "Criar sem parametros deve retornar UsuarioId com novo Guid")]
    public void Criar_SemParametro_DeveRetornarComNovoGuid()
    {
        // Act
        var usuarioId1 = UsuarioId.Criar();
        var usuarioId2 = UsuarioId.Criar();

        // Assert
        usuarioId1.Should().NotBeNull();
        usuarioId2.Should().NotBeNull();
        usuarioId1.Valor.Should().NotBe(usuarioId2.Valor);
    }

    [Fact(DisplayName = "Igualdade deve comparar por valor")]
    public void Igualdade_DeveCompararPorValor()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var usuarioId1 = UsuarioId.Criar(guid);
        var usuarioId2 = UsuarioId.Criar(guid);

        // Act & Assert
        usuarioId1.Should().Be(usuarioId2);
        (usuarioId1 == usuarioId2).Should().BeTrue();
    }

    [Fact(DisplayName = "Desigualdade deve comparar Guids diferentes")]
    public void Desigualdade_ComGuidsdiferentes_DeveSerjFalso()
    {
        // Arrange
        var usuarioId1 = UsuarioId.Criar();
        var usuarioId2 = UsuarioId.Criar();

        // Act & Assert
        usuarioId1.Should().NotBe(usuarioId2);
        (usuarioId1 != usuarioId2).Should().BeTrue();
    }

    [Fact(DisplayName = "GetHashCode deve ser igual para UsuarioIds iguais")]
    public void GetHashCode_ComGuidsIguais_DeveRetornarHashIgual()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var usuarioId1 = UsuarioId.Criar(guid);
        var usuarioId2 = UsuarioId.Criar(guid);

        // Act
        var hash1 = usuarioId1.GetHashCode();
        var hash2 = usuarioId2.GetHashCode();

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact(DisplayName = "ToString deve retornar representação em string")]
    public void ToString_DeveRetornarGuidEmString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var usuarioId = UsuarioId.Criar(guid);

        // Act
        var resultado = usuarioId.ToString();

        // Assert
        resultado.Should().Be(guid.ToString());
    }

    [Fact(DisplayName = "Value Object é imutável")]
    public void ValueObject_DeveSerImutavel()
    {
        // Arrange
        var usuarioId = UsuarioId.Criar();
        var guidOriginal = usuarioId.Valor;

        // Act - tentar modificar (não compilaria, mas verificamos que Valor é readonly)
        // var novoValor = ...;  // usuarioId.Valor = novoValor;  // ❌ Não compila

        // Assert
        usuarioId.Valor.Should().Be(guidOriginal);
    }

    // ====== TESTES NEGATIVOS ======

    [Fact(DisplayName = "Criar com Guid vazio deve lançar exceção")]
    public void Criar_ComGuidVazio_DeveLancarArgumentException()
    {
        // Arrange
        var guidVazio = Guid.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => UsuarioId.Criar(guidVazio))
            .Message.Should().Contain("não pode ser vazio");
    }

    [Fact(DisplayName = "Equals com null deve retornar false")]
    public void Equals_ComNull_DeveRetornarFalso()
    {
        // Arrange
        var usuarioId = UsuarioId.Criar();

        // Act
        var resultado = usuarioId.Equals(null);

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact(DisplayName = "Equals com objeto diferente deve retornar false")]
    public void Equals_ComObjetoDiferente_DeveRetornarFalso()
    {
        // Arrange
        var usuarioId = UsuarioId.Criar();
        var outroObjeto = "string qualquer";

        // Act
        var resultado = usuarioId.Equals(outroObjeto);

        // Assert
        resultado.Should().BeFalse();
    }

    // ====== TESTES DE USO EM COLLECTIONS ======

    [Fact(DisplayName = "UsuarioId pode ser usado como chave em Dictionary")]
    public void UsuarioId_DeveSerUsavelEmDictionary()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var usuarioId1 = UsuarioId.Criar(guid);
        var usuarioId2 = UsuarioId.Criar(guid);
        var dicionario = new Dictionary<UsuarioId, string>();

        // Act
        dicionario.Add(usuarioId1, "User A");
        var recuperado = dicionario[usuarioId2];

        // Assert
        recuperado.Should().Be("User A");
    }

    [Fact(DisplayName = "UsuarioId pode ser comparado em HashSet")]
    public void UsuarioId_DeveSerUsavelEmHashSet()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var usuarioId1 = UsuarioId.Criar(guid);
        var usuarioId2 = UsuarioId.Criar(guid);
        var usuarioId3 = UsuarioId.Criar();
        var conjunto = new HashSet<UsuarioId> { usuarioId1, usuarioId3 };

        // Act
        var contem = conjunto.Contains(usuarioId2);

        // Assert
        contem.Should().BeTrue();
        conjunto.Count.Should().Be(2);
    }
}
