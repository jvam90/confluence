namespace Confluence.Lib.Domain.Aggregates;

using Entities;
using ValueObjects;

/// <summary>
/// Aggregate Root que representa uma Base de Conhecimento.
/// É o ponto de entrada para todas as modificações relacionadas a artigos e categorias.
/// Mantém invariantes de negócio como unicidade de artigos e proprietário válido.
/// </summary>
public sealed class BaseConhecimento
{
    private string _nome = null!;
    private string _descricao = null!;
    private bool _isPublica;
    private readonly List<Artigo> _artigos = new();
    private DateTime _dataAtualizacao;

    public AggregateId Id { get; }
    public UsuarioId IdUsuarioPropietario { get; }

    public string Nome
    {
        get => _nome;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Nome não pode ser vazio", nameof(value));
            if (value.Length < 3 || value.Length > 100)
                throw new ArgumentException("Nome deve ter entre 3 e 100 caracteres", nameof(value));
            _nome = value.Trim();
        }
    }

    public string Descricao
    {
        get => _descricao;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Descrição não pode ser vazia", nameof(value));
            if (value.Length < 10 || value.Length > 500)
                throw new ArgumentException("Descrição deve ter entre 10 e 500 caracteres", nameof(value));
            _descricao = value.Trim();
        }
    }

    public bool IsPublica
    {
        get => _isPublica;
        private set => _isPublica = value;
    }

    public DateTime DataCriacao { get; private set; }

    public DateTime DataAtualizacao
    {
        get => _dataAtualizacao;
        private set => _dataAtualizacao = value;
    }

    public IReadOnlyList<Artigo> Artigos => _artigos.AsReadOnly();

    private BaseConhecimento(
        AggregateId id,
        UsuarioId idUsuarioPropietario,
        string nome,
        string descricao,
        bool isPublica = false)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(idUsuarioPropietario);
        ArgumentNullException.ThrowIfNull(nome);
        ArgumentNullException.ThrowIfNull(descricao);

        Id = id;
        IdUsuarioPropietario = idUsuarioPropietario;
        Nome = nome;
        Descricao = descricao;
        IsPublica = isPublica;
        DataCriacao = DateTime.UtcNow;
        _dataAtualizacao = DataCriacao;
    }

    /// <summary>
    /// Factory method para criar uma nova Base de Conhecimento.
    /// Invariante: Uma base necessita de um proprietário.
    /// </summary>
    public static BaseConhecimento Criar(
        UsuarioId idUsuarioPropietario,
        string nome,
        string descricao,
        bool isPublica = false)
    {
        return new BaseConhecimento(
            AggregateId.Criar(),
            idUsuarioPropietario,
            nome,
            descricao,
            isPublica);
    }

    /// <summary>
    /// Factory method para recriar uma Base existente (para persistência).
    /// </summary>
    public static BaseConhecimento ReconstruirDoRepositorio(
        AggregateId id,
        UsuarioId idUsuarioPropietario,
        string nome,
        string descricao,
        bool isPublica,
        DateTime dataCriacao,
        DateTime dataAtualizacao,
        IEnumerable<Artigo> artigos)
    {
        ArgumentNullException.ThrowIfNull(id);

        var base_ = new BaseConhecimento(
            id,
            idUsuarioPropietario,
            nome,
            descricao,
            isPublica)
        {
            DataCriacao = dataCriacao,
            DataAtualizacao = dataAtualizacao
        };

        foreach (var artigo in artigos ?? Enumerable.Empty<Artigo>())
            base_._artigos.Add(artigo);

        return base_;
    }

    /// <summary>
    /// Adiciona um novo artigo à base.
    /// Invariante: Sem artigos duplicados (por ID).
    /// </summary>
    public void AdicionarArtigo(Artigo artigo)
    {
        ArgumentNullException.ThrowIfNull(artigo);

        if (_artigos.Any(a => a.Id == artigo.Id))
            throw new InvalidOperationException($"Artigo com ID {artigo.Id} já existe na base");

        _artigos.Add(artigo);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Remove um artigo da base pelo seu ID.
    /// </summary>
    public void RemoverArtigo(ArtigoId idArtigo)
    {
        ArgumentNullException.ThrowIfNull(idArtigo);

        var artigo = _artigos.FirstOrDefault(a => a.Id == idArtigo);
        if (artigo is null)
            throw new InvalidOperationException($"Artigo com ID {idArtigo} não encontrado");

        _artigos.Remove(artigo);
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Obtém um artigo pelo seu ID.
    /// </summary>
    public Artigo? ObterArtigo(ArtigoId idArtigo)
    {
        ArgumentNullException.ThrowIfNull(idArtigo);
        return _artigos.FirstOrDefault(a => a.Id == idArtigo);
    }

    /// <summary>
    /// Obtém todos os artigos de uma categoria específica.
    /// </summary>
    public IReadOnlyList<Artigo> ObterArtigosPorCategoria(Categoria categoria)
    {
        ArgumentNullException.ThrowIfNull(categoria);
        return _artigos.Where(a => a.Categoria == categoria).ToList().AsReadOnly();
    }

    /// <summary>
    /// Publica a base (muda IsPublica para true).
    /// Invariante: Publicação é irreversível.
    /// </summary>
    public void Publicar()
    {
        if (IsPublica)
            throw new InvalidOperationException("Base de conhecimento já está publicada");

        IsPublica = true;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Despublica a base (muda IsPublica para false).
    /// </summary>
    public void Despublicar()
    {
        if (!IsPublica)
            throw new InvalidOperationException("Base de conhecimento não está publicada");

        IsPublica = false;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza o nome da base.
    /// </summary>
    public void AtualizarNome(string novoNome)
    {
        Nome = novoNome;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza a descrição da base.
    /// </summary>
    public void AtualizarDescricao(string novaDescricao)
    {
        Descricao = novaDescricao;
        DataAtualizacao = DateTime.UtcNow;
    }

    /// <summary>
    /// Retorna a quantidade de artigos na base.
    /// </summary>
    public int ContarArtigos() => _artigos.Count;

    public override bool Equals(object? obj)
    {
        if (obj is not BaseConhecimento base_)
            return false;
        return Id == base_.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => $"{Nome} ({_artigos.Count} artigos)";
}
