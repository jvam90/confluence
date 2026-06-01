namespace Confluence.Lib.Domain.Aggregates;

using ValueObjects;

/// <summary>
/// Aggregate Root que representa um Compartilhamento de recurso.
/// Controla o acesso compartilhado a bases ou artigos com múltiplos colaboradores.
/// Mantém invariantes como: sem colaboradores duplicados e sempre tem pelo menos um.
/// </summary>
public sealed class Compartilhamento
{
    private readonly List<Colaborador> _colaboradores = new();
    private DateTime _dataAtualizacao;

    public AggregateId Id { get; }
    public AggregateId IdBaseConhecimento { get; }
    public ArtigoId? IdArtigoCompartilhado { get; }
    public IReadOnlyList<Colaborador> Colaboradores => _colaboradores.AsReadOnly();
    public DateTime DataCompartilhamento { get; private set; }

    public DateTime DataAtualizacao
    {
        get => _dataAtualizacao;
        private set => _dataAtualizacao = value;
    }

    private Compartilhamento(
        AggregateId id,
        AggregateId idBaseConhecimento,
        ArtigoId? idArtigoCompartilhado,
        IEnumerable<Colaborador> colaboradores)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(idBaseConhecimento);
        ArgumentNullException.ThrowIfNull(colaboradores);

        if (!colaboradores.Any())
            throw new ArgumentException("Compartilhamento deve ter pelo menos um colaborador", nameof(colaboradores));

        Id = id;
        IdBaseConhecimento = idBaseConhecimento;
        IdArtigoCompartilhado = idArtigoCompartilhado;
        DataCompartilhamento = DateTime.UtcNow;
        _dataAtualizacao = DataCompartilhamento;

        foreach (var colaborador in colaboradores)
            _colaboradores.Add(colaborador);

        ValidarInvariantes();
    }

    /// <summary>
    /// Factory method para criar um novo compartilhamento.
    /// Invariante: Sem colaboradores duplicados.
    /// </summary>
    public static Compartilhamento Criar(
        AggregateId idBaseConhecimento,
        IEnumerable<Colaborador> colaboradores,
        ArtigoId? idArtigoCompartilhado = null)
    {
        return new Compartilhamento(
            AggregateId.Criar(),
            idBaseConhecimento,
            idArtigoCompartilhado,
            colaboradores);
    }

    /// <summary>
    /// Factory method para recriar um compartilhamento existente (para persistência).
    /// </summary>
    public static Compartilhamento ReconstruirDoRepositorio(
        AggregateId id,
        AggregateId idBaseConhecimento,
        ArtigoId? idArtigoCompartilhado,
        IEnumerable<Colaborador> colaboradores,
        DateTime dataCompartilhamento,
        DateTime dataAtualizacao)
    {
        ArgumentNullException.ThrowIfNull(id);

        var compartilhamento = new Compartilhamento(
            id,
            idBaseConhecimento,
            idArtigoCompartilhado,
            colaboradores)
        {
            DataCompartilhamento = dataCompartilhamento,
            DataAtualizacao = dataAtualizacao
        };

        return compartilhamento;
    }

    /// <summary>
    /// Adiciona um novo colaborador ao compartilhamento.
    /// Invariante: Sem usuários duplicados.
    /// </summary>
    public void AdicionarColaborador(Colaborador colaborador)
    {
        ArgumentNullException.ThrowIfNull(colaborador);

        if (_colaboradores.Any(c => c.IdUsuario == colaborador.IdUsuario))
            throw new InvalidOperationException(
                $"Usuário {colaborador.IdUsuario} já é colaborador neste compartilhamento");

        _colaboradores.Add(colaborador);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Remove um colaborador do compartilhamento.
    /// Invariante: Sempre deve manter pelo menos um colaborador.
    /// </summary>
    public void RemoverColaborador(UsuarioId idUsuario)
    {
        ArgumentNullException.ThrowIfNull(idUsuario);

        if (_colaboradores.Count == 1)
            throw new InvalidOperationException(
                "Não é possível remover o único colaborador do compartilhamento");

        var colaborador = _colaboradores.FirstOrDefault(c => c.IdUsuario == idUsuario);
        if (colaborador is null)
            throw new InvalidOperationException($"Usuário {idUsuario} não é colaborador");

        _colaboradores.Remove(colaborador);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Altera o nível de acesso de um colaborador.
    /// </summary>
    public void AlterarPermissaoColaborador(UsuarioId idUsuario, PermissaoAcesso novaPermissao)
    {
        ArgumentNullException.ThrowIfNull(idUsuario);
        ArgumentNullException.ThrowIfNull(novaPermissao);

        var colaborador = _colaboradores.FirstOrDefault(c => c.IdUsuario == idUsuario);
        if (colaborador is null)
            throw new InvalidOperationException($"Usuário {idUsuario} não é colaborador");

        var indice = _colaboradores.IndexOf(colaborador);
        _colaboradores[indice] = colaborador.ComNivelAcesso(novaPermissao);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Obtém o nível de acesso de um usuário.
    /// </summary>
    public PermissaoAcesso? ObterPermissaoUsuario(UsuarioId idUsuario)
    {
        ArgumentNullException.ThrowIfNull(idUsuario);
        return _colaboradores.FirstOrDefault(c => c.IdUsuario == idUsuario)?.NivelAcesso;
    }

    /// <summary>
    /// Verifica se um usuário tem um nível mínimo de acesso.
    /// </summary>
    public bool UsuarioTemAcesso(UsuarioId idUsuario, NivelAcesso nivelMinimo)
    {
        ArgumentNullException.ThrowIfNull(idUsuario);

        var permissao = ObterPermissaoUsuario(idUsuario);
        if (permissao is null)
            return false;

        return (int)permissao.Valor >= (int)nivelMinimo;
    }

    private void ValidarInvariantes()
    {
        if (!_colaboradores.Any())
            throw new InvalidOperationException("Compartilhamento deve ter pelo menos um colaborador");

        var usuariosUnicos = _colaboradores.Select(c => c.IdUsuario).Distinct().Count();
        if (usuariosUnicos != _colaboradores.Count)
            throw new InvalidOperationException("Não pode haver colaboradores duplicados");
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Compartilhamento compartilhamento)
            return false;
        return Id == compartilhamento.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => $"Compartilhamento para base {IdBaseConhecimento} ({_colaboradores.Count} colaboradores)";
}
