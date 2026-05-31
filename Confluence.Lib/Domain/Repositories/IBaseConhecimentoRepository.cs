namespace Confluence.Lib.Domain.Repositories;

using Aggregates;
using ValueObjects;

/// <summary>
/// Repository pattern para persistência de agregados BaseConhecimento.
/// Define o contrato que qualquer implementação de persistência deve seguir.
/// Implementa o princípio de Dependency Inversion (SOLID).
/// </summary>
public interface IBaseConhecimentoRepository
{
    /// <summary>
    /// Obtém uma base de conhecimento pelo seu ID.
    /// </summary>
    Task<BaseConhecimento?> ObterPorIdAsync(AggregateId id);

    /// <summary>
    /// Obtém todas as bases de conhecimento de um usuário proprietário.
    /// </summary>
    Task<IEnumerable<BaseConhecimento>> ObterPorProprietarioAsync(UsuarioId idProprietario);

    /// <summary>
    /// Obtém todas as bases de conhecimento públicas.
    /// </summary>
    Task<IEnumerable<BaseConhecimento>> ObterPublicasAsync();

    /// <summary>
    /// Obtém bases por termo de busca no nome ou descrição.
    /// </summary>
    Task<IEnumerable<BaseConhecimento>> BuscarPorTermoAsync(string termo);

    /// <summary>
    /// Persiste uma nova base ou atualiza uma existente.
    /// </summary>
    Task SalvarAsync(BaseConhecimento baseConhecimento);

    /// <summary>
    /// Remove uma base pelo seu ID.
    /// </summary>
    Task DeletarAsync(AggregateId id);

    /// <summary>
    /// Verifica se uma base existe.
    /// </summary>
    Task<bool> ExisteAsync(AggregateId id);

    /// <summary>
    /// Retorna a quantidade total de bases no repositório.
    /// </summary>
    Task<int> ContarAsync();
}
