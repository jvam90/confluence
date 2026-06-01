namespace Confluence.Lib.Domain.Factories;

using Aggregates;
using ValueObjects;

/// <summary>
/// Factory pattern que encapsula a criação complexa de agregados Compartilhamento.
/// Responsável por validações e garantir que o agregado seja criado em estado válido.
/// Implementa o Factory Method Pattern e Single Responsibility Principle.
/// </summary>
public interface IFabricaCompartilhamento
{
    /// <summary>
    /// Cria um novo compartilhamento com validações.
    /// Não persiste - apenas cria o objeto de domínio.
    /// </summary>
    Compartilhamento CriarCompartilhamento(
        AggregateId idBaseConhecimento,
        IEnumerable<Colaborador> colaboradores,
        ArtigoId? idArtigoCompartilhado = null);

    /// <summary>
    /// Cria um compartilhamento simples com um único colaborador.
    /// </summary>
    Compartilhamento CriarCompartilhamentoComumColaborador(
        AggregateId idBaseConhecimento,
        UsuarioId idColaborador,
        PermissaoAcesso nivelAcesso,
        ArtigoId? idArtigoCompartilhado = null);

    /// <summary>
    /// Valida os dados do compartilhamento.
    /// </summary>
    void ValidarDadosCompartilhamento(
        AggregateId idBaseConhecimento,
        IEnumerable<Colaborador> colaboradores);
}
