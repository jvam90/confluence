namespace Confluence.Lib.Domain.Repositories;

using Aggregates;
using ValueObjects;

/// <summary>
/// Repository pattern para persistência de agregados Compartilhamento.
/// Define o contrato que qualquer implementação de persistência deve seguir.
/// Implementa o princípio de Dependency Inversion (SOLID).
/// </summary>
public interface ICompartilhamentoRepository
{
    /// <summary>
    /// Obtém um compartilhamento pelo seu ID.
    /// </summary>
    Task<Compartilhamento?> ObterPorIdAsync(AggregateId id);

    /// <summary>
    /// Obtém todos os compartilhamentos de uma base de conhecimento.
    /// </summary>
    Task<IEnumerable<Compartilhamento>> ObterPorBaseConhecimentoAsync(AggregateId idBaseConhecimento);

    /// <summary>
    /// Obtém todos os compartilhamentos onde um usuário é colaborador.
    /// </summary>
    Task<IEnumerable<Compartilhamento>> ObterPorColaboradorAsync(UsuarioId idUsuario);

    /// <summary>
    /// Obtém compartilhamentos de um artigo específico.
    /// </summary>
    Task<IEnumerable<Compartilhamento>> ObterPorArtigoAsync(ArticuloId idArtigo);

    /// <summary>
    /// Persiste um novo compartilhamento ou atualiza um existente.
    /// </summary>
    Task SalvarAsync(Compartilhamento compartilhamento);

    /// <summary>
    /// Remove um compartilhamento pelo seu ID.
    /// </summary>
    Task DeletarAsync(AggregateId id);

    /// <summary>
    /// Verifica se um compartilhamento existe.
    /// </summary>
    Task<bool> ExisteAsync(AggregateId id);

    /// <summary>
    /// Retorna a quantidade total de compartilhamentos.
    /// </summary>
    Task<int> ContarAsync();
}
