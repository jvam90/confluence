namespace Confluence.Lib.Domain.Factories;

using Aggregates;
using ValueObjects;

/// <summary>
/// Factory pattern que encapsula a criação complexa de agregados BaseConhecimento.
/// Responsável por validações e garantir que o agregado seja criado em estado válido.
/// Diferencia-se de Domain Service pois não opera em agregados existentes.
/// Implementa o Factory Method Pattern e Single Responsibility Principle.
/// </summary>
public interface IFabricaBaseConhecimento
{
    /// <summary>
    /// Cria uma nova base de conhecimento com todas as validações necessárias.
    /// Não persiste - apenas cria o objeto de domínio.
    /// </summary>
    BaseConhecimento CriarNovaBase(
        UsuarioId idProprietario,
        string nome,
        string descricao,
        bool isPublica = false);

    /// <summary>
    /// Cria uma base com um artigo inicial.
    /// </summary>
    BaseConhecimento CriarBaseComArtigoInicial(
        UsuarioId idProprietario,
        string nomeBase,
        string descricaoBase,
        string tituloArtigo,
        ConteudoArtigo conteudoArtigo,
        Categoria categoria);

    /// <summary>
    /// Valida se os dados de uma base são válidos.
    /// Lança exceção se inválido.
    /// </summary>
    void ValidarDadosBase(
        UsuarioId idProprietario,
        string nome,
        string descricao);
}
