namespace Confluence.Lib.Domain.Services;

using Aggregates;
using ValueObjects;

/// <summary>
/// Domain Service que encapsula lógica de compartilhamento e controle de acesso.
/// Responsável por gerenciar quem tem acesso a quais recursos.
/// Implementa o Single Responsibility Principle (SOLID).
/// </summary>
public interface IServicoCompartilhamento
{
    /// <summary>
    /// Compartilha uma base de conhecimento com um usuário.
    /// Validações: proprietário deve ser o criador original, colaborador deve ser válido.
    /// </summary>
    Task<Compartilhamento> CompartilharBaseAsync(
        UsuarioId idProprietario,
        AggregateId idBase,
        UsuarioId idColaborador,
        PermissaoAcesso nivelAcesso);

    /// <summary>
    /// Compartilha um artigo específico com um usuário.
    /// </summary>
    Task<Compartilhamento> CompartilharArtigoAsync(
        UsuarioId idProprietario,
        AggregateId idBase,
        ArtigoId idArtigo,
        UsuarioId idColaborador,
        PermissaoAcesso nivelAcesso);

    /// <summary>
    /// Altera o nível de acesso de um colaborador.
    /// </summary>
    Task AlterarPermissaoColaboradorAsync(
        UsuarioId idProprietario,
        AggregateId idCompartilhamento,
        UsuarioId idColaborador,
        PermissaoAcesso novaPermissao);

    /// <summary>
    /// Remove um colaborador do compartilhamento.
    /// Validações: proprietário deve ser o criador, não pode deixar vazio.
    /// </summary>
    Task RemoverColaboradorAsync(
        UsuarioId idProprietario,
        AggregateId idCompartilhamento,
        UsuarioId idColaborador);

    /// <summary>
    /// Verifica se um usuário tem acesso a uma base.
    /// Retorna true se é proprietário ou colaborador.
    /// </summary>
    Task<bool> UsuarioTemAcessoAsync(
        UsuarioId idUsuario,
        AggregateId idBase);

    /// <summary>
    /// Verifica se um usuário tem um nível mínimo de acesso.
    /// </summary>
    Task<bool> UsuarioTemNivelAcessoAsync(
        UsuarioId idUsuario,
        AggregateId idBase,
        NivelAcesso nivelMinimo);

    /// <summary>
    /// Obtém o nível de acesso de um usuário em uma base.
    /// </summary>
    Task<PermissaoAcesso?> ObterPermissaoUsuarioAsync(
        UsuarioId idUsuario,
        AggregateId idBase);

    /// <summary>
    /// Obtém todos os colaboradores de uma base.
    /// </summary>
    Task<IEnumerable<Colaborador>> ObterColaboradoresAsync(AggregateId idBase);

    /// <summary>
    /// Remove todo compartilhamento de uma base.
    /// Usado quando uma base é deletada.
    /// </summary>
    Task RemoverCompartilhamentosBasePorDeletacaoAsync(AggregateId idBase);
}
