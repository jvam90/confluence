namespace Confluence.Lib.Domain.Factories;

using Aggregates;
using ValueObjects;

/// <summary>
/// Implementação da Factory para criação de agregados Compartilhamento.
/// Encapsula validações e lógica de criação complexa.
/// </summary>
public sealed class FabricaCompartilhamento : IFabricaCompartilhamento
{
    public Compartilhamento CriarCompartilhamento(
        AggregateId idBaseConhecimento,
        IEnumerable<Colaborador> colaboradores,
        ArticuloId? idArtigoCompartilhado = null)
    {
        ArgumentNullException.ThrowIfNull(idBaseConhecimento);
        ArgumentNullException.ThrowIfNull(colaboradores);

        ValidarDadosCompartilhamento(idBaseConhecimento, colaboradores);

        return Compartilhamento.Criar(
            idBaseConhecimento,
            colaboradores,
            idArtigoCompartilhado);
    }

    public Compartilhamento CriarCompartilhamentoComumColaborador(
        AggregateId idBaseConhecimento,
        UsuarioId idColaborador,
        PermissaoAcesso nivelAcesso,
        ArticuloId? idArtigoCompartilhado = null)
    {
        ArgumentNullException.ThrowIfNull(idBaseConhecimento);
        ArgumentNullException.ThrowIfNull(idColaborador);
        ArgumentNullException.ThrowIfNull(nivelAcesso);

        var colaborador = Colaborador.Criar(idColaborador, nivelAcesso);
        var colaboradores = new[] { colaborador };

        return CriarCompartilhamento(
            idBaseConhecimento,
            colaboradores,
            idArtigoCompartilhado);
    }

    public void ValidarDadosCompartilhamento(
        AggregateId idBaseConhecimento,
        IEnumerable<Colaborador> colaboradores)
    {
        ArgumentNullException.ThrowIfNull(idBaseConhecimento);
        ArgumentNullException.ThrowIfNull(colaboradores);

        var colaboradoresList = colaboradores.ToList();

        if (!colaboradoresList.Any())
            throw new ArgumentException(
                "Compartilhamento deve ter pelo menos um colaborador",
                nameof(colaboradores));

        var idsUnicos = colaboradoresList.Select(c => c.IdUsuario).Distinct().Count();
        if (idsUnicos != colaboradoresList.Count)
            throw new ArgumentException(
                "Não pode haver colaboradores duplicados",
                nameof(colaboradores));
    }
}
